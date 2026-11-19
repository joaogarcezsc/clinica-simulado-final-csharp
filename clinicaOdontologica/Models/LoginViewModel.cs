using System.ComponentModel.DataAnnotations;

namespace appOdonto.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [Display(Name = "CPF de Acesso")]
        public string Cpf { get; set; } = string.Empty;
    }
}