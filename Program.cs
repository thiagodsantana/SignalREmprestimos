using Microsoft.AspNetCore.SignalR;
using SignalREmprestimos.Models;
using SignalREmprestimos.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var emprestimos = new List<Emprestimo>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles(); // Procura por index.html
app.UseStaticFiles();  // Habilita servir arquivos da wwwroot


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// POST
app.MapPost("/api/emprestimos", (Emprestimo emprestimo, IHubContext<EmprestimoHub> hub) =>
{
    emprestimos.Add(emprestimo);
    Console.WriteLine($"Novo Empréstimo Criado: {emprestimo.Cliente} solicitou R${emprestimo.Valor} em {emprestimo.Parcelas}x.");
    return Results.Created($"/api/emprestimos/{emprestimo.Id}", emprestimo);
})
.WithName("SolicitarEmprestimo")
.WithOpenApi();

// PUT
app.MapPut("/api/emprestimos/{id}/{status}", async (string id, string status, IHubContext<EmprestimoHub> hub) =>
{
    var emprestimo = emprestimos.FirstOrDefault(e => e.Id == id);
    if (emprestimo == null) return Results.NotFound();

    emprestimo.Status = status;
    Console.WriteLine($"Status do Empréstimo {id} alterado para: {status}");

    await hub.Clients.All.SendAsync("StatusAtualizado", emprestimo);

    return Results.Ok(new { mensagem = $"Status do empréstimo {id} alterado para {status}.", emprestimo });
})
.WithName("AtualizarStatusEmprestimo")
.WithOpenApi();

app.MapHub<EmprestimoHub>("/emprestimohub");

app.Run();
