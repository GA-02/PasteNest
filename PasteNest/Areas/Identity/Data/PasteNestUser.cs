using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PasteNest.Areas.Identity.Data;

// Add profile data for application users by adding properties to the PasteNestUser class
public class PasteNestUser : IdentityUser
{
    [Required]
    [StringLength(20, MinimumLength = 4)]
    public string Nickname { get; set; } = string.Empty;
}

