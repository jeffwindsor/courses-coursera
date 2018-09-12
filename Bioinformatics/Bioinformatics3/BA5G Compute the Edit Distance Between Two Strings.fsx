System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "SequenceAlignments.fs"
open Bioinformatics
open SequenceAlignments

//  BA5G Compute the Edit Distance Between Two Strings
//    In 1966, Vladimir Levenshtein introduced the notion of the edit distance between two strings 
//    as the minimum number of edit operations needed to transform one string into another. Here, an edit 
//    operation is the insertion, deletion, or substitution of a single symbol. For example, TGCATAT can 
//    be transformed into ATCCGAT with five edit operations, implying that the edit distance between these 
//    strings is at most 5.

//    Edit Distance Problem: Find the edit distance between two strings.
//    Given: Two amino acid strings.
//    Return: The edit distance between these strings.
let id = "BA5G"
let ``Compute the Edit Distance Between Two Strings`` sigma s1 s2 =
    build Scoring.EditDistance s1 s2
    //|> (fun (g,m) -> print g; g;)
    |> Paths.``global``

 
// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =
    let s, _, _ = ``Compute the Edit Distance Between Two Strings`` 5 lines.[0] lines.[1]
    string s
let actualExpected (lines: string[]) = (actual lines, lines.[2])
let print = (printfn "%s")
// **************************************
// Runs
// **************************************
Problems.sample id actualExpected
Problems.extra id actualExpected
Problems.dataset id actual  |> print
Problems.rosalind id actual |> print