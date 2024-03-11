using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PizzeriaInForno.Models
{
    public class RiepilogoOrdine
    {
        [Key]
        public int RiepilogoOrdineId { get; set; }

        public int UtenteId { get; set; }

        public string DataOrdine { get; set; }

        public string IndirizzoSpedizione { get; set; }

        public string Nota { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal PrezzoTotale { get; set; }

        public string Stato { get; set; }

        public ICollection<OrdineItem> OrdineItems { get; set; }
        public Utente Utente { get; set; }
    }
}