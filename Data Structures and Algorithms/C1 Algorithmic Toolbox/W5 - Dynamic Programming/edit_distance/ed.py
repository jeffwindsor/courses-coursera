# Uses python3
def memoize(f):
    memo = {}
    def helper(x):
        if x not in memo:
            memo[x] = f(x)
        return memo[x]
    return helper

def substCost(x,y):
    if x == y: return 0
    else: return 2

@memoize
def edit_distance(s, t):
    i = len(t); j = len(s)

    if i == 0:  return j
    elif j == 0: return i

    return(min(edit_distance(t[:i-1], s) + 1,
        edit_distance(t, s[:j-1]) + 1,
        edit_distance(t[:i-1], s[:j-1]) + substCost(s[j-1], t[i-1])))

if __name__ == "__main__":
    print(edit_distance(input(), input()))
