(* 
	Course:		Programming Languages 
	Assignment:	1
	Author:		Jeff Windsor
	Date:		2013-01-14
*)

fun is_older(one:(int*int*int), two:(int*int*int)) =
	if 		#1 one < #1 two then true
	else if #1 one > #1 two then false
	else if #2 one < #2 two then true
	else if #2 one > #2 two then false
	else if #3 one < #3 two then true
	else false;

fun number_in_month(dts: (int*int*int) list, month:int) =
	if null dts then 0
	else (if (#2 (hd dts)) = month then 1 else 0) + number_in_month(tl dts, month)

fun number_in_months(dts: (int*int*int) list, months:int list) = 
	if null dts orelse null months then 0
	else number_in_month(dts, hd months) + number_in_months(dts, tl months)

fun dates_in_month(dts: (int*int*int) list, month:int) =
	if null dts then []
	else if (#2 (hd dts)) = month then (hd dts) :: dates_in_month(tl dts, month) 
	else dates_in_month(tl dts, month)

fun dates_in_months(dts: (int*int*int) list, months:int list) = 
	if null dts orelse null months then []
	else dates_in_month(dts, hd months) @ dates_in_months(dts, tl months)

fun get_nth(strings: string list, n:int) = 
	if null strings orelse n < 1 then ""
	else if n = 1 then hd strings
	else get_nth(tl strings, n - 1)

fun date_to_string(dt:(int*int*int)) = 
	let 
		val months = ["January", "February", "March", 
		              "April", "May", "June", "July", 
		              "August", "September", "October", 
		               "November", "December"]
	in
		get_nth(months,#2 dt) ^ 
		        " " ^ Int.toString(#3 dt) ^ 
		        ", " ^ Int.toString(#1 dt)
	end

fun number_before_reaching_sum(sum:int, values:int list) =
	let
		fun inner_nbrs(n:int, n_sum:int, remaining_values:int list) = 
			if (n_sum + (hd remaining_values)) < sum
			then inner_nbrs(n + 1, n_sum + (hd remaining_values), tl remaining_values)
			else n
	in
		(* call inner with seed values *)
		inner_nbrs(0,0,values)
	end

fun what_month(day:int) = 
	let
		val days_in_months = [31,28,31,30,31,30,31,31,30,31,30,31]
	in
		(* add one to the result since we are using "before reaching sum" *)
		number_before_reaching_sum(day,days_in_months) + 1
	end

fun month_range(day1:int, day2:int) = 
	if day1 > day2 then []
	else what_month(day1) :: month_range(day1 + 1,day2)

fun oldest(dts:(int * int * int) list) = 
	let
		fun inner_oldest(current:(int * int * int), dts:(int * int * int) list) =
			if null dts then current
			else if is_older(current, hd dts) then inner_oldest(current, tl dts)
			else inner_oldest(hd dts, tl dts)
	in
		if null dts then NONE
		else SOME(inner_oldest(hd dts, tl dts))
	end
