(* 	Unit Tests for Assignment 1. Windsor, Jeff. 2013-01-16. *)
use "Homework1.sml";

(* Question #1: is_older *)
(* Older Year*)
is_older((2000,1,1),(2001,1,1)) = true;
(* Younger Year*)
is_older((2001,1,1),(2000,1,1)) = false;
(* Older Month*)
is_older((2000,1,1),(2000,2,1)) = true;
(* Younger Month*)
is_older((2000,2,1),(2000,1,1)) = false;
(* Older Day*)
is_older((2000,1,1),(2000,1,2)) = true;
(* Younger Day*)
is_older((2000,1,2),(2000,1,1)) = false;
(* Equal *)
is_older((2000,1,1),(2000,1,1)) = false;

(* Question #2: number_in_month*)
(* empty list *)
number_in_month([],1) = 0;
(* no matching months *)
number_in_month([(2000,2,1),(2000,2,2)],1) = 0;
(* matching months *)
number_in_month([(2000,1,1),(2000,1,2),(2000,2,1),(2000,2,2)],1) = 2;

(* Question #3: number_in_months*)
(* empty list *)
number_in_months([],[1,2,3]) = 0;
number_in_months([(2000,12,1),(2000,12,2)],[]) = 0;
(* no matching months *)
number_in_months([(2000,12,1),(2000,12,2)],[1,2,3]) = 0;
(* matching months *)
number_in_months([(2000,1,1),(2000,1,2),(2000,2,1),(2000,2,2),
	(2000,3,1),(2000,3,2),(2000,12,1),(2000,12,2)],[1,2,3]) = 6;

(* Question #4: dates_in_month*)
(* empty list *)
dates_in_month([],1) = [];
(* no matching months *)
dates_in_month([(2000,2,1),(2000,2,2)],1) = [];
(* matching months *)
dates_in_month([(2000,1,1),(2000,1,2),(2000,2,1),(2000,2,2)],1) = [(2000,1,1),(2000,1,2)];

(* Question #5: dates_in_months*)
(* empty list *)
dates_in_months([],[1,2,3]) = [];
dates_in_months([(2000,12,1),(2000,12,2)],[]) = [];
(* no matching months *)
dates_in_months([(2000,12,1),(2000,12,2)],[1,2,3]) = [];
(* matching months *)
dates_in_months([(2000,1,1),(2000,1,2),(2000,2,1),(2000,2,2),(2000,3,1),
	(2000,3,2),(2000,12,1),(2000,12,2)],[1,2,3]) = [(2000,1,1),(2000,1,2),
	(2000,2,1),(2000,2,2),(2000,3,1),(2000,3,2)];

(* Question #6: get_nth *)
(* empty list *)
get_nth([],3) = "";
(* n not found *)
get_nth(["one","two","three"],0) = "";
get_nth(["one","two","three"],~10) = "";
get_nth(["one","two","three"],4) = "";
(* n found *)
get_nth(["one","two","three"],2) = "two";
get_nth(["one","two","three"],3) = "three";

(* Question #7: date_to_string *)
date_to_string((2000,20,1)) = " 1, 2000";
date_to_string((2000,1,1)) = "January 1, 2000";
date_to_string((2000,12,1)) = "December 1, 2000";

(* Question #8: number_before_reaching_sum *)
number_before_reaching_sum(0, [1,2,3,4,5,6,7,8,9,10]) = 0;
number_before_reaching_sum(9, [1,2,3,4,5,6,7,8,9,10]) = 3;
number_before_reaching_sum(10, [1,2,3,4,5,6,7,8,9,10])= 3;
number_before_reaching_sum(55, [1,2,3,4,5,6,7,8,9,10])= 9;

(* Question #9: what_month *)
what_month(31)=1;
what_month(31+28)=2;
what_month(31+28+31)=3;
what_month(31+28+31+30)=4;
what_month(31+28+31+30+31)=5;
what_month(31+28+31+30+31+30)=6;
what_month(31+28+31+30+31+30+31)=7;
what_month(31+28+31+30+31+30+31+31)=8;
what_month(31+28+31+30+31+30+31+31+30)=9;
what_month(31+28+31+30+31+30+31+31+30+31)=10;
what_month(31+28+31+30+31+30+31+31+30+31+30)=11;
what_month(31+28+31+30+31+30+31+31+30+31+30+31)=12;

(* Question #10: month_range *)
month_range(100,90) = [];
month_range(360,365) = [12,12,12,12,12,12];
month_range(31,32) = [1,2];

(* Question #11: oldest *)
oldest([]) = NONE;
oldest([(2000,1,1),(2000,1,2),(2000,2,1),(1999,2,2),(2000,3,1),
	(2000,3,2),(2000,12,1),(2000,12,2)]) = SOME((1999,2,2));
oldest([(2000,1,1),(2000,1,2),(2000,2,1),(1999,2,2),(1980,3,1),
	(2000,3,2),(2000,12,1),(2000,12,2)]) = SOME((1980,3,1));
