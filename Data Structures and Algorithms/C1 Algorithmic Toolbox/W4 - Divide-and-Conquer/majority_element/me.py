# Uses python3
import sys

def get_majority_element(a, n):
    if (n == 1):
        #print("one array ", a)
        return a[0]

    mid = n // 2
    l = get_majority_element(a[:mid], mid)
    r = get_majority_element(a[mid:], n - mid)

    if(l == r):
        #print("equal: ", l, " = ",r," ", a)
        return l
    lc = a.count(l)
    #print("left count: [", l, "] ",lc," > ", mid, " ", a)
    if(lc > mid):
        return l
    rc = a.count(r)
    #print("right count: [", r, "] ",rc, " > ", mid, " ", a)
    if(rc > mid):
        return r

    #print("no majority: ", a)
    return -1

if __name__ == '__main__':
    input = sys.stdin.read()
    n, *a = list(map(int, input.split()))
    if get_majority_element(a, n) != -1:
        print(1)
    else:
        print(0)
