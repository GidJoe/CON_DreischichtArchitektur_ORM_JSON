namespace CON_DreischichtArchitektur_ORM_JSON
{

    // Hier wird die Schnittstelle definiert, die die Datenbankzugriffe kapselt
    // Die Implementierung dieser Schnittstelle wird in der DAL.cs vorgenommen

    public interface IPersonDataAccess
    {
        List<Person> LoadAllPeople();
        void SaveAllPeople(List<Person> people);
        Person FindPersonByMID(string mid);
        void AddPerson(Person person);
        void UpdatePerson(Person personToUpdate);
        void DeletePerson(string mid);

    }
}
