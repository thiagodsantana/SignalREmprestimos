using Microsoft.AspNetCore.SignalR;
using SignalREmprestimos.Models;
using static SignalREmprestimos.Services.NotificationService;

namespace SignalREmprestimos.Services
{
    public class NotificationService(IHubContext<EmprestimoHub> hub) : INotificationService
    {
        private readonly IHubContext<EmprestimoHub> _hub = hub;

        public async Task NotificarNovaSolicitacao(Emprestimo emprestimo)
        {
            await _hub.Clients.Group("operadores").SendAsync("NovaSolicitacao", emprestimo);
        }

        public async Task NotificarStatusAtualizado(Emprestimo emprestimo)
        {
            await _hub.Clients.Group("clientes").SendAsync("StatusAtualizado", emprestimo);
            await _hub.Clients.Group("operadores").SendAsync("StatusAtualizado", emprestimo);
            await _hub.Clients.Group("financeiro").SendAsync("StatusAtualizado", emprestimo);
        }
    }
}
