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
            var totalVeiculos = await _context.Veiculos.CountAsync();

             var veiculos = await _context.Veiculos
                .Include(v => v.Carro)
                .Include(v => v.Caminhao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new
            {
                actual_page = page,
                total_pages = (int)Math.Ceiling(totalVeiculos / (double)pageSize),
                total_veiculos = totalVeiculos,
                data = veiculos
            };

            return Ok(response);
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
        public async Task<IActionResult> PutVeiculo(int id, VeiculoDTO veiculoDTO)
        {
            if (id != veiculoDTO.Id)
            {
                return BadRequest();
            }

            var veiculoExistente = await _context.Veiculos
                                        .Include(v => v.Carro)
                                        .Include(v => v.Caminhao)
                                        .FirstOrDefaultAsync(v => v.Id == id);

            if (veiculoExistente == null)
            {
                return NotFound();
            }

            if (veiculoDTO.TipoVeiculo == 1)
            {
                var carro = await _context.Carros
                            .FirstOrDefaultAsync(c => c.VeiculoId == veiculoExistente.Id);

                if (carro != null)
                {
                    carro.CapacidadePassageiro = veiculoDTO.CapacidadePassageiro ?? 0;
                    _context.Carros.Update(carro);
                }
                else
                {
                    carro = new Carro
                    {
                        VeiculoId = veiculoExistente.Id,
                        CapacidadePassageiro = veiculoDTO.CapacidadePassageiro ?? 0
                    };
                    _context.Carros.Add(carro);
                }

                if (veiculoExistente.Caminhao != null)
                {
                    _context.Caminhoes.Remove(veiculoExistente.Caminhao);
                }
            }
            else if (veiculoDTO.TipoVeiculo == 2)
            {
                var caminhao = await _context.Caminhoes
                            .FirstOrDefaultAsync(c => c.VeiculoId == veiculoExistente.Id);

                if (caminhao != null)
                {
                    caminhao.CapacidadeCarga = veiculoDTO.CapacidadeCarga ?? 0;
                    _context.Caminhoes.Update(caminhao);
                }
                else
                {
                    caminhao = new Caminhao
                    {
                        VeiculoId = veiculoExistente.Id,
                        CapacidadeCarga = veiculoDTO.CapacidadeCarga ?? 0
                    };
                    _context.Caminhoes.Add(caminhao);
                }

                if (veiculoExistente.Carro != null)
                {
                    _context.Carros.Remove(veiculoExistente.Carro);
                }
            }

            _context.Entry(veiculoExistente).CurrentValues.SetValues(veiculoDTO);

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

            return Ok(new 
            {
                veiculoExistente.Id,
                veiculoExistente.Placa,
                veiculoExistente.Ano,
                veiculoExistente.Modelo,
                veiculoExistente.Cor,
            });
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
