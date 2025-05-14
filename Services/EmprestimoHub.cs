using Microsoft.AspNetCore.SignalR;
using SignalREmprestimos.Models;

namespace SignalREmprestimos.Services
{
    public class EmprestimoHub : Hub
    {
        // Método para notificar quando o status de um empréstimo for atualizado
        public async Task StatusAtualizado(Emprestimo emprestimo)
        {
            await Clients.All.SendAsync("StatusAtualizado", emprestimo);
        }
    }

}
