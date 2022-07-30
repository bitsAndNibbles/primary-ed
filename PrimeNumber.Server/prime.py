#!/bin/env python3

def is_prime(n: int) -> bool:
    '''
    Returns True iff n is prime. From:
    https://stackoverflow.com/questions/15285534/isprime-function-for-python-language
    '''
    if n == 2 or n == 3: return True
    if n < 2 or n%2 == 0: return False
    if n < 9: return True
    if n%3 == 0: return False
    r = int(n**0.5)
    # since all primes > 3 are of the form 6n ± 1
    # start with f=5 (which is prime)
    # and test f, f+2 for being prime
    # then loop by 6. 
    f = 5
    while f <= r:
        if n % f == 0: return False
        if n % (f+2) == 0: return False
        f += 6
    return True

def next_prime(n) -> int:
    while True:
        n += 1
        if is_prime(n):
            break

    return n

if __name__ == '__main__':

    from datetime import datetime

    s = 100000000000000
    print(f"Primes after {s}:")

    start = datetime.now()
    for n in range(5):
        s = next_prime(s)
        print('\t' + str(s))
    stop = datetime.now()

    print(f"Cost: {stop - start} seconds")