# Uses python3
def fib(n):
    if (n < 2):
        return n
    else:
        a = 0
        b = 1
        t = 0
        for x in range(2, n + 1):
            t = a + b
            #print(x," : ",a," ",b," ",t)
            a = b
            b = t
        return b

if __name__ == '__main__':
	n = int(input())
	print(fib(n))
