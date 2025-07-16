using eAgenda.Infraestrutura.ORM.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace e_Agenda.WebApp.Dependencies;

public static class EntityFrameworkConfig
{
    public static void AddEntityFrameworkConfig(this IServiceCollection services, IConfiguration configuration) {
        var connectionString = configuration["SQL_CONNECTION_STRING"];

        services.AddDbContext<eAgendaDbContext>(options => options.UseSqlServer(connectionString));
    }
}
