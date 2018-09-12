# Uses python3
import sys
from collections import namedtuple

Segment = namedtuple('Segment', 'start end')

def optimal_points(segments):
    points = []

    window = Segment(-1, 1000000001)
    for s in sorted(segments):
        if(s.start <= window.end):
            #print( window, " + ", s, " => Close Window to ", Segment(max(window.start, s.start), min(window.end, s.end)))
            window = Segment(max(window.start, s.start), min(window.end, s.end))
        else:
            #print( window, " + ", s, " => Append Point ", window.end)
            points.append(window.end)
            window = s
    
    #Append last valid point
    if(window.start != -1):
        points.append(window.end)

    return points

if __name__ == '__main__':
    input = sys.stdin.read()
    n, *data = map(int, input.split())
    segments = list(map(lambda x: Segment(x[0], x[1]), zip(data[::2], data[1::2])))
    points = optimal_points(segments)
    print(len(points))
    for p in points:
        print(p, end=' ')
