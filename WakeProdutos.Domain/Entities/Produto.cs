namespace WakeProdutos.Domain.Entities
{
    public class Produto(string nome, int estoque, decimal valor)
    {
        public long Id { get; set; }
        public string Nome { get; set; } = nome;
        public int Estoque { get; set; } = estoque;
        public decimal Valor { get; set; } = valor;

        public static bool CheckData(string nome, int estoque, decimal valor, out Produto produto, out string? error)
        {
            produto = null;
            error = null;

            if (string.IsNullOrWhiteSpace(nome))
            {
                error = "Nome do produto não pode ser vazio.";
                return false;
            }

            if (valor < 0)
            {
                error = "O valor do produto não pode ser negativo.";
                return false;
            }

            produto = new Produto(nome.Trim(), estoque, valor);
            return true;
        }
    }

}
