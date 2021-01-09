using BlazorFluentUI;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CarManagement.Client.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.Toast;

namespace CarManagement.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //Dependency Injections 
            builder.Services.AddBlazorFluentUI();
            builder.Services.AddBlazoredToast();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<ICarsService, CarsService>();

            await builder.Build().RunAsync();
        }
    }
}
