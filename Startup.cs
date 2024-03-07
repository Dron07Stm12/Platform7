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
            services.AddMvc(delegate(MvcOptions mvc) {  mvc.EnableEndpointRouting = false; });  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.Map("/branch", branch =>
            {
                branch.UseMiddleware<QueryStringMiddleware>();
                branch.Use(async delegate (HttpContext context, Func<Task> tsk)
                {                                     
                    await context.Response.WriteAsync("Branch Middleware");
                    await tsk();    
                });
                
            });

            


           



            

            app.UseMiddleware<QueryStringMiddleware>();



            app.UseRouting();
            RequestDelegate request = delegate (HttpContext context) { return context.Response.WriteAsync("endpoint_request"); };
            app.UseEndpoints(delegate(IEndpointRouteBuilder endpoint) {
                endpoint.MapGet("/",request);
                //endpoint.MapGet("/",async delegate (HttpContext context) { await context.Response.WriteAsync("delegate_UseEndpoint"); });
            
            });
          
        }
    }
}



//������ �������� �������� Func, ������� ���������� � ����� Use(), ������������ ������ HttpContext.
//���� ������ ��������� �������� ������ ������� � ��������� ������� �������.

//������ �������� �������� ������������ ������ ������� -Func < Task > ��� RequestDelegate.
//���� ������� ������������ ��������� � ��������� ��������� middleware, �������� ����� ������������ ��������� �������.

//������ middleware ����������� �� ��� �����:
//Middleware ��������� ��������� ��������� ��������� ������� �� ������ await next()
//����� ���������� ����� next(), ������� �������� ��������� ������� ���������� ���������� � ���������




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

