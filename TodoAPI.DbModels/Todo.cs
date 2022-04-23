using ServiceStack.DataAnnotations;

namespace TodoAPI.DbModels;
public class Todo
{
    [AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int PercentageOfCompleteness { get; set; } = 1;
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpirationDate { get; set; }
}
