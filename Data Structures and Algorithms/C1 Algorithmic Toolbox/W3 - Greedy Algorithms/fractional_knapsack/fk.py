# Uses python3
import sys

def fk(capacity, weights, values):
    result = 0.0

    if (capacity == 0):
        return result

    rates = [(v / w, v, w) for v, w in zip(values, weights)]
    for (rate, value, weight) in sorted(rates, reverse=True):
        if ( capacity > weight ):
            result += value
            capacity -= weight
        else :
            return result + (rate * capacity)

    return result

if __name__ == "__main__":
    data = list(map(int, sys.stdin.read().split()))
    n, capacity = data[0:2]
    values = data[2:(2 * n + 2):2]
    weights = data[3:(2 * n + 2):2]
    opt_value = fk(capacity, weights, values)
    print("{:.4f}".format(opt_value))
