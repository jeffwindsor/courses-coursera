namespace Bioinformatics

module DeBruijn =
    type Edge<'a> =  {Label:'a; SourceId:'a; TargetId:'a;}
    type Node<'a> =  {Label:'a; InEdges: 'a list; OutEdges: 'a list;}
        
    let toNodes edges =
        let inboundEdges = edges |> Seq.groupBy (fun ie -> ie.TargetId) |> Dict.ofSeq
        let toInboundEdgeList = List.map (fun e -> e.SourceId)
        let toOutboundEdgeList = List.map (fun e -> e.TargetId)
        //Seperate definition to force the execution of list one before list two
        let one = 
            edges |> Seq.groupBy (fun ie -> ie.SourceId) 
            |> Seq.map (fun (key,items) -> 
                    {
                        Label = key
                        OutEdges = items |> List.ofSeq |> toOutboundEdgeList
                        InEdges  = 
                            if inboundEdges.ContainsKey(key) 
                            then
                                let result = inboundEdges.[key] 
                                inboundEdges.Remove(key) |> ignore
                                result
                            else Seq.empty<Edge<'a>>
                            |> List.ofSeq |> toInboundEdgeList
                    })
            |> Seq.toList
        let two =
            inboundEdges |> Dict.toSeq 
            |> Seq.map (fun (key,items) -> 
                        {
                            Label = key
                            InEdges = items |> List.ofSeq |> toInboundEdgeList
                            OutEdges= []
                        })
            |> Seq.toList

        Seq.append one two

    let toEulerianPath nodes =
        //http://www.graph-magics.com/articles/euler.php
        let dict = nodes |> Seq.map (fun n -> (n.Label,(n.InEdges,n.OutEdges))) |> Dict.ofSeq
        let find nodeLabel = if dict.ContainsKey(nodeLabel) then dict.[nodeLabel] else ([],[])

        let rec inner stack path nodeLabel =
            let (ins,outs) = find nodeLabel
            match outs |> List.isEmpty, stack |> List.isEmpty with
            // No out edges and no stack -> done return current node and path
            | true, true  -> 
                //printfn "No Edges, No Stack on %s. Return cycle %A" node cycle
                nodeLabel::path
            // No out edges -> rec with node added to path and top stack as current node 
            | true, false -> 
                //printfn "No Edges on %s add it to cycle. Rec New Node From Stack %s. [New Stack:%A. New Cycle:%A]" node (List.head stack) (List.tail stack) (node::cycle)
                inner (stack |> List.tail) (nodeLabel::path) (stack |> List.head)
            // else -> Remove out edge from node add node to stack, use out edge node as current
            | _ , _    -> 
                let next = outs |> List.head
                dict.[nodeLabel] <- (ins, outs |> List.tail)
                //printfn "Node %s Remove Edge %s. Add Node to Stack:%A" node node' (node::stack)
                inner (nodeLabel::stack) path next

        //Todo: this is only one way balance, two negative not checked
        let outboundUnBalanced =
            dict |> Dict.toSeq 
            |> Seq.choose (fun(key,(ins,outs)) -> 
                match (outs |> List.length) - (ins |> List.length) with 
                | n when n > 0 -> Some (key, n)
                | _ -> None
                )
            |> Seq.toArray

        match Array.length outboundUnBalanced with
        | 0 -> inner [] [] (dict.Keys |> Seq.head)
        | 1 -> inner [] [] (outboundUnBalanced |> Seq.pick (fun (k,n) -> if n > 0 then Some k else None))
        | _ -> []

    //Sort key and the seq of isolated edge nodes
    module Kmers =
        let private prefix k (kmer:string) = kmer.[..(k-2)]
        let private suffix (kmer:string)   = kmer.[1..]
        let toEdge k (kmer:string) = 
            {
                SourceId = prefix k kmer
                Label  = kmer
                TargetId= suffix kmer
            }
        let toPairEdge k (inbound:string) (outbound:string) =
            {
                SourceId  = (prefix k inbound, prefix k outbound)
                Label   = (inbound, outbound)
                TargetId = (suffix inbound, suffix outbound)
            }




