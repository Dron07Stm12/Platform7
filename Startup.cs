using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Platform;

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<MessageOptions> options)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.UseMiddleware<Population>();
            app.UseMiddleware<Capital>();


            app.UseRouting();

            

            

            app.UseEndpoints(delegate (IEndpointRouteBuilder endpoint)
            {
                endpoint.MapGet("routing",async delegate (HttpContext context) {
                 await context.Response.WriteAsync("useEndpoint");   
                
                });

                endpoint.MapGet("route",async delegate (HttpContext context) { await context.Response.WriteAsync("\t route"); });
              //  endpoint.MapGet("/capital/uk",new Population().Invoke);
             //   endpoint.MapGet("/population/paris", new Capital().Invoke);

            });

            app.Use(async delegate (HttpContext context, Func<Task> func) {

                await context.Response.WriteAsync("use");
            
            });

        }
    }
}



//Первый параметр делегата Func, который передается в метод Use(), представляет объект HttpContext.
//Этот объект позволяет получить данные запроса и управлять ответом клиенту.

//Второй параметр делегата представляет другой делегат -Func < Task > или RequestDelegate.
//Этот делегат представляет следующий в конвейере компонент middleware, которому будет передаваться обработка запроса.

//Работа middleware разбивается на две части:
//Middleware выполняет некоторую начальную обработку запроса до вызова await next()
//Затем вызывается метод next(), который передает обработку запроса следующему компоненту в конвейере




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


//RequestDelegate request2 = delegate (HttpContext context) { return context.Response.WriteAsync("\nrequest"); };

//Func<HttpContext, bool> func = delegate (HttpContext context) {

//    if (context.Request.Path == "/func")
//    {
//        return true;
//    }

//    else { return false; }

//};
//app.MapWhen(func, delegate (IApplicationBuilder builder) {

//    builder.Use(delegate (HttpContext context, Func<Task> tsk)
//    {

//        Task task = context.Response.WriteAsync(" use");
//        Task task2 = tsk();
//        return Task.WhenAll(task, task2);

//    });


//    builder.Use(delegate (HttpContext context, Func<Task> tsk)
//    {

//        Task task = context.Response.WriteAsync("\n use2");
//        Task task2 = tsk();
//        return Task.WhenAll(task, task2);

//    });

//    builder.Run(request2);

//});



//app.Map("/br", delegate (IApplicationBuilder builder)
//{

//    builder.Use(delegate (HttpContext context, Func<Task> tsk)
//    {

//        Task task = context.Response.WriteAsync(" use");
//        Task task2 = tsk();
//        return Task.WhenAll(task, task2);

//    });

//    RequestDelegate request = delegate (HttpContext context) { return context.Response.WriteAsync("\nrequest"); };

//    builder.Run(request);

//    builder.Run(delegate (HttpContext context) { return context.Response.WriteAsync("Requestdelegat"); });

//    builder.Use(delegate (HttpContext context, Func<Task> tsk)
//    {

//        Task task = context.Response.WriteAsync("\n use2");
//        Task task2 = tsk();
//        return Task.WhenAll(task, task2);

//    });


//    builder.Use(delegate (HttpContext context, Func<Task> tsk)
//    {

//        if (context.Request.Path == "/path")
//        {
//            return context.Response.WriteAsync("\n path");
//        }

//        else
//        {
//            return tsk.Invoke();
//        }
//        //Task task = context.Response.WriteAsync("\n use2");
//        //Task task2 = tsk();
//        //return Task.WhenAll(task, task2);

//    });


//});




//app.Map("/branch", branch =>
//{
//    branch.UseMiddleware<QueryStringMiddleware>();

//    branch.Use(async delegate (HttpContext context, Func<Task> tsk)
//    {
//        await context.Response.WriteAsync("Branch Middleware");
//        await tsk();
//    });

//});


//app.Map("/branch2", delegate (IApplicationBuilder builder) {

//    builder.Run(new QueryStringMiddleware().InvokeAsync);


//});

//RequestDelegate request2 = delegate (HttpContext context) { return context.Response.WriteAsync("\nrequest"); };

//app.Map("/br", delegate (IApplicationBuilder builder) {

//    builder.UseMiddleware<QueryStringMiddleware>();
//    builder.Run(request2);

//});







//public class Startup
//{
//    // This method gets called by the runtime. Use this method to add services to the container.
//    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
//    public void ConfigureServices(IServiceCollection services)
//    {
//        //services.Configure<MessageOptions>(options =>
//        //{
//        //    options.CityName = "Albany";
//        //    options.CountryName = "Ukrain";
//        //});

//        Action<MessageOptions> action = delegate (MessageOptions options)
//        {
//            options.CityName = "Stachaniv";
//            options.CountryName = "Ukrain";
//        };

//        services.Configure(action);

//        //services.Configure<MessageOptions>(delegate (MessageOptions configuration)
//        //{
//        //     configuration.CityName = "Stahxanov";
//        //     configuration.CountryName = "Ukraine";
//        //});

//    }

//    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<MessageOptions> options)
//    {
//        if (env.IsDevelopment())
//        {
//            app.UseDeveloperExceptionPage();
//        }



//        app.Use(async delegate (HttpContext context, Func<Task> func) {

//            if (context.Request.Path == "/location2")
//            {
//                await context.Response.WriteAsync($"{options.Value.CityName = "Kadiivka"} {options.Value.CountryName}");
//            }
//            else { await func.Invoke(); }

//        });

//        app.UseMiddleware<LocationMiddleware>();


//        app.UseRouting();
//        RequestDelegate request = delegate (HttpContext context) { return context.Response.WriteAsync("endpoint_request"); };
//        app.UseEndpoints(delegate (IEndpointRouteBuilder endpoint) {
//            endpoint.MapGet("/", request);
//            //endpoint.MapGet("/",async delegate (HttpContext context) { await context.Response.WriteAsync("delegate_UseEndpoint"); });

//        });

//    }
//}

