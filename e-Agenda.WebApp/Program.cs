using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Dominio.ModuloContato;
using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.WebApp.ActionFilters;
using e_Agenda.WebApp.Dependencies;
using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.BancoDeDados.ModuloCompromisso;
using eAgenda.Infraestrutura.BancoDeDados.ModuloContato;
using eAgenda.Infraestrutura.BancoDeDados.ModuloTarefa;
using eAgenda.Infraestrutura.BancoDeDados.ModuloDespesa;
using eAgenda.Infraestrutura.BancoDeDados.ModuloCategoria;

namespace e_Agenda.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews(options => {
            options.Filters.Add<ValidarModeloAttribute>();
            options.Filters.Add<LoggingActionAttribute>();
        });  
        
        builder.Services.AddScoped<IRepositorioContato, RepositorioContatoBD>();
        builder.Services.AddScoped<IRepositorioCompromisso, RepositorioCompromissoBD>();
        builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoriaBD>();
        builder.Services.AddScoped<IRepositorioDespesa, RepositorioDespesaBD>();
        builder.Services.AddScoped<IRepositorioTarefa, RepositorioTarefaBD>();

        builder.Services.AddSerilogConfig(builder.Logging);

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/erro");
        else 
            app.UseDeveloperExceptionPage();

        app.UseAntiforgery();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapDefaultControllerRoute();

        app.Run();
    }
}
