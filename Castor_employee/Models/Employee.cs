using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Castor_employee.Models;

public partial class Employee
{
    public int Id { get; set; }

    public int Cedula { get; set; }

    public string Nombre { get; set; } = null!;

    public string? FotoPath { get; set; }

    public DateTime FechaIngreso { get; set; }

    public int? IdCargo { get; set; }

    [JsonIgnore]
    public virtual JobPosition? IdCargoNavigation { get; set; }
}
