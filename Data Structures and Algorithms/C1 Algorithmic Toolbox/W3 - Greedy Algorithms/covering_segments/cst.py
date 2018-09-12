# Uses python3
from cs import optimal_points, Segment

def test(name, actual, expected):
    print(name, " : ", actual == expected, "[ ", actual, " : " ,expected, " ]")

def tops(tuples):
	segments = [Segment(a,b) for a,b in tuples]
	return optimal_points(segments)

test("Sample 1", tops([(1,3),(2,5),(3,6)]), [3])
test("Sample 2", tops([(4,7),(1,3),(2,5),(5,6)]), [3, 6])

test("Overlap", tops([(1,13),(11,15),(5,9),(3,7)]), [7, 15])
test("Overlap", tops([(1,10),(11,20),(21,30)]), [10, 20, 30])
test("Overlap", tops([(1,3),(2,6),(5,7)]), [3, 7])