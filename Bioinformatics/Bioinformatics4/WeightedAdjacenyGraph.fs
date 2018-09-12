namespace Bioinformatics

module WeightedAdjacenyGraph =
    type Edge<'key, 'weight> = { Id: 'key; Weight: 'weight }
    type Node<'key, 'weight, 'payload> = // Adjacency style
        {
            Id: 'key
            Edges: Edge<'key, 'weight> list
            Payload: 'payload
        }  
    type Graph<'key, 'weight, 'payload> = Node<'key, 'weight, 'payload> list
    
    let FindNode (g:Graph<'key, 'weight, 'payload>) v = 
        g |> List.find (fun n -> n.Id = v)

    let private connectNodeToId connectNodeToIdPayload vc weight node =
        {
            Id = node.Id
            Edges   = {Id=vc; Weight=weight}::node.Edges
            Payload  = connectNodeToIdPayload vc weight node
        }

    let ConnectNodes connectNodeToIdPayload fWeightA fWeightB aid bid (g:Graph<'key, 'weight, 'payload>) =
        let f = connectNodeToId connectNodeToIdPayload
        g |> List.map(fun node ->
                match node.Id with
                | v when v = aid -> node |> f bid (fWeightB node)
                | v when v = bid -> node |> f aid (fWeightA node)
                | _ -> node
            )

    let ConnectNodesOneWay connectNodeToIdPayload connectNodeWeight aid bid (g:Graph<'key, 'weight, 'payload>) =
        let f = connectNodeToId connectNodeToIdPayload
        g |> List.map(fun node ->
                match node.Id with
                | v when v = aid -> node |> f bid (connectNodeWeight node)
                | _ -> node
            )

    let ConnectNodesViaNewInnerNode connectNodeToIdPayload fWeightAI fWeightBI aid bid iid (g:Graph<'key, 'weight, 'payload>) =
        let f = connectNodeToId connectNodeToIdPayload
        g |> List.map(fun node ->
                let wai = fWeightAI node
                let wbi = fWeightBI node
                match node.Id with
                | v when v = aid -> node |> f iid wai
                | v when v = bid -> node |> f iid wbi
                | v when v = iid -> node |> f aid wai |> f bid wbi
                | _ -> node
            )

    let InitializeNode initializePayload v = 
        { 
            Id = v
            Edges   = []
            Payload = initializePayload v
        }

    let InitializeGraph initializePayload size =
        [0..(size - 1)] |> List.map (InitializeNode initializePayload)