namespace VeiculoAPI.Models
{
    public class Carro
    {
        public int Id { get; set; }
        public int CapacidadePassageiro { get; set; }
        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }
    }
}
