# Geta Tags for Optimizely

![](http://tc.geta.no/app/rest/builds/buildType:(id:GetaPackages_OptimizelyTags_00ci),branch:master/statusIcon)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Geta_geta-optimizely-tags&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Geta_geta-optimizely-tags)
[![Platform](https://img.shields.io/badge/Platform-.NET%205.0-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx)
[![Platform](https://img.shields.io/badge/EPiServer-%2012-orange.svg?style=flat)](http://world.episerver.com/cms/)

## Description

Geta Tags is a library that adds tagging functionality to Optimizely content.

## Features

- Define tag properties
- Query for data
- Admin page for managing tags
- Tags maintenance schedule job

See the [editor guide](docs/editor-guide.md) for more information.

## How to get started?

Start by installing NuGet package (use [Optimizely NuGet](https://nuget.optimizely.com/)):

```
dotnet add package Geta.Optimizely.Tags
```

Geta Tags library uses [tag-it](https://github.com/aehlke/tag-it) jQuery UI plugin for selecting tags.
To add Tags as a new property to your page types, you need to use the UIHint attribute like in this example:

```csharp
[UIHint("Tags")]
public virtual string Tags { get; set; }

[TagsGroupKey("mykey")]
[UIHint("Tags")]
public virtual string Tags { get; set; }

[CultureSpecific]
[UIHint("Tags")]
public virtual string Tags { get; set; }
```

Register tags in Startup.cs using folllowing service extension 

```csharp
services.AddGetaTags();
```

Then, call `UseGetaTags` in the `Configure` method:

```csharp
app.UseGetaTags();
```

Also, you have to add Razor pages routing support.

```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});
```

Use `ITagEngine` to query for data:

```csharp
IEnumerable<ContentData> GetContentByTag(string tagName);
IEnumerable<ContentData> GetContentsByTag(Tag tag);
IEnumerable<ContentData> GetContentsByTag(string tagName, ContentReference rootContentReference);
IEnumerable<ContentData> GetContentsByTag(Tag tag, ContentReference rootContentReference);
IEnumerable<ContentReference> GetContentReferencesByTags(string tagNames);
IEnumerable<ContentReference> GetContentReferencesByTags(IEnumerable<Tag> tags);
IEnumerable<ContentReference> GetContentReferencesByTags(string tagNames, ContentReference rootContentReference);
IEnumerable<ContentReference> GetContentReferencesByTags(IEnumerable<Tag> tags, ContentReference rootContentReference);
```

## Customize Tag-it behaviour
You can customize the [Tag-it.js](https://github.com/aehlke/tag-it) settings by using the GetaTagsAttribute.
The following settings can currently be customized

- allowSpaces - defaults to **false**
- allowDuplicates - defaults to **false**
- caseSensitive - defaults to **true**
- readOnly - defaults to **false**
- tagLimit - defaults to **-1** (none)

```csharp
[CultureSpecific]
[UIHint("Tags")]
[GetaTags(AllowSpaces = true, AllowDuplicates = true, CaseSensitive = false, ReadOnly = true)]
public virtual string Tags { get; set; }
```

## Local development setup

Use Foundation project in the solution for testing. Follow [Foundation](https://github.com/episerver/Foundation/tree/main) project's setup guide.

## Package maintainer

https://github.com/marisks

## Changelog

[Changelog](CHANGELOG.md)
