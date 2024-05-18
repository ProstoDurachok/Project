using SQLite;
using System;
using System.Collections.Generic;
using System.Text;


namespace App2.Models
{
    [Table("Ispolnitel")]
    public class Ispolnitel
        {   
            [PrimaryKey, AutoIncrement]
            public int ID_Ispolnitel { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string Patronymic { get; set; }
            public string Loginad { get; set; }
            public string Passwordd { get; set; }
            public string Email { get; set; }
            public string Opis { get; set; }
            public string Skils { get; set; }
    }
}
