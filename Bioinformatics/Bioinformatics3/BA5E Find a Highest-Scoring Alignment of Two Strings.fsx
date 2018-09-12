System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "SequenceAlignments.fs"
#load "MatrixFiles.fs"
open Bioinformatics
open SequenceAlignments

//  BA5E Find a Highest-Scoring Alignment of Two Strings
//    Global Alignment Problem: Find the highest-scoring alignment between two strings using a scoring matrix.
//    Given: Two amino acid strings.
//    Return: The maxisigmam alignment score of these strings followed by an alignment achieving this maxisigmam score. 
//          Use the BLOSUM62 scoring matrix and indel penalty σ = 5. 
//          (If sigmaltiple alignments achieving the maxisigmam score exist, you may return any one.)

//  Algorithm : Needleman-Wunsch
//TODO:      Allows larger sequences to be globally aligned. 
//      https://en.wikipedia.org/wiki/Needleman–Wunsch_algorithm
//      http://www.avatar.se/molbioinfo2001/dynprog/dynamic.html
let id = "BA5E"

let ``Find a Highest-Scoring Alignment of Two Strings`` sigma s1 s2 =
    let sf = MatrixFiles.fileToScoringFunction "BLOSUM62"
    let sm = Scoring.NeedlemanWunsch sf sf sigma  //match and mismatch use same matrix
    
    build sm s1 s2
    //|> (fun g -> print g; g;)
    |> Paths.``global``
  
// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =
    let s, s1, s2 = ``Find a Highest-Scoring Alignment of Two Strings`` -5 lines.[0] lines.[1]
    [| string s; s1; s2;|]
let actualExpected (lines: string[]) = (actual lines, lines.[2..])
let print = Seq.iter (printfn "%s")
// **************************************
// Runs
// **************************************
Problems.sample id actualExpected
Problems.extra id actualExpected
Problems.dataset id actual  |> print
Problems.rosalind id actual |> print

//``Find a Highest-Scoring Alignment of Two Strings`` "ILYPRQSMICMSFCFWDM" "ILIPRQQMGCFPFPWHFDFCF" 
//``Find a Highest-Scoring Alignment of Two Strings`` "ILYPRQSMICMSFCFWDMWKKDVP" "ILIPRQQMGCFPFPWHFDFCFWSAHHS" 
//ILYPRQSMICMSFCF-WDM--WKKDVP
//ILIPRQQMGCFPFPWHFDFCFWSAHHS