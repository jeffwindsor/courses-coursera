System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)

#load "Bioinformatics.fs"
#load "Support.fs"
open Bioinformatics


let skew = function
    | 'C' -> -1
    | 'G' ->  1
    | _   ->  0

let skews nucs = 
    nucs |> Seq.scan (fun s n -> s + skew n) 0

let skewsi nucs =
    skews nucs |> Seq.mapi (fun i s -> (i,s))


//let minimumSkewLengths nucs =
//    nucs
//    |> Seq.fold (fun (index, s', minimum, minIndexes) nuc ->
//            match skew nuc with
//            | s when s < minimum -> (index+1, s, s, [index])
//            | s when s = minimum -> (index+1, s, minimum, index::minIndexes)
//            | s                  -> (index+1, s, minimum, minIndexes)
//            
//            
//            ) (1, 0, 0, [0])
//    |> fun (_,_,_,indexes) -> indexes
//    |> Seq.rev
//
//let genome = "Skew" |> Files.firstLine
//
//// 2.3 0 Step6 - Minimum Skew
//genome |> minimumSkewLengths |> Seq.printsn
//
//// 2.3 0 Step4 - Skew
//genome |> skews |> Seq.iter (printf "%i ")
//

