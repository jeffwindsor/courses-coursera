# Uses python3
from lcm import lcm

for (a,b,e) in [(6,8,24),(28851538,1183019,1933053046),(226553150,1023473145,46374212988031350)]:
    actual = lcm(a,b)
    if ( e == actual ):
        print (a,",",b , " -> ", e)
    else:
        print (a,",",b , " -> ", e, " <> ", actual)
