using Characters;
using System;

namespace Trees
{
    public class DecisionTreeNode
    {
        private string value;
        public string Value { get => value; set => this.value = value; }

        private DecisionTreeNode negativeChild;
        public DecisionTreeNode NegativeChild { get => negativeChild; set => negativeChild = value; }

        private DecisionTreeNode positiveChild;
        public DecisionTreeNode PositiveChild { get => positiveChild; set => positiveChild = value; }

        public DecisionTreeNode() : this(string.Empty) { }
        public DecisionTreeNode(string value) { this.value = value; }


        public DecisionTreeNode Visit(NPCBody body)
        {
            switch(value)
            {
                case "SeesEnemy":
                    return (body.SeesEnemy) ? positiveChild : negativeChild;
                case "HearEnemy":
                    return (body.HearEnemy) ? positiveChild : negativeChild;
                case "EnemyInRange":
                    return (body.EnemyInRange) ? positiveChild : negativeChild;
                case string s when s.Contains("HP >"):
                    return (body.Health > GetNumericValue(value)) ? positiveChild : negativeChild;
                default:
                    return null;
            }
        }

        private static int GetNumericValue(string value)
        {
            var parts = value.Split(' ');

            if (!int.TryParse(parts[parts.Length - 1], out var num))
                throw new ArgumentException("Input in wrong format");

            return num;
        }
    }
}
