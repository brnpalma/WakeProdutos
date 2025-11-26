using WakeProdutos.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WakeProdutos.API.Filters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(Result<string>.Fail(400, "O corpo da requisição é inválido.", null));
            }
        }
    }
}
