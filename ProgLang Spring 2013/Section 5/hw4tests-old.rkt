#lang racket

(require "hw4.rkt") 

;; A simple library for displaying a 2x3 grid of pictures: used
;; for fun in the tests below (look for "Tests Start Here").

(require (lib "graphics.rkt" "graphics"))

(open-graphics)

(define window-name "Programming Languages, Homework 4")
(define window-width 700)
(define window-height 500)
(define border-size 100)

(define approx-pic-width 200)
(define approx-pic-height 200)
(define pic-grid-width 3)
(define pic-grid-height 2)

(define (open-window)
  (open-viewport window-name window-width window-height))

(define (grid-posn-to-posn grid-posn)
  (when (>= grid-posn (* pic-grid-height pic-grid-width))
    (error "picture grid does not have that many positions"))
  (let ([row (quotient grid-posn pic-grid-width)]
        [col (remainder grid-posn pic-grid-width)])
    (make-posn (+ border-size (* approx-pic-width col))
               (+ border-size (* approx-pic-height row)))))

(define (place-picture window filename grid-posn)
  (let ([posn (grid-posn-to-posn grid-posn)])
    ((clear-solid-rectangle window) posn approx-pic-width approx-pic-height)
    ((draw-pixmap window) filename posn)))

(define (place-repeatedly window pause stream n)
  (when (> n 0)
    (let* ([next (stream)]
           [filename (cdar next)]
           [grid-posn (caar next)]
           [stream (cdr next)])
      (place-picture window filename grid-posn)
      (sleep pause)
      (place-repeatedly window pause stream (- n 1)))))

;; Tests Start Here

; These definitions will work only after you do some of the problems
; so you need to comment them out until you are ready.
; Add more tests as appropriate, of course.

(define nums (sequence 0 5 1))

(define files (string-append-map 
               (list "dan" "dog" "curry" "dog2") 
               ".jpg"))

(define funny-test (stream-for-n-steps funny-number-stream 16))

; a zero-argument function: call (one-visual-test) to open the graphics window, etc.
(define (one-visual-test)
  (place-repeatedly (open-window) 0.5 (cycle-lists nums files) 27))

; similar to previous but uses only two files and one position on the grid
(define (visual-zero-only)
  (place-repeatedly (open-window) 0.5 (stream-add-zero dan-then-dog) 27))


;#########################################
; MY TESTS
;#########################################
(define (test name expected actual)
  (if (equal? expected actual)
      (display (string-append "PASSED: " name "\n"))
      (begin [display (string-append "FAILED: " name ". Expected [")]
             [display expected]
             [display "] but was ["]
             [display actual]
             [display "]\n"])
      ))

;(test "Not Equal Test" "A" "B") 
;(test "Equal Test" "A" "A") 
;(test "Int Equal Test" 1 1 ) 
;(test "List Equal Test" (list 1 2 3 4) (list 1 2 3 4)) 
;#############################################

(test "sequence - in order by stride" 
      '(3 5 7 9 11) (sequence 3 11 2))
(test "sequence - high not included if not on stride" 
      '(3 6) (sequence 3 8 3))
(test "sequence - high > low => empty list" 
      '() (sequence 3 2 1) )

(test "string-append-map - no suffix fine"
      '("one" "two" "three") 
      (string-append-map '("one" "two" "three") ""))
(test "string-append-map - suffix on end with no spaces"
      '("one.jpg" "two.jpg" "three.jpg") 
      (string-append-map '("one" "two" "three") ".jpg"))

;(require test-engine/racket-tests) 


(check-error
 (list-nth-mod '(1 2 3 4 5) -1)
 ("list-nth-mod: negative number"))
;(test "list-nth-mod"      '() (list-nth-mod '(1 2 3 4 5) 0))