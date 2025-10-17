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
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class PessoaV2Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PessoaV2Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaV2ResponseDTO>>> GetPessoas()
        {
            var pessoas = await _context.PessoasV2.ToListAsync();
            return Ok(pessoas.Select(p => new PessoaV2ResponseDTO
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
        public async Task<ActionResult<PessoaV2ResponseDTO>> GetPessoa(int id)
        {
            var pessoa = await _context.PessoasV2.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            return Ok(new PessoaV2ResponseDTO
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
        public async Task<ActionResult<PessoaV2ResponseDTO>> CreatePessoa([FromBody] PessoaV2CreateDTO pessoaDTO)
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
            var cpfExists = await _context.PessoasV2.AnyAsync(p => p.CPF == pessoaDTO.CPF);
            if (cpfExists)
            {
                return BadRequest(new { message = "CPF já cadastrado" });
            }

            // Verificar se email já existe (se preenchido)
            if (!string.IsNullOrEmpty(pessoaDTO.Email))
            {
                var emailExists = await _context.PessoasV2.AnyAsync(p => p.Email == pessoaDTO.Email);
                if (emailExists)
                {
                    return BadRequest(new { message = "Email já cadastrado" });
                }
            }

            var pessoa = new PessoaV2
            {
                Nome = pessoaDTO.Nome,
                Sexo = pessoaDTO.Sexo,
                Email = pessoaDTO.Email,
                DataNascimento = pessoaDTO.DataNascimento,
                Naturalidade = pessoaDTO.Naturalidade,
                Nacionalidade = pessoaDTO.Nacionalidade,
                CPF = pessoaDTO.CPF,
                Endereco = pessoaDTO.Endereco,
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now
            };

            _context.PessoasV2.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, new PessoaV2ResponseDTO
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePessoa(int id, [FromBody] PessoaV2UpdateDTO pessoaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoa = await _context.PessoasV2.FindAsync(id);
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
            var cpfExists = await _context.PessoasV2.AnyAsync(p => p.CPF == pessoaDTO.CPF && p.Id != id);
            if (cpfExists)
            {
                return BadRequest(new { message = "CPF já cadastrado" });
            }

            // Verificar se email já existe em outro registro (se preenchido)
            if (!string.IsNullOrEmpty(pessoaDTO.Email))
            {
                var emailExists = await _context.PessoasV2.AnyAsync(p => p.Email == pessoaDTO.Email && p.Id != id);
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
            pessoa.Endereco = pessoaDTO.Endereco;
            pessoa.DataAtualizacao = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.PessoasV2.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            _context.PessoasV2.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

