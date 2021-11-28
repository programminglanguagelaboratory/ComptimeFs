module ComptimeFs.Cli.Program

open ComptimeFs
open System.IO

[<EntryPoint>]
let main args =
    let moduleName, paths =
        match Array.toList args with
        | "-m" :: moduleName :: paths -> moduleName, paths
        | paths -> "Files", paths

    let files =
        paths
        |> Seq.map (fun p -> p, File.ReadAllBytes p)
        |> Map.ofSeq

    FileSystemGenerator.generate moduleName files
    |> fun sourceCode -> File.WriteAllText("FileSystem.fs", sourceCode)

    0
