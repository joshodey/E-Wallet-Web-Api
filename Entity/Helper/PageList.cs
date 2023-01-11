using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Helper
{
    public class PageList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PageList(List<T> users, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                CurrentPage = pageNumber
            };
            AddRange(users);
        }
        public static PageList<T> ToPagedList(IEnumerable<T> sources, int pageNumber, int pageSize)
        {
            var count = sources.Count();

            var items = sources
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();

            return new PageList<T>(items, count, pageNumber, pageSize);
        }

    }
}
