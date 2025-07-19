# DQD.UnixFileStatus

A .NET library providing access to the (mostly) full content of `struct stat` on Linux / FreeBSD / OS X.

## Using

Add a reference to the `DQD.UnixFileStatus` NuGet package, then call methods of the `FileEx` class:
```
var status = FileEx.GetFileStatus("/dev/console");

if (FileEx.TryGetFileStatus("/usr/bin/ls", out var status))
{
  ...
}

if (FileEx.IsLink("/dev/stdout"))
{
  ...
}

if (FileEx.IsRegularFile("/etc/passwd"))
{
  ...
}

## Source Code

The repository for this library's source code is found at:

* https://github.com/logiclrd/DQD.UnixFileStatus/

## License

The library is provided under the MIT Open Source license. See `LICENSE.md` for details.
