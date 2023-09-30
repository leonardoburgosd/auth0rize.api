namespace cens.auth.application.Wrappers
{
    public class Response<T>
    {
        public Response() { }
        public Response(T data, string message = null) { Success = true; Message = message; Data = data; }
        public Response(string menssage) { Success = false; Message = menssage; }


        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}
