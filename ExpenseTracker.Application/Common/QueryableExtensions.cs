namespace ExpenseTracker.Application.Common
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            int page = pageNumber < 1 ? 1 : pageNumber;
            int size = pageSize < 1 ? 10 : (pageSize > 50 ? 50 : pageSize);

            return query.Skip((page - 1) * size).Take(size);
        }
    }
}
