using SignalREmprestimos.Models;
using SignalREmprestimos.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o SignalR
builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Cria um reposit�rio em mem�ria para armazenar empr�stimos
var emprestimos = new List<Emprestimo>();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseStaticFiles();

// Endpoint para solicitar um empr�stimo
app.MapPost("/api/emprestimos", async (Emprestimo emprestimo, INotificationService notificacaoService) =>
{
    emprestimos.Add(emprestimo);

    // Notifica operadores
    await notificacaoService.NotificarNovaSolicitacao(emprestimo);

    return Results.Created($"/api/emprestimos/{emprestimo.Id}", emprestimo);
});

// Endpoint para atualizar o status de um empr�stimo
app.MapPut("/api/emprestimos/{id}/{status}", async (string id, string status, INotificationService notificacaoService) =>
{
    var emprestimo = emprestimos.FirstOrDefault(e => e.Id == id);
    if (emprestimo == null) return Results.NotFound();

    emprestimo.Status = status;

    Console.WriteLine($"Status do Empr�stimo {id} alterado para: {status}");

    await notificacaoService.NotificarStatusAtualizado(emprestimo);

    return Results.Ok(new { mensagem = $"Status do empr�stimo {id} alterado para {status}.", emprestimo });
});

// Endpoint para listar empr�stimos aprovados
app.MapGet("/api/emprestimos/aprovados", () =>
{
    var aprovados = emprestimos.Where(e => e.Status == "Aprovado");
    return Results.Ok(aprovados);
});

// Endpoint para marcar empr�stimo como pago
app.MapPut("/api/emprestimos/{id}/pagar", async (string id, INotificationService notificacaoService) =>
{
    var emprestimo = emprestimos.FirstOrDefault(e => e.Id == id);
    if (emprestimo == null) return Results.NotFound();

    emprestimo.Status = "Pago";
    Console.WriteLine($"Empr�stimo {id} foi marcado como pago.");

    await notificacaoService.NotificarStatusAtualizado(emprestimo);

    return Results.Ok(new { mensagem = $"Empr�stimo {id} pago com sucesso.", emprestimo });
});

// Mapeia o SignalR Hub
app.MapHub<EmprestimoHub>("/emprestimohub");

app.MapFallbackToFile("cliente.html");

app.Run();
