using Funq;
using ServiceStack;
using NUnit.Framework;
using TodoAPI.ServiceInterface;
using TodoAPI.ServiceModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using TodoAPI.DbModels;

namespace TodoAPI.Tests;

public class IntegrationTest
{
    const string BaseUri = "http://localhost:2000/";
    private readonly ServiceStackHost appHost;

    class AppHost : AppSelfHostBase
    {
        public AppHost() : base(nameof(IntegrationTest), typeof(TodosService).Assembly) { }

        public override void Configure(Container container)
        {
            container.Register<IDbConnectionFactory>(c =>
            new OrmLiteConnectionFactory("Server=localhost; Port=5432; User Id=user; Password=Pa$$w0rd; Database=ToDoTest", PostgreSqlDialect.Provider));

            using var db = container.Resolve<IDbConnectionFactory>().Open();
            db.DropAndCreateTable<Todo>();
            db.Save<Todo>(new Todo
            {
                Title = "InitialTitle",
                Description = "InitialDescription",
                PercentageOfCompleteness = 1,
                ExpirationDate = System.DateTime.UtcNow.AddYears(1)
            });
        }
    }

    public IntegrationTest()
    {
        appHost = new AppHost()
            .Init()
            .Start(BaseUri);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        appHost.Dispose();
    }

    [Test]
    public void Correct_Calling_All_Endpoints()
    {
        var client = new JsonServiceClient(BaseUri);

        //POST Test
        var addTodo = client.Post(new CreateTodo { 
            Description = "TestDescription",
            ExpirationDate = System.DateTime.UtcNow.AddYears(1),
            Title = "TestTitle"
        });
        Assert.That(addTodo.Id, Is.EqualTo(2));
        Assert.That(addTodo.Title, Is.EqualTo("TestTitle"));

        //GET All todos
        var todos = client.Get(new GetAllTodos{});
        Assert.That(todos, Is.Not.Null);

        //GET todo
        var todo = client.Get(new GetTodo
        {
            Id = 1
        });
        Assert.That(todo.Title, Is.EqualTo("InitialTitle"));

        //GET todos within date rage
        todos = client.Get(new GetTodosInGivenTime
        {
            Time = System.DateTime.UtcNow.AddMonths(11)
        });
        Assert.That(todos, Is.Not.Null);

        //PUT Todo object
        todo = client.Put(new UpdateTodo
        {
            Id = 1,
            Title = "InitialTitle",
            Description = "UpdatedTitle",
            PercentageOfCompleteness = 10,
            ExpirationDate = System.DateTime.UtcNow.AddYears(1).AddMinutes(1)
        });

        Assert.That(todo.Description, Is.EqualTo("UpdatedTitle"));

        //PUT Todo percentage
        todo = client.Put(new UpdatePercentage
        {
            Id = 1,
            PercentageOfCompleteness = 10
        });
        Assert.That(todo.PercentageOfCompleteness, Is.EqualTo(10));

        //DELETE Todo
        client.Delete(new DeleteTodo { Id = 1 });
        client.Delete(new DeleteTodo { Id = 2 });
        //GET customers
        todos = client.Get(new GetAllTodos { });
        Assert.That(todos, Is.Empty);
    }
}