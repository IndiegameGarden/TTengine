// (c) 2017 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt

namespace Game1.Systems
{
    /// <summary>
    /// Defines the order in which all Game1 Systems are executed
    /// </summary>
    public class SystemsSchedule
    {
        // Systems in UPDATE loop
        public const int
            LevelBuilderSystem = 5,
            InputToMotionSystem = 15;

        // Systems in DRAW loop
        // ...

    }
}
