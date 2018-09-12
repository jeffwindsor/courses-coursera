# Uses python3
from ds import optimal_summands

def test(name, actual, expected):
    print(name, " : ", actual == expected, "[ ", actual, " : " ,expected, " ]")

test("Sample 1", optimal_summands(6), [1,2,3])
test("Sample 2", optimal_summands(8), [1,2,5])
test("Sample 3", optimal_summands(2), [2])
test("Odd Even", optimal_summands(3), [1,2])