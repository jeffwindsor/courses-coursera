System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "SequenceAlignments.fs"
#load "MatrixFiles.fs"
open Bioinformatics
open SequenceAlignments

//  BA5F Find a Highest-Scoring Local Alignment of Two Strings
//    Local Alignment Problem: Find the highest-scoring local alignment between two strings.
//    Given: Two amino acid strings.
//    Return: The maximum score of a local alignment of the strings, followed by a local alignment of these 
//      strings achieving the maximum score. Use the PAM250 scoring matrix and indel penalty σ = 5. 
//      (If multiple local alignments achieving the maximum score exist, you may return any one.)

//Algorithm : Smith-Waterman (modified for speed enhancements)
//      to calculate the local alignment of two sequences.
//      https://en.wikipedia.org/wiki/Smith–Waterman_algorithm
//      http://vlab.amrita.edu/?sub=3&brch=274&sim=1433&cnt=1
let id = "BA5F"

let ``Find a Highest-Scoring Local Alignment of Two Strings`` sigma s1 s2 =
    let sf = MatrixFiles.fileToScoringFunction "PAM250"
    let sm = Scoring.SmithWaterman sf sf sigma
    
    build sm s1 s2 
    //|> (fun (g,m) -> print g; printfn "%A" m; (g,m);)
    |> Paths.locals
    
// **************************************
// INPUT - OUTPUT
// **************************************
let actuals (lines: string[]) =
    ``Find a Highest-Scoring Local Alignment of Two Strings`` -5 lines.[0] lines.[1]
    |> Seq.map (fun (s, a1, a2) -> [| string s; a1; a2;|])

let actualExpected (lines: string[]) = (actuals lines, lines.[2..])
let print = Seq.iteri (fun i xs -> printfn "Actual %i" i; xs |> Seq.iter (printfn "%s"))
// **************************************
// Runs
// **************************************
Problems.sampleAny id actualExpected
Problems.extraAny  id actualExpected
Problems.dataset id actuals  |> print
Problems.rosalind id actuals |> print

// Example @ http://vlab.amrita.edu/?sub=3&brch=274&sim=1433&cnt=1
//build (Score.smithWaterman (fun a b -> if a=b then 5 else -3) -4) "CGTGAATTCAT" "GACTTAC" 
//|> (fun (g,m) -> print g; printfn "%A" m; (g,m);)
//||> Find.localAlignments
