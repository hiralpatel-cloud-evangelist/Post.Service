using Post.Service.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Service.DTO.FilterDto;

namespace Post.Service.Base.HelperClasses
{
    public class Pagination<T> where T : class
    {
        public static async Task Data(int pageSize, int pageIndex, PagedViewResponse<T> list, IQueryable<T> data)
        {
            list.Data = await data
                                .Skip(pageSize * (pageIndex - 1))
                                .Take(pageSize)
                                .ToListAsync();

            list.CurrentPage = pageIndex;
            list.RecordCount = await data.CountAsync();
            list.NumberofPages = Convert.ToInt32(Math.Ceiling((double)list.RecordCount / pageSize));
            list.PageSize = pageSize;

            var pages = new List<int>();
            if (pageIndex > 2)
            {
                pages.Add(pageIndex - 2);
                pages.Add(pageIndex - 1);
            }
            else if (pageIndex > 1)
            {
                pages.Add(pageIndex - 1);
            }
            if (list.RecordCount > 0)
                pages.Add(pageIndex);
            if (pageIndex + 1 <= list.NumberofPages && pages.Count() < 10)
                pages.Add(pageIndex + 1);
            if (pageIndex + 2 <= list.NumberofPages && pages.Count() < 10)
                pages.Add(pageIndex + 2);
            if (pageIndex + 3 <= list.NumberofPages && pages.Count() < 10)
                pages.Add(pageIndex + 3);
            if (pageIndex + 4 <= list.NumberofPages && pages.Count() < 10)
                pages.Add(pageIndex + 4);
            if (pageIndex + 5 <= list.NumberofPages && pages.Count() < 10)
                pages.Add(pageIndex + 5);
            if (pageIndex + 6 <= list.NumberofPages && pages.Count() < 10)
                pages.Add(pageIndex + 6);
            if (pageIndex + 7 <= list.NumberofPages && pages.Count() < 10)
                pages.Add(pageIndex + 7);
            if (pageIndex + 8 <= list.NumberofPages && pages.Count() < 10)
                pages.Add(pageIndex + 8);
            if (pageIndex + 9 <= list.NumberofPages && pages.Count() < 10)
                pages.Add(pageIndex + 9);

            list.Pages = pages;
        }
    }
}
