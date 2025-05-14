using Microsoft.AspNetCore.SignalR;
using SignalREmprestimos.Models;
using SignalREmprestimos.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o SignalR
builder.Services.AddSignalR();

// Habilita o CORS (caso o frontend esteja em outro domínio)
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

// Habilita CORS para permitir requisições de qualquer origem
app.UseCors("AllowAll");

// Servir arquivos estáticos (caso necessário)
app.UseStaticFiles(); // Serve arquivos na pasta wwwroot

// Endpoint para solicitar um empréstimo
app.MapPost("/api/emprestimos", async (Emprestimo emprestimo, IHubContext<EmprestimoHub> hub) =>
{
    emprestimos.Add(emprestimo);

    // Notifica todos os operadores conectados sobre a nova solicitação
    await hub.Clients.All.SendAsync("NovaSolicitacao", emprestimo);

    return Results.Created($"/api/emprestimos/{emprestimo.Id}", emprestimo);
});

// Endpoint para atualizar o status de um empréstimo
app.MapPut("/api/emprestimos/{id}/{status}", async (string id, string status, IHubContext<EmprestimoHub> hub) =>
{
    var emprestimo = emprestimos.FirstOrDefault(e => e.Id == id);
    if (emprestimo == null) return Results.NotFound();

    // Atualiza o status do empréstimo
    emprestimo.Status = status;

    // Log de mudança de status
    Console.WriteLine($"Status do Empréstimo {id} alterado para: {status}");

    // Envia a atualização de status para todos os clientes conectados via SignalR
    await hub.Clients.All.SendAsync("StatusAtualizado", emprestimo);

    // Retorna a resposta com o status atualizado
    return Results.Ok(new { mensagem = $"Status do empréstimo {id} alterado para {status}.", emprestimo });
});

// Mapeia o endpoint do SignalR Hub
app.MapHub<EmprestimoHub>("/emprestimohub");

// Mapeia o fallback para servir a página principal (caso você tenha arquivos estáticos)
app.MapFallbackToFile("cliente.html");

app.Run();
