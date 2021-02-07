# Unisave usage - backend compilation

A backend is uploaded and the corresponding source code files and
library DLLs are stored in object storage.

A job that compiles these files is scheduled to run and the resulting
files will be sent back to the caller who will save those files in
object storage.

> **Security:** The compilation has to take place within the container,
> not the dockerfile, as it basically performs untrusted code execution.

The compilation process requires:

- source code files
- .NET version (reference the proper `mscorlib.dll`)
- C# language version (csc `langversion` flag)
- `#define` directives
- Unisave framework and libraries
- client libraries

> **Note:** use the `csc.rsp` file to pass compiler parameters.

The container to perform the compilation in `mono:something` container.

TODO ...
