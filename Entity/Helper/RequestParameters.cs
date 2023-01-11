using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Helper
{
    public class RequestParameters
    {
        /// <summary>
        /// Max number of items per page
        /// </summary>
        const int MaxPageSize = 10;
        /// <summary>
        /// return the current page
        /// </summary>
        public int PageNumber { get; set; } = 1;
        
        private int _pageSize = 10;
        /// <summary>
        /// return number of items on a page
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
    }
}
