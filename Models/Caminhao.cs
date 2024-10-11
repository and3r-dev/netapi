namespace VeiculoAPI.Models
{
    public class Caminhao
    {
        public int Id { get; set; }
        public decimal CapacidadeCarga { get; set; }
        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }
    }
}
