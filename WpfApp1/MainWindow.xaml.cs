using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CON_DreischichtArchitektur_ORM_JSON;


/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        

        // Using ObservableCollection to automatically update UI when items are added/removed
        public ObservableCollection<Person> Mitarbeiters { get; set; }
        
        public static BLL _dataAccess = new BLL(new DAL());


        public MainWindow()
        {
            // You might set _dataAccess here using a service locator or other means
            InitializeComponent();
            InitializeComponent();
            LoadData();
            lstMitarbeiter.ItemsSource = Mitarbeiters;
        }



        private void LoadData()
        {
            // You can replace this with your data loading logic
            Mitarbeiters = new ObservableCollection<Person>(_dataAccess.GetAllPeople());
        }

        private void btnCreateMitarbeiter_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddMitarbeiterWindow();
            if (addWindow.ShowDialog() == true && addWindow.NewMitarbeiter != null)
            {
                // Adjust the date format if necessary
                DateTime parsedDate;
                if (DateTime.TryParse(addWindow.NewMitarbeiter.Geburtsdatum, out parsedDate))
                {
                    addWindow.NewMitarbeiter.Geburtsdatum = parsedDate.ToString("dd.MM.yyyy");
                }

                Mitarbeiters.Add(addWindow.NewMitarbeiter);
                _dataAccess.AddPerson(addWindow.NewMitarbeiter);
            }
        }

        private void btnDeleteMitarbeiter_Click(object sender, RoutedEventArgs e)
        {
            if (lstMitarbeiter.SelectedItem is Person selectedPerson)
            {
                // Remove from ObservableCollection
                Mitarbeiters.Remove(selectedPerson);

                // Remove from data store
                if (!_dataAccess.DeletePerson(selectedPerson.MID))
                {
                    MessageBox.Show("An error occurred while deleting the Mitarbeiter.");
                }
            }
            else
            {
                MessageBox.Show("Please select a Mitarbeiter to delete.");
            }
        }

    }
}