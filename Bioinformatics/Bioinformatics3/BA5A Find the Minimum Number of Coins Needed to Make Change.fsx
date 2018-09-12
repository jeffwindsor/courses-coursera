System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Files.fs"
open Bioinformatics

let problemNumber = "BA5A"
//  BA5A Find the Minimum Number of Coins Needed to Make Change
//    The Change Problem:Find the minimum number of coins needed to make change.
//    Given: An integer money and an array Coins of positive integers.
//    Return: The minimum number of coins with denominations Coins that changes money.
//  LINK: https://stepic.org/lesson/An-Introduction-to-Dynamic-Programming-The-Change-Problem-243/step/1?course=Comparing-Genes-Proteins-and-Genomes-(Bioinformatics-III)&unit=2169
let minimumCoins coins value =
    let cache = Array.create (value + 1) 0
    let minChange coins (cache:int []) value = 
        coins
        |> Seq.filter (fun coin -> coin <= value)
        |> Seq.map    (fun coin -> cache.[value - coin] + 1)
        |> Seq.min 

    {1..value}
    |> Seq.fold (fun _ value -> 
        let min = minChange coins cache value
        cache.[value] <- min
        min
        ) 0

let analyse (lines: string[]) =
    let coins = lines.[1].Split(',') |> Array.map int
    let value = int lines.[0]
    minimumCoins coins value

let test (lines: string[]) =
    let expected = int lines.[2] 
    (analyse lines, expected)

Problems.sample problemNumber test
Problems.extra problemNumber test
Problems.dataset problemNumber analyse
Problems.rosalind problemNumber analyse

