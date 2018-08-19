namespace Smart.Windows.Interactivity
{
    using System.Reflection;

    public class MethodDescriptor
    {
        public MethodInfo Method { get; }

        public bool HasParameter { get; }

        public MethodDescriptor(MethodInfo method, bool hasParameter)
        {
            Method = method;
            HasParameter = hasParameter;
        }
    }
}
