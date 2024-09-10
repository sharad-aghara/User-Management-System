using System;
using System.Collections.Generic;

namespace UMS.DAL.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Gender { get; set; }

    public string? PhotoPath { get; set; }

    public int StateId { get; set; }

    public int DistrictId { get; set; }

    public int TalukaId { get; set; }

    public string? Hobbies { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int RoleId { get; set; }

    public string? PasswordHash { get; set; }

    public bool? IsApproved { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsPasswordChanged { get; set; } = false;

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
