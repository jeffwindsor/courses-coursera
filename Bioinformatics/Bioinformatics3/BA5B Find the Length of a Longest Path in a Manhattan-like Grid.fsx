System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
open Bioinformatics

let problemNumber = "BA5B"
//  BA5B Find the Length of a Longest Path in a Manhattan-like Grid
//    Length of a Longest Path in the Manhattan Tourist Problem: Find the length of a longest path in a rectangular city.
//    Given: Integers n and m, followed by an n × (m+1) matrix Down and an (n+1) × m matrix Right. The two matrices are separated by the "-" symbol.
//    Return: The length of a longest path from source (0, 0) to sink (n, m) in the n × m rectangular grid whose edges are defined by the matrices Down and Right.
//  LINK: https://stepic.org/lesson/The-Manhattan-Tourist-Problem-Revisited-261/step/9?course=Comparing-Genes-Proteins-and-Genomes-(Bioinformatics-III)&unit=2170
let ``Find the Length of a Longest Path in a Manhattan-like Grid`` n m (down:int[,]) (right:int[,]) =
    let result = Array2D.create (n+1) (m+1) 0
    let getdown i j  = result.[i-1,j] + down.[i-1,j]
    let getright i j = result.[i,j-1] + right.[i,j-1]
    [1..n] |> Seq.iter (fun i -> result.[i,0] <- getdown i 0)
    [1..m] |> Seq.iter (fun j -> result.[0,j] <- getright 0 j)
    [1..n] |> Seq.iter (fun i ->
        [1..m] |> Seq.iter (fun j -> result.[i,j] <- max (getdown i j) (getright i j) )
        )
    result.[n,m]
    
let analyse (lines: string[]) =
    let ints = lines.[0] |> Files.Lines.toInts
    let n, m = ints.[0], ints.[1]
    
    let down  = lines.[1..n]       |> Files.Lines.toInts2D (m+1)
    let right = lines.[n+2..n+2+n] |> Files.Lines.toInts2D m
        
    ``Find the Length of a Longest Path in a Manhattan-like Grid`` n m down right

let test (lines: string[]) =
    let expected = lines |> Array.last |> int
    (analyse lines, expected)

Problems.sample problemNumber test
Problems.extra problemNumber test
Problems.dataset problemNumber analyse
Problems.rosalind problemNumber analyse