using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fase03.Infra.Data.Context;

public class AppDbContextMigration : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        #region Localizar o arquivo appsettings.json

        var configurationBuilder = new ConfigurationBuilder();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        configurationBuilder.AddJsonFile(path, optional: false);

        #endregion

        #region Capturar a connectionstring do arquivo appsettings.json

        var root = configurationBuilder.Build();
        var connectionString = root.GetConnectionString("ContatosContext");

        #endregion

        #region Fazer a injeção de dependência na classe SqlServerContext

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(connectionString);

        return new AppDbContext(builder.Options);

        #endregion
    }
}