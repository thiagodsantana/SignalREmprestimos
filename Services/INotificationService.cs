using SignalREmprestimos.Models;

namespace SignalREmprestimos.Services
{
    public interface INotificationService
    {
        Task NotificarNovaSolicitacao(Emprestimo emprestimo);
        Task NotificarStatusAtualizado(Emprestimo emprestimo);
    }
}
