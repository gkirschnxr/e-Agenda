using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Dominio.ModuloContato;
using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using e_Agenda.Infraestrutura.Arquivos.ModuloCategoria;
using e_Agenda.Infraestrutura.Arquivos.ModuloCompromisso;
using e_Agenda.Infraestrutura.Arquivos.ModuloContato;
using e_Agenda.Infraestrutura.Arquivos.ModuloTarefa;
using e_Agenda.WebApp.ActionFilters;
using e_Agenda.WebApp.Dependencies;
using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.ModuloDespesa;
using Serilog;
using Serilog.Events;

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
        
        builder.Services.AddScoped<ContextoDados>((_) => new ContextoDados(true));
        builder.Services.AddScoped<IRepositorioContato, RepositorioContato>();
        builder.Services.AddScoped<IRepositorioCompromisso, RepositorioCompromisso>();
        builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
        builder.Services.AddScoped<IRepositorioDespesa, RepositorioDespesa>();
        builder.Services.AddScoped<IRepositorioTarefa, RepositorioTarefa>();

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
