using System;
using System.Collections.Generic;

namespace clinicaOdontologica.Models;

public partial class Procedimento
{
    public int Codigo { get; set; }

    public string Descricao { get; set; } = null!;

    public decimal Valor { get; set; }

    public int PacienteId { get; set; }

    public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

    public virtual Paciente Paciente { get; set; } = null!;
}
