﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;


namespace RestSupplyDB.Models
{
    [Table("RolesSet")]
    public partial class RolesSet : IdentityRole
    {
    }
}