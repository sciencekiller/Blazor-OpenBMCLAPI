using AntDesign.ProLayout;
using Blazor_OpenBMCLAPI.BackEnd;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Options;
using System.Globalization;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Toolbelt.Blazor.I18nText;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();
builder.Services.AddI18nText(options =>
{
    options.PersistanceLevel = PersistanceLevel.None;
    options.GetInitialLanguageAsync = (_, _) => ValueTask.FromResult(CultureInfo.CurrentUICulture.Name);
});
builder.Services.AddControllers();

//</4.1 Add Toolbelt>

//<1.1 Add Localization>
var supportedCultures = new[]
{
                new CultureInfo("en"),
                new CultureInfo("zh-CN")
};
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(supportedCultures[0]);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
builder.Services.AddLocalization();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(sp.GetService<NavigationManager>()!.BaseUri)
});
builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "bmclapi";
            options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
        });
builder.Services.AddScoped<UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
//<1.2 Configure RequestLocalization>
var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
if (localizationOptions != null)
{
    app.UseRequestLocalization(localizationOptions.Value);
}
//</1.2 Configure RequestLocalization>
app.UseAuthentication();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

//<1.3 Add Minimal API>
app.MapGet("Culture/Set",(HttpRequest request, [FromQuery] string culture, [FromQuery] string redirectUri) =>
{
    if (culture != null)
    {
        request.HttpContext.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(
             new RequestCulture(culture, culture)),
              new CookieOptions
              {
                  Path = "/",
                  Expires = DateTime.Now.AddYears(1)
              });
    }

    return Results.LocalRedirect(redirectUri);
});
app.UseMiddleware<InvalidateSessionMiddleware>();
//</1.3 Add Minimal API>
await Initialize.Run();
app.Run();
