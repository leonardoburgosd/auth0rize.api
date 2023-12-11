using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cens.auth.drive.Entities
{
    public class ResponseDrive<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}