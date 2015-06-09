
namespace PXengine.Systems
{
    /// <summary>
    /// Defines the order in which all Systems are executed
    /// </summary>
    public class SystemsSchedule
    {
        // Systems in UPDATE loop
        public const int
            UserControlSystem = 1,
            ScrollingSystem = 1,
            ThingSystem = 1;

        // Systems in DRAW loop
        //public const int 
    }
}
