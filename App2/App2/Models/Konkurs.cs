using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Models
{
    [Table("Konkurs")]    
        public class Konkurs
        {
            [PrimaryKey, AutoIncrement]
            public int ID_koncurs { get; set; }
            public int IdIspolnitel { get; set; }
            public int IdZakazchik { get; set; }
            public string Opisaniye { get; set; }
            public int Vremya { get; set; }
            public int Price { get; set; }
            public string Kategoria { get; set; }

        }
}
