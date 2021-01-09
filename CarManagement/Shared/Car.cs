using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;

namespace CarManagement.Shared
{
    [DebuggerDisplay("{DebuggerDisplay,nq")]
    public partial class Car
    {
        //Properties
        private string DebuggerDisplay
        {
            get { return ToString(); }
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MinLength(5, ErrorMessage = "Name must be at least {1} characters long.")]
        public string Name { get; set; }

        [Range(typeof(DateTime), "1950-01-01", "2020-12-31", ErrorMessage = "Production date must be between {1} and {2}.")]
        public DateTime DateProduced { get; set; } = DateTime.ParseExact("2020-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
        public string Color { get; set; }

        [RegularExpression(@"^[0-9]+([\.,]?[0-9]*)?$", ErrorMessage = "Km/Lt must be a valid number.")]
        [Range(1.0, 500.0, ErrorMessage = "Kilometers/Liter must be between {1} and {2}.")]
        public double KmPerLiter { get; set; }

        [Range(1, 10, ErrorMessage = "Cylinders must be between {1} and {2}.")]
        public int Cylinders { get; set; }

        [Range(1, 300, ErrorMessage = "Horse Power must be between {1} and {2}.")]
        public int HorsePower { get; set; }

        public bool IsStillProduced { get; set; }

        //Methods
        public override string ToString()
        {
            return $"{Id}. {Name}";
        }

        /// <summary>
        /// Copy values to target Car.
        /// </summary>
        public void CopyTo(Car Target)
        {
            if (Target == null)
                return;

            Target.Id = Id;
            Target.Name = Name;
            Target.DateProduced = DateProduced;
            Target.Color = Color;
            Target.KmPerLiter = KmPerLiter;
            Target.Cylinders = Cylinders;
            Target.HorsePower = HorsePower;
            Target.IsStillProduced = IsStillProduced;

        }
    }
}
