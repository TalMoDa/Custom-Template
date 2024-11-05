using System;
using System.Collections.Generic;

namespace My.Custom.Template.Data.Entities.EF
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}