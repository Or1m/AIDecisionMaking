import numpy as np
import math

class ID3:
    def __process_continuous_features(self, df, column_name, entropy):
        unique_values = sorted(df[column_name].unique())
        subset_gains = []

        for i in range(0, len(unique_values) - 1):
            threshold = unique_values[i]

            subsetA = df[df[column_name] <= threshold]
            subsetB = df[df[column_name] > threshold]

            instances = df.shape[0]
            
            subsetA_probability = subsetA.shape[0] / instances
            subsetB_probability = subsetB.shape[0] / instances

            threshold_gain = entropy - subsetA_probability * self.__calculate_entropy(subsetA) - subsetB_probability * self.__calculate_entropy(subsetB)
            subset_gains.append(threshold_gain)

        winner = subset_gains.index(max(subset_gains))
        winner_threshold = unique_values[winner]

        df[column_name] = np.where(df[column_name] <= winner_threshold, f"<= {winner_threshold}", f"> {winner_threshold}")
        return df

    def __calculate_entropy(self, df):
        if df["Decision"].dtype != "object":
            return 0

        instances = df.shape[0]
        decisions = df["Decision"].value_counts().keys().tolist()
        entropy = 0

        for i in range (0, len(decisions)):
            num_of_decisions = df["Decision"].value_counts().tolist()[i]

            class_probability = num_of_decisions / instances
            entropy = entropy - class_probability * math.log(class_probability, 2)

        return entropy

    def __find_decision(self, df):
        entropy = self.__calculate_entropy(df)

        instances = df.shape[0]
        columns = df.shape[1]
        gains = []

        for i in range(0, columns - 1):
            column_name = df.columns[i]
            column_type = df[column_name].dtypes

            if (column_type == "int64"):
                df = self.__process_continuous_features(df, column_name, entropy)

            classes = df[column_name].value_counts()
            gain = entropy
            
            for j in range(0, len(classes)):
                current_class = classes.keys().tolist()[j]
                subdataset = df[df[column_name] == current_class]
                
                subdataset_entropy = self.__calculate_entropy(subdataset)

                subdataset_instances = subdataset.shape[0]
                class_probability = subdataset_instances / instances

                gain = gain - class_probability * subdataset_entropy
                
            gains.append(gain)
        
        winner_idx = gains.index(max(gains))
        winner_name = df.columns[winner_idx]
        return winner_name

    def build_decision_tree(self, df, root):
        tmp_root = root
        df_copy = df.copy()

        winner_name = self.__find_decision(df)

        for i in range(0, df.shape[1] - 1):
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

    def print_output(self, root, subdataset, winner_name, current_class):
        if (len(subdataset["Decision"].value_counts().tolist()) == 1):
            final_decision = subdataset["Decision"].value_counts().keys().tolist()[0]
            print(f"{root}{winner_name} is {current_class}")
            print(f"{root + 1}{final_decision}")
        elif subdataset.shape[1] == 1:
            final_decision = subdataset["Decision"].value_counts().idxmax()
            print(f"{root}{winner_name} is {current_class}")
            print(f"{root + 1}{final_decision}")
        else:
            print(f"{root}{winner_name} is {current_class}")
            root += 1
            self.build_decision_tree(subdataset, root)
