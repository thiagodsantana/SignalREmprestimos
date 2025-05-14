using Microsoft.AspNetCore.SignalR;
using SignalREmprestimos.Models;
using SignalREmprestimos.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o SignalR
builder.Services.AddSignalR();

// Habilita o CORS (caso o frontend esteja em outro dom�nio)
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

// Habilita CORS para permitir requisi��es de qualquer origem
app.UseCors("AllowAll");

// Servir arquivos est�ticos (caso necess�rio)
app.UseStaticFiles(); // Serve arquivos na pasta wwwroot

// Endpoint para solicitar um empr�stimo
app.MapPost("/api/emprestimos", async (Emprestimo emprestimo, IHubContext<EmprestimoHub> hub) =>
{
    emprestimos.Add(emprestimo);

    // Notifica todos os operadores conectados sobre a nova solicita��o
    await hub.Clients.All.SendAsync("NovaSolicitacao", emprestimo);

    return Results.Created($"/api/emprestimos/{emprestimo.Id}", emprestimo);
});

// Endpoint para atualizar o status de um empr�stimo
app.MapPut("/api/emprestimos/{id}/{status}", async (string id, string status, IHubContext<EmprestimoHub> hub) =>
{
    var emprestimo = emprestimos.FirstOrDefault(e => e.Id == id);
    if (emprestimo == null) return Results.NotFound();

    // Atualiza o status do empr�stimo
    emprestimo.Status = status;

    // Log de mudan�a de status
    Console.WriteLine($"Status do Empr�stimo {id} alterado para: {status}");

    // Envia a atualiza��o de status para todos os clientes conectados via SignalR
    await hub.Clients.All.SendAsync("StatusAtualizado", emprestimo);

    // Retorna a resposta com o status atualizado
    return Results.Ok(new { mensagem = $"Status do empr�stimo {id} alterado para {status}.", emprestimo });
});

// Mapeia o endpoint do SignalR Hub
app.MapHub<EmprestimoHub>("/emprestimohub");

// Mapeia o fallback para servir a p�gina principal (caso voc� tenha arquivos est�ticos)
app.MapFallbackToFile("cliente.html");

app.Run();
