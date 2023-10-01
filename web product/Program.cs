using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Security.Claims;
using web_product;
using web_product.Controllers;
using web_product.Entities;
using web_product.Models;
using web_product.Repository;
using static System.Collections.Specialized.BitVector32;
using System.Text;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);



var adminRole = new Role("admin");

var people = new List<Autorize>
{
    new Autorize("admin@gmail.com", "12345", adminRole)  
};




builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/accessdenied";
    });


builder.Services.AddDbContext<MsqlContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.



builder.Services.AddScoped<IRepository<Products>, ProductRepository>();


builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

app.UseHttpsRedirection();


app.UseStaticFiles();



app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


//ѕеренаправл€ет пользовател€ без прав
app.MapGet("/accessdenied", async (HttpContext context) =>
{
    context.Response.StatusCode = 403;
    await context.Response.WriteAsync("Access Denied");
});

app.MapGet("/login", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
   
    string loginForm = @"<!DOCTYPE html>
    <html>
    
    <body>
        <h2>Login Form</h2>
        <form  method='post'>
            <p>
                <label>Email</label><br />
                <input name='email' />
            </p>
            <p>
                <label>Password</label><br />
                <input type='password' name='password' />
            </p>
            <input type='submit' value='Login' />
 
        </form>
    </body>
    </html>";
    await context.Response.WriteAsync(loginForm);
});

app.MapPost("/login", async (string? returnUrl, HttpContext context) =>
{
    // получаем из формы email и пароль
    var form = context.Request.Form;
    // если email и/или пароль не установлены, посылаем статусный код ошибки 400
    if (!form.ContainsKey("email") || !form.ContainsKey("password"))
        return Results.BadRequest("Email и/или пароль не установлены");

    string email = form["email"];
    string password = form["password"];

    // находим пользовател€ 
    Autorize? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);
    // если пользователь не найден, отправл€ем статусный код 401
    if (person is null) return Results.Unauthorized();

    var claims = new List<Claim>
    {
        new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
        new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role.Name)
    };
    // создаем объект ClaimsIdentity
    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    // установка аутентификационных куки
    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    return Results.Redirect(returnUrl ?? "/admin");
});


app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return "ƒанные удалены";
});

app.MapControllerRoute  (
            name: "Default",
            pattern: "{controller=Admin}/{action=Index}/{id?}" );


app.Run();
