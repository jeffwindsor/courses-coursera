#lang racket
(provide (all-defined-out))

(define (sequence low high stride)
  (if (> low high)
      null
      (cons low (sequence (+ low stride) high stride))))

(define (string-append-map xs suffix)
  (map (lambda(s) (string-append s suffix)) xs))

(define (list-nth-mod xs n)
  (cond [(< n 0)(error "list-nth-mod: negative number")]
        [(null? xs)(error "list-nth-mod: empty list")]
        [#t (if (= (remainder n (length xs)) 0)
                (car xs)
                (car (list-tail xs (remainder n (length xs)))))])) 

(define (stream-for-n-steps s n)
  (cond [(< n 1)(error "streams-for-n-steps: non positive number")]
        [#t (let ([pr (s)])
              (cons (car pr) (if (= n 1)
                                 null
                                 (stream-for-n-steps (cdr pr) (- n 1)))))]))

(define funny-number-stream
  (letrec ([f (lambda (x) (cons (if (= 0 (modulo x 5)) (- x) x) (lambda() (f (+ x 1)))))])
    (lambda() (f 1))))

(define dan-then-dog
  (letrec ([f (lambda (x) (cons (if (odd? x) "dan.jpg" "dog.jpg") (lambda() (f (+ x 1)))))])
    (lambda() (f 1))))

(define (stream-add-zero s)
  (let ([pr (s)])
    (lambda() (cons (cons 0 (car pr))
                    (stream-add-zero (cdr pr))))))

(define (cycle-lists xs ys)
  (letrec ([f (lambda (n) (cons (cons (list-nth-mod xs n) (list-nth-mod ys n))
                                (lambda() (f (+ n 1)))))])
    (lambda() (f 0))))

(define (vector-assoc v vec)
  (letrec ([f (lambda(i) 
                (cond [(= i (vector-length vec)) #f]
                      [(and (pair? (vector-ref vec i)) 
                            (equal? (car (vector-ref vec i)) v)) 
                       (vector-ref vec i)]
                      [#t (f (+ i 1))]))])
    (f 0)))

 ;(define v1 (cached-assoc (list (list 1 2) (list 3 4) (list 5 6)) 5))
(define (cached-assoc xs n)
  (let ([cache (make-vector n)]
        [i 0])
    (lambda(v) 
      (let ([answer (vector-assoc v cache)])
        (if (equal? #f answer)
            (begin [set! answer (assoc v xs)]
                   [vector-set! cache i answer]
                   [if (= (+ i 1) n) 
                       (set! i 0) 
                       (set! i (+ i 1))]
                   answer)
            answer)))))

