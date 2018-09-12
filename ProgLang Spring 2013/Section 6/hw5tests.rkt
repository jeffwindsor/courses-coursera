#lang racket

(require test-engine/racket-tests) 
(require "hw5.rkt")

(define test-env (list (cons "x" (int 3)) (cons "y" (int 4)))) ; x=3, y =4
(define test-mlet*-env (list (cons "a" (int 1)) (cons "b" (add (var "a") (int 2))) (cons "c" (add (var "b") (int 3)))))
(define test-pair (apair (add (int 2) (int 2)) (add (int 2) (int 3)))) ;(4,5)
(define rs (cons (var "x") (cons (int 3) null)))
(define ms (apair (var "x") (apair (int 3) (aunit))))
(define test-nonrec-fn (fun #f "n" (add (var "n") (var "n"))))
(define test-closure (closure test-env (fun "n-x" "n" (add (var "n") (var "x")))))
(define test-rec-fn (fun "sum-count-down" "n" (ifgreater (var "n") 
                                                         (int 0) 
                                                         (add (var "n") 
                                                              (call (var "sum-count-down") 
                                                                    (add (var "n")(int -1))))
                                                         (int 0))))


;warm up 
(check-expect (racketlist->mupllist rs) ms)
(check-expect (mupllist->racketlist ms) rs)

;All values (including closures) evaluate to themselves. For example, (eval-exp (int 17)) would return (int 17), not 17.
(check-expect (eval-under-env (int 4) null) (int 4))  ;Int - return as value
(check-expect (eval-under-env (aunit) null) (aunit)) ;AUnit - return as value
(check-expect (eval-under-env (closure test-env test-nonrec-fn) null) (closure test-env test-nonrec-fn)) ;Closure - return as value
(check-expect (eval-under-env (apair (int 4) (int 5)) null) (apair (int 4) (int 5)))  ;APair - return as value

;A variable evaluates to the value associated with it in the environment.
(check-expect (eval-under-env (var "x") test-env) (int 3)) ; return env var
(check-error  (eval-under-env (var "z") test-env)) ;missingin env, throws error

;An addition evaluates its subexpressions and assuming they both produce integers,
;  produces the integer that is their sum. (Note this case is done for you to get you pointed in the right direction.)
(check-expect (eval-under-env (add (int 5) (int 6)) null) (int 11)) ;add
(check-expect (eval-under-env (add (var "x") (int 6)) test-env) (int 9)) ;add
(check-error (eval-under-env (add (int 5) (aunit)) null)) ;non int

;Functions are lexically scoped: A function evaluates to a closure holding the function and the current environment.
(check-expect (eval-under-env test-nonrec-fn test-env) (closure test-env test-nonrec-fn))

;An ifgreater evaluates its first two subexpressions to values v1 and v2 respectively.
;  If both values are integers, it evaluates its third subexpression 
;  if v1 is a strictly greater integer than v2 else it evaluates its fourth subexpression.
(check-expect (eval-under-env (ifgreater (int 1) (int 2) (var "bang") (int 8)) null) (int 8)) ;add
(check-expect (eval-under-env (ifgreater (int 2) (int 1) (int 7) (var "bang")) null) (int 7)) ;add
(check-error (eval-under-env (ifgreater (int 5) (aunit) (aunit) (aunit)) null)) ;non int
(check-error (eval-under-env (ifgreater (aunit) (int 5) (aunit) (aunit)) null)) ;non int

;An mlet expression evaluates its first expression to a value v. 
;  Then it evaluates the second expression to a value, in an environment
;  extended to map the name in the mlet expression to v.
(check-expect (eval-under-env (mlet "let-x" (int 12) (var "y")) test-env) (int 4)) 
(check-expect (eval-under-env (mlet "let-x" (add (int 6) (int 6)) (var "let-x")) test-env) (int 12))
(check-expect (eval-under-env (mlet "y" (int 12) (var "y")) test-env) (int 12))  ;shadowing applicable ????????

;A call evaluates its first and second subexpressions to values. If the first is not a closure, it is an
;  error. Else, it evaluates the closure’s function’s body in the closure’s environment extended to
;  map the function’s name to the closure (unless the name field is #f) and the function’s argument 
;  to the result of the second subexpression.
(check-error (eval-under-env (call (aunit) (int 0)))) ;calling a non-closure
(check-expect (eval-under-env (call test-nonrec-fn (int 5)) test-env) (int 10))  ;no-env double input
(check-expect (eval-under-env (call test-closure (int 5)) test-env) (int 8)) ;adds input to x var in env
(check-expect (eval-under-env (call test-rec-fn (int 5)) test-env) (int 15))

;A pair expression evaluates its two subexpressions and produces a (new) pair holding the results.
(check-expect (eval-under-env test-pair null) 
              (apair (int 4) (int 5))) 

;A fst expression evaluates its subexpression. If the result for the subexpression is a pair, then the
;  result for the fst expression is the e1 field in the pair.
(check-expect (eval-under-env (fst test-pair) null) 
              (int 4))
(check-error (eval-under-env fst(aunit) null))

;A snd expression evaluates its subexpression. If the result for the subexpression is a pair, then
;  the result for the snd expression is the e2 field in the pair.
(check-expect (eval-under-env (snd test-pair) null) 
              (int 5))
(check-error (eval-under-env snd(aunit) null))

;An isaunit expression evaluates its subexpression. If the result is unit, then the result for the
;  isunit expression is the mupl integer 1, else the result is the mupl integer 0.
(check-expect (eval-exp (isaunit (aunit))) (int 1))
(check-expect (eval-exp (isaunit (add (int 3) (int 7)))) (int 0))

;** Problem 3 **
;a. returns a mupl expression that when run evaluates e1 and if the result is mupl’s aunit then it evaluates e2 and that is the overall result, else it evaluates e3 
(check-expect (eval-exp (ifaunit (aunit) (add (int 5) (int 4)) (int 10) )) (int 9)) ;e1 = aunit so give eval e2
(check-expect (eval-exp (ifaunit (int 11) (int 10) (add (int 5) (int 4)) )) (int 9)) ;e1 != aunit so give eval of e3

;b. mlet* sequential binding of list ot values, 
(check-expect (eval-exp (mlet* test-mlet*-env (add (int 10) (var "c")))) (int 16))
(check-expect (eval-exp (mlet* test-mlet*-env (add (int 10) (var "b")))) (int 13))

;c - ifeq
(check-expect (eval-exp (ifeq (int 1) (int 0) (aunit) (add (int 4) (int 4)))) (int 8))
(check-expect (eval-exp (ifeq (int 0) (int 1) (aunit) (add (int 4) (int 4)))) (int 8))
(check-expect (eval-exp (ifeq (int 1) (int 1) (add (int 4) (int 4)) (aunit))) (int 8))

;4 - Mupl map
(define test-mupl-map-fun (fun #f "x" (add (int 1) (var "x"))))
(define test-mupl-map-lst (apair (int 1) (apair (int 2) (apair (int 3) (aunit)))))
(define test-mupl-map-expected (apair (int 2) (apair (int 3) (apair (int 4) (aunit)))))

(check-expect (eval-exp (call (mupl-map test-mupl-map-fun) test-mupl-map-lst))
                        test-mupl-map-expected)

;5 

(check-expect (eval-exp (call (call mupl-mapAddN (int 1)) test-mupl-map-lst))
                        test-mupl-map-expected)

(test)

; a test case that uses problems 1, 2, and 4
; should produce (list (int 10) (int 11) (int 16))
;(define test1
;  (mupllist->racketlist
;   (eval-exp (call (call mupl-mapAddN (int 7))
;                   (racketlist->mupllist 
;                    (list (int 3) (int 4) (int 9)))))))
;(check-expect test1 (list (int 10) (int 11) (int 16)))