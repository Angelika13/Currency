using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Currency.Models
{
    public class Valuta
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string EngName { get; set; }
        public int Nominal { get; set; }
        public int ISO_Num_Code { get; set; }
        public string ISO_Char_Code { get; set; }

    }
    public class ValCurs
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public DateTime CursDate { get; set; }
        public int ValutaId { get; set; }
        public Valuta Valuta { get; set; }
    }
    public class CursShow
    {
        public decimal Curs { get; set; }
        public string Str { get; set; }
    }
}
