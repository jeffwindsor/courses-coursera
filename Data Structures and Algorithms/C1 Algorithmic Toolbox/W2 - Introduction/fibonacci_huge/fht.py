# Uses python3
from fh import pisanoPeriodLength, fh

def testPisanoPeriodLength():
	print("PISANO PERIOD")
	expected = [0,1,3,8,6,20,24,16,12,24,60,10,24,28,48,40,24,36,24,18,60,16,30,48,24,100,84,72,48,14,120,30,48,40,36,80,24,76,18,56,60,40,48,88,30,120,48,32,24,112,300]
	for i in range(1, len(expected) + 1):
		p = pisanoPeriodLength(i)
		print(" ",i,"->",p," ", p == expected[i])
		assert(pisanoPeriodLength(i) == expected[i])

#testPisanoPeriodLength()
assert(fh(281621358815590,30524) == 11963)

#for (a,b) in [(1,2),(99,19824153),(get_fibonaccihuge)]:
#    print (a,",",b , " -> ", )
