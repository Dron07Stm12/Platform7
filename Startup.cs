using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Platform7
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Первый параметр делегата Func, который передается в метод Use(), представляет объект HttpContext.
            //Этот объект позволяет получить данные запроса и управлять ответом клиенту.

            //Второй параметр делегата представляет другой делегат -Func < Task > или RequestDelegate.
            //Этот делегат представляет следующий в конвейере компонент middleware, которому будет передаваться обработка запроса.

            //Работа middleware разбивается на две части:
            //Middleware выполняет некоторую начальную обработку запроса до вызова await next()
            //Затем вызывается метод next(), который передает обработку запроса следующему компоненту в конвейере



            app.Use(async delegate (HttpContext context,Func<Task> func) {
            
            
                await  func.Invoke();
                await context.Response.WriteAsync($"\nstatus code: {context.Response.StatusCode}"); 
            
            });



            app.Use(async delegate (HttpContext context, Func<Task> func)
            {

                if (context.Request.Query["custom"] == "true")
                {
                   await  context.Response.WriteAsync("custom Middleware\n");
                }

              await  func.Invoke();

            });

            app.UseMiddleware<QueryStringMiddleware>();
           

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(" Hello World!");
                });
            });
        }
    }
}


//Func<HttpContext, Func<Task>, Task> func = async delegate (HttpContext context, Func<Task> tsk)
//{
//    if (context.Request.Method == HttpMethods.Get && context.Request.Query["custom"] == "true")
//    {
//        await context.Response.WriteAsync("func Custom Middleware\n");
//    }


//    await tsk.Invoke();
//};


//app.Use(func);

//app.Use(async delegate (HttpContext context, Func<Task> func)
//{


//    if (context.Request.Path == "/use")
//    {
//        //await context.Response.WriteAsync("dron");
//        await context.Response.WriteAsync(@"
//                    <!DOCTYPE html>
//                    <html lang=""en"">
//                    <head><title>Response</title></head>
//                    <body>
//                    <h2>Formatted Response  </h2>
//                    </body>
//                    </html>");


//    }

//    else
//    {
//        await func.Invoke();
//    }


//});
