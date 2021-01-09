using CarManagement.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarManagement.Client.Services
{
    /// <summary>
    /// Cars Service interface 
    /// </summary>
    public interface ICarsService
    {
        //Events
        /// <summary>
        /// Mainly to notify UI. 
        /// </summary>
        event Action CarsChanged;

        //Properties
        IList<Car> Cars { get; set; }

        //Methods        
        Task HardReset();
        Task FetchCarsAsync();
        Task DeleteCarAsync(int CarId);
        Task<int> InsertCarAsync(Car NewCar);
        Task UpdateCarAsync(int CarId, Car NewCar);
    }
}
