# Uses python3
import sys


def get_coins(amount, coins):
    if coins = []:
        return []
    coin = coin[0]
    split = (0, amount) if (coin > amount) else (amount // coin, mount % coin)
    return (coin, fst split) :: get_coins(snd split, tail coins)

def get_change(amount):
    initial_coins = [10,5,1]
    coin_amounts = get_coins(amount, coins)
    amounts = zip(*coin_amounts)[1]
    return sum(amounts)


if __name__ == '__main__':
    n = int(sys.stdin.read())
    print(get_change(n))
