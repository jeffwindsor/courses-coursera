// 1.2.0.2 - Reverse Complement

System.IO.Directory.SetCurrentDirectory (__SOURCE_DIRECTORY__)
#load "Support.fs"
#load "Cellular.fs"

open Bioinformatics

let dnaReverseComplement dnaString =
    dnaString
    |> Dna.lex
    |> Dna.toReverseComplement

//"GCTAGCT" |> dnaReverseComplement |> Seq.toString

"ReverseComplement" 
|> Files.toLines
|> Seq.head
|> dnaReverseComplement
|> Seq.toString

let testExtra = 
    let lines = "ReverseComplement-Extra" |> Files.toLines
    let actual = lines |> Seq.head |> dnaReverseComplement |> Seq.toString
    let expected = lines |> Seq.tail |> Seq.head 
    actual = expected

