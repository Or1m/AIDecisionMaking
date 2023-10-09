import numpy as np
import math

class CART:
    def __process_continuous_features(self, df, column_name):
        unique_values = sorted(df[column_name].unique())
        subset_ginis = []

        for i in range(0, len(unique_values) - 1):
            threshold = unique_values[i]

            subsetA = df[df[column_name] <= threshold]
            subsetB = df[df[column_name] > threshold]

            subsetA_rows = subsetA.shape[0]
            subsetB_rows = subsetB.shape[0]

            instances = df.shape[0]
            
            decision_for_subsetA = subsetA["Decision"].value_counts().tolist()
            decision_for_subsetB = subsetB["Decision"].value_counts().tolist()

            gini_subsetA = 1
            gini_subsetB = 1

            for j in range(0, len(decision_for_subsetA)):
                gini_subsetA = gini_subsetA - math.pow((decision_for_subsetA[j] / subsetA_rows), 2)

            for j in range(0, len(decision_for_subsetB)):
                gini_subsetB = gini_subsetB - math.pow((decision_for_subsetB[j] / subsetB_rows), 2)

            gini = (subsetA_rows / instances) * gini_subsetA + (subsetB_rows / instances) * gini_subsetB

            subset_ginis.append(gini)

        winner = subset_ginis.index(min(subset_ginis))
        winner_threshold = unique_values[winner]

        df[column_name] = np.where(df[column_name] <= winner_threshold, f"<= {winner_threshold}", f"> {winner_threshold}")
        return df

    def __find_decision(self, df):
        instances = df.shape[0]
        columns = df.shape[1]

        ginis = []

        for i in range(0, columns - 1):
            column_name = df.columns[i]
            column_type = df[column_name].dtypes

            if (column_type == "int64"):
                df = self.__process_continuous_features(df, column_name)

            classes = df[column_name].value_counts()
            gini = 0
            
            for j in range(0, len(classes)):
                current_class = classes.keys().tolist()[j]

                subdataset = df[df[column_name] == current_class]
                subdataset_instances = subdataset.shape[0]

                decision_list = subdataset["Decision"].value_counts().tolist()
                subgini = 1

                for k in range(0, len(decision_list)):
                    subgini = subgini - math.pow((decision_list[k]/ subdataset_instances), 2)

                gini = gini + (subdataset_instances / instances) * subgini

            ginis.append(gini)
            
        winner_idx = ginis.index(min(ginis))
        winner_name = df.columns[winner_idx]
        return winner_name

    def build_decision_tree(self, df, root):
        tmp_root = root
        df_copy = df.copy()

        winner_name = self.__find_decision(df)

        columns = df.shape[1]
        for i in range(0, columns - 1):
            column_name = df.columns[i]
            if column_name != winner_name:
                df[column_name] = df_copy[column_name]

        classes = df[winner_name].value_counts().keys().tolist()

        for i in range (0, len(classes)):
            current_class = classes[i]
            subdataset = df[df[winner_name] == current_class]
            subdataset = subdataset.drop(columns=[winner_name])
            
            self.print_output(root, subdataset, winner_name, current_class)
            root = tmp_root * 1

    def print_output(self, root, subdataset, winner_gain_name, current_class):
        if (len(subdataset["Decision"].value_counts().tolist()) == 1):
            final_decision = subdataset["Decision"].value_counts().keys().tolist()[0]
            print(f"{root}{winner_gain_name} is {current_class}")
            print(f"{root + 1}{final_decision}")
        elif subdataset.shape[1] == 1:
            final_decision = subdataset["Decision"].value_counts().idxmax()
            print(f"{root}{winner_gain_name} is {current_class}")
            print(f"{root + 1}{final_decision}")
        else:
            print(f"{root}{winner_gain_name} is {current_class}")
            root += 1
            self.build_decision_tree(subdataset, root)
