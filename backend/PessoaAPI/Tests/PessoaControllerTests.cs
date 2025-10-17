using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PessoaAPI.Controllers;
using PessoaAPI.Data;
using PessoaAPI.DTOs;
using PessoaAPI.Services;
using Xunit;
using FluentAssertions;
using Moq;

namespace PessoaAPI.Tests
{
    public class PessoaControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly PessoaController _controller;
        private readonly JWTService _jwtService;

        public PessoaControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:Key", "EstaEhUmaChaveSecretaSuperSeguraParaJWT123456789"},
                    {"Jwt:Issuer", "PessoaAPI"},
                    {"Jwt:Audience", "PessoaAPIUsers"}
                })
                .Build();

            _jwtService = new JWTService(configuration);
            _controller = new PessoaController(_context);
        }

        [Fact]
        public async Task GetPessoas_ShouldReturnEmptyList_WhenNoPessoasExist()
        { 
            var result = await _controller.GetPessoas(); 
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var pessoas = okResult?.Value as IEnumerable<PessoaResponseDTO>;
            pessoas.Should().BeEmpty();
        }

        [Fact]
        public async Task CreatePessoa_ShouldReturnCreatedPessoa_WhenValidData()
        { 
            var pessoaDTO = new PessoaCreateDTO
            {
                Nome = "João Silva",
                CPF = "12345678909",  // CPF válido corrigido
                DataNascimento = new DateTime(1990, 1, 1),
                Email = "joao@email.com"
            };
 
            var result = await _controller.CreatePessoa(pessoaDTO); 
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            var pessoa = createdResult?.Value as PessoaResponseDTO;
            pessoa.Should().NotBeNull();
            pessoa!.Nome.Should().Be("João Silva");
            pessoa.CPF.Should().Be("123.456.789-09");  // Formatação corrigida
        }

        [Fact]
        public async Task CreatePessoa_ShouldReturnBadRequest_WhenCPFIsInvalid()
        {
            var pessoaDTO = new PessoaCreateDTO
            {
                Nome = "João Silva",
                CPF = "12345678900", // CPF inválido
                DataNascimento = new DateTime(1990, 1, 1)
            };

            var result = await _controller.CreatePessoa(pessoaDTO);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreatePessoa_ShouldReturnBadRequest_WhenCPFAlreadyExists()
        {
            var pessoaDTO1 = new PessoaCreateDTO
            {
                Nome = "João Silva",
                CPF = "12345678909",  // CPF válido corrigido
                DataNascimento = new DateTime(1990, 1, 1)
            };

            var pessoaDTO2 = new PessoaCreateDTO
            {
                Nome = "Maria Santos",
                CPF = "12345678909",  // Mesmo CPF
                DataNascimento = new DateTime(1985, 5, 15)
            };

            await _controller.CreatePessoa(pessoaDTO1);
            var result = await _controller.CreatePessoa(pessoaDTO2);
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetPessoa_ShouldReturnNotFound_WhenPessoaDoesNotExist()
        {
            var result = await _controller.GetPessoa(999);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdatePessoa_ShouldReturnNotFound_WhenPessoaDoesNotExist()
        {
            var pessoaDTO = new PessoaUpdateDTO
            {
                Nome = "João Silva",
                CPF = "12345678901",
                DataNascimento = new DateTime(1990, 1, 1)
            };

            var result = await _controller.UpdatePessoa(999, pessoaDTO);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeletePessoa_ShouldReturnNotFound_WhenPessoaDoesNotExist()
        {
            var result = await _controller.DeletePessoa(999);
            result.Should().BeOfType<NotFoundResult>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

