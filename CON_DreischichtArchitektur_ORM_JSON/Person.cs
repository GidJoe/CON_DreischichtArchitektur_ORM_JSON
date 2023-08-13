using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CON_DreischichtArchitektur_ORM_JSON
{
    public class Person
    {
        public string? MID { get; set; }
        public string? Vorname { get; set; }
        public string? Nachname { get; set; }
        public string? Geburtsdatum { get; set; }
        public int UrlaubsTageGesamt { get; set; }
        public int RestUrlaub { get; set; }

    }

}
