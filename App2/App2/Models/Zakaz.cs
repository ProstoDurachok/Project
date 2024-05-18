using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Models
{
    [Table("Zakaz")]
    public class Zakaz
    {
        [PrimaryKey, AutoIncrement]
        public int ID_zakaz { get; set; }
        public int IdZakazchik { get; set; }
        public int IdIspolnitel { get; set; }
        public string Opisaniye { get; set; }
        public int Vremya { get; set; }
        public int Price { get; set; }
        public string Kategoria { get; set; }
        public string Status { get; set; }

    }
}
