using System;
using System.Data.Entity;
using System.Linq;

namespace PizzeriaInForno.Models
{
    public class DBContext : DbContext
    {
        // Il contesto è stato configurato per utilizzare una stringa di connessione 'DBContext' dal file di configurazione 
        // dell'applicazione (App.config o Web.config). Per impostazione predefinita, la stringa di connessione è destinata al 
        // database 'PizzeriaInForno.Models.DBContext' nell'istanza di LocalDb. 
        // 
        // Per destinarla a un database o un provider di database differente, modificare la stringa di connessione 'DBContext' 
        // nel file di configurazione dell'applicazione.
        public DBContext()
            : base("name=DBContext")
        {
        }

        // Aggiungere DbSet per ogni tipo di entità che si desidera includere nel modello. Per ulteriori informazioni 
        // sulla configurazione e sull'utilizzo di un modello Code, vedere http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Prodotto> Prodotti { get; set; }

        public virtual DbSet<Utente> Utenti { get; set; }

        public virtual DbSet<OrdineItem> OrdineItems { get; set; }

        public virtual DbSet<RiepilogoOrdine> RiepilogoOrdini { get; set; }

        //}

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

}