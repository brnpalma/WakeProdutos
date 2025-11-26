namespace WakeProdutos.Shared.Results
{
    public class Result<T>
    {
        public bool Sucesso { get; set; }
        public int Status { get; set; }
        public T? Data { get; set; }
        public string Mensagem { get; set; }

        private Result() { }

        public static Result<T> Ok(T data, int status) => new() 
        { 
            Status = status,
            Sucesso = true, 
            Data = data 
        };

        public static Result<T> Fail(int status, string error, T? data) => new() 
        { 
            Status = status,
            Data = data,
            Sucesso = false, 
            Mensagem = error 
        };
    }
}
