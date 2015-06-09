using Artemis.Interface;

namespace PXengine.Comps
{
    /// <summary>
    /// Health of an Entity
    /// </summary>
    public class HealthComp: IComponent
    {
        /// <summary>Current health value where 0 or less means dead</summary>
        public double Health = 100;

        public void DecreaseHealth(double amount)
        {
            Health -= amount;
        }
    }
}
