module ComptimeFs

module Codegen =
    [<Literal>]
    let Template =
        """module {{ moduleName }}
let private fileSystem : Map<string, byte seq> = {{ fileSystem }}

let tryRead (path: string) : byte seq option = Map.tryFind path fileSystem
"""

    let generateContentsStr (contents: byte array) : string =
        contents
        |> Seq.map (fun b -> $"%u{b}uy")
        |> String.concat "; "
        |> fun bss -> $"[| {bss} |]"

    let generate (moduleName: string) (fileSystem: Map<string, byte array>) : string =
        let moduleNameStr = $"``{moduleName}``"

        let fileSystemStr =
            fileSystem
            |> Map.toSeq
            |> Seq.map (fun (path, contents) -> $""""{path}", {generateContentsStr contents}""")
            |> String.concat "; "
            |> fun fss -> $"Map.ofSeq [| {fss} |]"

        Template
        |> fun t -> t.Replace("{{ moduleName }}", moduleNameStr)
        |> fun t -> t.Replace("{{ fileSystem }}", fileSystemStr)

module Mounter =
    let resolve (mount: string) : string * byte array =
        let src, dst =
            match mount.Split ':' with
            | [| src |] -> src, src
            | [| src; dst |] -> src, dst
            | _ -> failwith $"invalid mount: {mount}"
        dst, System.IO.File.ReadAllBytes src
