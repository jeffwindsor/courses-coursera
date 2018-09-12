# Uses python3
from fk import fk

def test(name, actual, expected):
    actual = "{:.4f}".format(actual)
    print(name, " : ", actual == expected, "[ ", actual, " : " ,expected, " ]")

test("Sample 1", fk(50, [20,50,30],[60,100,120]), "180.0000")
test("Sample 2", fk(10, [30], [500]), "166.6667")
test("Case 2 of 13", fk(1000, [30], [500]), "500.0000")
