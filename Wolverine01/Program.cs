
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine();

var app = builder.Build();

// An endpoint to create a new issue
app.MapPost("/issues/create", (CreateIssue body, IMessageBus bus) => bus.InvokeAsync(body));


app.Run();