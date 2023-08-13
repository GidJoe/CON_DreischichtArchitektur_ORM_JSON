using System.Linq;

namespace CON_DreischichtArchitektur_ORM_JSON
{

    /* BLL ist der Business Logic Layer und hier findet die eigentliche Logik der Anwendung statt
     * Wenn wir Zugriff auf den DAL benötigen, dann holen wir uns diesen über die Dependency Injection von außen
     * 
     * Einige Methoden sind hier mit Lambda-Ausdrücken und LINQ geschrieben, um die Möglichkeiten von C# zu zeigen
     * Für ein besseres Verständis was Lambdas und LINQ sind, schaut euch bitte die alternativen mit "SimpleSyntax" an.
     */

    public class BLL
    {
        private IPersonDataAccess _dataAccess;

        // Konstruktor, der ein IPersonDataAccess akzeptiert
        // Dependency Injection
        public BLL(IPersonDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public List<Person> GetAllPeople()
        {

            return _dataAccess.LoadAllPeople();


        }

        public Person GetPersonByMID(string mid)
        {
            return _dataAccess.FindPersonByMID(mid);
        }

        public void AddPerson(Person person)
        {
            _dataAccess.AddPerson(person);
        }

        public void UpdatePerson(Person personToUpdate)
        {
            _dataAccess.UpdatePerson(personToUpdate);
        }

        public bool DeletePerson(string mid)
        {
            try
            {
                _dataAccess.DeletePerson(mid);
                return true;
            }
            catch
            {
                // Logging wäre hier sinnvoll
                return false;
            }
        }


        public List<Person> GetEveryoneOver60()
        {
            var allPeople = _dataAccess.LoadAllPeople();
            return allPeople.Where(p =>
            {
                DateTime dob;
                if (DateTime.TryParse(p.Geburtsdatum, out dob))
                {
                    int age = DateTime.Now.Year - dob.Year;
                    if (DateTime.Now.DayOfYear < dob.DayOfYear) age -= 1;
                    return age > 60;
                }
                return false;
            }).ToList();
        }

        public List<Person> GetEveryoneOver60SimpleSyntax()
        {
            var allPeople = _dataAccess.LoadAllPeople();
            var result = new List<Person>();

            foreach (var person in allPeople)
            {
                try
                {
                    DateTime dob = DateTime.Parse(person.Geburtsdatum);
                    int age = DateTime.Now.Year - dob.Year;
                    if (DateTime.Now.DayOfYear < dob.DayOfYear)
                        age -= 1;

                    if (age > 60)
                    {
                        result.Add(person);
                    }
                }
                catch (FormatException)
                {
                    // Error handling hinzufügen
                    continue;
                }
            }

            return result;
        }


        public int GetUrlaubsanspruchAllerMitarbeiter()
        {
            var allPeople = _dataAccess.LoadAllPeople();
            return allPeople.Sum(p => p.UrlaubsTageGesamt);
        }

        public int GetUrlaubsanspruchMitarbeiter(string mid)
        {
            var person = _dataAccess.FindPersonByMID(mid);
            if (person != null)
                return person.UrlaubsTageGesamt;
            else
                return -1; //
        }

        public List<Person> GetAlleMitarbeiterWhosBirthdayIsInTheNext7Days()
        {
            var allPeople = _dataAccess.LoadAllPeople();
            return allPeople.Where(p =>
            {
                DateTime dob;
                if (DateTime.TryParse(p.Geburtsdatum, out dob))
                {
                    var today = DateTime.Today;
                    var nextWeek = today.AddDays(7);
                    var thisYearBirthday = new DateTime(today.Year, dob.Month, dob.Day);
                    return thisYearBirthday >= today && thisYearBirthday <= nextWeek;
                }
                return false;
            }).ToList();
        }

        public List<Person> GetAlleMitarbeiterWhosBirthdayIsInTheNext7DaysSimpleSyntax()
        {
            var allPeople = _dataAccess.LoadAllPeople();
            var result = new List<Person>();

            DateTime today = DateTime.Today;
            DateTime nextWeek = today.AddDays(7);

            foreach (var person in allPeople)
            {
                DateTime dob;
                if (DateTime.TryParse(person.Geburtsdatum, out dob))
                {
                    DateTime thisYearBirthday = new DateTime(today.Year, dob.Month, dob.Day);
                    if (thisYearBirthday >= today && thisYearBirthday <= nextWeek)
                    {
                        result.Add(person);
                    }
                }
            }

            return result;
        }

        public List<Person> SearchForPerson(string query)
        {
            var allPeople = _dataAccess.LoadAllPeople();
            return allPeople.Where(p =>
                p.MID.Contains(query) ||
                p.Vorname.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                p.Nachname.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                p.Geburtsdatum.Contains(query) ||
                p.UrlaubsTageGesamt.ToString().Contains(query) ||
                p.RestUrlaub.ToString().Contains(query)

            ).ToList();
        }

        public List<Person> SearchForPersonSimpleSyntax(string query)
        {
            var allPeople = _dataAccess.LoadAllPeople();
            var result = new List<Person>();

            foreach (var person in allPeople)
            {
                if (person.MID.Contains(query) ||
                    person.Vorname.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    person.Nachname.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    person.Geburtsdatum.Contains(query) ||
                    person.UrlaubsTageGesamt.ToString().Contains(query) ||
                    person.RestUrlaub.ToString().Contains(query))
                {
                    result.Add(person);
                }
            }
            return result;
        }

    }
}
