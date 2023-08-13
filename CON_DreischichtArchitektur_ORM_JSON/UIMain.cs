using System;
using Spectre.Console;
using System.Collections.Generic;

namespace CON_DreischichtArchitektur_ORM_JSON
{
    class UIMain
    {
        
        /* Thema: Dreischichtarchitektur, Interface, Dependency Injection und JSON Serialisierung/Deserialisierung
         * UIMain.cs = User Interface Layer
         * BLL.cs = Business Logic Layer
         * DAL.cs = Data Access Layer
         * 
         * Bitte den Pfad zur JSON Datei in der DAL.cs anpassen
         * 
         * In dieser Projektmappe sieht man ein einfaches Beispiel für die Dreischichtarchitektur und Dependency Injection per Constructor.
         * Wir nutzen Dependency Injection, um die BLL von der DAL zu entkoppeln und die DAL austauschbar zu machen.
         * Somit können wir z.B. eine andere DAL nutzen, die Daten aus einer Datenbank oder einer anderen Quelle lädt.
         * 
         * Dependency Injection ist ein Entwurfsmuster, bei dem die Abhängigkeiten eines Objekts nicht innerhalb des Objekts
         * selbst erzeugt werden, sondern von außen bereitgestellt werden. Auch hier gilt das Prinzip der lose gekoppelten
         * Architektur. Die Abhängigkeiten werden also nicht innerhalb des Objekts erzeugt, sondern von außen bereitgestellt.
         * 
         * 
         * Von Marc W. IT42+
         */


        
        

                
        static readonly BLL logic = new BLL(new DAL());
     
        // Wir instanzieren hier die BLL mit einer neu Instanzierten DAL-Objekt.
        // Dank Polymorphie können wir hier auch eine andere DAL nutzen, die z.B. Daten aus einer Datenbank lädt.

        static void Main(string[] args)
        {

            Ui();

        }

        public static void Ui()
        {

            AnsiConsole.Write(new FigletText("DAL Editor").Color(Color.Yellow));
            AnsiConsole.WriteLine();

            bool continueRunning = true;

            while (continueRunning)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Bitte wählen Sie eine Option aus[/]")
                        .AddChoices(new[] {
                            "Alle Mitarbeiter anzeigen",
                            "Suchen",
                            "Mitarbeiter hinzufügen",
                            "Mitarbeiter löschen",
                            "\tMitarbeiter älter als 60 anzeigen",
                            "\tGesamturlaubsanspruch aller Mitarbeiter anzeigen",
                            "\tUrlaubsanspruch eines bestimmten Mitarbeiters anzeigen",
                            "\tMitarbeiter, die in den nächsten 7 Tagen Geburtstag haben, anzeigen",
                            "Beenden"
                        }));

                switch (choice)
                {
                    case "Alle Mitarbeiter anzeigen":
                        DisplayPeople(logic.GetAllPeople());
                        break;
                    case "Suchen":
                        Search();
                        break;

                    case "\tMitarbeiter älter als 60 anzeigen":
                        DisplayPeople(logic.GetEveryoneOver60());
                        break;
                    case "\tGesamturlaubsanspruch aller Mitarbeiter anzeigen":
                        AnsiConsole.MarkupLine($"[blue]Gesamturlaubsanspruch:[/] [green]{logic.GetUrlaubsanspruchAllerMitarbeiter()}[/]");
                        break;
                    case "\tUrlaubsanspruch eines bestimmten Mitarbeiters anzeigen":
                        var mid = AnsiConsole.Ask<string>("[green]Bitte geben Sie die MID des Mitarbeiters ein:[/]");
                        AnsiConsole.MarkupLine($"[blue]Urlaubsanspruch für MID {mid}:[/] [green]{logic.GetUrlaubsanspruchMitarbeiter(mid)}[/]");
                        break;
                    case "\tMitarbeiter, die in den nächsten 7 Tagen Geburtstag haben, anzeigen":
                        DisplayPeople(logic.GetAlleMitarbeiterWhosBirthdayIsInTheNext7Days());
                        break;
                    case "Einen neuen Mitarbeiter erstellen und hinzufügen":
                        CreateAndAddPerson();
                        break;
                    case "Mitarbeiter löschen":
                        DeleteMitarbeiter();
                        break;
                    case "Beenden":
                        continueRunning = false;
                        break;
                }
            }
        }

        static void DeleteMitarbeiter()
        {
            var mid = AnsiConsole.Ask<string>("[green]Bitte geben Sie die MID des Mitarbeiters ein, den Sie löschen möchten:[/]");
            var result = logic.DeletePerson(mid);
            if (result)
            {
                AnsiConsole.MarkupLine("[green]Mitarbeiter erfolgreich gelöscht![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Fehler beim Löschen des Mitarbeiters. Überprüfen Sie die MID und versuchen Sie es erneut.[/]");
            }
        }

        static void CreateAndAddPerson()
        {
            var newPerson = new Person
            {
                MID = AnsiConsole.Ask<string>("[green]Bitte geben Sie die MID ein:[/]"),
                Vorname = AnsiConsole.Ask<string>("[green]Bitte geben Sie den Vornamen ein:[/]"),
                Nachname = AnsiConsole.Ask<string>("[green]Bitte geben Sie den Nachnamen ein:[/]"),
                Geburtsdatum = AnsiConsole.Ask<string>("[green]Bitte geben Sie das Geburtsdatum (dd.MM.yyyy) ein:[/]"),
                UrlaubsTageGesamt = AnsiConsole.Ask<int>("[green]Bitte geben Sie den Gesamturlaubsanspruch ein:[/]"),
                RestUrlaub = AnsiConsole.Ask<int>("[green]Bitte geben Sie den Resturlaub ein:[/]")
            };

            logic.AddPerson(newPerson);
            AnsiConsole.MarkupLine("[green]Mitarbeiter erfolgreich hinzugefügt![/]");
        }

        static void DisplayPeople(List<Person> people)
        {
            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Green);

            table.AddColumn("MID").Centered();
            table.AddColumn("Name").Centered();
            table.AddColumn("Geburtsdatum").Centered();
            table.AddColumn("UrlaubsTageGesamt").Centered();
            table.AddColumn("RestUrlaub").Centered();

            foreach (var person in people)
            {
                table.AddRow(
                    person.MID,
                    $"{person.Vorname} {person.Nachname}",
                    person.Geburtsdatum ?? "ERROR",
                    person.UrlaubsTageGesamt.ToString(), // Convert to string
                    person.RestUrlaub.ToString()  // Convert to string
                );
            }

            AnsiConsole.Write(table);
        }

        static void Search()
        {
            var query = AnsiConsole.Ask<string>("[green]Bitte geben Sie den Suchbegriff ein:[/]");
            var results = logic.SearchForPerson(query);
            if (results.Count > 0)
            {
                DisplayPeople(results);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Keine Ergebnisse gefunden.[/]");
            }
        }

    }
}

