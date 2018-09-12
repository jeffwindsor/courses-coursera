# Uses python3
import sys

def merge_sort_and_count(A):
    if len(A) == 1:
        return 0

    m = int(len(a)/2)
    (lc, L) = merge_sort_and_count(A[:m])
    (rc, R) = merge_sort_and_count(A[m:])
    (ac, A) = merge_and_count(L,R)
    return lc+rc+ac, A

def merge_and_count(A,B):
    C = []
    count=i=j=0
    la = len(A)
    lb= len(B)
    while la > i and lb < j:
        C.extend(min(A[i],B[j]))
        if B[j] < A[i]:
            count += len(A)
            j += 1
        else:
            i += 1

    C.append(A if A else B)
    return count, C

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
