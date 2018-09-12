# Uses python3
import sys

def optimal_summands(n):
	summands = []
	i = 1
	r = n
	while(r > 0):
		summand = i if (r > i * 2) else r
		summands.append(summand)
		r -= summand
		i += 1
	return summands

if __name__ == '__main__':
    input = sys.stdin.read()
    n = int(input)
    summands = optimal_summands(n)
    print(len(summands))
    for x in summands:
        print(x, end=' ')
