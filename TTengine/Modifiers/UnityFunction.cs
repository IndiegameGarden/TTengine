
namespace TTengine.Modifiers
{
    /// <summary>
    /// A Function that returns the input value.
    /// </summary>
    public class UnityFunction: Function
    {
        public override double Value(double inp)
        {
            return inp;
        }
    }
}
