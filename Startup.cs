using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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



            app.Map("/branch", branch =>
            {
                branch.UseMiddleware<QueryStringMiddleware>();
                branch.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync($"Branch Middleware");
                });
            });


            RequestDelegate request2 = delegate (HttpContext context) { return context.Response.WriteAsync("\nrequest"); };

            Func<HttpContext, bool> func = delegate (HttpContext context) {

                if (context.Request.Path == "/func")
                {
                    return true;
                }

                else { return false; }
            
            };
            app.MapWhen(func, delegate(IApplicationBuilder builder) {

                builder.Use(delegate (HttpContext context, Func<Task> tsk)
                {

                    Task task = context.Response.WriteAsync(" use");
                    Task task2 = tsk();
                    return Task.WhenAll(task, task2);

                });


                builder.Use(delegate (HttpContext context, Func<Task> tsk)
                {

                    Task task = context.Response.WriteAsync("\n use2");
                    Task task2 = tsk();
                    return Task.WhenAll(task, task2);

                });

                builder.Run(request2);
            
            });

            



            app.Map("/br", delegate (IApplicationBuilder builder)
            {

                builder.Use(delegate (HttpContext context, Func<Task> tsk)
                {

                    Task task = context.Response.WriteAsync(" use");
                    Task task2 = tsk();
                    return Task.WhenAll(task, task2);

                });

                RequestDelegate request = delegate (HttpContext context) { return context.Response.WriteAsync("\nrequest"); };

                builder.Run(request);   

                builder.Run(delegate(HttpContext context) { return context.Response.WriteAsync("Requestdelegat"); });

                builder.Use(delegate (HttpContext context, Func<Task> tsk)
                {

                    Task task = context.Response.WriteAsync("\n use2");
                    Task task2 = tsk();
                    return Task.WhenAll(task, task2);

                });


                builder.Use(delegate (HttpContext context, Func<Task> tsk)
                {

                    if (context.Request.Path == "/path")
                    {
                        return context.Response.WriteAsync("\n path");
                    }

                    else
                    {
                        return tsk.Invoke();
                    }
                    //Task task = context.Response.WriteAsync("\n use2");
                    //Task task2 = tsk();
                    //return Task.WhenAll(task, task2);

                });


            });










            //app.Map("/branch",  delegate (IApplicationBuilder builder)                          
            //{ 

            //     builder.Use( delegate(HttpContext context, Func<Task> tsk) 
            //     {
            //      //   return context.Response.WriteAsync("Branch Middleware2");
            //         return tsk();  
            //         //Task task1= context.Response.WriteAsync("Branch Middleware2");
            //         //Task task2= tsk();
            //         //return Task.WhenAll(task1, task2); 

            //    });

            //    builder.Use(async delegate (HttpContext context, Func<Task> func)
            //    {
            //        await context.Response.WriteAsync("\nBranch Middleware3");
            //        await func.Invoke();
            //    });



            //    builder.Use(delegate (HttpContext context, Func<Task> tsk)
            //    {

            //        Task task = context.Response.WriteAsync($"\n HTTPS Request: {context.Request.IsHttps}");
            //        Task task2 = tsk();

            //        //  return task;
            //        return Task.WhenAll(task, task2);
            //    });





            //});





            //app.Use(async delegate (HttpContext context,Func<Task> func) {


            //    await  func.Invoke();
            //    await context.Response.WriteAsync($"\nstatus code: {context.Response.StatusCode}"); 

            //});


            //app.Use(async delegate(HttpContext context, Func<Task> tsk) {

            //    if (context.Request.Path == "/short")
            //    {
            //        await context.Response.WriteAsync("short");
            //    }

            //    else { await tsk.Invoke(); }
            //});



            //app.Use(async delegate (HttpContext context, Func<Task> func)
            //{

            //    if (context.Request.Query["custom"] == "true")
            //    {
            //       await  context.Response.WriteAsync("custom Middleware\n");
            //    }

            //  await  func.Invoke();

            //});

            //app.UseMiddleware<QueryStringMiddleware>();


            app.UseRouting();
            RequestDelegate request = delegate (HttpContext context) { return context.Response.WriteAsync("endpoint_request"); };
            app.UseEndpoints(delegate(IEndpointRouteBuilder endpoint) {
                endpoint.MapGet("/",request);
                //endpoint.MapGet("/",async delegate (HttpContext context) { await context.Response.WriteAsync("delegate_UseEndpoint"); });
            
            });


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync(" Hello World!");
            //    });
            //});
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
