using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PizzeriaInForno.Models
{
    public class Prodotto
    {
        [Key]
        public int ProdottoId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Max 255 caratteri")]
        public string NomeProdotto { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Max 1000 caratteri")]
        public string FotoProdotto { get; set; }

        [Required]
        [Column(TypeName = "decimal(4,2)")]
        [Range(1, 99, ErrorMessage = "Scegli un prezzo da 1€ a 99€")]
        public decimal PrezzoProdotto { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 caratteri")]
        public string TempoDiPreparazione { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Max 1000 caratteri")]
        public string Ingredienti {  get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Devi scegliere una categoria!")]
        public string Categoria { get; set; }

        public OrdineItem OrdineItem { get; set; }
    }
}