module ComptimeFs.Cli.Program

open ComptimeFs
open System.IO

[<EntryPoint>]
let main args =
    let moduleName, mounts =
        match Array.toList args with
        | "-m" :: moduleName :: paths -> moduleName, paths
        | paths -> "FileSystem", paths

    Mounter.resolve mounts
    |> Codegen.generate moduleName
    |> fun sourceCode -> File.WriteAllText("FileSystem.fs", sourceCode)

    0
