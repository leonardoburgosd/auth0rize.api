using Microsoft.AspNetCore.Http;

namespace cens.auth.drive.Entities
{
    public class UpdateFileRequest
    {
        public string BaseCodeFolder { get; set; }
        public IFormFile File { get; set; }
        public bool OnSharedDrive { get; set; } = false;
        public int SharedDriveId { get; set; } = 0;
    }
}