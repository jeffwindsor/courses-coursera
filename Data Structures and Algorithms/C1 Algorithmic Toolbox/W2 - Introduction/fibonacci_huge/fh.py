# Uses python3
import sys

def pisanoPeriodLength(n):
    if(n < 2):
        return 1

    end = [0,1]
    limit = (n**2) - 1
    c = end
    for i in range(1, limit + 1):
        c = [c[1], sum(c)%n]
        if c == end:
            return i
    return limit

def fib(n):
    if (n < 2):
        return n
    else:
        a = 0
        b = 1
        t = 0
        for x in range(2, n + 1):
            t = a + b
            a = b
            b = t
        return b

def fh(n, m):
    nprime = n % pisanoPeriodLength(m)
    return fib(nprime) % m

if __name__ == '__main__':
    input = sys.stdin.read();
    n, m = map(int, input.split())
    print(fh(n, m))
