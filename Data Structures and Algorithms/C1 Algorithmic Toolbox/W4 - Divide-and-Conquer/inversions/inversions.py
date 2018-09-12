# Uses python3
import sys

def mergeSort(a, inversions):
    if len(a)<2: return a

    m = int(len(a)/2)
    return merge(mergeSort(a[:m], inversions), mergeSort(a[m:], inversions))

def merge(l,r, inversions):
    result=[]
    i=j=0
    while i<len(l) and j<len(r):
        if l[i] < r[j]:
            result.append(l[i])
            i+=1
        else:
            result.append(r[j])
            j+=1
    while (i<len(l)):
        result.append(l[i])
        i+=1
    while (j<len(r)):
        result.append(r[j])
        j+=1
    return result




def get_number_of_inversions(a, b, left, right):
    number_of_inversions = 0
    if right - left <= 1:
        return number_of_inversions
    ave = (left + right) // 2
    number_of_inversions += get_number_of_inversions(a, b, left, ave)
    number_of_inversions += get_number_of_inversions(a, b, ave, right)
    #write your code here
    return number_of_inversions

if __name__ == '__main__':
    input = sys.stdin.read()
    n, *a = list(map(int, input.split()))
    b = n * [0]
    print(get_number_of_inversions(a, b, 0, len(a)))
