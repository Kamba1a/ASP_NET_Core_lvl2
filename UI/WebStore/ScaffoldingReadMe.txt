Scaffolding has generated all the files and added the required dependencies.

However the Application's Startup code may required additional changes for things to work end to end.
Add the following code to the Configure method in your Application's Startup class if not already done:

        app.UseMvc(routes =>
        {
          routes.MapRoute(
            name : "areas",
            template : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
        });


Мои заметки:
Шаблон выше дан для старой версии ASP.NET, сейчас шаблон выглядит как:
        
        app.UseEndpoints(endpoints =>
        {
          endpoints.MapRoute(
            name : "areas",
            pattern : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
        });

        или:

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
        }