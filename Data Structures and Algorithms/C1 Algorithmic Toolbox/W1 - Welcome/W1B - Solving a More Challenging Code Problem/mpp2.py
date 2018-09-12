# Uses python3
max1 = 0
max2 = 0
n = int(input())
for i in [int(x) for x in input().split()]:
    if i > max1:
        max2 = max1
        max1 = i
    elif i > max2:
        max2 = i

print(max1 * max2)
