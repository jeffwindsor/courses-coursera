# Uses python3
import sys

def binary_search(xs, x):
    def inner(xs, low, high, key):
        if(high < low):
            return - 1
        mid = ((high - low) // 2) + low
        midv = xs[mid]
        if(key == midv):
            return mid
        elif(key < midv):
            return inner(xs, low, mid - 1, key)
        else:
            return inner(xs, mid + 1, high, key)

    return inner(xs, 0, len(xs) - 1, x)

if __name__ == '__main__':
    input = sys.stdin.read()
    data = list(map(int, input.split()))
    n = data[0]
    m = data[n + 1]
    a = data[1 : n + 1]
    for x in data[n + 2:]:
        # replace with the call to binary_search when implemented
        print(binary_search(a, x), end = ' ')
