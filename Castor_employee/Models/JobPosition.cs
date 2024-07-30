using System;
using System.Collections.Generic;

namespace Castor_employee.Models;

public partial class JobPosition
{
    public int IdCargo { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
