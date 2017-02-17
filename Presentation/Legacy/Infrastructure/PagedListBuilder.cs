using System.Collections.Generic;
using PagedList;

namespace Presentation.Legacy.Infrastructure
{
    public static class PagedListBuilder<T> where T: class
    {
        public static IPagedList<T> Build(IEnumerable<T> entities, int pageNumber, int pageSize, int totalItemCount)
        {
            return new StaticPagedList<T>(entities, pageNumber, pageSize, totalItemCount);
        } 
    }
}