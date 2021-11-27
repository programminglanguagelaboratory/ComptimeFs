module ComptimeFs.Cli.Program

open ComptimeFs
open System.IO

[<EntryPoint>]
let main argv =
    let files =
        argv
        |> Seq.map (fun p -> p, File.ReadAllBytes p)
        |> Map.ofSeq

    FileSystemGenerator.generate "FileSystem" files
    |> fun sourceCode -> File.WriteAllText("FileSystem.fs", sourceCode)

    0
