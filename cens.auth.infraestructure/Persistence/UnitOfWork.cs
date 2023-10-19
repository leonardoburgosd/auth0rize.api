using cens.auth.domain.Primitives;
using cens.auth.domain.User;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace cens.auth.infraestructure.Persistence;
public class UnitOfWork : IUnitOfWork
{
    public IUserRepository User { get; private set; }
    private readonly IDbConnection _connection;

    public UnitOfWork(IConfiguration configuration)
    {
        _connection = new SqlConnection(configuration["connectionStrings:auth"]);
        User = new UserRepository(_connection);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
