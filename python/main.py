import sys
import pandas as pd
import os.path

from ID3_final import ID3
from D45_final import D45
from CART_final import CART

valid_algorithms = ["ID3", "D45", "CART"]
default_path = "real_data/skeleton_actions.csv"
default_algorithm = valid_algorithms[0]
default_separator = ';'

# Usage: python .\main.py [algorithm - id3/d45/cart] [path to csv] [separator] > filename.txt
if __name__ == "__main__":
    path = default_path
    algorithm = default_algorithm
    separator = default_separator

    if len(sys.argv) <= 1:
        print("No arguments provided")
        sys.exit(0)
    elif len(sys.argv) > 1:
        alg = sys.argv[1].upper()
        if alg not in valid_algorithms:
            print("Invalid algorithm provided")
            sys.exit(0)
        algorithm = alg
        
        if len(sys.argv) > 2:
            pth = sys.argv[2]
            if os.path.isfile(pth):
                path = pth
            else:
                print("Bad filepath provided")
                sys.exit(0)

        if len(sys.argv) > 3:
            sep = sys.argv[3]
            if len(sep) == 1:
                separator = sep
            else:
                print("Bad separator provided")
                sys.exit(0)


    df = pd.read_csv(path, sep=separator)
    # You have to modify this line if your data are in different format
    df.rename(columns = {'Sees enemy':'Sees enemy', 'Hear enemy':'Hear enemy', 'Enemy in range':'Enemy in range', 'HP':'HP', 'Action':'Decision'}, inplace = True)
    
    tree = None

    if algorithm == "ID3":
        tree = ID3()
    elif algorithm == "D45":
        tree = D45()
    else:
        tree = CART()

    tree.build_decision_tree(df, 0)