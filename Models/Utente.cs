using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PizzeriaInForno.Models
{
    public class Utente
    {
        [Key]
        public int UtenteId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Max 255 caratteri")]
        public string Email { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Max 16 caratteri/numeri")]
        public string Password { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max 30 caratteri")]
        public string Nome { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max 100 caratteri")]
        public string Cognome { get; set; }

        [Required]
        [Column(TypeName = "decimal(11,0)")]
        public decimal Telefono { get; set; }

        public string Ruolo { get; set; } = "user";

        public ICollection<OrdineItem> OrdineItems { get; set; }
        public ICollection<RiepilogoOrdine> RiepilogoOrdini { get; set; }
    }
}