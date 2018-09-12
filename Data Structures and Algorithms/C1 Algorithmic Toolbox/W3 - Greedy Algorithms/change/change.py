# Uses python3
import sys

def get_coins(amount, coins):
    if amount == 0 or not coins:
        return 0
    coin = coins[0]
    (amount, remainder) = (0, amount) if (coin > amount) else (amount // coin, amount % coin)
    return amount + get_coins(remainder, coins[1:])

def get_change(amount):
    initial_coins = [10,5,1]
    return get_coins(amount, initial_coins)

if __name__ == '__main__':
    n = int(sys.stdin.read())
    print(get_change(n))
