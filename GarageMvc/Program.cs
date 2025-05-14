using GarageMvc.Models;
using GarageMvc.TokenHandler;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthenticationTokenHandler>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5217/api/"); 
   
}).AddHttpMessageHandler<AuthenticationTokenHandler>();



builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

});



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
    options =>
    {
        options.LoginPath = "/Authentication/Login"; // Redirect to login page
        options.AccessDeniedPath = "/Home/AccessDeniedPage"; // Redirect if access is denied
        options.SlidingExpiration = true; // Renew cookie on activity
        options.Cookie.HttpOnly = true; // Prevent access to cookies via JavaScript
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Enforce HTTPS
        options.Cookie.SameSite = SameSiteMode.Strict; // Prevent CSRF attacks
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Set cookie expiration
    }


    );




builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();
app.MapHub<NotificationHub>("/notificationhub");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //app.UseStatusCodePagesWithReExecute("/Home/StatusCode", "?code={0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
