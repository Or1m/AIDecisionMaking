using Characters;
using Characters.Enums;
using System;
using System.IO;

namespace Trees
{
    public class DecisionTree
    {
        private DecisionTreeNode root;
        public DecisionTreeNode Root { get => root; set => root = value; }

        public DecisionTree() : this(null) { }
        public DecisionTree(DecisionTreeNode root) { this.root = root; }


        public ENPCAction Traverse(NPCBody body)
        {
            var result = TraverseInternal(root, body);
            
            if (!Enum.TryParse(result, out ENPCAction action))
                throw new ArgumentOutOfRangeException(nameof(action), "Failed to convert value to enum");

            return action;
        }
        private string TraverseInternal(DecisionTreeNode node, NPCBody body)
        {
            var next = node.Visit(body);
            if (next == null)
                return node.Value;

            return TraverseInternal(next, body);
        }

        public static DecisionTree FromFile(string filePath)
        {
            var tree = new DecisionTree();
            var lines = File.ReadAllLines(filePath);

            tree.Root = new DecisionTreeNode();
            ParseFileRecursive(tree.Root, lines, 0);

            return tree;
        }
        private static void ParseFileRecursive(DecisionTreeNode node, string[] lines, int iter)
        {
            var line = lines[iter];
            var level = int.Parse(line[..1]);
            bool isFalseNode = false;

            if (iter == lines.Length - 1 || int.Parse(lines[iter + 1][..1]) < level)
            {
                node.Value = node.Value[1..];
                return;
            }

            line = line[1..];

            if (line.Contains("False"))
            {
                isFalseNode = true;
                node.Value = line.Split(' ')[0];
            }
            else if (line.Contains("True"))
            {
                node.Value = line.Split(' ')[0];
            }
            else if (line.Contains("<="))
            {
                isFalseNode = true;
                node.Value = line.Replace("is ", "");
            }
            else if (line.Contains(">"))
            {
                node.Value = line.Replace("is ", "");
            }

            if (isFalseNode)
            {
                node.NegativeChild = new DecisionTreeNode(lines[++iter]);
                ParseFileRecursive(node.NegativeChild, lines, iter);

                int index = -1;
                for (int i = iter; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(level.ToString()))
                    {
                        index = i;
                        break;
                    }
                }

                if (index != -1)
                {
                    node.PositiveChild = new DecisionTreeNode(lines[++index]);
                    ParseFileRecursive(node.PositiveChild, lines, index);
                }
            }
            else
            {
                node.PositiveChild = new DecisionTreeNode(lines[++iter]);
                ParseFileRecursive(node.PositiveChild, lines, iter);

                int index = -1;
                for (int i = iter; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith(level.ToString()))
                    {
                        index = i;
                        break;
                    }
                }

                if (index != -1)
                {
                    node.NegativeChild = new DecisionTreeNode(lines[++index]);
                    ParseFileRecursive(node.NegativeChild, lines, index);
                }
            }
        }
    }
}
