namespace Dell.Academy.Atividade.Application.ViewModels
{
    public class EnderecoViewModel
    {
        public long Id { get; set; }
        public string Rua { get; set; }
        public int Numero { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public long FuncionarioId { get; set; }
    }
}