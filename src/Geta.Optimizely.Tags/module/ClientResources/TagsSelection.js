define([
        "dojo/_base/declare",
        "dijit/form/TextBox",
        "https://cdn.jsdelivr.net/npm/@yaireo/tagify",
        "https://cdn.jsdelivr.net/npm/@yaireo/dragsort"
    ],
    function (
        declare,
        TextBox,
        Tagify,
        DragSort
    ) {
        return declare([TextBox], {

            _tagify: null,
            _abortController: new AbortController(),
            postCreate: function () {
                this.inherited(arguments);
                this._createTags();
            },

            destroy: function () {
                this._destroyTags();
                this.inherited(arguments);
            },

            _createTags: function () {
                this._destroyTags();
                this._tagify = new Tagify(this.textbox, {
                    onChangeAfterBlur: false,
                    whitelist: [],
                    originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(','),
                    duplicates: this.allowDuplicates,
                    userInput: !this.readOnly,
                    maxTags: this.maxTags,
                    dropdown: {
                        enabled: 2,
                        caseSensitive: this.caseSensitive
                    },
                    delimiters: this.allowSpaces ? "," : ",| ",
                    trim: this.allowSpaces, // this helps the space delimiter above work better
                });

                new window.DragSort(this._tagify.DOM.scope, {
                    selector: '.'+this._tagify.settings.classNames.tag,
                    callbacks: {
                        dragEnd: this._onDragEnd.bind(this)
                    }
                })

                this.textbox.addEventListener('focus', (event) => { // divert focus to tagify input from standard input.
                    event.preventDefault();
                    const tagifyInput = document.querySelector('.tagify__input');
                    tagifyInput.focus();
                })

                this._tagify.on('input', this._onInput.bind(this))
                this._tagify.on('add change', this._onChange.bind(this))
                this._tagify.on('edit:updated', this._onEdit.bind(this))
            },

            _onDragEnd: function (elm) {
                this._tagify.updateValueByDOMTags()
                this.onChange(elm.value);
            },

            _onChange: function (e) {
                this._set(e.detail.value);
            },

            _onEdit: function (e) {
                this._tagify.updateValueByDOMTags()
                this.textbox.value = this._tagify.value.map(tag => tag.value).toString()
                this._set(this._tagify.value.map(tag => tag.value).toString())
            },

            _onInput: function (e) {
                const value = e.detail.value;
                this._tagify.whitelist = null;

                this._abortController && this._abortController.abort();
                this._abortController = new AbortController();

                fetch(`/getatags?groupKey=${this.groupKey || ""}&term=${value}`, {signal: this._abortController.signal})
                    .then((res) =>  res.json())
                    .then((newWhitelist) => {
                        this._tagify.whitelist = newWhitelist.filter((tag) => this.allowSpaces ? true : !tag.includes(" "));
                        this._tagify.dropdown.show(value)
                    })

                this._tagify.dropdown.hide();
            },

            _destroyTags: function () {
                this._tagify && this._tagify.destroy();
                this._tagify = null;
            },

            _setValueAttr: function (value, priorityChange) {
                this.inherited(arguments);
                this._started && !priorityChange && this._createTags();
            },
        });
    });