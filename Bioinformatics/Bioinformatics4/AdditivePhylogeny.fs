namespace Bioinformatics

open Matrices
module AdditivePhylogeny =
    type Vertice = int
    type Weight = int
    type Edge = Vertice * Weight
    let edgeVertice = fst
    let edgeWeight = snd
    type Node = Vertice * Edge list  // Adjacency style
    type Graph = Node list
    type Path = Edge list
    type InsertLocation = Edge * Edge * Edge
    type InsertType = NotFound of Vertice * int | Split of Edge * Edge | Rename of Vertice
    
    let paths (g:Graph) start finish : Path list = 
        let map = g |> Map.ofList
        let rec loop (route:Edge list) visited = [
            let current = route |> List.head |> edgeVertice
            if current = finish then
                yield List.rev route
            else
                for next in map.[current] do
                    if visited |> Set.contains (edgeVertice next) |> not then
                        yield! loop (next::route) (visited |> Set.add (edgeVertice next) ) 
        ]
        loop [(start,0)] (start |> Set.singleton)

    let shortestPath (g:Graph) start finish =
        paths g start finish
        |> List.minBy List.length

    let twoNodeGraph a b weight : Graph = [ (a,[(b,weight)]); (b,[(a,weight)]) ]
    let toAdjacencies (g:Graph)  = 
        List.collect (fun (v,es) -> es |> List.map (fun (v',w) -> v, v', w)) g
    
    //Assumes distances are a nxn metric space
    let findInsertLocation (distances:Weight[,]) (leaf:Vertice) : InsertLocation =
        //printf "findInsertLocation for leaf:%i" leaf
        //Limit to Calculable coordinates
        //  For (i,k) where i < k because the value for (i,k) equals the value of (k,i) and (i,i) always equals zero
        //  For (i,k) where i or k <> j because distance calculation is only valid for non j points
        let leafDistance (i,k) : Weight = max 0 ((distances.[i,leaf] + distances.[leaf,k] - distances.[i,k]) / 2)
        let edgeToLeafVertice i dj : Edge = (i, distances.[i,leaf] - dj)    
        
        leaf |> Segments |> L1LessThanL2 |> WithoutIndex leaf
        |> List.map (fun c -> leafDistance c, c)
        |> List.minBy fst
        |> (fun (dj, (i,k)) -> edgeToLeafVertice i dj, (leaf,dj), edgeToLeafVertice k dj)
        //|> (fun a -> printfn " -> %A" a; a)

    let rename (g : Graph) a b =
        List.map (fun (i,ies) -> 
            match i with
            | i when i = a -> b, ies
            | _ -> i, ies |> List.map (fun (i,w) -> if i=a then (b,w) else (i,w))
        ) g

    let split n (g : Graph) (a,wa) (b,wb) (c,wc) : Graph =
        let v = max n (g |> List.maxBy fst |> fst |> (+) 1)
        let replace (es:Edge list) a b w :Edge list = 
            List.map (fun (i,iw) -> if i = a then (b,w) else (i,iw)) es
        (c, [(v,wc)])
        ::(v, [(c,wc);(a,wa);(b,wb)])
        ::List.map (fun (i,ies) -> 
                match i with
                | i when i = a -> i, replace ies b v wa
                | i when i = b -> i, replace ies a v wb
                | _ -> (i,ies)
            ) g 

    let insert n (g:Graph) (((start,ws),(leaf,wl),(finish,_)):InsertLocation) : Graph =
        shortestPath g start finish 
        //|> (fun path -> printfn "%A" path; path;)
        |> List.fold
            (fun acc (right, inbetween) -> 
                match acc with
                | NotFound (left, remaining) -> 
                    match remaining - inbetween with
                    | r when r < 0 -> Split ((left, remaining), (right, inbetween - remaining))
                    | r when r = 0 -> Rename right 
                    | r -> NotFound (right, r) //keep going
                | f -> f) 
            (NotFound (0, ws))
        |> function 
            | Rename a -> rename g a leaf
            | Split (a, b) -> split n g a b (leaf,wl)
            | NotFound (right,r) -> failwith (sprintf "Insert Action Not Found %i %i" right r)
        
    let createGraph n (distances: int[,]) =
        let findLocation = findInsertLocation distances
        match n with
        | n when n > 1 -> 
            [2 .. n - 1]
            |> Seq.fold (fun g leaf -> 
                //printfn "Fold G: %A" g; 
                leaf |> findLocation |> insert n g) (twoNodeGraph 0 1 distances.[0,1])
        | _ -> failwith "distance matrix to small"
