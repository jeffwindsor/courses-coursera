namespace Bioinformatics
open WeightedAdjacenyGraph
open System.Collections.Generic

module SmallParismony =
    type Nucleotide = char
    type Dna = Nucleotide[]
    type Score = int
    type NucleotideScore = Score[]  //n=4; ACGT
    type HammingDistance = int      //n=2 Child 1, Child 2
    type Payload = 
        { 
            Ns:Nucleotide[]
            Scores:NucleotideScore[]
        }
    type Vertice = int
    type ParismonyNode = Node<Vertice, Score, Payload>
    type ParismonyGraph = Graph<Vertice, Score, Payload>

    let NucleotideAlphabetIndexes = [| 0..3 |]
    let IndexToNucletide = function | 0 -> 'A' | 1 -> 'C' | 2 -> 'G'| 3 -> 'T' | _ -> failwith "Not valid index"
    let NucleotideToIndex = function | 'A' -> 0 | 'C' -> 1 | 'G'  -> 2 | 'T' -> 3 | _ -> failwith "Not valid nucleotide"
    let NucleotideScoreToNucleotide fs (ns:NucleotideScore) : Nucleotide = 
        ns 
        |> Array.mapi (fun i s -> 
//            printf "  [%i]=%i" i s
            let n = IndexToNucletide i
//            printfn " -> %c" n
            (n,fs n s)) 
        |> Seq.minBy snd |> fst
    let NucleotideScoresToDna fs scores : Dna = scores |> Array.mapi (fun i score -> 
//            printfn "Nucleotide [%i]:" i
            let r = NucleotideScoreToNucleotide (fs i) score
//            printfn "  result : %A" r
            r
        )
    let PayloadNucleotideScoresToDna fs payload = 
//        printfn "PayloadNucleotideScoresToDna %A" payload.Scores
        { Ns= NucleotideScoresToDna fs payload.Scores; Scores=payload.Scores }
    let NodeNucleotideScoresToDna fs node = 
        if Array.isEmpty node.Payload.Ns 
        then {Id=node.Id; Edges=node.Edges; Payload= PayloadNucleotideScoresToDna fs node.Payload}
        else node
    
    let MinScore (ns:NucleotideScore) index : Score = 
        NucleotideAlphabetIndexes
        |> Array.map (fun i -> if i = index then ns.[i] else ns.[i] + 1)
        |> Array.min
    let MergeNucleotideScore a b : NucleotideScore = 
        Array.zip (NucleotideAlphabetIndexes |> Array.map (MinScore a)) (NucleotideAlphabetIndexes |> Array.map (MinScore b))
        |> Array.map (fun (a,b) -> a + b)
    let MergeNucleotideScores a b : NucleotideScore[] =
        Array.zip a b |> Array.map (fun (a,b) -> MergeNucleotideScore a b )
    let MergePayloadScores child1 child2 =
        { Ns = [||]; Scores = MergeNucleotideScores child1.Payload.Scores child2.Payload.Scores }
    let NodeMergePayloadScore node child1 child2 ={ Id = node.Id; Edges = node.Edges; Payload = MergePayloadScores child1 child2 }
    
    let HammingDistance (one:seq<char>) (two:seq<char>) = Seq.map2 (fun a b -> if a = b then 0 else 1) one two |> Seq.sum
    let ParsimonyScore (g:ParismonyGraph) = g |> Seq.collect (fun n -> n.Edges) |> Seq.map (fun e -> e.Weight) |> Seq.sum
    let matchScore n n' = if n=n' then 0 else 1
    let InitializeGraph n (connections:seq<int*int>) (leavesWithDna:seq<int*string>) =
        let initialLeafPayload dna =
            let nucleotides = dna |> Problems.Strings.toChars
            { 
                Ns = nucleotides
                Scores = (Array.init nucleotides.Length (fun i -> 
                    let nucleotidei = nucleotides.[i]
                    let scoreIndex = NucleotideToIndex nucleotidei
                    Array.init 4 (fun i -> matchScore i scoreIndex)))
            }
        let dnaById = leavesWithDna |> Map.ofSeq
        let childrenByParentId = 
            connections
            |> Seq.groupBy (fun (a,_) -> a)
            |> Seq.map (fun (a, items) -> a, items |> Seq.map (fun (_,b) -> {Id=b; Weight=0}) |> Seq.toList)
            |> Map.ofSeq
        let edges id = 
            match Map.tryFind id childrenByParentId with
            | None -> []
            | Some es -> es
        let payload id = 
            match Map.tryFind id dnaById with
            | Some dna -> (initialLeafPayload dna)
            | None -> { Ns=Array.empty<char>; Scores=Array.empty<NucleotideScore> }
        [ 0 .. n] 
        |> List.map (fun id -> {Id=id; Edges=edges id; Payload = payload id})
    
    let SolveGraph (g:ParismonyGraph) : ParismonyGraph =
//        printfn "SolveGraph"
        let notVisited n = Array.isEmpty n.Payload.Scores
        let visited n = n |> notVisited |> not
        let child1 (dict:IDictionary<Vertice,ParismonyNode>) node = dict.[node.Edges.[0].Id]
        let child2 (dict:IDictionary<Vertice,ParismonyNode>) node = dict.[node.Edges.[1].Id]
        let nodesReadyToProcess (dict:IDictionary<Vertice,ParismonyNode>) = 
            dict 
            |> Seq.map (fun (KeyValue(_,v)) -> v) 
            |> Seq.filter (fun node -> node |> notVisited && child1 dict node |> visited  && child2 dict node |> visited)
            |> Seq.toList
        let map = g |> List.map (fun n -> (n.Id ,n)) |> Map.ofList
        let dict = new Dictionary<Vertice,ParismonyNode>(map)
        let score node = NodeMergePayloadScore node (child1 dict node) (child2 dict node)
        let compileDna fs node = NodeNucleotideScoresToDna fs node
        let set node = dict.[node.Id] <- node; node;
        let rec buildScores nodesToProcess = 
//            printfn "build scores %A" (List.map (fun n -> n.Id) nodesToProcess)
            match nodesToProcess with
            | [node] -> 
//                printfn "Set ROOT [%i]" node.Id
                node |> score |> compileDna (fun _ _ s -> s) |> set
            | nodes  -> 
                nodes |> List.iter ( score >> set >> ignore )
                buildScores (nodesReadyToProcess dict)
        let root = buildScores (nodesReadyToProcess dict)       
        let rec assignDna node =
//            printfn "AssignDna %A" node
            if List.isEmpty node.Edges 
            then ()
            else
                let fs i n s = s + (matchScore n node.Payload.Ns.[i])
//                printfn "Child 1"
                child1 dict node |> compileDna fs |> set |> assignDna
//                printfn "Child 2"
                child2 dict node |> compileDna fs |> set |> assignDna
        assignDna root
        
        dict |> Seq.map (fun (KeyValue(_,v)) -> v) |> Seq.toList //end