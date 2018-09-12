# Uses python3
from me import get_majority_element

def test(name, actual, expected):
    print(name, " : ", actual == expected, "[ ", actual, " : " ,expected, " ]")

test("Sample 1", get_majority_element([2,3,9,2,2], 5), 2)
test("Sample 1", get_majority_element([2,2,9,3,2], 5), 2)

test("Sample 2", get_majority_element([1,2,3,4], 4), -1)
test("Sample 3", get_majority_element([1,2,3,1], 4), -1)
