using System.Collections.Generic;

namespace VeiculoAPI.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string? Placa { get; set; }
        public int Ano { get; set; }
        public string? Cor { get; set; }
        public string? Modelo { get; set; }

        public Carro? Carro { get; set; }
        public Caminhao? Caminhao { get; set; }
        public List<Revisao>? Revisoes { get; set; }
    }
}
