System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
open Bioinformatics
open System.Collections.Generic

let problemNumber = "BA5D"
//  BA5D Find the Longest Path in a DAG
//    Longest Path in a DAG Problem: Find a longest path between two nodes in an edge-weighted DAG.
//    Given: An integer representing the source node of a graph, followed by an integer representing the sink node of the graph, followed by an edge-weighted graph. The graph is represented by a modified adjacency list in which the notation "0->1:7" indicates that an edge connects node 0 to node 1 with weight 7.
//    Return: The length of a longest path in the graph, followed by a longest path. (If multiple longest paths exist, you may return any one.)
type Edge<'a> =  {SourceId:'a; TargetId:'a; Weight:int;}
let toEdges (line:string) = 
    let values = line.Replace("->",":").Split(':')  // example 0->1:7
    {SourceId=values.[0]; TargetId=values.[1]; Weight=int values.[2];}

//NOTE: SHOULD HAVE DONE TOPOLOGICAL SORTING RATHER THAN THIS HOMEGROWN BACKTRACK APPROACH
let maxWeightedPath<'a when 'a:equality and 'a:comparison> sourceId sinkId edges =
    let sources = 
        edges 
        |> Seq.groupBy (fun ie -> ie.TargetId) 
        |> Seq.map (fun (key, values) -> (key, values |> Seq.toList))
        |> Map.ofSeq
    let cache = new Dictionary<'a, (int * 'a list) option>()
    cache.[sourceId] <- Some (0,[sourceId])      //set sourceId as terminus node for path
    let rec inner (nodeId: 'a) =
        printfn "Node: %A" nodeId
        cache.[nodeId] <-
            match Map.tryFind nodeId sources with
            | None -> None
            | Some edges ->
                let validPaths = 
                    edges
                    |> List.choose (fun e -> 
                        match (if cache.ContainsKey(e.SourceId) then cache.[e.SourceId] else inner e.SourceId) with
                        | None -> None
                        | Some (w,path) -> Some (w + e.Weight, path@[e.TargetId])
                        )
                match validPaths with
                | [] -> None
                | xs -> Some (xs |> Seq.maxBy fst)

        cache.[nodeId]

    inner sinkId
    
let ``Find the Longest Path in a DAG`` sourceId sinkId edges =
    match maxWeightedPath sourceId sinkId edges with
    | None -> (0,"")
    | Some (w, ids) -> 
        let path = ids |> Seq.map string |> String.concat "->"
        (w,path)

let analyse (lines: string[]) =
    let last = (Array.length lines) - 1
    let sourceId = lines.[0]
    let sinkId   = lines.[1]
    let edges    = lines.[2..(last-2)] |> Seq.map toEdges

    ``Find the Longest Path in a DAG`` sourceId sinkId edges

let test (lines: string[]) =
    let last = (Array.length lines) - 1
    let expected = (int lines.[last-1], lines.[last])
    (analyse lines, expected)

Problems.sample problemNumber test
Problems.extra problemNumber test
Problems.dataset problemNumber analyse
Problems.rosalind problemNumber analyse