using System;
using System.Collections.Generic;

namespace UMS.DAL.Models;

public partial class Taluka
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int DistrictId { get; set; }

    public virtual District District { get; set; } = null!;
}
