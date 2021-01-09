using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using CarManagement.Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarManagement.Server.Services
{
    /// <summary>
    /// CarContext json implementation.
    /// </summary>
    public class CarJsonContext : ICarContext
    {
        //Fields 
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _carsRelativePath = @"Assets\Json\Cars.json";
        private readonly string _carsBackupRelativePath = @"Assets\Json\CarsBackup.json";

        //Properties
        /// <summary>
        /// Get Cars json file path. 
        /// </summary>
        private string CarsPath
        {
            get
            {
                string absolutePath = _hostEnvironment.ContentRootPath;
                string relativePath = _carsRelativePath;
                return Path.Combine(absolutePath, relativePath);
            }
        }
        /// <summary>
        /// Get backup Cars json file path. 
        /// </summary>
        private string CarsBackupPath
        {
            get
            {
                string absolutePath = _hostEnvironment.ContentRootPath;
                string relativePath = _carsBackupRelativePath;
                return Path.Combine(absolutePath, relativePath);
            }
        }

        //Constructors
        /// <summary>
        /// Inject needed services/ 
        /// </summary>
        public CarJsonContext(IWebHostEnvironment HostEnvironment)
        {
            _hostEnvironment = HostEnvironment;
        }

        //Methods 
        /// <summary>
        /// Get single Car. 
        /// </summary>
        public async Task<Car> GetAsync(int CarId)
        {
            var cars = await GetAsync();
            return cars.FirstOrDefault(c => c.Id == CarId);
        }

        /// <summary>
        /// Get all Cars. 
        /// </summary>
        public async Task<IEnumerable<Car>> GetAsync()
        {
            //Read and deserialize data 
            string data = await File.ReadAllTextAsync(CarsPath);
            return JsonConvert.DeserializeObject<IEnumerable<Car>>(data);
        }

        /// <summary>
        /// Delete Car. 
        /// </summary>
        public async Task<bool> DeleteAsync(int CarId)
        {
            //Get all Cars
            var cars = await GetAsync();
            int initialCount = cars.Count();

            //Remove by Id 
            cars = cars.Where(c => c.Id != CarId);
            int finalCount = cars.Count();

            //Save back
            await SaveAsync(cars);

            //Return true if Cars removed
            return (initialCount != finalCount);
        }

        /// <summary>
        /// Delete Car. 
        /// </summary>
        public async Task<bool> DeleteAsync(Car Car)
        {
            if (Car == null)
                return await Task.FromResult(false);
            else
                return await DeleteAsync(Car.Id);
        }

        /// <summary>
        /// Save single Car. 
        /// If CarId exists, then saves.
        /// If CarId does not exist, then calculates new available Id and inserts. 
        /// </summary>
        public async Task SaveAsync(Car Car)
        {
            if (Car == null)
                return;

            //Get all Cars 
            var cars = (await GetAsync()).ToList();

            //Check if Car exists 
            var dbCar = cars.FirstOrDefault(c => c.Id == Car.Id);

            //If not exists, add to list 
            if (dbCar == null)
            {
                //Mark CarId = -1, so it get's a new incremental Id when saved
                Car.Id = -1;
                cars.Add(Car);
            }
            //Else update its values
            else
                Car.CopyTo(dbCar);

            //Save Cars back to Context
            await SaveAsync(cars);
        }

        /// <summary>
        /// Save all Cars. 
        /// If CarId exists, then saves.
        /// If CarId does not exist, then calculates new available Id and inserts. 
        /// </summary>
        public async Task SaveAsync(IEnumerable<Car> Cars)
        {
            //Give new Incremntal Id to new entries
            int nextId = Cars.Max(c => c.Id) + 1;
            if (nextId == 0)
                nextId = 1;

            foreach (var car in Cars.Where(c => c.Id <= 0))
                car.Id = nextId++;

            //Serialize and write 
            string content = JsonConvert.SerializeObject(Cars, Formatting.Indented);
            await File.WriteAllTextAsync(CarsPath, content);
        }

        /// <summary>
        /// For development purposes only.
        /// Resets Context data. 
        /// </summary>
        public void HardReset()
        {
            /*
            string data = await File.ReadAllTextAsync(CarsBackupPath);
            var cars = JsonConvert.DeserializeObject<IEnumerable<Car>>(data);
            string content = JsonConvert.SerializeObject(cars, Formatting.Indented);
            await File.WriteAllTextAsync(CarsPath, content);
            */

            File.Copy(CarsBackupPath, CarsPath, true);
        }
    }
}
