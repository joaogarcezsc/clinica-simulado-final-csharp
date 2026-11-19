using System;
using System.Collections.Generic;

namespace clinicaOdontologica.Models;

public partial class Agendamento
{
    public int Codigo { get; set; }

    public DateTime DataHora { get; set; }

    public string StatusAgendamento { get; set; } = null!;

    public int PacienteId { get; set; }

    public int ProcedimentoId { get; set; }

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual Procedimento Procedimento { get; set; } = null!;
}
