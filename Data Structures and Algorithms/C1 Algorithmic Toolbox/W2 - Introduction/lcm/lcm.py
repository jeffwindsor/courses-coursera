# Uses python3
import sys

def lcm(a, b):
    def gcd(a, b):
        return a if (b == 0) else gcd(b,a % b)

    return (a * b) // gcd(a,b)

if __name__ == '__main__':
    input = sys.stdin.read()
    a, b = map(int, input.split())
    print(lcm(a, b))
