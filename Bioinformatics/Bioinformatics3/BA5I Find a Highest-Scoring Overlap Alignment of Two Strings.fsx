System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "SequenceAlignments.fs"
open Bioinformatics
open SequenceAlignments

//  BA5I Find a Highest-Scoring Overlap Alignment of Two Strings
//    When we assembled genomes, we discussed how to use overlapping reads to assemble a genome, a problem that was complicated by errors in reads. We would like to find overlaps between error-prone reads as well.
//    An overlap alignment of strings v = v1 ... vn and w = w1 ... wm is a global alignment of a suffix of v with a prefix of w. An optimal overlap alignment of strings v and w maximizes the global alignment score between an i-suffix of v and a j-prefix of w (i.e., between vi ... vn and w1 ... wj) among all i and j.
//    Overlap Alignment Problem: Construct a highest-scoring overlap alignment between two strings.
//    Given: Two protein strings s and t, each of length at most 1000.
//    Return: The score of an optimal overlap alignment of v and w, followed by an alignment of a suffix v’ of v and a prefix w’ of w achieving this maximum score. Use an alignment score in which matches count +1 and both the mismatch and indel penalties are 2. (If multiple overlap alignments achieving the maximum score exist, you may return any one.)
let id = "BA5I"
let ``Find a Highest-Scoring Overlap Alignment of Two Strings`` s1 s2 =
    let sm = Scoring.SmithWaterman (fun _ _ -> 1) (fun _ _ -> -2) -2
    
    build sm s1 s2
    |> (fun g -> (if g.Terminus.X < 25 && g.Terminus.Y < 25 then print g); g;)
    |> Paths.overlaps

 
// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =
    ``Find a Highest-Scoring Overlap Alignment of Two Strings`` lines.[0] lines.[1]
    //|> List.map (fun a -> printfn "%A" a; a;)
    |> Seq.map (fun (s,a1,a2) -> [| string s; String.concat "" a1; String.concat "" a2; |])
    
let actualExpected (lines: string[]) = (actual lines, [| lines.[2];lines.[3];lines.[4]; |])
let print = Seq.iter (Array.iter (printfn "%s"))
// **************************************
// Runs
// **************************************
Problems.sampleAny id actualExpected
Problems.extraAny id actualExpected
Problems.dataset id actual  |> print
Problems.rosalind id actual |> print
