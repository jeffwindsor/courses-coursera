//Another Graph for String Reconstruction | Step 6
//Walking in the de Bruijn Graph | Step 7
System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Bioinformatics.fs"
open Bioinformatics
open DeBruijn

let toEdgeGraph k = (Seq.map (Kmers.toEdge k)) >> Seq.groupBy (fun ie -> ie.NodeIn)

let toEdgeGraphFromKmers kmers =
    let k = kmers |> Seq.head |> String.length
    kmers |> toEdgeGraph k

let toEdgeGraphFromGenome k genome =
    genome |> toKmers k |> toEdgeGraph k

let sprintEdgeGraph xs= 
    xs 
    |> Seq.map (fun (key,ies) -> (key, (ies |> Seq.map (fun ie -> ie.NodeOut) |> Seq.sort)))
    |> Seq.sortBy fst
    |> Seq.map (fun (a,ies) -> sprintf "%s -> %s" a (ies |> String.concat ","))

let printEdgeGraph xs =
    xs |> sprintEdgeGraph |> Seq.iter (printfn "%s")

//==========================================================
//Tests
["DeBruijn-Sample";"DeBruijn-Tests"]
|> Execute.testOnDataSets
        (fun lines -> 
            let k = int lines.[0]
            let genome = lines.[1]
            (toEdgeGraphFromGenome k genome |> sprintEdgeGraph |> Seq.toArray, lines.[2..] |> Array.sort))
        
"DeBruijn-Walk-Tests-input"
|> Execute.testOnDataSet 
    (fun lines -> 
        let expected = "DeBruijn-Walk-Tests-expected" |> Files.toLines |> Seq.toArray
        let actual   = toEdgeGraphFromKmers lines |> sprintEdgeGraph |> Seq.toArray
        (actual, expected)
        )    
//==========================================================

//Another Graph for String Reconstruction | Step 6
Execute.onDataSet (fun lines -> toEdgeGraphFromGenome (int lines.[0]) lines.[1]) printEdgeGraph "DeBruijn"

//Walking in the de Bruijn Graph | Step 7
Execute.onDataSet (fun lines -> toEdgeGraphFromKmers lines) printEdgeGraph "DeBruijn-Walk"

//Stop and Think
"TAATGCCATGGGATGTT" |> toEdgeGraphFromGenome 2 |> printEdgeGraph
printfn ""
"TAATGCCATGGGATGTT" |> toEdgeGraphFromGenome 3 |> printEdgeGraph
printfn ""
"TAATGCCATGGGATGTT" |> toEdgeGraphFromGenome 4 |> printEdgeGraph
printfn ""
"TAATGGGATGCCATGTT" |> toEdgeGraphFromGenome 3 |> printEdgeGraph
