namespace BlogApp.Core.Entities;

public class BlogCategory:BaseEntity
{
    public Blog Blog { get; set; }
    public int BlogId { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
}


