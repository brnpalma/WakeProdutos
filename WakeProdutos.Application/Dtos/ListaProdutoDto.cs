namespace WakeProdutos.Application.Dtos
{
    public class ListaProdutoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public int Estoque { get; set; }
        public decimal Valor { get; set; }
    }
}
