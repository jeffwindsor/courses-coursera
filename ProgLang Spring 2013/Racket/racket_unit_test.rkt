#lang racket

;##############################################
; TEST FUNCTIONS
;##############################################
(define (test name expected actual)
  (display 
    (if (equal? expected actual)
        (string-append "PASSED: " name "\n")
        (string-append "FAILED: " name ". Expected [" expected "] but was [" actual "]\n")
        )))