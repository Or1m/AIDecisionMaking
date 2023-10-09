namespace Characters.Senses
{
    public interface ISense<T>
    {
        bool IsActivated { get; }
        float DistanceToTarget { get; }
        T Target { get; }

        bool TryGetTarget(out T outTarget);
    }
}
