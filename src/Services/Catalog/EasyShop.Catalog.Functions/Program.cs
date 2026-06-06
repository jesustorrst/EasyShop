using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

// 🎯 Esta es la nueva sintaxis limpia para Azure Workers v2.x
var builder = FunctionsApplication.CreateBuilder(args);

// Aquí adentro puedes registrar tus servicios si en el futuro lo necesitas
// builder.Services.AddScoped<IYourService, YourService>();

var host = builder.Build();

await host.RunAsync();