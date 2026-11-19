using System;
using System.Collections.Generic;

namespace clinicaOdontologica.Models;

public partial class Paciente
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public DateOnly DataNascimento { get; set; }

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

    public virtual ICollection<Procedimento> Procedimentos { get; set; } = new List<Procedimento>();
}
