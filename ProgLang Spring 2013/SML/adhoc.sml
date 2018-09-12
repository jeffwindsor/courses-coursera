
(*
fun null xs = case xs of [] => true | _ => false
fun null xs = xs=[]
fun null xs = if null xs then true else false
fun null xs = ((fn z => false) (hd xs)) handle List.Empty => true
*)

fun null xs = ((fn z => false) (hd xs)) handle List.Empty => true

val x = null []
val y = null [1]