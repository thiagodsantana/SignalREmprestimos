using Microsoft.AspNetCore.SignalR;

namespace SignalREmprestimos.Services
{
    public class EmprestimoHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var grupo = httpContext?.Request.Query["grupo"].ToString().ToLower();

            if (!string.IsNullOrWhiteSpace(grupo))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, grupo);
                Console.WriteLine($"Conexão {Context.ConnectionId} adicionada ao grupo '{grupo}'");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var grupo = httpContext?.Request.Query["grupo"].ToString().ToLower();

            if (!string.IsNullOrWhiteSpace(grupo))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, grupo);
                Console.WriteLine($"Conexão {Context.ConnectionId} removida do grupo '{grupo}'");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
