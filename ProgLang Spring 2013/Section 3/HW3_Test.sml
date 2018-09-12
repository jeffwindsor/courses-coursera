use "smlUnit.sml";
use "HW3.sml";

describe("only_capitals: that takes a string list and returns a string list that has only "
    ^ "the strings in the argument that start with an uppercase letter",
[
    expect("Given an empty list, it should return an empty list", 
            assert_equals([], only_capitals([]))),
	expect("Given an no capitals, it should return an empty list", 
           assert_equals([], only_capitals(["one","two","three"]))),
    expect("Given an some capitals, it should return only capitols", 
           assert_equals(["One","Three"], only_capitals(["One","two","Three","four"])))
]); 
describe("longest_string1: that takes a string list and returns the longest string in the list. "
    ^ "If the list is empty, return empty string"
    ^ "In the case of a tie, return the string closest to the beginning of the list.",
[
    expect("Given an empty list, should return an empty string",
        assert_equals("", longest_string1([]))),
    expect("Given a single entry list should return the single entry",
        assert_equals("one", longest_string1(["one"]))),
    expect("Given a list of words should return the longest string",
        assert_equals("Three", longest_string1(["One","two","Three","four"]))),
    expect("Given a tie, should return first match",
        assert_equals("Three", longest_string1(["One","two","Three","four","apple"])))
]);
describe("longest_string2: that takes a string list and returns the longest string in the list. "
    ^ "If the list is empty, return empty string"
    ^ "In the case of a tie, return the string closest to the end of the list.",
[
    expect("Given an empty list, should return an empty string",
        assert_equals("", longest_string2([]))),
    expect("Given a single entry list should return the single entry",
        assert_equals("one", longest_string2(["one"]))),
    expect("Given a list of words should return the longest string",
        assert_equals("Three", longest_string2(["One","two","Three","four"]))),
    expect("Given a tie, should return last match",
        assert_equals("apple", longest_string2(["One","two","Three","four","apple"])))
]);
describe("longest_string3: that takes a string list and returns the longest string in the list. "
    ^ "If the list is empty, return empty string"
    ^ "In the case of a tie, return the string closest to the beginning of the list.",
[
    expect("Given an empty list, should return an empty string",
        assert_equals("", longest_string3([]))),
    expect("Given a single entry list should return the single entry",
        assert_equals("one", longest_string3(["one"]))),
    expect("Given a list of words should return the longest string",
        assert_equals("Three", longest_string3(["One","two","Three","four"]))),
    expect("Given a tie, should return first match",
        assert_equals("Three", longest_string3(["One","two","Three","four","apple"])))
]);
describe("longest_string4: that takes a string list and returns the longest string in the list. "
    ^ "If the list is empty, return empty string"
    ^ "In the case of a tie, return the string closest to the end of the list.",
[
    expect("Given an empty list, should return an empty string",
        assert_equals("", longest_string4([]))),
    expect("Given a single entry list should return the single entry",
        assert_equals("one", longest_string4(["one"]))),
    expect("Given a list of words should return the longest string",
        assert_equals("Three", longest_string4(["One","two","Three","four"]))),
    expect("Given a tie, should return last match",
        assert_equals("apple", longest_string4(["One","two","Three","four","apple"])))
]);

describe("longest_capitalized: that takes a string list and returns the longest string in the list. "
    ^ "If the list is empty, return empty string"
    ^ "In the case of a tie, return the string closest to the beginning of the list.",
[
    expect("Given an empty list, should return an empty string",
        assert_equals("", longest_capitalized([]))),
    expect("Given no capitols, should return the empty string",
        assert_equals("", longest_capitalized(["one"]))),
    expect("Given a list of words should return the longest capitalized string",
        assert_equals("Four", longest_capitalized(["One","two","three","Four"]))),
    expect("Given a tie, should return first capitalized match",
        assert_equals("Four", longest_capitalized(["One","two","three","Four","Five"])))
]);

