using Microsoft.Extensions.Configuration;
using PessoaAPI.Services;
using Xunit;

namespace PessoaAPI.Tests
{
    public class JWTServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly JWTService _jwtService;

        public JWTServiceTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:Key", "EstaEhUmaChaveSecretaSuperSeguraParaJWT123456789"},
                    {"Jwt:Issuer", "PessoaAPI"},
                    {"Jwt:Audience", "PessoaAPIUsers"}
                })
                .Build();

            _jwtService = new JWTService(_configuration);
        }

        [Fact]
        public void ValidateUser_ShouldReturnTrue_WhenValidCredentials()
        { 
            var result = _jwtService.ValidateUser("admin", "admin123"); 
            Assert.True(result);
        }

        [Fact]
        public void ValidateUser_ShouldReturnFalse_WhenInvalidCredentials()
        { 
            var result = _jwtService.ValidateUser("admin", "wrongpassword"); 
            Assert.False(result);
        }

        [Fact]
        public void ValidateUser_ShouldReturnFalse_WhenUserDoesNotExist()
        { 
            var result = _jwtService.ValidateUser("nonexistent", "password"); 
            Assert.False(result);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidToken()
        { 
            var token = _jwtService.GenerateToken("admin"); 
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Theory]
        [InlineData("admin", "admin123", true)]
        [InlineData("user", "user123", true)]
        [InlineData("test", "test123", true)]
        [InlineData("admin", "wrong", false)]
        [InlineData("wrong", "admin123", false)]
        public void ValidateUser_ShouldReturnCorrectResult(string username, string password, bool expected)
        { 
            var result = _jwtService.ValidateUser(username, password); 
            Assert.Equal(expected, result);
        }
    }
}

