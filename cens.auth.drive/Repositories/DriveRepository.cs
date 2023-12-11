using System.Net.Http.Headers;
using cens.auth.drive.Entities;
using cens.auth.drive.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace cens.auth.drive.Repositories
{
    public class DriveRepository : IDriveRepository
    {
        #region Inyeccion
        private string baseLogin = "";
        private string baseFileAvatar = "";
        private string endpointFile = "";

        public DriveRepository(IConfiguration configuration)
        {
            endpointFile = Environment.GetEnvironmentVariable(configuration["drive:access:base"]!.ToString());
            baseLogin = Environment.GetEnvironmentVariable(configuration["drive:access:auth"]!.ToString());
            baseFileAvatar = Environment.GetEnvironmentVariable(configuration["drive:baseAvatar"]!.ToString());
        }
        #endregion

        public async Task<LoginResponse> auth(string usuario, string password)
        {
            LoginResponse? response = new LoginResponse();
            using (var cliente = new HttpClient())
            {
                var contenido = new StringContent(JsonConvert.SerializeObject(new { Username = usuario, Password = password }), System.Text.Encoding.UTF8, "application/json");
                var respuesta = await cliente.PostAsync(baseLogin + "autenticacion/login", contenido);
                if (respuesta.IsSuccessStatusCode)
                {
                    var resultado = await respuesta.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<LoginResponse>(resultado);
                }
            }
            return response!;
        }

        public async Task<ResponseDrive<UpdateFileResponse>> uploadFile(IFormFile file, string token)
        {
            ResponseDrive<UpdateFileResponse>? response = new ResponseDrive<UpdateFileResponse>();

            using (HttpClient client = new HttpClient())
            using (MultipartFormDataContent formData = new MultipartFormDataContent())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                formData.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                formData.Add(new StringContent(baseFileAvatar), "baseCodeFolder");
                formData.Add(new StringContent("true"), "onSharedDrive");
                formData.Add(new StringContent("10"), "sharedDriveId");

                HttpResponseMessage responseMssg = await client.PostAsync(endpointFile + "file/upload", formData);
                if (responseMssg.IsSuccessStatusCode)
                {
                    string result = await responseMssg.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ResponseDrive<UpdateFileResponse>>(result);
                }
            }
            return response!;
        }
    }
}