describe("rev_string that takes a string and returns the string that is the same characters in reverse order.",
[
    expect("Given empty string, should return empty string",
        assert_equals("",rev_string(""))),
    expect("Given a string, should return reverse string",
        assert_equals("1234 5 6789",rev_string("9876 5 4321")))
]);

describe("count_wildcards takes a pattern and returns how many Wildcard patterns it contains.",
[
    expect("Given empty pattern, should return 0",
        assert_equals(0,count_wildcards(UnitP))),
    expect("Given 1 wildcard, should return 1",
        assert_equals(1,count_wildcards(TupleP([UnitP,Wildcard,Variable("X")])))),
    expect("Given 2 wildcars, should return 2",
        assert_equals(2,count_wildcards(TupleP([Wildcard,Wildcard,Variable("X")]))))
]);

describe("count_wild_and_variable_lengths that takes a pattern and returns the number of Wildcard patterns it contains plus the sum of the string lengths of all the variables in the variable patterns it contains.",
[
    expect("Given empty pattern, should return 0",
        assert_equals(0,count_wild_and_variable_lengths(UnitP))),
    expect("Given , should return 1",
        assert_equals(6,count_wild_and_variable_lengths(TupleP([UnitP,Variable("X12345")])))),
    expect("Given , should return 2",
        assert_equals(3,count_wild_and_variable_lengths(TupleP([Wildcard,Wildcard,Variable("X")]))))
]);

describe("count_some_var that takes a string and a pattern (as a pair) and returns the number of times the string appears as a variable in the pattern.",
[
    expect("Given no variable of name, should return 0",
        assert_equals(0,count_some_var("X", TupleP([Variable("X12345"),Variable("Y12345")])))),
    expect("Given 1 variable of name, should return 1",
        assert_equals(1,count_some_var("Y12345", TupleP([Variable("X12345"),Wildcard,Variable("Y12345")])))),
    expect("Given n variables of name, should return n",
        assert_equals(2,count_some_var("Y12345", TupleP([Variable("X12345"),Variable("Y12345"),Wildcard,Variable("Y12345")]))))
]);

describe("check_pat",
[
    expect("Given no variables, should return true",
        assert_equals(true,check_pat(UnitP))),
    expect("Given unique variables, should return true",
        assert_equals(true,check_pat(TupleP([Variable("X12345"),Wildcard,Variable("Y12345")])))),
    expect("Given duplicate variables, should return false",
        assert_equals(false,check_pat(TupleP([Variable("X12345"),Variable("Y12345"),Wildcard,Variable("Y12345")]))))
]);


describe("match value with pattern",
[
    expect("Given no matches, should return NONE",
        assert_equals(NONE,match(Unit, ConstP(11)))),
    expect("Given wildcard, should return SOME[]",
        assert_equals(SOME([]),match(Unit, Wildcard))),
    expect("Given variable , should return varName,value",
        assert_equals(SOME([("varName",Unit)])
            ,match(Unit, Variable("varName")))),
    expect("Given unit and unitp, should return some []",
        assert_equals(SOME([]),match(Unit, UnitP))),
    expect("Given mathcing constants, should return some[]",
        assert_equals(SOME([]),match(Const(17), ConstP(17)))),
    expect("Given non matching constants, should return some[]",
        assert_equals(NONE,match(Const(17), ConstP(11)))),
    expect("Given constructors with mathcing strings, should return result of match on v,p pairs",
        assert_equals(SOME([("varName",Unit)]),
            match(Constructor("X",Unit), ConstructorP("X",Variable("varName")))
            )),
    expect("Given constructors without mathcing strings, should return NONE",
        assert_equals(NONE,
            match(Constructor("X",Unit), ConstructorP("Y",Variable("varName")))
            )),
    expect("Given Tuple with matches, should return nested matches appened",
        assert_equals(SOME([("varName1",Unit),("varName2",Const(1))]),
            match(Tuple([Unit,Const(1)]),
                TupleP([Variable("varName1"),Variable("varName2")]))
            )),
    expect("Given Tuple with no matches, should return none",
        assert_equals(NONE,
            match(Tuple([Unit]),TupleP([ConstP(13)]))
            ))
]);