System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
open Bioinformatics

//  BA5C Find a Longest Common Subsequence of Two Strings 
//    Longest Common Subsequence Problem
//    Given: Two strings.
//    Return: A longest common subsequence of these strings.
//  LINK: https://stepic.org/lesson/Backtracking-in-the-Alignment-Graph-245/step/5?course=Comparing-Genes-Proteins-and-Genomes-(Bioinformatics-III)&unit=2172
module SequenceAlignments =
    type Directions = None | Right | Down | RightDown 
    let private score (_,vLeftUp) (_,vLeft) (_,vUp) isMatch =
        match isMatch with
        | true -> (RightDown, vLeftUp + 1)
        | false when vLeft > vUp 
                -> (Right, vLeft)
        | _    -> (Down, vUp)
    let compareArrays one two =
        let g = Array2D.create (Array.length one + 1) (Array.length two + 1) (None,0)
        //populate - order is important otherwise would have used initialize
        [1 .. (Array2D.length1 g) - 1] |> Seq.iter (fun i ->
            [1 .. (Array2D.length2 g) - 1] |> Seq.iter (fun j -> 
                g.[i,j] <- score g.[i-1,j-1] g.[i,j-1] g.[i-1,j] (one.[i-1] = two.[j-1])
              )
            )
        g

let problemNumber = "BA5C"
open SequenceAlignments
let ``Find a Longest Common Subsequence of Two Strings`` one two =
    let dg = compareArrays one two
    let rec backtrack i j =
        match dg.[i,j] with
        | (None,_)      -> []
        | (Down,_)      -> backtrack (i-1) j
        | (Right,_)     -> backtrack i (j-1)
        | (RightDown,_) -> one.[i-1] :: backtrack (i-1) (j-1)

    backtrack ((Array2D.length1 dg) - 1) ((Array2D.length2 dg) - 1)
    |> Seq.rev
    |> Seq.map string
    |> String.concat ""

//``Find a Longest Common Subsequence of Two Strings`` "GCGATC" "CTGACG"

let analyse (lines: string[]) =
    let one = lines.[0] |> Files.Lines.toCharArray
    let two = lines.[1] |> Files.Lines.toCharArray
    ``Find a Longest Common Subsequence of Two Strings`` one two

let test (lines: string[]) =
    let expected = lines |> Array.last
    (analyse lines, expected)

Problems.sample problemNumber test
Problems.extra problemNumber test
Problems.dataset problemNumber analyse
Problems.rosalind problemNumber analyse