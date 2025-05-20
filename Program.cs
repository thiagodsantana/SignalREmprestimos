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

// Cria um repositório em memória para armazenar empréstimos
var emprestimos = new List<Emprestimo>();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseStaticFiles();

// Endpoint para solicitar um empréstimo
app.MapPost("/api/emprestimos", async (Emprestimo emprestimo, INotificationService notificacaoService) =>
{
    emprestimos.Add(emprestimo);

    // Notifica operadores
    await notificacaoService.NotificarNovaSolicitacao(emprestimo);

    return Results.Created($"/api/emprestimos/{emprestimo.Id}", emprestimo);
});

// Endpoint para atualizar o status de um empréstimo
app.MapPut("/api/emprestimos/{id}/{status}", async (string id, string status, INotificationService notificacaoService) =>
{
    var emprestimo = emprestimos.FirstOrDefault(e => e.Id == id);
    if (emprestimo == null) return Results.NotFound();

    emprestimo.Status = status;

    Console.WriteLine($"Status do Empréstimo {id} alterado para: {status}");

    await notificacaoService.NotificarStatusAtualizado(emprestimo);

    return Results.Ok(new { mensagem = $"Status do empréstimo {id} alterado para {status}.", emprestimo });
});

// Endpoint para listar empréstimos aprovados
app.MapGet("/api/emprestimos/aprovados", () =>
{
    var aprovados = emprestimos.Where(e => e.Status == "Aprovado");
    return Results.Ok(aprovados);
});

// Endpoint para marcar empréstimo como pago
app.MapPut("/api/emprestimos/{id}/pagar", async (string id, INotificationService notificacaoService) =>
{
    var emprestimo = emprestimos.FirstOrDefault(e => e.Id == id);
    if (emprestimo == null) return Results.NotFound();

    emprestimo.Status = "Pago";
    Console.WriteLine($"Empréstimo {id} foi marcado como pago.");

    await notificacaoService.NotificarStatusAtualizado(emprestimo);

    return Results.Ok(new { mensagem = $"Empréstimo {id} pago com sucesso.", emprestimo });
});

// Mapeia o SignalR Hub
app.MapHub<EmprestimoHub>("/emprestimohub");

app.MapFallbackToFile("cliente.html");

app.Run();
