---
title: DocumentLoader
description: Utility for loading Documents from memory or files.
search:
    boost: 0.002
---

<link rel="stylesheet" href="/StardewUI/stylesheets/reference.css" />

/// html | div.api-reference

# Class DocumentLoader

## Definition

<div class="api-definition" markdown>

Namespace: [StardewUI.Framework.Dom](index.md)  
Assembly: StardewUI.dll  

</div>

Utility for loading [Document](document.md)s from memory or files.

```cs
public static class DocumentLoader
```

**Inheritance**  
[Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) ⇦ DocumentLoader

## Members

### Methods

 | Name | Description |
| --- | --- |
| [LoadFromFile(FileInfo)](#loadfromfilefileinfo) | Loads a [Document](document.md) from markup in a file. | 
| [TryLoadFromFileAsync(FileInfo)](#tryloadfromfileasyncfileinfo) | Loads a [Document](document.md) from markup in a file using asynchronous I/O. | 

## Details

### Methods

#### LoadFromFile(FileInfo)

Loads a [Document](document.md) from markup in a file.

```cs
public static StardewUI.Framework.Dom.Document LoadFromFile(System.IO.FileInfo file);
```

##### Parameters

**`file`** &nbsp; [FileInfo](https://learn.microsoft.com/en-us/dotnet/api/system.io.fileinfo)  
The file containing the document markup.

##### Returns

[Document](document.md)

  The parsed [Document](document.md).

##### Remarks

This method is designed to be called from SMAPI's content loader, and throws exceptions normally associated with SMAPI's content pipeline.

-----

#### TryLoadFromFileAsync(FileInfo)

Loads a [Document](document.md) from markup in a file using asynchronous I/O.

```cs
public static System.Threading.Tasks.Task<StardewUI.Framework.Dom.Document> TryLoadFromFileAsync(System.IO.FileInfo file);
```

##### Parameters

**`file`** &nbsp; [FileInfo](https://learn.microsoft.com/en-us/dotnet/api/system.io.fileinfo)  
The file containing the document markup.

##### Returns

[Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1)<[Document](document.md)>

  The parsed [Document](document.md), or `null` if the file does not exist or the markup contained in the file is invalid.

-----

