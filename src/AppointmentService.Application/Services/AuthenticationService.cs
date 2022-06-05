using AppointmentService.Domain.Models;
using AppointmentService.Domain.Services;
using AppointmentService.Shared.Dto;
using AppointmentService.Shared.Settings;
using AppointmentService.Shared.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OperationResult;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentService.Application.Services
{
    public sealed class AuthenticationService : AuthenticationServiceImp
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        public AuthenticationService(HttpClient httpClient, AppSettings appSettings)
        {
            _httpClient = httpClient;
            _appSettings = appSettings;
        }

        public async Task<Result<AuthResponseViewModel>> LogIn(AuthenticationRequest user)
        {
            try
            {
                var request = new StringContent(
                    JsonConvert.SerializeObject(user, Formatting.Indented,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }),
                        Encoding.UTF8,
                        "application/json");

                using var httpResponse = await 
                    _httpClient.PostAsync($"v1/accounts:signInWithPassword?key={_appSettings.FirebaseToken}",
                    request);

                httpResponse.EnsureSuccessStatusCode();

                var responseObject = await httpResponse.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<AuthResponseViewModel>(responseObject, 
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                return result;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
