using Microsoft.AspNetCore.Mvc;
using CarManagement.Server.Services;
using System.Linq;
using System.Threading.Tasks;
using CarManagement.Shared;
using System;

namespace CarManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        //Fields
        /// <summary>
        /// HostingEnvironment to read local JSON file. 
        /// </summary>
        private readonly ICarContext _carContext;

        //Constructors
        /// <summary>
        /// Inject Controller. 
        /// </summary>
        public CarsController(ICarContext CarContext)
        {
            _carContext = CarContext;
        }

        //API 
        /// <summary>
        /// Get all cars. 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _carContext.GetAsync();
            return Ok(cars);
        }

        /// <summary>
        /// Get car by id. 
        /// </summary>
        [HttpGet("{CarId}")]
        public async Task<IActionResult> GetCar(int CarId)
        {
            var cars = await _carContext.GetAsync();
            var car = cars.FirstOrDefault(c => c.Id == CarId);

            if (car == null)
                return BadRequest($"Car with Id='{CarId}' not found.");
            else
                return Ok(car);
        }

        [HttpPost()]
        public async Task<IActionResult> AddCar(Car NewCar)
        {
            //Check if Car exists in Context
            var dbCar = await _carContext.GetAsync(NewCar.Id);

            //If not exists, save  
            if (dbCar == null)
            {
                await _carContext.SaveAsync(NewCar);

                string message = $"Car with Id='{NewCar.Id}' has beed added.";
                var response = new ServiceResponse<Car>(true, message, NewCar);
                return Ok(response);
            }
            //Else return 
            else
            {
                string message = $"Car with Id='{NewCar.Id}' already exists.";
                var response = new ServiceResponse<Car>(false, message);
                return BadRequest(response);
            }
        }

        [HttpPut("{CarId}")]
        public async Task<IActionResult> UpdateCar(int CarId, Car NewCar)
        {
            //Check if Car exists in Context
            var dbCar = await _carContext.GetAsync(CarId);
            NewCar.Id = CarId;

            //If not exists, return 
            if (dbCar == null)
            {
                string message = $"Car with Id='{CarId}' not found.";
                var response = new ServiceResponse<Car>(false, message);
                return BadRequest(response);
            }
            //Else save new values 
            else
            {
                await _carContext.SaveAsync(NewCar);

                string message = $"Car with Id='{NewCar.Id}' has beed updated.";
                var response = new ServiceResponse<Car>(true, message, NewCar);
                return Ok(response);
            }
        }

        [HttpDelete("{CarId}")]
        public async Task<IActionResult> DeleteCar(int CarId)
        {
            //Check if Car exists in Context
            var dbCar = await _carContext.GetAsync(CarId);

            //If not exists, return 
            if (dbCar == null)
            {
                string message = $"Car with Id='{CarId}' not found.";
                var response = new ServiceResponse<int>(false, message);
                return BadRequest(response);
            }
            //Else delete 
            else
            {
                await _carContext.DeleteAsync(CarId);

                string message = $"Car with Id='{CarId}' has beed deleted.";
                var response = new ServiceResponse<int>(true, message, CarId);
                return Ok(response);
            }
        }

        /// <summary>
        /// For demo puproses only. 
        /// Resets Context data. 
        /// </summary>
        [HttpPost("Reset")]
        public Task<IActionResult> Reset()
        {
            try
            {
                _carContext.HardReset();

                string message = $"Context has been hard reset.";
                var response = new ServiceResponse<Car>(true, message);
                return Task.FromResult<IActionResult>(Ok(response));
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                var response = new ServiceResponse<Car>(true, message);
                return Task.FromResult<IActionResult>(BadRequest(response));
            }
        }
    }
}
