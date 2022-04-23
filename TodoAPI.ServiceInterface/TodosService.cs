using ServiceStack;
using ServiceStack.OrmLite;
using System;
using TodoAPI.DbModels;
using TodoAPI.ServiceModel;

namespace TodoAPI.ServiceInterface;
public class TodosService : Service
{

    #region Gets implementations
    //Get all todos.
    //I wasn't sure if I had to check whether is not completed or if not expired so I'm returning all of them as it was said.
    public object Get(GetAllTodos getAllTodosRequest)
    {
        //Returning not done: var todos = Db.Select<Todo>(x => x.PercentageOfCompleteness != 100);
        //Returning not expired: var todos = Db.Select<Todo>(x => x.ExpirationDate >= DateTime.Now);

        //Get all elements of a table.
        var todos = Db.Select<Todo>();
        return todos;
    }

    //Get individual todo.
    public object Get(GetTodo getTodoRequest)
    {
        //Get element of a table by given Id.
        var todo = Db.SingleById<Todo>(getTodoRequest.Id);
        if (todo == null) return HttpError.BadRequest("Not found Todo item with given Id."); //Returning Http 404 error with given message.
        return todo;
    }

    //Get todos in given time.
    //My implementation of GetIncomingToDo operation. In url send in UTC format time, than Db selects all 
    //todos which date is beetwen today and date given in url.
    public object Get(GetTodosInGivenTime getTodosInGivenTimeRequest)
    {
        var todos = Db.Select<Todo>(x => x.ExpirationDate >= DateTime.UtcNow && x.ExpirationDate <= getTodosInGivenTimeRequest.Time);
        return todos;
    }
    #endregion

    #region Posts implementations
    //Create todo
    public object Post(CreateTodo createTodoRequest)
    {
        var todo = new Todo
        {
            Title = createTodoRequest.Title,
            Description = createTodoRequest.Description,
            ExpirationDate = createTodoRequest.ExpirationDate,
        };

        Db.Save(todo);
        return todo;
    }
    #endregion

    #region Puts implementations
    //Update todo.
    public object Put(UpdateTodo updateTodoRequest)
    {
        var todo = Db.SingleById<Todo>(updateTodoRequest.Id);
        if(todo == null) return HttpError.BadRequest("Not found Todo item with given Id.");

        todo.Title = updateTodoRequest.Title;
        todo.Description = updateTodoRequest.Description;
        todo.PercentageOfCompleteness = updateTodoRequest.PercentageOfCompleteness;
        todo.ExpirationDate = updateTodoRequest.ExpirationDate;
        
        Db.Update(todo);
        return todo;
    }

    //Update percentage of todo.
    //IMPORTANT: If Percentage from request is 100 it means that Todo is Done!
    public object Put(UpdatePercentage updatePercentageRequest)
    {
        var todo = Db.SingleById<Todo>(updatePercentageRequest.Id);
        if (todo == null) return HttpError.BadRequest("Not found Todo item with given Id.");

        todo.PercentageOfCompleteness = updatePercentageRequest.PercentageOfCompleteness;

        Db.Update(todo);
        return todo;
    }
    #endregion

    #region Deletes Implementations
    //Delete todo.
    public object Delete(DeleteTodo deleteTodoRequest)
    {
        //Checking if exists Todo with given id in Database.
        var isAnyTodo = Db.Exists<Todo>(x => x.Id == deleteTodoRequest.Id);
        if(!isAnyTodo) return HttpError.BadRequest("Not found Todo item with given Id.");

        Db.DeleteById<Todo>(deleteTodoRequest.Id);
        return new { };
    }
    #endregion
}