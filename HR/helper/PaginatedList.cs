namespace HR.helper
{
    public class PaginatedList<T> where T : class
    {
        public List<T> Items { get; set; }

        public int pagesCount { get; set; }

        static int pageSize = 5;

        public PaginatedList(List<T> items, int NoOfPage)
        {
            Items = items;
            pagesCount = NoOfPage;
        }
        public static PaginatedList<T> CreatePagnation(List<T> source, int pageNumber)
        {
            int countPages = (int)Math.Ceiling(source.Count / (double)pageSize);

            List<T> item = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(item, countPages);
        }
    
}
}
