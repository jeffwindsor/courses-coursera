namespace Bioinformatics

module AdjacenyGraph =
    type Node<'key, 'payload> = // Adjacency style
        {
            Id: 'key
            Edges: 'key list
            Payload: 'payload
        }  
    type Graph<'key, 'payload> = Node<'key, 'payload> list

    let FindNode (g:Graph<'key, 'payload>) v = 
        g |> List.find (fun n -> n.Id = v)

    let private connectNodeToId connectNodeToIdPayload id node =
        {
            Id = node.Id
            Edges   = id::node.Edges
            Payload  = connectNodeToIdPayload id node
        }

    let ConnectNodes connectNodeToIdPayload aid bid (g:Graph<'key, 'payload>) =
        let f = connectNodeToId connectNodeToIdPayload
        g |> List.map(fun node ->
                match node.Id with
                | v when v = aid -> node |> f bid
                | v when v = bid -> node |> f aid
                | _ -> node
            )

    let ConnectNodesOneWay connectNodeToIdPayload aid bid (g:Graph<'key, 'payload>) =
        let f = connectNodeToId connectNodeToIdPayload
        g |> List.map(fun node ->
                match node.Id with
                | v when v = aid -> node |> f bid
                | _ -> node
            )

    let ConnectNodesViaNewInnerNode connectNodeToIdPayload aid bid iid (g:Graph<'key, 'payload>) =
        let f = connectNodeToId connectNodeToIdPayload
        g |> List.map(fun node ->
                match node.Id with
                | v when v = aid -> node |> f iid
                | v when v = bid -> node |> f iid
                | v when v = iid -> node |> f aid |> f bid
                | _ -> node
            )

    let InitializeNode initializeNodePayload id = 
        { 
            Id = id
            Edges   = []
            Payload = initializeNodePayload id
        }

    let InitializeGraph initializeNodePayload size =
        [0..(size - 1)] |> List.map (InitializeNode initializeNodePayload)

        