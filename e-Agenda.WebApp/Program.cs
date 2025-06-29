using e_Agenda.Dominio.ModuloCompromissos;
using e_Agenda.Dominio.ModuloContato;
using e_Agenda.Dominio.ModuloTarefa;
using e_Agenda.Infraestrutura.Arquivos.Compartilhado;
using e_Agenda.Infraestrutura.Arquivos.ModuloCategoria;
using e_Agenda.Infraestrutura.Arquivos.ModuloCompromisso;
using e_Agenda.Infraestrutura.Arquivos.ModuloContato;
using e_Agenda.Infraestrutura.Arquivos.ModuloTarefa;
using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Infraestrutura.ModuloDespesa;

namespace e_Agenda.WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();

        //builder.Services.AddSingleton<ContextoDados>(delegate (IServiceProvider serviceProvider) {
        //    return new ContextoDados(true);        
        //});

        //builder.Services.AddSingleton<ContextoDados>((IServiceProvider serviceProvider) => {
        //    return new ContextoDados(true);        
        //});        
        
        builder.Services.AddScoped<ContextoDados>((_) => new ContextoDados(true));
        builder.Services.AddScoped<IRepositorioContato, RepositorioContato>();
        builder.Services.AddScoped<IRepositorioCompromisso, RepositorioCompromisso>();
        builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
        builder.Services.AddScoped<IRepositorioDespesa, RepositorioDespesa>();
        builder.Services.AddScoped<IRepositorioTarefa, RepositorioTarefa>();

        var app = builder.Build();

        app.UseAntiforgery();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapDefaultControllerRoute();

        app.Run();
    }
}
