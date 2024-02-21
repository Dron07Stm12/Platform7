using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;


namespace Platform7
{
    public class QueryStringMiddleware
    {
        private RequestDelegate request;

        public QueryStringMiddleware(RequestDelegate request)
        {
            this.request = request;  
        }


        public async Task InvokeAsync(HttpContext context) {

            StringValues strings = "custom";  

            if (context.Request.Method == HttpMethods.Get && context.Request.Query[strings] == "true")
            {
                await context.Response.WriteAsync("class  Middleware\n");
            }
            
            await request.Invoke(context);
        }

    }
}
