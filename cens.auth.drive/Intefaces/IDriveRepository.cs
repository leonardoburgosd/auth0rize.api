using cens.auth.drive.Entities;
using Microsoft.AspNetCore.Http;

namespace cens.auth.drive.Intefaces
{
    public interface IDriveRepository
    {
        Task<ResponseDrive<UpdateFileResponse>> uploadFile(IFormFile file, string token);
        Task<LoginResponse> auth(string usuario, string password);
    }
}