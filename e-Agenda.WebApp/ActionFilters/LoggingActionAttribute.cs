using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace e_Agenda.WebApp.ActionFilters;

public class LoggingActionAttribute : ActionFilterAttribute
{
    private readonly ILogger<LoggingActionAttribute> _logger;

    public LoggingActionAttribute(ILogger<LoggingActionAttribute> logger) {
        _logger = logger;
    }

    public override void OnActionExecuted(ActionExecutedContext context) {
        var result = context.Result;

        if (result is ViewResult viewResult && viewResult.Model is not null) {
            _logger.LogInformation("Ação de endpoint executada com sucesso. {@Modelo}", viewResult.Model);
        }

        base.OnActionExecuted(context);
    }
}
