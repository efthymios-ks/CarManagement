using System.Collections.Generic;

namespace CarManagement.Shared
{
    public partial class Car
    {
        /// <summary>
        /// Available Car colors
        /// </summary>
        public static IEnumerable<string> AvailableColors { get; } = new string[]
        {
            "Black", "Red", "White", "Yellow", "Blue", "Green"
        };

    }
}
