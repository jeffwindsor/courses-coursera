# Uses python3
import sys

def fld(n):
    if (n < 2):
        return n
    else:
        a = 0
        b = 1
        t = 0
        for x in range(2, n + 1):
            t = (a + b) % 10
            a = b
            b = t
        return b

if __name__ == '__main__':
    input = sys.stdin.read()
    n = int(input)
    print(fld(n))
