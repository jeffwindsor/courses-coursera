
(* anon function *)
fun one (x,y)= x+y
val two = fn(x,y) => x+y

(* map to list *)
fun map(f,xs) =
    case xs of
        [] => []
    |   x::xs' => (f,x)::map(f,xs')

(* filter list *)
fun filter(f,xs) =
    case xs of
        [] => []
    |   x::xs' => if f(x)
                  then x::(filter(f,xs'))
                  else filter(f,xs')


fun all_even xs = filter(fn v => v mod 2 = 0, xs)

val abc = 12

fun abcx(x)= x + abc

(* pipeline operator *)
infix |> 
fun x |> f = f x

(* Currying and iterators *)
(* range function  *)
fun range i j = if i > j then []
                else i::range((i+1),j)
val count_up_to = range 1

(* exists *)
fun exists p []     = false
|   exists p x::xs  = p x orelse exists p xs

(* partial application *)
fun hasZero = exists (fn x => x = 0)
