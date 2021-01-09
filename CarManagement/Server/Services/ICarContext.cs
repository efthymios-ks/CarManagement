using CarManagement.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarManagement.Server.Services
{
    /// <summary>
    /// Car Context provider.
    /// </summary>
    public interface ICarContext
    {
        //Methods
        /// <summary>
        /// Get single Car. 
        /// </summary>
        Task<Car> GetAsync(int CarId);
        /// <summary>
        /// Get all Cars. 
        /// </summary>
        Task<IEnumerable<Car>> GetAsync();

        /// <summary>
        /// Delete Car. 
        /// </summary>
        Task<bool> DeleteAsync(int CarId);
        /// <summary>
        /// Delete Car. 
        /// </summary>
        Task<bool> DeleteAsync(Car Car);

        /// <summary>
        /// Save single Car. 
        /// If CarId exists, then saves.
        /// If CarId does not exist, then calculates new available Id and inserts. 
        /// </summary>
        Task SaveAsync(Car Car);
        /// <summary>
        /// Save all Cars. 
        /// If CarId exists, then saves.
        /// If CarId does not exist, then calculates new available Id and inserts. 
        /// </summary>
        Task SaveAsync(IEnumerable<Car> Cars);

        /// <summary>
        /// For development purposes only.
        /// Resets Context data. 
        /// </summary>
        void HardReset();
    }
}
