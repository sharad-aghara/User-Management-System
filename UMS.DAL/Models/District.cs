using System;
using System.Collections.Generic;

namespace UMS.DAL.Models;

public partial class District
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int StateId { get; set; }

    public virtual State State { get; set; } = null!;

    public virtual ICollection<Taluka> Talukas { get; set; } = new List<Taluka>();
}
