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
                    whitelist: [],
                    originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(','),
                    duplicates: this.allowDuplicates,
                    userInput: !this.readOnly,
                    maxTags: this.maxTags,
                    dropdown: {
                        enabled: 2,
                        caseSensitive: this.caseSensitive
                    },
                    pattern: this.allowSpaces ? "^\\S+$" : null,
                });
                
                new window.DragSort(this._tagify.DOM.scope, {
                    selector: '.'+this._tagify.settings.classNames.tag,
                    callbacks: {
                        dragEnd: this._onDragEnd.bind(this)
                    }
                })

                this._tagify.on('input', this._onInput.bind(this))
                this._tagify.on('change', this._onAdd.bind(this))
                this._tagify.on('add', this._onAdd.bind(this));
            },
            
            _onDragEnd: function (elm) {
                this._tagify.updateValueByDOMTags()
                this._set(elm.value);
                this.onChange(elm.value);
            },
            
            _onAdd: function (e) {
                this.onChange(e.target.value);
                this._set(e.target.value)
            },

            _onInput: function (e) {
                const value = e.detail.value;
                this._tagify.whitelist = null;

                this._abortController && this._abortController.abort();
                this._abortController = new AbortController();
                
                fetch('/getatags?groupKey=' + value, {signal: this._abortController.signal})
                    .then((res) =>  res.json())
                    .then((newWhitelist) => {
                        this._tagify.whitelist = newWhitelist;
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