﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TP_03_Module_05.BO
{
    public class Pizza
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Nom { get; set; }
        public Pate Pate { get; set; }
        
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    }
}