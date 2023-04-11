using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MainApp;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

builder.AddDefaultServices();
builder.AddCachingServices();
builder.AddMicrosoftServices();
builder.AddHostedServices();
builder.AddSingletonServices();
builder.AddScopedServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

/*
 * 1st Authentication then 
 * 2nd Authorization
 */
app.UseAuthentication();
app.UseAuthorization();

app.UseRewriter(
    new RewriteOptions().Add(
        context =>
        {
            if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut")
            {
                context.HttpContext.Response.Redirect("/");
            }
        }
        ));

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
