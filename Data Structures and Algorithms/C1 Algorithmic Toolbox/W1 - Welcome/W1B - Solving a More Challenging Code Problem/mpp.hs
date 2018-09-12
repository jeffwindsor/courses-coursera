import Data.Char

multiply (a,b) = a * b

top2 xs = foldl top2Max (0,0) xs
  where top2Max (a,b) x = (max a x, min a (max b x))

main :: IO()
main = do
    n <- getLine
    numbers <- getContents
    print( multiply $ top2 $ map read $ words numbers )
