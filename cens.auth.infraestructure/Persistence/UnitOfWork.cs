using cens.auth.domain.Application;
using cens.auth.domain.Primitives;
using cens.auth.domain.User;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace cens.auth.infraestructure.Persistence;
public class UnitOfWork : IUnitOfWork
{
    public IUserRepository User { get; private set; }

    public IApplicationRepository Application { get; private set; }


    private readonly IDbConnection _connection;

    public UnitOfWork(IConfiguration configuration)
    {
        string? connectionString = Environment.GetEnvironmentVariable(configuration["connectionStrings:auth"]!.ToString());
        _connection = new SqlConnection(connectionString);
        User = new UserRepository(_connection);
        Application = new ApplicationRepository(_connection);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
