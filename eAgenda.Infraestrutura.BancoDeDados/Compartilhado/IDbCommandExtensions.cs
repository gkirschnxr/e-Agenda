using System.Data;

namespace eAgenda.Infraestrutura.BancoDeDados.Compartilhado;

public static class IDbCommandExtensions
{
    public static void AddParameter(this IDbCommand command, string name, object value) {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;

        command.Parameters.Add(parameter);
    }
}
