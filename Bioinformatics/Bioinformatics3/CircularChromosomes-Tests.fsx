#load "Files.fs"
#load "Chromosomes.fs"
open Bioinformatics
open Chromosomes

//Problems.areEqual (fromLine "(0 1 2 3 4 5)") ([|[|0;1;2;3;4;5;|]|] |> Array.toSeq) "rom Line No Sign One"
//Problems.areEqual (fromLine "(+1 -2 +3 -4 +5 -6)")  [|[|1;-2;3;-4;5;-6;|]|] "From Line Signed One"
//Problems.areEqual (fromLine "(+1 -3 -6 -5)(+2 -4)") [| [|1;-3;-6;-5;|]; [|2;-4;|] |] "From Line Signed Multi"

//Problems.areEqual (toString [|1;2;3;4;5;|]) "(+1 +2 +3 +4 +5)" "Single To String"
//Problems.areEqual (toString [|1;-2;3;-4;5;-6;|]) "(+1 -2 +3 -4 +5 -6)" "Single To String Signed"
//Problems.areEqual (toString [| [|1;-3;-6;-5;|]; [|2;-4;|] |]) "(+1 -3 -6 -5)(+2 -4)" "To String Signed"

"(+1 -2 +3 -4 +5 -6 +7 -8 -9 +10 -11 -12 +13 -14 +15 -16 -17 -18 +19 +20 -21 +22 +23 +24 -25 +26 -27 +28 -29 +30 +31 -32 -33 +34 -35 +36 +37 +38 -39 +40 -41 -42 -43 +44 +45 +46 +47 +48 -49 -50 +51 -52 -53 -54 +55 -56 -57 +58 -59 -60 -61 -62 +63 +64 +65 -66 +67)" |> fromLine |> Seq.map toCycle |> Cycles.toStringConcat
Problems.areEqual ("(+1 -2 -3 +4)" |> fromLine |> Seq.head |> toCycle |> Seq.toArray) [|1; 2; 4; 3; 6; 5; 7; 8;|] "toCycle sample"

Problems.areEqual 
    ([|1; 2 ;4 ;3 ;6; 5; 7; 8|] |> nodesToChromosome |> Seq.toArray) 
    [|1; -2; -3; 4;|] 
    "toChromosome"

Problems.areEqual 
    ([BlackEdge(1,2);BlackEdge(4,3);BlackEdge(6,5)] |> blackToColoredEdges |> Seq.toList)
    [ColoredEdge (2,4); ColoredEdge (3,6); ColoredEdge (5,1)]
    "blackToColored"
