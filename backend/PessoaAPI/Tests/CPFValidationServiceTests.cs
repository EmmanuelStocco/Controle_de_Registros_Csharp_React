using PessoaAPI.Services;
using Xunit;

namespace PessoaAPI.Tests
{
    public class CPFValidationServiceTests
    {
        [Theory]
        [InlineData("12345678909", true)]  // CPF válido corrigido
        [InlineData("11144477735", true)]
        [InlineData("12345678900", false)]
        [InlineData("11111111111", false)]
        [InlineData("00000000000", false)]
        [InlineData("123456789", false)]
        [InlineData("123456789012", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void IsValidCPF_ShouldReturnCorrectResult(string cpf, bool expected)
        { 
            var result = CPFValidationService.IsValidCPF(cpf); 
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("12345678909", "123.456.789-09")]  // CPF válido corrigido
        [InlineData("11144477735", "111.444.777-35")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void FormatCPF_ShouldReturnFormattedCPF(string cpf, string expected)
        { 
            var result = CPFValidationService.FormatCPF(cpf); 
            Assert.Equal(expected, result);
        }
    }
}

