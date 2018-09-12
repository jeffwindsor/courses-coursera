# Uses python3
from bs import binary_search

def test(name, actual, expected):
    print(name, " : ", actual == expected, "[ ", actual, " : " ,expected, " ]")

for (x, expected) in zip([8, 1, 23, 1, 11],[2, 0, -1, 0, -1]):
    test("Sample 1", binary_search([1, 5, 8, 12, 13], x), expected)
