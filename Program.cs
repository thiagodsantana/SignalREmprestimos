using Microsoft.AspNetCore.SignalR;
using SignalREmprestimos.Models;
using SignalREmprestimos.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o SignalR
builder.Services.AddSignalR();

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
app.MapPost("/api/emprestimos", async (Emprestimo emprestimo, IHubContext<EmprestimoHub> hub) =>
{
    emprestimos.Add(emprestimo);

    // Notifica apenas operadores sobre a nova solicitação
    await hub.Clients.Group("operadores").SendAsync("NovaSolicitacao", emprestimo);

    return Results.Created($"/api/emprestimos/{emprestimo.Id}", emprestimo);
});

// Endpoint para atualizar o status de um empréstimo
app.MapPut("/api/emprestimos/{id}/{status}", async (string id, string status, IHubContext<EmprestimoHub> hub) =>
{
    var emprestimo = emprestimos.FirstOrDefault(e => e.Id == id);
    if (emprestimo == null) return Results.NotFound();

    emprestimo.Status = status;

    Console.WriteLine($"Status do Empréstimo {id} alterado para: {status}");

    // Notifica todos os grupos sobre a atualização de status
    var grupos = new[] { "clientes", "operadores", "financeiro" };
    foreach (var grupo in grupos)
    {
        await hub.Clients.Group(grupo).SendAsync("StatusAtualizado", emprestimo);
    }

    return Results.Ok(new { mensagem = $"Status do empréstimo {id} alterado para {status}.", emprestimo });
});

// Endpoint para listar empréstimos aprovados
app.MapGet("/api/emprestimos/aprovados", () =>
{
    var aprovados = emprestimos.Where(e => e.Status == "Aprovado");
    return Results.Ok(aprovados);
});

// Endpoint para marcar empréstimo como pago
app.MapPut("/api/emprestimos/{id}/pagar", async (string id, IHubContext<EmprestimoHub> hub) =>
{
    var emprestimo = emprestimos.FirstOrDefault(e => e.Id == id);
    if (emprestimo == null) return Results.NotFound();

    emprestimo.Status = "Pago";
    Console.WriteLine($"Empréstimo {id} foi marcado como pago.");

    var grupos = new[] { "clientes", "operadores", "financeiro" };
    foreach (var grupo in grupos)
    {
        await hub.Clients.Group(grupo).SendAsync("StatusAtualizado", emprestimo);
    }

    return Results.Ok(new { mensagem = $"Empréstimo {id} pago com sucesso.", emprestimo });
});

// Mapeia o SignalR Hub
app.MapHub<EmprestimoHub>("/emprestimohub");

app.MapFallbackToFile("cliente.html");

app.Run();
