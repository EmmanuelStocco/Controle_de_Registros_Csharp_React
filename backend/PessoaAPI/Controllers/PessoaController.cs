using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PessoaAPI.Data;
using PessoaAPI.DTOs;
using PessoaAPI.Models;
using PessoaAPI.Services;

namespace PessoaAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/pessoa")]
    [Authorize]
    public class PessoaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PessoaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaResponseDTO>>> GetPessoas()
        {
            var pessoas = await _context.Pessoas.ToListAsync();
            return Ok(pessoas.Select(p => new PessoaResponseDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Sexo = p.Sexo,
                Email = p.Email,
                DataNascimento = p.DataNascimento,
                Naturalidade = p.Naturalidade,
                Nacionalidade = p.Nacionalidade,
                CPF = CPFValidationService.FormatCPF(p.CPF),
                Endereco = p.Endereco,
                DataCadastro = p.DataCadastro,
                DataAtualizacao = p.DataAtualizacao
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaResponseDTO>> GetPessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return Ok(new PessoaResponseDTO
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Sexo = pessoa.Sexo,
                Email = pessoa.Email,
                DataNascimento = pessoa.DataNascimento,
                Naturalidade = pessoa.Naturalidade,
                Nacionalidade = pessoa.Nacionalidade,
                CPF = CPFValidationService.FormatCPF(pessoa.CPF),
                Endereco = pessoa.Endereco,
                DataCadastro = pessoa.DataCadastro,
                DataAtualizacao = pessoa.DataAtualizacao
            });
        }

        [HttpPost]
        public async Task<ActionResult<PessoaResponseDTO>> CreatePessoa([FromBody] PessoaCreateDTO pessoaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validação de CPF
            if (!CPFValidationService.IsValidCPF(pessoaDTO.CPF))
            {
                return BadRequest(new { message = "CPF inválido" });
            }

            // Verificar se CPF já existe
            var cpfExists = await _context.Pessoas.AnyAsync(p => p.CPF == pessoaDTO.CPF);
            if (cpfExists)
            {
                return BadRequest(new { message = "CPF já cadastrado" });
            }

            // Verificar se email já existe (se preenchido)
            if (!string.IsNullOrEmpty(pessoaDTO.Email))
            {
                var emailExists = await _context.Pessoas.AnyAsync(p => p.Email == pessoaDTO.Email);
                if (emailExists)
                {
                    return BadRequest(new { message = "Email já cadastrado" });
                }
            }

            var pessoa = new Pessoa
            {
                Nome = pessoaDTO.Nome,
                Sexo = pessoaDTO.Sexo,
                Email = pessoaDTO.Email,
                DataNascimento = pessoaDTO.DataNascimento,
                Naturalidade = pessoaDTO.Naturalidade,
                Nacionalidade = pessoaDTO.Nacionalidade,
                CPF = pessoaDTO.CPF,
                Endereco = string.IsNullOrWhiteSpace(pessoaDTO.Endereco) ? null : pessoaDTO.Endereco, // v1: endereço opcional
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, new PessoaResponseDTO
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Sexo = pessoa.Sexo,
                Email = pessoa.Email,
                DataNascimento = pessoa.DataNascimento,
                Naturalidade = pessoa.Naturalidade,
                Nacionalidade = pessoa.Nacionalidade,
                CPF = CPFValidationService.FormatCPF(pessoa.CPF),
                DataCadastro = pessoa.DataCadastro,
                DataAtualizacao = pessoa.DataAtualizacao
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePessoa(int id, [FromBody] PessoaUpdateDTO pessoaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            // Validação de CPF
            if (!CPFValidationService.IsValidCPF(pessoaDTO.CPF))
            {
                return BadRequest(new { message = "CPF inválido" });
            }

            // Verificar se CPF já existe em outro registro
            var cpfExists = await _context.Pessoas.AnyAsync(p => p.CPF == pessoaDTO.CPF && p.Id != id);
            if (cpfExists)
            {
                return BadRequest(new { message = "CPF já cadastrado" });
            }

            // Verificar se email já existe em outro registro (se preenchido)
            if (!string.IsNullOrEmpty(pessoaDTO.Email))
            {
                var emailExists = await _context.Pessoas.AnyAsync(p => p.Email == pessoaDTO.Email && p.Id != id);
                if (emailExists)
                {
                    return BadRequest(new { message = "Email já cadastrado" });
                }
            }

            pessoa.Nome = pessoaDTO.Nome;
            pessoa.Sexo = pessoaDTO.Sexo;
            pessoa.Email = pessoaDTO.Email;
            pessoa.DataNascimento = pessoaDTO.DataNascimento;
            pessoa.Naturalidade = pessoaDTO.Naturalidade;
            pessoa.Nacionalidade = pessoaDTO.Nacionalidade;
            pessoa.CPF = pessoaDTO.CPF;
            pessoa.Endereco = string.IsNullOrWhiteSpace(pessoaDTO.Endereco) ? null : pessoaDTO.Endereco; // v1: endereço opcional
            pessoa.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

