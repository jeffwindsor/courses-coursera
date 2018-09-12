namespace Bioinformatics

module CircularChromosomes = 
    type Block = Block of int
    let value (Block v) = v
    let blockToString (Block a) = sprintf "%+i" a
    
    type Chromosome = Chromosome of Block array
    let blocks (Chromosome bs) = bs
    let blockCount (Chromosome bs) = Array.length bs
    let chromosomeToString (Chromosome bs) = Seq.map blockToString bs |> String.concat " " |> sprintf "(%s)"

    type Genome = Genome of Chromosome array
    let chromosomes (Genome cs) = cs
    let chromosomeCount (Genome cs) = Array.length cs
    let genomeBlockCount (Genome cs) = cs |> Array.map blockCount |> Array.sum
    let genomeToString (Genome cs) = cs |> Seq.map chromosomeToString |> String.concat ""
    let genomeFromInput (s:string) =    // Example input :  "(+1 -3 -6 -5)(+2 -4)"
        s.Split([|")(";"(";")"|], System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (Files.Lines.splitMap (int >> Block) >> Chromosome)
        |> Genome.Genome

    module Cycles =
        type Vertex = Vertex of int

        type Edge = Vertex * Vertex
        let private source ((a,_):Edge) = a
        let private target ((_,b):Edge) = b

        /// black edge represents a block in chromosome
        /// direction of edge determined by sign on block ( + leads to --> and - leads to <-- )
        type BlackEdge = Black of Edge
        type BlackEdgeCycle = BlackCycle of int * BlackEdge list

        /// colored edge is gap between black edges
        /// circular dna so last colored edge will be between last and first
        type Color = Red | Blue
        type ColoredEdge =  Colored of Edge * Color
        let coloredEdgePoint (Colored ((Vertex i, Vertex j), _)) = (i,j)
        let coloredEdgeColor (Colored (_,c)) = c

        type ColoredEdgeCycle = ColoredCycle of int * ColoredEdge list
        let coloredEdgeCycleEdgesByColor color (ColoredCycle (_,es)) = es |> List.filter (fun e -> color = coloredEdgeColor e) 

        type Adjacency = {Red: int; Blue: int}
        type AdjacencyList = AdjacencyList of Adjacency array
        type ConnectedList = ConnectedList of Set<int> list
        
        let private chromosomeToBlackCycle chromosome =
            let blockToBlackEdge b =
                let tail i = Vertex ((abs i*2) - 1)
                let arrow i = Vertex (abs i*2)
                let blockToEdge (Block i) = if i > 0 then tail i, arrow i else arrow i, tail i
                b |> blockToEdge |> Black
            let edges = chromosome |> blocks |> Seq.map blockToBlackEdge |> Seq.toList
            let maxVertexValue = edges |> Seq.fold (fun acc (Black (Vertex a, Vertex b))-> max a b |> max acc) 0
            BlackCycle (maxVertexValue,edges)
        
        let private blackCycleToColoredCycle color (BlackCycle (maxNode, blackEdges)) =
            let directed =
                Seq.append blackEdges [Seq.head blackEdges]
                |> Seq.pairwise 
                |> Seq.map (fun (Black edge, Black edge') -> Colored ((target edge, source edge'), color))
            ColoredCycle (maxNode, directed |> Seq.toList)

        let mergeColoredCycles ccs =
            ccs 
            |> Seq.toList 
            |> List.map (fun (ColoredCycle (n,es)) -> (n,es))
            |> List.unzip
            |> fun (ns, ess) -> ColoredCycle (List.max ns, List.collect (fun es -> es) ess)

        let genomeToColoredCycles color = 
            chromosomes 
            >> Seq.map chromosomeToBlackCycle 
            >> Seq.map (blackCycleToColoredCycle color)

        let coloredCycleToAdjacencyList (ColoredCycle (maxV, edges)) =
            let a = Array.create (maxV+1) {Red=0; Blue=0;}
            let set i j c = 
                a.[i] <-    match c, a.[i] with
                            | Red, {Red=_;Blue=b} -> {Red=j;Blue=b}
                            | Blue, {Red=r;Blue=_} -> {Red=r;Blue=j}

            edges
            |> Seq.iter (fun (Colored ((Vertex i, Vertex j), color)) -> set i j color; set j i color;)
            AdjacencyList a

        let coloredCycleToConnectedComponents (ColoredCycle (maxV, edges)) =
            let nodeCycleIds = Array.init (maxV + 1) (fun i -> i)
            let update i j = 
                match nodeCycleIds.[i], nodeCycleIds.[j] with
                | vi, vj when vi > vj -> nodeCycleIds.[j] <- vi; true;
                | _,_ -> false
            let updateBiDirectional (i,j) = update i j || update j i
            let updatePoints points = points |> List.exists updateBiDirectional

            let points = edges |> List.map coloredEdgePoint 
            while updatePoints points do ()

            nodeCycleIds
            //|> (fun a -> printfn "Node Cycles"; Seq.iteri (fun i c -> printfn "%i -> %i" i c) a; printfn ""; a;)
            |> Seq.mapi (fun i cid -> (i,cid))
            |> Seq.groupBy snd
            |> Seq.filter (fun (cid,_) -> cid > 0)
            |> Seq.map (fun (_,xs) -> xs |> Seq.map (fun (x,_) -> x) |> Set.ofSeq)  //CYCLE TYPE?
            |> Seq.toList
            |> ConnectedList

        let mergeGenomesToColoredCycles genomeP genomeQ =
            Seq.append (genomeToColoredCycles Red genomeP) (genomeToColoredCycles Blue genomeQ)
            |> mergeColoredCycles
            
        let coloredEdgesToBlackEdges coloredEdges =
            Seq.append [Seq.last coloredEdges] coloredEdges
                |> Seq.pairwise 
                |> Seq.map (fun (Colored (edge,_), Colored (edge',_)) -> Black (target edge, source edge'))

        let blackEdgesToChromosome edges =
            let blackEdgeToBlock = function 
                | Black (Vertex a, Vertex b) when a < b -> Block (b/2)
                | Black (Vertex a,_) -> Block -(a/2)
            printf "Blk Edges: "; edges |> Seq.iter (fun (Black (Vertex a, Vertex b)) -> printf "(%i,%i)" a b); printfn "";
            edges |> Seq.map blackEdgeToBlock |> Seq.toArray |> Chromosome




// let vertexToString (Vertex a) = sprintf "%i" a
// let blackEdgeToString (Black (a,b)) = sprintf "%s %s" (vertexToString a) (vertexToString b)
// let coloredEdgeToString (Colored ((a,b),_)) = sprintf "(%s,%s)" (vertexToString a) (vertexToString b)        
//
//let coloredEdgesToAdjacencyList size edges =
//    let a = Array.create (size+1) []
//    let set i j = a.[i] <- j::a.[i]
//    edges
//    |> Seq.map (fun (Colored (Vertex i, Vertex j)) -> (i,j))
//    |> Seq.iter (fun (i,j) -> set i j; set j i;)
//    AdjacencyList a

//        type AdjacencyMap = Map<Vertex, Set<Vertex>>
//        type CycleGraph = { BlockCount:int; Vertices:Vertex[]; Graph:AdjacencyMap; }
        
//        let fromGenomes gp gq =
//            let ces  = Array.append (genomeToColoredEdges gp) (genomeToColoredEdges gq)
//            let vertices = ces |> Array.collect (fun e -> [|source e; target e|]) |> Array.distinct
//            let map = ces |> toAdjacencyMap
//            let visited = Set.empty<Vertex>
//
//            { 
//                BlockCount = genomeBlockCount gp + genomeBlockCount gq;
//                Vertices = vertices;
//                Graph = map; 
//            }
//
//        let cycleCount g =
//            //UNDONE
////            let visited = Set.empty 
////            let keys = Map. g.Graph 
//            0

//    /// cycles 1,2 -> +1 node, 4,3 -> -2 node.  Reverse of Chromosome.toCycle
//    let nodesToChromosome (c: int seq) =
//        seq {   use iter = c.GetEnumerator() 
//                while iter.MoveNext() do
//                let a = iter.Current
//                if iter.MoveNext() then
//                    let b = iter.Current
//                    if a < b then yield b / 2 else yield -a / 2
//                }
     