namespace SignalREmprestimos.Models
{
    public class Emprestimo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Cliente { get; set; }
        public decimal Valor { get; set; }
        public int Parcelas { get; set; }
        public string Status { get; set; } = "Aguardando";
    }

}
