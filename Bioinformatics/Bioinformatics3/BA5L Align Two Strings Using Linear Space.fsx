System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "MatrixFiles.fs"
#load "LinearSpaceAlignments.fs"
open Bioinformatics

//  BA5L Align Two Strings Using Linear Space
//    Global Alignment in Linear Space Problem
//    Find the highest-scoring alignment between two strings using a scoring matrix in linear space.
//    Given: Two long amino acid strings (of length approximately 10,000).
//    Return: The maximum alignment score of these strings, followed by an alignment achieving this maximum score. Use the BLOSUM62 scoring matrix and indel penalty σ = 5.
//
//    Hirschberg's algorithm : https://en.wikipedia.org/wiki/Hirschberg%27s_algorithm
//
open LinearSpaceAlignments
let id = "BA5L"
let ``Align Two Strings Using Linear Space`` mu sigma s1 s2 =
    let score = needlemanWunschScore mu sigma sigma (s1|>s2sa) (s2|>s2sa)
    let a1,a2 = Hirschberg mu sigma sigma s1 s2
    (Array.last score), a1, a2
     
// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =
    let mu = MatrixFiles.fileToScoringFunction "BLOSUM62"
    let sigma = -5
    let score, a1, a2  = ``Align Two Strings Using Linear Space`` mu sigma lines.[0] lines.[1]
    [| string score; a1; a2; |]

let actualExpected (lines: string[]) = (actual lines, [|lines.[2];lines.[3];lines.[4]|])
let print xs = xs |> Seq.iter (printfn "%s")
// **************************************
// Runs
// **************************************
//needlemanWunschScore (fun x y -> if x=y then 2 else -1) -2 -2 ("AGTACGCA"|> s2sa) ("TATGC"|> s2sa)
//hirschbergMidPoint (fun x y -> if x=y then 2 else -1) -2 -2 ("AGTACGCA"|> s2sa) (String.length "AGTACGCA") ("TATGC"|> s2sa)
//Hirschberg (fun x y -> if x=y then 2 else -1) -2 -2 "AGTACGCA" "TATGC"
//``Align Two Strings Using Linear Space`` "BLOSUM62" -5 "N" "LY"
//hirschbergMidPoint (MatrixFiles.fileToScoringFunction "BLOSUM62") -5 -5 ("N"|> s2sa) (String.length "N") ("LY"|> s2sa)

Problems.sample id actualExpected
Problems.extra id actualExpected
Problems.dataset id actual  |> print
Problems.rosalind id actual |> print
