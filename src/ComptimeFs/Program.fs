module ComptimeFs.Program

open System.IO

module FileSystemGenerator =
    [<Literal>]
    let Template =
        """module {{ moduleName }}
let private files: Map<string, byte seq> = Map.ofSeq {{ files }}

let tryRead (path: string) : byte seq option = Map.tryFind path files
"""

    let generate (moduleName: string) (files: Map<string, byte array>) : string =
        let moduleNameStr = $"``{moduleName}``"
        let filesStr = "[||]"

        Template
        |> fun t -> t.Replace("{{ moduleName }}", moduleNameStr)
        |> fun t -> t.Replace("{{ files }}", filesStr)

[<EntryPoint>]
let main argv =
    let files =
        argv
        |> Seq.map (fun p -> p, File.ReadAllBytes p)
        |> Map.ofSeq

    FileSystemGenerator.generate "FileSystem" files
    |> fun sourceCode -> File.WriteAllText("FileSystem.fs", sourceCode)

    0
