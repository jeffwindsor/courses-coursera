(* Dan Grossman, Coursera PL, HW2 Provided Tests *)
use "hw2provided.sml";

(* These are just two tests for problem 2; you will want more.

   Naturally these tests and your tests will use bindings defined 
   in your solution, in particular the officiate function, 
   so they will not type-check if officiate is not defined.
 *)
all_except_option("test", []) = NONE;
all_except_option("test", ["one","two","three","four"]) = NONE;
all_except_option("test", ["one","two","test","three","four"]) = SOME(["one","two","three","four"]);
all_except_option("test", ["one","two","three","four","test"]) = SOME(["one","two","three","four"]);

get_substitutions1([["Fred","Fredrick"],["Elizabeth","Betty"],["Freddie","Fred","F"]], "") 
  = [];
get_substitutions1([["Fred","Fredrick"],["Elizabeth","Betty"],["Freddie","Fred","F"]], "test") 
  = [];
get_substitutions1([["Fred","Fredrick"],["Elizabeth","Betty"],["Freddie","Fred","F"]], "Fred") 
  = ["Fredrick","Freddie","F"];
get_substitutions1([["Fred","Fredrick"],["Jeff","Jeffrey"],["Geoff","Jeff","Jeffrey"]],"Jeff")
  = ["Jeffrey","Geoff","Jeffrey"];

get_substitutions2([["Fred","Fredrick"],["Elizabeth","Betty"],["Freddie","Fred","F"]], "") 
  = [];
get_substitutions2([["Fred","Fredrick"],["Elizabeth","Betty"],["Freddie","Fred","F"]], "test") 
  = [];
get_substitutions2([["Fred","Fredrick"],["Elizabeth","Betty"],["Freddie","Fred","F"]], "Fred") 
  = ["Fredrick","Freddie","F"];
get_substitutions2([["Fred","Fredrick"],["Jeff","Jeffrey"],["Geoff","Jeff","Jeffrey"]],"Jeff")
  = ["Jeffrey","Geoff","Jeffrey"];

similar_names([["Fred","Fredrick"],["Elizabeth","Betty"],["Freddie","Fred","F"]],
                      {first="Fred", middle="W", last="Smith"}) =
        [{first="Fred", last="Smith", middle="W"},
                    {first="Fredrick", last="Smith", middle="W"},
                    {first="Freddie", last="Smith", middle="W"},
                    {first="F", last="Smith", middle="W"}] ;

card_color(Clubs,King)    = Black;
card_color(Spades,King)   = Black;
card_color(Hearts,King)   = Red;
card_color(Diamonds,King) = Red;

card_value(Diamonds,Ace) = 11;
card_value(Diamonds,King) = 10;
card_value(Diamonds,Queen) = 10;
card_value(Diamonds,Jack) = 10;
card_value(Diamonds,Num(10)) = 10;
card_value(Diamonds,Num(9)) = 9;
card_value(Diamonds,Num(8)) = 8;
card_value(Diamonds,Num(7)) = 7;
card_value(Diamonds,Num(6)) = 6;
card_value(Diamonds,Num(5)) = 5;
card_value(Diamonds,Num(4)) = 4;
card_value(Diamonds,Num(3)) = 3;
card_value(Diamonds,Num(2)) = 2;

val e = IllegalMove;
val cards = [(Clubs,King),(Diamonds,Num(2)),(Spades,Jack)];
remove_card(cards,(Clubs,King),e) = [(Diamonds,Num(2)),(Spades,Jack)];
(remove_card(cards,(Clubs,Queen),e) handle IllegalMove => [] ) = [];

all_same_color(cards) = false;
all_same_color([(Clubs,King),(Clubs,Num(2)),(Spades,Jack)]) = true;
all_same_color([(Hearts,King),(Hearts,Num(2)),(Diamonds,Jack)]) = true;

sum_cards(cards)=22;
sum_cards([])=0;

score(cards,0)  = 66; (* over goal multi color *)
score(cards,68) = 46; (* under goal multi color *)
val cardsSameColor = [(Clubs,King),(Clubs,Num(5)),(Spades,Jack)];
score(cardsSameColor,0) = 37; (* over goal same color 75/2*)
score(cardsSameColor,80) = 27; (* under goal same color 55/2*)


(*correct behavior: raise IllegalMove *)
fun provided_test1 () = 
    let 
      val cards = [(Clubs,Jack),(Spades,Num(8))]
      val moves = [Draw,Discard(Hearts,Jack)]
    in
      officiate(cards,moves,42)
    end;
(provided_test1() handle IllegalMove => ~1) = ~1;

(*correct behavior: return 3 *)
fun provided_test2 () = 
    let 
      val cards = [(Clubs,Ace),(Spades,Ace),(Clubs,Ace),(Spades,Ace)]
	    val moves = [Draw,Draw,Draw,Draw,Draw]
    in
 	    officiate(cards,moves,42)
    end;
provided_test2() = 3;


fun no_moves_test () = 
    let 
      val cards = [(Clubs,Jack),(Spades,Num(3))]
      val moves = []
    in
      officiate(cards,moves,0)
    end;
no_moves_test() = 0;

fun draw_test () = 
    let 
      val cards = [(Clubs,Jack),(Hearts,Num(3))]
      val moves = [Draw]
    in
      officiate(cards,moves,0)
    end;
draw_test()=30 div 2;

fun over_draw_test () = 
    let 
      val cards = [(Clubs,Jack),(Hearts,Num(3))]
      val moves = [Draw,Draw,Draw]
    in
      officiate(cards,moves,13)
    end;
over_draw_test()=13-13;

fun discard_test () = 
    let 
      val cards = [(Clubs,Jack),(Hearts,Num(3))]
      val moves = [Draw,Draw,Discard(Clubs,Jack)]
    in
      officiate(cards,moves,13)
    end;
discard_test()=(13-3) div 2;

