public class VeiculoDTO
{
    public int Id { get; set; } 
    public int TipoVeiculo { get; set; } 
    public string Placa { get; set; }
    public int Ano { get; set; }
    public string Modelo { get; set; }
    public string Cor { get; set; }
    public int? CapacidadePassageiro { get; set; } 
    public decimal? CapacidadeCarga { get; set; } 
}
