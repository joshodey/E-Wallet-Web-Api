using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;

namespace Entity.DTOModels
{
    public class APIPageResponseListDto : ApiResponse
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public static APIPageResponseListDto Success(object data, int page, int totalPage,
            int totalCount, bool hasPreviousPage, bool hasNextPage)
        {
            return new APIPageResponseListDto
            {
                Page = page,
                TotalPages = totalPage,
                TotalCount = totalCount,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage,
                Data = data
            };
        }

       
    }
}
