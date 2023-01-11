namespace Entity.Models
{
    public class ApiResponse
    {


        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ApiResponse(string message = null)
        {
            Succeeded = true;
            Message = message;
        }
        public static ApiResponse Success(object data)
        {
            return new ApiResponse { Data = data, Message = "Success" };
        }
    }
}