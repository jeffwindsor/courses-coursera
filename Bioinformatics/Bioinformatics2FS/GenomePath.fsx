System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Bioinformatics.fs"
open Bioinformatics

let genomePathToString (paths: string seq) =
    let first = paths |> Seq.head |> String.toChars
    let l = first |> Array.length
    let initial = first |> Array.rev |> Array.toList
    paths 
    |> Seq.tail
    |> Seq.fold (fun acc p -> p.[l-1]::acc) initial
    |> Seq.rev
    |> Char.toString

//Answer
Execute.onDataSet
    (fun lines -> genomePathToString lines)
    (printfn "%s")
    "GenomePath"

//Tests
let lines = "GenomePath-Tests" |> Files.toLines |> Seq.toArray
let expected = lines.[0]
let actual = genomePathToString lines.[1..]

actual = expected