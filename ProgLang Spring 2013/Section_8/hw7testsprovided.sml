(* University of Washington, Programming Languages, Homework 7
   hw7testsprovided.sml *)
(* Will not compile until you implement preprocess and eval_prog *)

(* These tests do NOT cover all the various cases, especially for intersection *)

use "hw7.sml";

(* Must implement preprocess_prog and Shift before running these tests *)

fun real_equal(x,y) = Real.compare(x,y) = General.EQUAL;

(* Preprocess tests *)
let
	val Point(a,b) = preprocess_prog(LineSegment(3.2,4.1,3.2,4.1))
	val Point(c,d) = Point(3.2,4.1)
in
	if real_equal(a,c) andalso real_equal(b,d)
	then (print "PASS: preprocess converts a LineSegment to a Point successfully\n")
	else (print "FAIL: preprocess does not convert a LineSegment to a Point succesfully\n")
end;

let 
	val LineSegment(a,b,c,d) = preprocess_prog (LineSegment(0.5,2.0,0.4,2.0))
	val LineSegment(e,f,g,h) = LineSegment(0.4,2.0,0.5,2.0)
in
	if real_equal(a,e) andalso real_equal(b,f) andalso real_equal(c,g) andalso real_equal(d,h)
	then (print "PASS: preprocess flips an improper LineSegment successfully\n")
	else (print "FAIL: preprocess does not flip an improper LineSegment successfully\n")
end;

let 
	val LineSegment(a,b,c,d) = preprocess_prog (LineSegment(1.00000999,1.0,1.0,2.0))
	val LineSegment(e,f,g,h) = LineSegment(1.00000999,1.0,1.0,2.0)
in
	if real_equal(a,e) andalso real_equal(b,f) andalso real_equal(c,g) andalso real_equal(d,h)
	then (print "PASS: preprocess flips an improper LineSegment successfully\n")
	else (print "FAIL: preprocess does not flip an improper LineSegment successfully\n")
end;

let 
	val Intersect(LineSegment(a,b,c,d),_) = preprocess_prog (Intersect(LineSegment(3.2,4.1,~3.2,~4.1),Point(1.0,1.0)))
	val LineSegment(e,f,g,h) = LineSegment(~3.2,~4.1,3.2,4.1)
in
	if real_equal(a,e) andalso real_equal(b,f) andalso real_equal(c,g) andalso real_equal(d,h)
	then (print "PASS: preprocess flips an improper LineSegment within Interect successfully\n")
	else (print "FAIL: preprocess does not flip an improper LineSegment within Interect successfully\n")
end;

let 
	val Shift(_,_,LineSegment(a,b,c,d)) = preprocess_prog (Shift(1.0,1.0,LineSegment(3.2,4.1,~3.2,~4.1)))
	val LineSegment(e,f,g,h) = LineSegment(~3.2,~4.1,3.2,4.1)
in
	if real_equal(a,e) andalso real_equal(b,f) andalso real_equal(c,g) andalso real_equal(d,h)
	then (print "PASS: preprocess flips an improper LineSegment within Shift successfully\n")
	else (print "FAIL: preprocess does not flip an improper LineSegment within Interect successfully\n")
end;

let 
	val Let(_,LineSegment(a,b,c,d),_) = preprocess_prog (Let("text",LineSegment(3.2,4.1,~3.2,~4.1),Point(1.0,1.0)))
	val LineSegment(e,f,g,h) = LineSegment(~3.2,~4.1,3.2,4.1)
in
	if real_equal(a,e) andalso real_equal(b,f) andalso real_equal(c,g) andalso real_equal(d,h)
	then (print "PASS: preprocess flips an improper LineSegment within Let successfully\n")
	else (print "FAIL: preprocess does not flip an improper LineSegment within Let successfully\n")
end;

(* eval_prog tests with Shift*)

let 
	val actual = eval_prog (preprocess_prog ( Shift(1.0,1.0,Intersect(Line(1.0,1.0),Line(1.0,0.0)))), [])
	val exp = NoPoints
in
	case actual of
		 NoPoints => (print "PASS: Shift of NoPoints worked\n")
		 | _ => (print "FAIL: Shift of NoPoints is not working properly\n")
end;

let 
	val Point(a,b) = (eval_prog (preprocess_prog (Shift(3.0, 4.0, Point(4.0,4.0))), []))
	val Point(c,d) = Point(7.0,8.0) 
in
	if real_equal(a,c) andalso real_equal(b,d)
	then (print "PASS: Shift of Point worked\n")
	else (print "FAIL: Shift of Point is not working properly\n")
end;
let 
	val Line(a,b) = (eval_prog (preprocess_prog (Shift(3.0, 4.0, Line(1.0,2.0))), []))
	val Line(c,d) = Line(1.0, (2.0+4.0-1.0*3.0)) 
in
	if real_equal(a,c) andalso real_equal(b,d)
	then (print "PASS: Shift of Line worked\n")
	else (print "FAIL ** Shift of Line is not working properly\n")
end;
let 
	val VerticalLine(a) = (eval_prog (preprocess_prog (Shift(3.0, 4.0, VerticalLine(7.0))), []))
	val VerticalLine(c) = VerticalLine(7.0+3.0) 
in
	if real_equal(a,c) 
	then (print "PASS: Shift of VerticalLine worked\n")
	else (print "FAIL: Shift of VerticalLine is not working properly\n")
end;
let 
	val LineSegment(ax1,ay1,ax2,ay2) = (eval_prog (preprocess_prog (Shift(3.0, 4.0, LineSegment(1.0,2.0,3.0,4.0))), []))
	val LineSegment(bx1,by1,bx2,by2) = LineSegment(1.0+3.0,2.0+4.0,3.0+3.0,4.0+4.0) 
in
	if real_equal(ax1,bx1) andalso real_equal(ax2,bx2) andalso real_equal(ay1,by1) andalso real_equal(ay2,by2)
	then (print "PASS: Shift of LineSegment worked\n")
	else (print "FAIL: Shift of LineSegment is not working properly\n")
end;

(* Using a Var *)
let 
	val Point(a,b) = (eval_prog (Shift(3.0,4.0,Var "a"), [("a",Point(4.0,4.0))]))
	val Point(c,d) = Point(7.0,8.0) 
in
	if real_equal(a,c) andalso real_equal(b,d)
	then (print "PASS: eval_prog with 'a' in environment is working properly\n")
	else (print "FAIL: eval_prog with 'a' in environment is not working properly\n")
end;


(* With Variable Shadowing *)
let 
	val Point(a,b) = (eval_prog (Shift(3.0,4.0,Var "a"), [("a",Point(4.0,4.0)),("a",Point(1.0,1.0))]))
	val Point(c,d) = Point(7.0,8.0) 
in
	if real_equal(a,c) andalso real_equal(b,d)
	then (print "PASS: eval_prog with shadowing 'a' in environment is working properly\n")
	else (print "FAIL: eval_prog with shadowing 'a' in environment is not working properly\n")
end;
