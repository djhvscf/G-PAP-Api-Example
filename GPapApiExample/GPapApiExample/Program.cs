﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GPapApiExample
{
    class Program
    {
        private static HttpClient _httpClient;
        private const string REST_URL = "https://localhost:44378/{0}";

        static void Main(string[] args)
        {
            User user = LoginAsync().Result;

            if (user != null)
            {
                CreatePackage(user.accessToken);
                //GetMyPackagesAsync(user.accessToken).Wait();
            }
        }

        /// <summary>
        /// Método para loguear en el api y obtener un accessToken
        /// </summary>
        /// <returns></returns>
        private static async Task<User> LoginAsync()
        {
            User user = null;
            try
            {
                string username = "intcomex";
                string password = ">+@d3a9_R7m.^m5E";

                var uri = new Uri(string.Format(REST_URL, "api/v1/User/login"));
                var jsonRequest = JsonConvert.SerializeObject(new { Username = username, Password = password });
                var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                requestMessage.Content = stringContent;

                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                }

                var response = await _httpClient.SendAsync(requestMessage);

                //if (response.IsSuccessStatusCode)
                //{
                var content = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<User>(content);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return user;
        }

        private static async void CreatePackage(string token)
        {
            Package package = new Package()
            {
                Address = "CALLE 32 con AV 2, DIAGONAL A LA EMBAJADA DE ESPAÑA. Edf. Alefbet",
                Address2 = string.Empty,
                AddressDelivery = "12345, test, test, TEST, test, 1, 1, 1",
                AddressDelivery2 = string.Empty,
                Barrio = "San Bosco",
                BigPackages = 0,
                Cantidad = 1,
                Canton = "San José Centro",
                CantonDelivery = "TEST",
                ClientName = "test test test",
                Comments = "costa test",
                Distrito = "Mata Redonda",
                DistritoDelivery = "TEST",
                FechaRuta = "11-11-2020",
                Latitude = string.Empty,
                LatitudeDelivery = string.Empty,
                LittlePackages = 1.0,
                Longitude = string.Empty,
                LongitudeDelivery = string.Empty,
                MediumPackages = 0,
                MethodPayment = "Cash",
                PaidOrigin = false,
                Phone = "1234567890",
                Phone2 = string.Empty,
                PhoneDelivery = "25691811",
                PhoneDelivery2 = string.Empty,
                Product = "costa test:1",
                Provincia = "test",
                ProvinciaDelivery = "TEST",
                Quantity = 49.0,
                Transport = "Motorcycle",
                Bill = "MyTestMyTest",
                ToGAM = true
            };

            try
            {


                var client = new RestClient("https://localhost:44378/");
                //var request = new RestRequest("api/v1/Package/Create", Method.POST);
                //request.RequestFormat = DataFormat.Json;
                //request.AddBody(pack);
                //request.AddHeader("access-token", token);

                //var response = client.Execute(request);
                //var obj = JObject.Parse(response.Content);

                var uri = new Uri(string.Format(REST_URL, "api/v1/Package/Create"));
                var jsonRequest = JsonConvert.SerializeObject(package);
                var stringContent = new StringContent(jsonRequest, Encoding.UTF8, "text/json");

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
                requestMessage.Content = stringContent;

                //requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $" {token}");

                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");

                var response = _httpClient.PostAsync(uri, new StringContent(jsonRequest, Encoding.UTF8, "application/json")).Result;

                //if (response.IsSuccessStatusCode)
                //{
                var content = await response.Content.ReadAsStringAsync();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Ejemplo de como obtener todos los paquetes creados por mi usuario
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static async Task GetMyPackagesAsync(string token)
        {
            try
            {
                var uri = new Uri(string.Format(REST_URL, "api/Package/getPackages"));

                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

                requestMessage.Headers.Add("access-token", token);

                if (_httpClient == null)
                {
                    _httpClient = new HttpClient();
                }

                var response = await _httpClient.SendAsync(requestMessage);

                //if (response.IsSuccessStatusCode)
                //{
                var content = await response.Content.ReadAsStringAsync();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
