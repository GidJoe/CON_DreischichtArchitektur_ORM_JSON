using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;

namespace CON_DreischichtArchitektur_ORM_JSON
{
    /* Hier haben wir unseren Dataaccess-Layer
    * Dieser ist für den Zugriff auf die Datenbank zuständig und kapselt die Datenbankzugriffe
    * Die Schnittstelle für die Datenbankzugriffe wird in der IPersonDataAccess.cs definiert
    *  In der DAL.cs werden auschließlich notwendige Methoden für den Zugriff auf die Datenbank geschrieben
    */
    public class DAL : IPersonDataAccess
    {
        private const string _pathToJsonFile = @"C:\Users\MWB\OneDrive\CloudRepos\CON_DreischichtArchitektur_ORM_JSON\CON_DreischichtArchitektur_ORM_JSON\MitarbeiterDB.json";

        public List<Person> LoadAllPeople()
        {

            if (File.Exists(_pathToJsonFile))
            {

                var jsonData = File.ReadAllText(_pathToJsonFile);
                return JsonConvert.DeserializeObject<List<Person>>(jsonData) ?? new List<Person>();
            }
            return new List<Person>();
        }


        public void SaveAllPeople(List<Person> people)
        {
            var jsonData = JsonConvert.SerializeObject(people, Formatting.Indented);
            File.WriteAllText(_pathToJsonFile, jsonData);

        }

        public Person FindPersonByMID(string mid)
        {

            return LoadAllPeople().FirstOrDefault(p => p.MID == mid) ?? new Person();

        }

        public void AddPerson(Person person)
        {
            var people = LoadAllPeople();
            people.Add(person);
            SaveAllPeople(people);
        }

        public void UpdatePerson(Person personToUpdate)
        {
            var people = LoadAllPeople();
            var index = people.FindIndex(p => p.MID == personToUpdate.MID);

            if (index != -1)
            {
                people[index] = personToUpdate;
                SaveAllPeople(people);
            }
        }

        public void DeletePerson(string mid)
        {
            var people = LoadAllPeople();
            var person = people.FirstOrDefault(p => p.MID == mid);
            if (person != null)
            {
                people.Remove(person);
                SaveAllPeople(people);
            }

        }
    }
}
