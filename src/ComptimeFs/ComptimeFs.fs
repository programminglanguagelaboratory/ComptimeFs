﻿module ComptimeFs

module FileSystemGenerator =
    [<Literal>]
    let Template =
        """module {{ moduleName }}
let private files: Map<string, byte seq> = Map.ofSeq {{ files }}

let tryRead (path: string) : byte seq option = Map.tryFind path files
"""

    let generateContentsStr (contents: byte array) : string =
        contents
        |> Seq.map (fun b -> $"%u{b}uy")
        |> String.concat "; "
        |> fun bss -> $"[| {bss} |]"

    let generate (moduleName: string) (files: Map<string, byte array>) : string =
        let moduleNameStr = $"``{moduleName}``"

        let filesStr =
            files
            |> Map.toSeq
            |> Seq.map (fun (path, contents) -> $""""{path}", {generateContentsStr contents}""")
            |> String.concat "; "
            |> fun fss -> $"[| {fss} |]"

        Template
        |> fun t -> t.Replace("{{ moduleName }}", moduleNameStr)
        |> fun t -> t.Replace("{{ files }}", filesStr)