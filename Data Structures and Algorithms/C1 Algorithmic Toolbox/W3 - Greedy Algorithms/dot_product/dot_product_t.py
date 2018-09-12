from dot_product import min_dot_product

def test(name, actual, expected):
    print(name, " : ", actual==expected, "[ ", actual, " : " ,expected, " ]")

test("Sample 1", min_dot_product([23], [39]), 897)
test("Sample 2", min_dot_product([1,3,-5], [-2,4,1]), -25)
