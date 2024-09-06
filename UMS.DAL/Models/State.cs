using System;
using System.Collections.Generic;

namespace UMS.DAL.Models;

public partial class State
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}
