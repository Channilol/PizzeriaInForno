using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzeriaInForno.Models
{
    public class OrdineItem
    {
        [Key]
        public int OrdineItemId { get; set; }

        public int RiepilogoOrdineId { get; set; }

        public int ProdottoId{ get; set; }

        public int UtenteId { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Min. 1, Max. 10")]
        public int Quantita { get; set; }

        public decimal PrezzoOrdine { get; set; }

        public RiepilogoOrdine RiepilogoOrdine { get; set; }
        public Utente Utente { get; set; }
        public Prodotto Prodotto { get; set; }
    }
}