using System.ComponentModel.DataAnnotations;

namespace PessoaAPI.DTOs
{
    public class PessoaCreateDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(1, ErrorMessage = "Sexo deve ser M ou F")]
        public string? Sexo { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        [StringLength(50, ErrorMessage = "Naturalidade deve ter no máximo 50 caracteres")]
        public string? Naturalidade { get; set; }

        [StringLength(50, ErrorMessage = "Nacionalidade deve ter no máximo 50 caracteres")]
        public string? Nacionalidade { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter exatamente 11 dígitos")]
        public string CPF { get; set; } = string.Empty;
    }

    public class PessoaUpdateDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(1, ErrorMessage = "Sexo deve ser M ou F")]
        public string? Sexo { get; set; }

        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Data de nascimento é obrigatória")]
        public DateTime DataNascimento { get; set; }

        [StringLength(50, ErrorMessage = "Naturalidade deve ter no máximo 50 caracteres")]
        public string? Naturalidade { get; set; }

        [StringLength(50, ErrorMessage = "Nacionalidade deve ter no máximo 50 caracteres")]
        public string? Nacionalidade { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter exatamente 11 dígitos")]
        public string CPF { get; set; } = string.Empty;
    }

    public class PessoaResponseDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Sexo { get; set; }
        public string? Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? Naturalidade { get; set; }
        public string? Nacionalidade { get; set; }
        public string CPF { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }

    public class PessoaV2CreateDTO : PessoaCreateDTO
    {
        [Required(ErrorMessage = "Endereço é obrigatório na versão 2")]
        [StringLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
        public string Endereco { get; set; } = string.Empty;
    }

    public class PessoaV2UpdateDTO : PessoaUpdateDTO
    {
        [Required(ErrorMessage = "Endereço é obrigatório na versão 2")]
        [StringLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
        public string Endereco { get; set; } = string.Empty;
    }

    public class PessoaV2ResponseDTO : PessoaResponseDTO
    {
        public string Endereco { get; set; } = string.Empty;
    }

    public class LoginDTO
    {
        [Required(ErrorMessage = "Username é obrigatório")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password é obrigatório")]
        public string Password { get; set; } = string.Empty;
    }

    public class TokenResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}

