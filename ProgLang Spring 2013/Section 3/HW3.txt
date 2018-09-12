(* Coursera Programming Languages, Homework 3, Provided Code *)

exception NoAnswer

datatype pattern = Wildcard
		 | Variable of string
		 | UnitP
		 | ConstP of int
		 | TupleP of pattern list
		 | ConstructorP of string * pattern

datatype valu = Const of int
	      | Unit
	      | Tuple of valu list
	      | Constructor of string * valu

fun g f1 f2 p =
	let 
		val r = g f1 f2 
    in
		case p of
		    Wildcard          => f1 ()
		  | Variable x        => f2 x
		  | TupleP ps         => List.foldl (fn (p,i) => (r p) + i) 0 ps
		  | ConstructorP(_,p) => r p
		  | _                 => 0
    end

(**** for the challenge problem only ****)

datatype typ = Anything
	     | UnitT
	     | IntT
	     | TupleT of typ list
	     | Datatype of string

(**** you can put all your code here ****)

val only_capitals = List.filter (fn s => Char.isUpper(String.sub(s, 0)))
val longest_string1 = List.foldl (fn (s1, s2) => if String.size(s1) > String.size(s2) then s1 else s2) ""
val longest_string2 = List.foldl (fn (s1, s2) => if String.size(s1) >= String.size(s2) then s1 else s2) ""

fun longest_string_helper f = List.foldl (fn (s1, s2) => if f(String.size(s1),String.size(s2)) then s1 else s2) "" 
val longest_string3 = longest_string_helper (fn(i,j) => i > j)
val longest_string4 = longest_string_helper (fn(i,j) => i >= j)

val longest_capitalized = longest_string1 o only_capitals
val rev_string = String.implode o List.rev o String.explode

fun first_answer f xs = 
	case xs of 
	  [] => raise NoAnswer
	| x::xs' => case f(x) of
				  NONE    => first_answer f xs'
				| SOME(y) => y

fun all_answers f xs = 
	let 
		fun inner(acc, [])= SOME acc
		|   inner(acc, x::xs') =
					 case f(x) of
						   NONE   => NONE
						 | SOME y => inner ((y @ acc), xs')
	in
		inner([],xs)
	end

val count_wildcards = g (fn _ => 1) (fn s => 0)
val count_wild_and_variable_lengths = g (fn _ => 1) (fn s => String.size s)
fun count_some_var(var,p)= g (fn _ => 0) (fn s => if var = s then 1 else 0) p

fun check_pat p =
	let
		fun get_vars pat = 
			case pat of
				Variable x => [x]
				| TupleP xs => List.foldl (fn (x, acc) => get_vars(x) @ acc) [] xs
				| ConstructorP(_, x)  => get_vars(x)
				| _ => []

		fun all_unique ss =
			case ss of
				[]		=> true
			|	s::ss'	=> if List.exists (fn s' => s' = s) ss' then false
						   else all_unique ss'
	in
		all_unique(get_vars(p))
	end

fun match(v,p) =
	case (p,v) of 
		(Variable s, _) => SOME [(s,v)]
	|	(ConstructorP(s2,pc), Constructor(s1,vc)) => if s1=s2 then match(vc,pc) else NONE
	|	(ConstP pc, Const vc) => if vc=pc then SOME[] else NONE
	|	(TupleP ps, Tuple vs) => if List.length(ps) = List.length(vs)
						 then ((all_answers match)(ListPair.zip(vs,ps)))
						 else NONE
	|	(UnitP, Unit) => SOME []
	|	(Wildcard, _) => SOME []
	|	_ => NONE

fun first_match v ps =
	 SOME(first_answer (fn p => match(v,p)) ps)
	 handle NoAnswer => NONE
