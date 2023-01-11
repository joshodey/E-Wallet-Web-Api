namespace Entity.DTOModels
{
    public class APIListResponseDto : ApiResponse
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool  HasNextPage { get; set; }
        public int CurrentPage { get; set; }

        public static APIListResponseDto Success(object data,int CurrentPage, int page, int totalPage,
            int totalCount, bool hasPreviousPage, bool hasNextPage)
        {
            return new APIListResponseDto
            {
                CurrentPage=CurrentPage,
                Page = page,
                TotalPages = totalPage,
                TotalCount = totalCount,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage,
                Data = data
            };
        }

    }

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
            return new ApiResponse {Data = data, Message = "Success"};
        }
    }

}
