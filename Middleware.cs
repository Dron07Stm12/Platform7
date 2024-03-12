using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;


namespace Platform7
{
    public class QueryStringMiddleware
    {
        private RequestDelegate request;

        public QueryStringMiddleware(RequestDelegate request)
        {
            this.request = request;  
        }

        public QueryStringMiddleware()
        {
                
        }


        public async Task InvokeAsync(HttpContext context) {

            StringValues strings = "custom";  

            if (context.Request.Method == HttpMethods.Get && context.Request.Query[strings] == "true")
            {
                await context.Response.WriteAsync("class  Middleware\n");
            }

            if (request != null)
            {
                await request.Invoke(context);
            }
            
           
        }

    }

   public class LocationMiddleware
   {
        private RequestDelegate request;
        private MessageOptions message;
        public LocationMiddleware(RequestDelegate _request,IOptions<MessageOptions> options)
        {
           request = _request;
           message= options.Value;
        }


        public async Task Invoke(HttpContext context) {

            if (context.Request.Path == "/location")
            {
                await context.Response.WriteAsync($"{message.countryName}  {message.cityName}");
            }

            if (request != null) { await request(context); }
        }


   }


}
