using Blazored.Toast.Services;
using CarManagement.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CarManagement.Client.Services
{
    /// <summary>
    /// Cars Service implementation 
    /// </summary>
    public class CarsService : ICarsService
    {
        //Events
        /// <summary>
        /// Mainly to notify UI. 
        /// </summary>
        public event Action CarsChanged;

        //Fields 
        /// <summary>
        /// HttpClient for REST Api. 
        /// </summary>
        private readonly HttpClient _httpClient;
        private readonly IToastService _toastService;

        //Properties
        /// <summary>
        /// Car collection. 
        /// </summary>
        public IList<Car> Cars { get; set; } = new List<Car>();

        //Constructors
        /// <summary>
        /// Inject services. 
        /// </summary>
        public CarsService(HttpClient HttpClient, IToastService ToastService)
        {
            _httpClient = HttpClient;
            _toastService = ToastService;
        }

        //Methods 
        /// <summary>
        /// Invoke CarChanged event.
        /// </summary>
        protected void OnCarsChanged()
        {
            CarsChanged?.Invoke();
        }

        /// <summary>
        /// For development purpose only. 
        /// Request hard reset on Context. 
        /// </summary>
        public async Task HardReset()
        {
            var result = await _httpClient.PostAsJsonAsync("api/Cars/Reset", string.Empty);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<Car>>();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                _toastService.ShowSuccess(response.Message);
                OnCarsChanged();
            }
            else
                _toastService.ShowError(response.Message);
        }

        /// <summary>
        /// Fetch all cars. 
        /// </summary>
        public async Task FetchCarsAsync()
        {
            Cars = await _httpClient.GetFromJsonAsync<IList<Car>>("api/Cars");

            OnCarsChanged();
        }

        /// <summary>
        /// Delete car. 
        /// </summary>
        /// <returns></returns>
        public async Task DeleteCarAsync(int CarId)
        {
            var result = await _httpClient.DeleteAsync($"api/Cars/{CarId}");
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                _toastService.ShowSuccess(response.Message);

                //Update temp Car collection as well, without refetching
                Cars = Cars.Where(c => c.Id != CarId).ToList();

                //Notify UI
                OnCarsChanged();
            }
            else
                _toastService.ShowError(response.Message);
        }

        /// <summary>
        /// Insert new car. 
        /// </summary>
        public async Task<int> InsertCarAsync(Car NewCar)
        {
            var result = await _httpClient.PostAsJsonAsync($"api/Cars", NewCar);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<Car>>();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                _toastService.ShowSuccess(response.Message);

                //Update temp Car collection as well, without refetching
                NewCar = response.Data;
                Cars.Add(NewCar);

                //Notify UI 
                OnCarsChanged();

                //Return new Id
                return NewCar.Id;
            }
            else
            {
                //Notify UI 
                _toastService.ShowError(response.Message);
                return -1;
            }
        }

        /// <summary>
        /// Update car. 
        /// </summary>
        /// <returns></returns>
        public async Task UpdateCarAsync(int CarId, Car NewCar)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/Cars/{CarId}", NewCar);
            var response = await result.Content.ReadFromJsonAsync<ServiceResponse<Car>>();

            if (result.StatusCode == HttpStatusCode.OK)
            {
                _toastService.ShowSuccess(response.Message);

                //Update temp Car collection as well, without refetching
                var dbCar = Cars.FirstOrDefault(c => c.Id == CarId);
                NewCar.CopyTo(dbCar);

                //Notify UI
                OnCarsChanged();
            }
            else
                _toastService.ShowError(response.Message);
        }
    }
}
