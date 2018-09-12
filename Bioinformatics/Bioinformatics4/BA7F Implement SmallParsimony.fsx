System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Problems.fs"
#load "Matrices.fs"
#load "WeightedAdjacenyGraph.fs"
#load "SmallParismony.fs"
open Bioinformatics
open Bioinformatics.SmallParismony

// **************************************
//Small Parsimony Problem
//Find the most parsimonious labeling of the internal nodes of a rooted tree
//Given: An integer n followed by an adjacency list for a rooted binary tree with n leaves labeled by DNA strings.
//Return: The minimum parsimony score of this tree, followed by the adjacency list of the tree corresponding to labeling internal nodes by DNA strings in order to minimize the parsimony score of the tree.
//Note: Remember to run SmallParsimony on each individual index of the strings at the leaves of the tree.
// **************************************
let id = "BA7F"

let splitInput (line:string) = 
    let seperators = [|"->"|]
    let sides = line.Split(seperators,System.StringSplitOptions.RemoveEmptyEntries)
    sides.[0], sides.[1]

let actual (lines: string[]) = 
    // INPUTS
    let leafCount = int lines.[0]
    let adjacenyList = lines.[1 .. (2 * leafCount - 2)]
    let n = Array.length adjacenyList;
    let leavesInput = 
        adjacenyList.[..(leafCount-1)] 
        |> Seq.mapi (fun bid line ->
            let (aid,dna) = splitInput line
            (int aid), bid, dna)

    let leavesWithDna = leavesInput |> Seq.map (fun (_, bid, dna) -> bid, dna)
    let leafConnections = leavesInput |> Seq.map (fun (aid, bid, _) -> aid, bid)
    let innerConnections = 
        adjacenyList.[leafCount..]
        |> Seq.map (splitInput >> (fun (aid, bid) -> (int aid), (int bid)))

    // GRAPH BUILD
    let g = 
        InitializeGraph n (Seq.append leafConnections innerConnections) leavesWithDna
        //|> List.map (fun n -> printfn "Id:%i Edges:%A : %A" n.Id n.Edges n.Payload; n; )
        |> SolveGraph
        |> List.map (fun n -> printfn "[%i] %s : %A" n.Id (n.Payload.Ns |> Problems.Chars.toString) n.Edges; n)
        
    // OUTPUTS
    let score = ParsimonyScore g
    let map = g |> List.map (fun n -> n.Id, n) |> Map.ofList
    let pair (n:ParismonyNode) i = 
        let e = n.Edges.[i]
        (n.Payload.Ns |> Problems.Chars.toString, map.[e.Id].Payload.Ns |> Problems.Chars.toString, e.Weight)
    let answer dna1 dna2 score = sprintf "%s->%s:%i" dna1 dna2 score
    let adjs = 
        g 
        |> List.filter (fun n -> (List.isEmpty n.Edges) |> not)
        // both children
        |> List.collect (fun n -> [pair n 0; pair n 1]) 
        // show adj both ways
        |> List.collect (fun (dna1, dna2, score) -> [answer dna1 dna2 score; answer dna1 dna2 score;])
    (sprintf "%i" score)::adjs

let withExpected (lines: string[]) = 
    let n = int lines.[0]
    let start = (2 * n - 1)
    let expected = lines.[start..] |> Array.toList
    (actual lines, expected)

//let print (g:DistanceGraph) = 
//    g |> toAnswerFormat
//    |> Seq.iter (fun item -> printfn "%s" item)
// **************************************
// RUNS
// **************************************
Solve id Sample withExpected ||> AreEqualListComparison
// Solve id Sample withExpected ||> AreEqualListComparison
// Solve id "SampleFromVideo" withExpected ||> AreEqualListComparison
// Solve id Extra withExpected ||> AreEqualListComparison 
// Solve id Dataset actual |> print
// Solve id Rosalind actual |> print