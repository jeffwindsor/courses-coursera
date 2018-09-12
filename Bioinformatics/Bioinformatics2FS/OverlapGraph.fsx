System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Bioinformatics.fs"
open Bioinformatics

let overlap (l:int) (one:string) (two:string) =
    one.[1..] = two.[..(l-2)]

let overlapGraph xs =
    let l = xs |> Seq.head |> String.length
    xs 
    |> Seq.collect (fun x -> 
            Seq.choose (fun x' -> 
                            if overlap l x x'
                            then Some (x,x')
                            else None
                                ) xs)

//Answer
Execute.onDataSet 
    (fun lines -> overlapGraph lines) 
    (Seq.iter (fun (a,b) -> (printfn "%s -> %s" a b) ))
    "OverlapGraph"

//Tests
let test0 =
    let lines = "OverlapGraph-Tests" |> Files.toLines |> Seq.toArray
    let expected = "OverlapGraph-Tests-Result" |> Files.toLines |> Seq.sort |> Seq.toArray
    let actual = 
        overlapGraph lines 
        |> Seq.map (fun (a,b) -> (sprintf "%s -> %s" a b) )
        |> Seq.sort |> Seq.toArray

    actual = expected


//let universalString k =
//    let alpha = ["0";"1"] |> List.toSeq
//    let rec inner = function
//        | 1 -> alpha
//        | n -> 
//            Seq.cartesian alpha (inner (n-1))
//            |> Seq.map (fun (a,b) -> a + b)
//    inner k |> Seq.reduce (+)
//     
//universalString 4