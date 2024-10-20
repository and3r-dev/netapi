using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeiculoAPI.Data;
using VeiculoAPI.Models;

namespace VeiculoAPI.Controllers
{
    [Route("api/veiculos")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> GetVeiculos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var veiculos = await _context.Veiculos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return Ok(veiculos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> GetVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }
            return veiculo;
        }

        [HttpPost]
        public async Task<ActionResult<Veiculo>> PostVeiculo(VeiculoDTO veiculoDTO)
        {
             var veiculo = new Veiculo
                {
                    Placa = veiculoDTO.Placa,
                    Ano = veiculoDTO.Ano,
                    Modelo = veiculoDTO.Modelo,
                    Cor = veiculoDTO.Cor
                };

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            if (veiculoDTO.TipoVeiculo == 1)
            {
                var carro = new Carro
                {
                    VeiculoId = veiculo.Id,
                    CapacidadePassageiro = veiculoDTO.CapacidadePassageiro ?? 0
                };

                _context.Carros.Add(carro);
            } else if (veiculoDTO.TipoVeiculo == 2)
            {
                var caminhao = new Caminhao
                {
                    VeiculoId = veiculo.Id,
                    CapacidadeCarga = veiculoDTO.CapacidadeCarga ?? 0M
                };

                _context.Caminhoes.Add(caminhao);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVeiculo), new { id = veiculo.Id }, veiculo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVeiculo(int id, Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return BadRequest();
            }

            _context.Entry(veiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VeiculoExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVeiculo(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}
