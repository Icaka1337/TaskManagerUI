namespace TaskManagerUI.Models
{
    public class PageResultViewModel<T>
    {
        
            public List<T> Items { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int PageSize { get; set; }
            public int TotalCount { get; set; }

            public PageResultViewModel()
            {
                Items = new List<T>();
            }
        
    }
}
