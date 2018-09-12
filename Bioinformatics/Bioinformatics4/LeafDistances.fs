namespace Bioinformatics

open System.Collections.Generic
module LeafDistances =
    type 'a Edge =  'a * 'a * int
    type 'a Adjacency = 'a * (('a * int) list)
 
    let toAdjacencyList (edges: 'a Edge seq) = 
        let source (a,b,w) = a
        let weightedTarget (a,b,w) = (b,w)
        edges 
        |> Seq.groupBy source
        |> Seq.map (fun (k,es) -> (k, Seq.map weightedTarget es |> Seq.toList))
        |> List.ofSeq

    let search<'a when 'a:comparison> g start =
        let costs = new Dictionary<'a, int>() // node cost
        let rec search' queue = 
            match queue with
            | [] -> ()
            | (node,_) :: queueTail -> 
                // Look to current adjacencies and queue up new items with priority
                match Map.tryFind node g with
                | None -> ()
                | Some adjs -> 
                    let adjQueue = adjs |> List.choose (fun (target, edgeCost) -> 
                            let targetCost = costs.[node] + edgeCost
                            match (costs.ContainsKey(target) = false) || targetCost < costs.[target] with
                            | false -> None
                            | true  -> 
                                costs.[target] <- targetCost
                                Some (target, targetCost)
                            )
                    let queue' = (queueTail@adjQueue) |> List.sortByDescending snd
                    search' (adjQueue |> List.append queueTail |> List.sortByDescending snd)
        costs.[start] <- 0    // Start cost of zero
        search' [(start,0)]   // Search from Start
        costs |> Seq.map (|KeyValue|) //Return node * total cost from start

    let LeafDistance (edges: string Edge seq) =
        let adjacencies = edges |> toAdjacencyList
        let leaves = adjacencies |> Seq.filter (fun (k,vs) -> Seq.length vs = 1) |> Seq.map (fun (k,_) -> k) |> Seq.toArray
        let g = adjacencies |> Map.ofSeq
        //Create Grid - do not calculate anything twice : reversed or diagnols
        let n = Array.length leaves
        let values = Array2D.create n n 0

        [0 .. n - 2]
        |> Seq.iter (fun x ->
            let lpath = leaves.[x] |> search g |> Map.ofSeq
            [0 .. n - 1]
            |> Seq.iter (fun y -> 
                let v = if x = y 
                        then 0 
                        else lpath.[leaves.[y]]
                values.[x,y] <- v
                values.[y,x] <- v)
            )
        // Print to format
        [0 .. n - 1]
        |> Seq.map (fun x ->
            [0 .. n - 1] 
            |> Seq.map (fun y -> string values.[x,y])
            |> String.concat " "
            )
        |> Seq.toArray

