namespace Characters.Skeleton
{
    public class SkeletonBrain : NPCBrain
    {
        private readonly string treeFileName = "skeleton";
        protected override string TreeFileName => treeFileName;

        private readonly StateMachine stateMachine = new SkeletonStateMachine();
        protected override StateMachine StateMachine => stateMachine;
    }
}
