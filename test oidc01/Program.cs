//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using test_oidc01;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//await builder.Build().RunAsync();

using System;
using System.Net.Http;
using test_oidc01;
//using Balosar.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddHttpClient("Balosar.ServerAPI")
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project.
builder.Services.AddScoped(provider =>
{
    var factory = provider.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient("Balosar.ServerAPI");
});

//builder.Services.AddOidcAuthentication(options =>
//{
//    options.ProviderOptions.ClientId = "ui";
//    options.ProviderOptions.Authority = "https://localhost:44300/";
//    options.ProviderOptions.ResponseType = "code";

//    // Note: response_mode=fragment is the best option for a SPA. Unfortunately, the Blazor WASM
//    // authentication stack is impacted by a bug that prevents it from correctly extracting
//    // authorization error responses (e.g error=access_denied responses) from the URL fragment.
//    // For more information about this bug, visit https://github.com/dotnet/aspnetcore/issues/28344.
//    //
//    options.ProviderOptions.ResponseMode = "query";
//    //options.AuthenticationPaths.RemoteRegisterPath = "https://localhost:44300/Identity/Account/Register";

//    // Add the "roles" (OpenIddictConstants.Scopes.Roles) scope and the "role" (OpenIddictConstants.Claims.Role) claim
//    // (the same ones used in the Startup class of the Server) in order for the roles to be validated.
//    // See the Counter component for an example of how to use the Authorize attribute with roles
//    options.ProviderOptions.DefaultScopes.Add("roles");
//    options.UserOptions.RoleClaim = "role";
//});
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("oidc", options.ProviderOptions);
});
var host = builder.Build();
await host.RunAsync();
