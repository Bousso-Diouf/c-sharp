﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Gestion.Model
{
    public class Utilisateur:Personne
    {
        [MaxLength(20)]
        public string Identifiant { get; set; }

        [MaxLength(512)]
        public string MotDePasse { get; set; }

        [MaxLength(20)]
        public string Statut { get; set; }
    }
}
