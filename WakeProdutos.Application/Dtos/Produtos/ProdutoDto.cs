namespace WakeProdutos.Application.Dtos.Produtos
{
    public class ProdutoDto
    {
        public bool Sucesso { get; set; }
        public long Id { get; set; }
        public string Nome { get; set; }
        public int Estoque { get; set; }
        public decimal Valor { get; set; }
        public string Mensagem { get; set; }
    }
}
