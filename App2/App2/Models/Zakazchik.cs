using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Models
{
    [Table("Zakazchik")]
    public class Zakazchik
    {
        [PrimaryKey, AutoIncrement]
        public int ID_zakazchik { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronymic { get; set; }
        public string Loginad { get; set; }
        public string Passwordd { get; set; }
        public string Email { get; set; }
    }
}
