#Uses python3

import sys

def min_dot_product(xs, ys):
    xs = sorted(xs)
    ys = sorted(ys, reverse=True)
    products = [x*y for x, y in zip(xs, ys)]
    return sum(products)

if __name__ == '__main__':
    input = sys.stdin.read()
    data = list(map(int, input.split()))
    n = data[0]
    a = data[1:(n + 1)]
    b = data[(n + 1):]
    print(min_dot_product(a, b))
