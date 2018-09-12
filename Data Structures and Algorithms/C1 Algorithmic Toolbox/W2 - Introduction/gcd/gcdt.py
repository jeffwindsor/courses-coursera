# Uses python3
from gcd import gcd

for (a,b) in [(1,2),(99,19824153),(2000000000,128)]:
    print (a,",",b , " -> ", gcd(a,b))
