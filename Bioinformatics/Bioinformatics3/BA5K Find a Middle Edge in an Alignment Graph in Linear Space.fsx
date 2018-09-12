System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
#load "MatrixFiles.fs"
#load "LinearSpaceAlignments.fs"
open Bioinformatics
        
type Scores = int[]
type Names = string[]
let s2a = Files.Lines.toCharacterStringArray
let left (s:Names) mid = if mid = 0 then [||] else s.[.. mid - 1]
let right (s:Names) mid = if mid = 0 then s else s.[mid ..]

let score mu sigmaInsert sigmaDelete inames jnames =
    let il, jl = Array.length inames, Array.length jnames
    let arrayLength = il + 1
    let scoreColumn (previous:Scores, current:Scores) j =
        seq { 0 .. il }
        |> Seq.iter (function
                    | 0  -> current.[0]   <- previous.[0] + sigmaInsert  // initial values for header
                    | i  -> current.[i] <-
                            [
                                previous.[i] + sigmaInsert  //insert
                                current.[i-1] + sigmaDelete //delete
                                previous.[i-1] + mu inames.[i-1] jnames.[j-1] //alignment
                            ] |> List.max)
        current, previous //make current the previous and re-use previous for current (wil get overwritten)
    // fold score columns until last is reached, initial column has cumulative gap costs for insertions
    let c,p = 
        seq { 1 .. jl } 
        |> Seq.fold scoreColumn (Array.init arrayLength (fun i -> i * sigmaInsert), 
                                 Array.zeroCreate<int> arrayLength)
    p,c  //swap for return

//  BA5K Find a Middle Edge in an Alignment Graph in Linear Space
//    Middle Edge in Linear Space Problem: Find a middle edge in the alignment graph in linear space.
//    Given: Two amino acid strings.
//    Return: A middle edge in the alignment graph of these strings, where the optimal path is defined by the BLOSUM62 scoring matrix and a linear indel penalty equal to 5. Return the middle edge in the form “(i, j) (k, l)”, where (i, j) connects to (k, l).
//
//    Hirschberg's algorithm : https://en.wikipedia.org/wiki/Hirschberg%27s_algorithm
//
let id = "BA5K"
let ``Find a Middle Edge in an Alignment Graph in Linear Space`` mu sigma s1 s2 =
    let inames = s1 |> s2a
    let jnames = s2 |> s2a
    let jmid = (Array.length jnames) / 2
    // add one to jmid so we score one past the mid, allowing calculation of mid edge
    let midCol,nextCol = score mu sigma sigma inames (left jnames (jmid + 1))
    let imid = midCol
               |> Seq.mapi (fun i x -> i, x)
               |> Seq.maxBy snd
               |> fst
    //deterimine next col max score from mid point to determine edge
    printfn "right:%i diag:%i" nextCol.[imid] nextCol.[imid+1]
    if nextCol.[imid] > nextCol.[imid+1]
    then (imid,jmid,imid,jmid+1)    //right
    else (imid,jmid,imid+1,jmid+1)  //diag

// **************************************
// INPUT - OUTPUT
// **************************************
let actual (lines: string[]) =
    let mu = MatrixFiles.fileToScoringFunction "BLOSUM62"
    let i,j,k,l = ``Find a Middle Edge in an Alignment Graph in Linear Space`` mu -5 lines.[0] lines.[1]
    sprintf "(%i, %i) (%i, %i)" i j k l

let actualExpected (lines: string[]) = (actual lines, lines.[2])
let print = (printfn "%s")

// **************************************
// Runs
// **************************************
//``Find a Middle Edge in an Alignment Graph in Linear Space`` (fun i j -> if i=j then 1 else 0) 0 "ATTCAA" "ACGGAA"

Problems.sample id actualExpected
Problems.extra id actualExpected
Problems.dataset id actual  |> print
Problems.rosalind id actual |> print
