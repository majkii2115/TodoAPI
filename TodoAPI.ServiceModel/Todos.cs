using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAPI.DbModels;

namespace TodoAPI.ServiceModel;

#region Gets
[Route("/todos/getAll", "GET")]
public class GetAllTodos : IReturn<IEnumerable<Todo>> {}

[Route("/todos/getOne/{Id}", "GET")]
public class GetTodo : IReturn<Todo> 
{
    public int Id { get; set; }
}

[Route("/todos/getInTime/{Time}", "GET")]
public class GetTodosInGivenTime : IReturn<IEnumerable<Todo>> 
{
    public DateTime Time { get; set; }
}

#endregion

#region Posts
[Route("/todos/add", "POST")]
public class CreateTodo : IReturn<Todo>
{
    public string Title { get; set; }
    public string Descritpion { get; set; }
    public DateTime ExpirationDate { get; set; }
}
#endregion

#region Puts
[Route("/todos/updateTodo/{Id}", "PUT")]
public class UpdateTodo : IReturn<Todo>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Descritpion { get; set; }
    public int PercentageOfCompleteness { get; set; }
    public DateTime ExpirationDate { get; set; }
}

[Route("/todos/updatePercentage/{Id}", "PUT")]
public class UpdatePercentage : IReturn<Todo>
{
    public int Id { get; set; }
    public int Percentage { get; set; }
}
#endregion

#region Deletes
[Route("/todos/delete/{Id}", "DELETE")]
public class DeleteTodo : IReturnVoid
{
    public int Id { get; set; }
}
#endregion