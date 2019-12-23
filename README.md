# tree

![](https://i.imgur.com/v81Ctdf.png)

C# implementation of unix-like `tree` command for listing contents of directories.

Tested in VS 2017 with .NET 4.0, 4.6, probably works in other versions too.

```
Usage: FsTree [dir] [-a] [-L level]
  -a - show unix 'hidden' files/dirs like .git, .vs, .gitignore
  -L level - do not descend more than <level> directories deep
Examples:
  FsTree
  FsTree myDir
  FsTree -a -L 3
```
