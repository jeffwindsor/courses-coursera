namespace Bioinformatics

/// NxN matrix of distances
module Matrices =
    type Matrix<'a> = 'a[,]
    type Segment = int * int
    type WeightedSegment<'a> = Segment * 'a

    let SegmentsById ids : Segment List =
        ids |> List.collect (fun a -> 
            ids |> List.map (fun b -> (a,b)))
    let Segments n : Segment List = SegmentsById [0 .. n - 1]

    let L1LessThanL2 ss = ss |> List.filter (fun (a,b) -> a < b)
    let Contains indices a = indices |> Set.contains a 
    let ContainsEither indices a b = (Contains indices a || Contains indices b)
    let ContainsBoth indices a b = (Contains indices a && Contains indices b)
    let WithoutIndex index = List.filter (fun (a,b) -> a <> index && b <> index)
    let WithoutIndices indices = List.filter (fun (a,b) -> ContainsEither indices a b |> not)
    
    let UpdateValue (m:Matrix<'a>) i j value =
        m.[i,j] <- value
        m.[j,i] <- value

    let DistinctVertices<'a>(wss:'a WeightedSegment list) =
        wss 
        |> List.collect (fun ((a,b),_) -> [a;b;])
        |> List.distinct

    let MatrixWeighted (m:Matrix<'a>) =
        List.map (fun ((a,b) as p) -> (p,m.[a,b]))

    let inline TotalsBy n valueAt =
        let ids = [| 0..(n - 1) |]
        ids |> Array.map (fun a -> 
            ids |> Array.map (fun b -> valueAt a b) |> Array.sum)

    let inline MatrixTotals n (m:Matrix<'a>) =
        TotalsBy n (fun a b -> m.[a,b])
