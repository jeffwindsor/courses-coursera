#lang racket

(require test-engine/racket-tests) 
(require "hw4.rkt") 

(check-expect (sequence 3 11 2) '(3 5 7 9 11)) ;"sequence - in order by stride"
(check-expect (sequence 3 8 3) '(3 6)) ;"sequence - high not included if not on stride"       
(check-expect (sequence 3 2 1) '());  "sequence - high > low => empty list"       

(check-expect (string-append-map '("one" "two" "three") "") '("one" "two" "three")) ; "string-append-map - no suffix fine"             
(check-expect (string-append-map '("one" "two" "three") ".jpg") '("one.jpg" "two.jpg" "three.jpg"));  "string-append-map - suffix on end with no spaces"      

(check-error (list-nth-mod '(1 2 3 4 5) -1) "list-nth-mod: negative number")
(check-error (list-nth-mod '() 1) "list-nth-mod: empty list")
(check-expect (list-nth-mod '(0 1 2 3 4) 0) 0)
(check-expect (list-nth-mod '(0 1 2 3 4) 2) 2)
(check-expect (list-nth-mod '(0 1 2 3 4) 17) 2)

(define (test-stream x) (cons x (lambda() (test-stream (+ x 1)))))
(define s (lambda() (test-stream 1)))

(check-error (stream-for-n-steps s 0) "streams-for-n-steps: non positive number")
(check-expect (stream-for-n-steps s 1) '(1))
(check-expect (stream-for-n-steps s 5) '(1 2 3 4 5))

(check-expect (stream-for-n-steps funny-number-stream 10) '(1 2 3 4 -5 6 7 8 9 -10))

(check-expect (stream-for-n-steps dan-then-dog 5) 
              '("dan.jpg" "dog.jpg" "dan.jpg" "dog.jpg" "dan.jpg"))

(check-expect (stream-for-n-steps (stream-add-zero dan-then-dog) 5) 
              '((0 . "dan.jpg") (0 . "dog.jpg") (0 . "dan.jpg") (0 . "dog.jpg") (0 . "dan.jpg")))

(check-expect (stream-for-n-steps (cycle-lists '(1 2 3) '("a" "b")) 8) 
              '((1 . "a") (2 . "b") (3 . "a") (1 . "b") (2 . "a") (3 . "b") (1 . "a") (2 . "b")))

;process in order
(check-expect (vector-assoc 3 (vector (cons 1 "B") (cons 3 "A") (cons 3 "B"))) (cons 3 "A"))
;allows the skipping of non pairs
(check-expect (vector-assoc 3 (vector (cons 1 "B") 1 (cons 2 "C") 3 (cons 3 "A"))) (cons 3 "A"))
;not found = false
(check-expect (vector-assoc "A" (vector (cons 1 "B") (cons 2 "C") (cons 3 "A"))) #f)
(check-expect (vector-assoc "A" #()) #f)

(define ls '((1 . "a") (2 . "b") (3 . "a") (4 . "b") (5 . "a") (6 . "b") (7 . "a") (8 . "b")))
(define ca (cached-assoc ls 5))
(check-expect (ca 3) (cons 3 "a"))
(check-expect (ca 8) (cons 8 "b"))


(test)
;(test "list-nth-mod"      '() (list-nth-mod '(1 2 3 4 5) 0))