System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "..\BioinformaticsFS\Support.fs"
#load "..\BioinformaticsFS\Bioinformatics.fs"
#load "..\BioinformaticsFS\Debruijn.fs"
open Bioinformatics
open DeBruijn

let toNonBreakingPaths (nodes:seq<Node<string>>) =
    let ns = nodes |> Seq.map (fun n -> (n.Label,(List.length n.InEdges, List.length n.OutEdges, n.OutEdges))) 
    let map = ns |> Map.ofSeq
    let rec buildPath (path:string list) (label:string) =
        let (li,lo,edge) = map.Item label
        if   li = 1 && lo = 1 
        then
            //printf "-%s  (%i-%i)" label li lo
            buildPath (label::path) (List.head edge)
        else 
            //printfn "-%s  (%i-%i)|" label li lo
            (label::path) |> Seq.rev
    ns
    |> Seq.filter  (fun (_,(li,lo,_)) -> lo > 0 && not (li = 1 && lo = 1))
    |> Seq.collect (fun (key,(li,lo,outEdges)) -> 
                        //printf "Starting : |%s (%i-%i)" key li lo
                        outEdges |> List.map (buildPath [key]))
    
let contigs k kmers = 
    kmers
    |> Seq.map (Kmers.toEdge k)
    |> toNodes
    |> toNonBreakingPaths
    |> Seq.map String.unwindowed

//==========================================================
//Tests
["ContigGeneration-Sample";"ContigGeneration-Extra"]
|> Execute.testOnDataSets
    (fun lines ->
        let i = lines |> Array.findIndex (fun s -> s = "Output:")
        let inputs   = lines.[1..i-1]
        let k = inputs.[0] |> String.length
        let actual = contigs k inputs |> Seq.toArray |> Array.sort
        let expected = lines.[i+1..] |> Array.sort

        (actual, expected)
    )
//==========================================================
    
//Assembling Genomes from Read-Pairs | Step 6
Execute.onDataSet 
    (fun lines -> 
        let k = lines.[0] |> String.length
        contigs k lines
    )
    (Seq.iter (printfn "%s"))
    "ContigGeneration"

//"TAATGCCATGGGATGTT"
//|> String.windowed 8
//|> Seq.sort
//|> Seq.iter (fun s -> printf "(%s|%s) " s.[..2] s.[5..] )
