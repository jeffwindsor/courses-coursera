System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "CircularChromosomes.fs"
open Bioinformatics
open CircularChromosomes
open CircularChromosomes.Cycles

type Graph = Graph of AdjacencyList * ConnectedList * ColoredEdge list list
let coloredCycleToGraph cl = Graph ((coloredCycleToAdjacencyList cl), (coloredCycleToConnectedComponents cl), [(cl |> coloredEdgeCycleEdgesByColor Red)])
let graphNonTrivialCycle (Graph (_, (ConnectedList cls), _)) = cls |> List.tryFind (fun s -> Set.count s > 2)

let coloredEdgesToChromosome redEdges =
    redEdges |> coloredEdgesToBlackEdges |> blackEdgesToChromosome

let graphTwoBreak (Graph ((AdjacencyList adjs), (ConnectedList cls), redEdges)) i =

    printfn "Connected"; Seq.iter (printfn "%A") cls;
    printfn "Adjacency"; adjs |> Seq.iteri (fun i {Red=r;Blue=b} -> printfn "%i -> R:%i B:%i" i r b);
    printf  "Red Edges: "; redEdges |> Seq.iter (fun es -> es |> Seq.iter (fun (Colored ((Vertex a, Vertex b),_)) -> printf "(%i,%i)" a b); printf " | "); printfn "";
    printfn "Chromo   : %s" (redEdges |> Seq.map (coloredEdgesToChromosome >> chromosomeToString) |> String.concat "")

    let redBlue {Red=r;Blue=b} = r, b 
    let i', j = redBlue adjs.[i]
    let j', j'b = redBlue adjs.[j]
    let _, i'b = redBlue adjs.[i']

    //Modify Adj List
    printfn "  Found i:%i i':%i j:%i j':%i" i i' j j'
    printfn "  Breaking (%i-%i) (%i-%i) -> Forming (%i-%i) (%i-%i)" i i' j j' i j i' j'
    adjs.[i]  <- {Red=j;Blue=j}
    adjs.[i'] <- {Red=j';Blue=i'b}
    adjs.[j]  <- {Red=i;Blue=i}
    adjs.[j'] <- {Red=i';Blue=j'b}

    //Modify Cycles
    let cls' = 
        let s' = [i';j'] |> Set.ofList
        cls 
        |> Seq.map (fun s -> if Set.contains i s then Set.difference s s' else s)
        |> Seq.append [s']
        |> Seq.toList

    //Swap ends that were broken
    let mutable remainder = []
    let tail = List.tail redEdges
    let header = 
        redEdges
        |> List.head
        |> List.choose
            (fun ((Colored ((va, Vertex b), color)) as c) -> 
                match b with
                | b when b = i  -> remainder <- [Colored ((va, Vertex j'), color)]; None;
                | b when b = j  -> remainder <- [Colored ((va, Vertex i'), color)]; None;
                | b when b = i' -> Some (Colored ((va, Vertex j), color))
                | b when b = j' -> Some (Colored ((va, Vertex i), color))
                | _ -> Some c
                )
           
    Graph ((AdjacencyList adjs), (ConnectedList cls'), header::remainder::tail)

//2-Break Sorting Problem
//Find a shortest transformation of one genome into another by 2-breaks.
//Given: Two genomes with circular chromosomes on the same set of synteny blocks.
//Return: TThe sequence of genomes resulting from applying a shortest sequence of 2-breaks transforming one genome into the other.
let id = "BA6D"
//let graphPrint (Graph ((AdjacencyList adjs), (ConnectedList cls), redEdges)) =
//    printfn "********  GRAPH **************"
//    printfn "Connected"; Seq.iter (printfn "%A") cls;
//    printfn "Adjacency"; adjs |> Seq.iteri (fun i {Red=r;Blue=b} -> printfn "%i -> R:%i B:%i" i r b);
//    printfn "Red Edges"; redEdges |> Seq.iter (fun (Colored ((Vertex a, Vertex b),_)) -> printf "(%i,%i)" a b); printfn "";

let ``2-Break Sorting Problem`` p q =
    let g = mergeGenomesToColoredCycles (genomeFromInput p) (genomeFromInput q)
            |> fun es -> 
                let blue = es |> coloredEdgeCycleEdgesByColor Blue
                printf  "Blues Edges: "; blue |> Seq.iter (fun (Colored ((Vertex a, Vertex b),_)) -> printf "(%i,%i)" a b); printfn "";
                es;
            |> coloredCycleToGraph

    let rec inner g =
        match graphNonTrivialCycle g with
        | Some c -> graphTwoBreak g (c |> Set.toSeq |> Seq.head) |> inner
        | None   -> g

    inner g |> ignore
    
    [|""|]

// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =  ``2-Break Sorting Problem`` lines.[0] lines.[1]
let actualExpected (lines: string[]) = (actual lines, lines.[2..])
let print steps = steps |> Seq.iter (printfn "%s")

// **************************************
// Runs
// **************************************
//``2-Break Sorting Problem`` "(+1 +2 +3 +4 +5 +6)" "(+1 -3 -6 -5)(+2 -4)"

Problems.sample id actualExpected
//Problems.extra id actualExpected
//Problems.dataset id actual  |> print
//Problems.rosalind id actual |> print