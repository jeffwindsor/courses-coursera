(*
** EXAMPLES **
describe("Function",
[
    expect("given-when-then", 
        let 
            val dates = []
            val month = 1
            val expected = 0
            val got = number_in_month(dates, month)
        in
            assert_intEquals(expected, got)
        end
    ),

    expect("given month 1, and one matching date in a list of two, it"
          ^" should return 1", 
        let 
            val dates = [(2013,1,1),(2013,2,1)]
            val month = 1
            val expected = 2
            val got = number_in_month(dates, month)
        in
            assert_intEquals(expected, got)
        end
    )
]);  

*)

(* This is the outermost container for a set of related tests.
 * It handles the string compilation and printing of the tests*)
fun describe (msg : string, tests : (int*string) list) =
    let
        (* compile all the tests into a single printable string *)
        fun stringify (ms : (int*string) list) =
            if null ms then
                "\n"
            else
                "\t\t" ^ #2 (hd ms) ^ "\n" ^ stringify(tl ms)

        (* count the number of failed tests *)
        fun sum_list (xs : (int*string) list) =
           if null xs then 0 
           else #1 (hd xs) + sum_list(tl xs)
    in

        (* don't print the print object after each section, 
         * just print the tests *)
        Control.Print.out := {say=fn _=>(), flush=fn()=>()};

        print("\n\t" ^ msg ^ "\n" 
          ^ stringify(tests) 
          ^ "\t\t" ^ Int.toString(sum_list(tests))
          ^ " tests failed \n\n")
    end


(* this creates the message for a single test, including the description
 * of the test, and converting its result into a pass/fail message *)
fun expect (msg: string, e: bool * (string * string) option) =
    let 
        val passed = #1 e
        val info = #2 e

        (* if available, include the elaborated results, e.g.
         * what was expected vs what was got *)
        fun elab(msg) =
            if isSome(info) 
            then 
                msg
                ^ "\n\t\t   expected: " ^ #1 (valOf info)  
                ^ "\n\t\t   got: " ^ #2 (valOf info)
            else 
                msg

    in
      (* TODO: the 0 and 1 here are a quick fix for the 
         sum_list function, and should probably be removed. 
         The sum_list function should do it's own counting. *)      
        if passed then
            (0, "PASS -- " ^ msg)
        else
            (1, "FAIL -- " ^ elab(msg))     
    end

fun assert_true ( e: bool ) =
    (e = true, SOME("true", "false"))

fun assert_false ( e: bool ) =
    (e = false, SOME("false", "true"))

(* this can do polymorphic comparisons, but cannot print elaborate
 * fail messages with expected vs got values  *)
fun assert_equals (e, x) =
    (e = x, NONE)

fun assert_intEquals (e, x) =
    (e = x, SOME(Int.toString(e), Int.toString(x)))

fun assert_strEquals (e, x) =
    (e =x, SOME(e, x))