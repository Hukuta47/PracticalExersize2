using System;
using System.Collections.Generic;

namespace API_Comfort.Models;

public partial class Partner
{
    public int Id { get; set; }

    public int TypeId { get; set; }

    public string Name { get; set; } = null!;

    public string LegalAddress { get; set; } = null!;

    public string Inn { get; set; } = null!;

    public string DirectorFullName { get; set; } = null!;

    public string NumberPhone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[]? Logo { get; set; }

    public double Rating { get; set; }

    public virtual PartnerType Type { get; set; } = null!;
}
