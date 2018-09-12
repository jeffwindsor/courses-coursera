System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "SequenceAlignments.fs"
open Bioinformatics
open SequenceAlignments

//  BA5H Find a Highest-Scoring Fitting Alignment of Two Strings
//    Say that we wish to compare the approximately 20,000 amino acid-long NRP synthetase from Bacillus brevis with the approximately 600 amino acid-long A-domain from Streptomyces roseosporus, the bacterium that produces the powerful antibiotic Daptomycin. We hope to find a region within the longer protein sequence v that has high similarity with all of the shorter sequence w. Global alignment will not work because it tries to align all of v to all of w; local alignment will not work because it tries to align substrings of both v and w. Thus, we have a distinct alignment application called the Fitting Alignment Problem.
//    “Fitting” w to v requires finding a substring v′ of v that maximizes the global alignment score between v′ and w among all substrings of v.
//    Fitting Alignment Problem: Construct a highest-scoring fitting alignment between two strings.
//    Given: Two DNA strings v and w, where v has length at most 10000 and w has length at most 1000.
//    Return: The maximum score of a fitting alignment of v and w, followed by a fitting alignment achieving this 
//      maximum score. Use the simple scoring method in which matches count +1 and both the mismatch and indel penalties 
//      are equal to 1. (If multiple fitting alignments achieving the maximum score exist, you may return any one.)
let id = "BA5H"
let ``Find a Highest-Scoring Fitting Alignment of Two Strings`` s1 s2 =
    let sm = Scoring.SmithWaterman (fun _ _ -> 1) (fun _ _ -> -1) -1
    
    build sm s1 s2
    |> (fun g -> (if g.Terminus.X < 25 && g.Terminus.Y < 25 then print g); g;)
    |> Paths.fittings

 
// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =
    ``Find a Highest-Scoring Fitting Alignment of Two Strings`` lines.[0] lines.[1]
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