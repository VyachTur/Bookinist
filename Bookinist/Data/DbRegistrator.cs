using Bookinist.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bookinist.Data
{
    internal static class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) => services
            .AddDbContext<BookinistDB>(
                opt =>
                {
                    var type = configuration["Type"];

                    switch(type)
                    {
                        case null: throw new InvalidOperationException("Не определён тип БД");
                        default: throw new InvalidOperationException($"Тип подключения {type} не поддерживается");

                        case "MSSQL":
                            opt.UseSqlServer(configuration.GetConnectionString(type));
                            break;
                        case "SQLite":
                            opt.UseSqlite(configuration.GetConnectionString(type));
                            break;
                        case "InMemory":
                            opt.UseInMemoryDatabase("Bokinist.db");
                            break;
                    }
                })
            .AddTransient<DbInitializer>()
            ;
    }
}
