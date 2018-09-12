System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "WeightedAdjacencyGraph.fs"
open Bioinformatics.WeightedAdjacencyGraph

let ``Given No Valid Paths an empty list is returned`` =
    (paths 2 6 [(1,[(2,1);(3,1)]);(2,[(3,1)]);(3,[(4,1)]);(4,[(2,1)]);(5,[(6,1)]);(6,[(5,1)])]) = []
let ``Given Multiple Paths a list of lists is returned`` = 
    (paths 1 4 [(1,[(2,1);(3,2)]);(2,[(3,1)]);(3,[(4,2)]);(4,[(2,1)]);(5,[(6,1)]);(6,[(5,1)])]) = [[(1, 0); (2, 1); (3, 1); (4, 2)]; [(1, 0); (3, 2); (4, 2)]]