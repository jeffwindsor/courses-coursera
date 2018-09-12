(* Dan Grossman, Coursera PL, HW2 Provided Code *)

(* if you use this function to compare two strings (returns true if the same
   string), then you avoid several of the functions in problem 1 having
   polymorphic types that may be confusing *)
fun same_string(s1 : string, s2 : string) =
    s1 = s2

(* put your solutions for problem 1 here *)

fun all_except_option(except, strings) = 
    let 
        fun all_except(xs)    = 
            case xs of 
                []    => []
            |   y::ys => if same_string(y,except) 
                         then ys 
                         else y :: all_except(ys)
        val result = all_except(strings)
    in 
        if result = strings then NONE else SOME(result) 
    end

fun get_substitutions1(substitutions, name) = 
    case substitutions of
        []      =>  []
    |   xs::xss =>  case all_except_option(name,xs) of
                      NONE     => get_substitutions1(xss,name)
                    | SOME(ys) => ys @ get_substitutions1(xss,name)

fun get_substitutions2(substitutions, name) = 
    let 
        fun inner_tail(xss, acc) = 
            case xss of
                []      => acc
            |   ys::yss => case all_except_option(name,ys) of
                              NONE     => inner_tail(yss, acc)
                            | SOME(zs) => inner_tail(yss, acc @ zs)
    in
        inner_tail(substitutions,[])
    end 

fun similar_names(substitutions, {first,middle,last}) =
    let
        fun substitute_first_name(first_names) =
            case first_names of
                []    => []
            |   x::xs => {first=x,middle=middle,last=last} :: substitute_first_name(xs)
    in
        {first=first,middle=middle,last=last} :: substitute_first_name(get_substitutions2(substitutions,first))
    end

(* you may assume that Num is always used with values 2, 3, ..., 9
   though it will not really come up *)
datatype suit = Clubs | Diamonds | Hearts | Spades
datatype rank = Jack | Queen | King | Ace | Num of int 
type card = suit * rank

datatype color = Red | Black
datatype move = Discard of card | Draw 

exception IllegalMove

(* put your solutions for problem 2 here *)
fun card_color(c:card)=
    case c of
        (Clubs,_)    => Black
    |   (Spades,_)   => Black
    |   (Hearts,_)   => Red
    |   (Diamonds,_) => Red

fun card_value(c:card)=
    case c of
        (_,Ace)    => 11
    |   (_,King)   => 10
    |   (_,Queen)  => 10
    |   (_,Jack)   => 10
    |   (_,Num(i)) => i


fun remove_card(cards, card, e) = 
    let 
        fun inner([])    = []
        |   inner(y::ys) = if card = y
                           then ys 
                           else y :: inner(ys)
        val result = inner(cards)
    in 
        if result = cards then raise e else result 
    end

fun all_same_color(cards) =
    case cards of
        []               => true
    |   _::[]            => true
    |   one::(two::rest) => card_color(one)=card_color(two)
                            andalso all_same_color(two::rest) 

fun sum_cards(cards) =
    let
        fun inner([],acc)    = acc
        |   inner(c::cs,acc) = inner(cs, acc + card_value(c))
    in
        inner(cards,0)
    end

fun score(cards,goal) =
    let
        val sum = sum_cards(cards)
        val prelim = if sum > goal 
                     then (sum-goal) * 3
                     else goal - sum
    in
        if all_same_color(cards) then (prelim div 2) else prelim 
    end

fun officiate(cards,moves,goal) = 
    let
        fun inner(_,held,[]) = score(held,goal)  (* end game *)
        |   inner(deck,held,move::remaining_moves) =
                case move of
                    Discard(discard_card) => inner(deck,remove_card(held,discard_card,IllegalMove),remaining_moves)
                |   Draw          => 
                        case deck of 
                            []                    => score(held,goal)  (* end game *)    
                        |   drawn::remaining_deck => 
                                let
                                    val held_after_draw = drawn::held
                                in
                                    if sum_cards(held_after_draw) > goal
                                    then score(held_after_draw,goal) (* end game *)
                                    else inner(remaining_deck,held_after_draw,remaining_moves)
                                end
    in
        inner(cards,[],moves)
    end


