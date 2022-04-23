using Funq;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using TodoAPI.DbModels;
using TodoAPI.ServiceInterface;

[assembly: HostingStartup(typeof(TodoAPI.AppHost))]

namespace TodoAPI;

public class AppHost : AppHostBase, IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            // Configure ASP.NET Core IOC Dependencies
        });

    public AppHost() : base("TodoAPI", typeof(TodosService).Assembly) {}

    public override void Configure(Container container)
    {
        //Connecting to PostreSql local database with given below connection string.
        container.Register<IDbConnectionFactory>(c =>
            new OrmLiteConnectionFactory("Server=localhost; Port=5432; User Id=user; Password=Pa$$w0rd; Database=ToDo", PostgreSqlDialect.Provider));
        using var db = container.Resolve<IDbConnectionFactory>().Open();
        db.CreateTableIfNotExists<Todo>();
    }
}
