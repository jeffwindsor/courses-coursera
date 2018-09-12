t#lang racket  ;always used in dr racket
(provide (all-defined-out)) 
(define x 3)  ;val x = 3
(define y(+ x 2)) ;val y = x + 2
;fun cube1 x = x * x * x
(define cube1 (lambda (x)(* x x x)))
;syntatic sugar
(define (cube2 x)(* x x x ))

;if =>  (if e1 e2 e3) no then else
(define (pow1 x y)
  (if (= y 0) 1
      (* x (pow1 x (- y 1)))))

;**************************************
; Lists
;**************************************
(define (my-sum xs)
  (if (null? xs)
      0
      (+ (car xs) (my-sum (cdr xs)))))

(define (my-append xs ys)
  (if (null? xs)
      ys
      (cons (car xs) (my-append (cdr xs) ys))))

(define (my-map f xs)
  (if (null? xs)
      null
      (cons (f (car xs)) (my-map (cdr xs)))))