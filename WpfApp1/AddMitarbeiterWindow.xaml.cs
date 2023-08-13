using CON_DreischichtArchitektur_ORM_JSON;
using System.Windows;

namespace WpfApp1
{
    public partial class AddMitarbeiterWindow : Window
    {
        public Person NewMitarbeiter { get; private set; }

        public AddMitarbeiterWindow()
        {
            InitializeComponent();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            int urlaubsTageGesamt;
            int restUrlaub;

            if (!int.TryParse(UrlaubsTageGesamt.Text, out urlaubsTageGesamt) ||
                !int.TryParse(RestUrlaub.Text, out restUrlaub))
            {
                MessageBox.Show("Please enter valid numbers for UrlaubsTageGesamt and RestUrlaub.");
                return;
            }

            NewMitarbeiter = new Person
            {
                MID = txtMID.Text,
                Vorname = txtVorname.Text,
                Nachname = txtNachname.Text,
                Geburtsdatum = dpGeburtsdatum.SelectedDate.HasValue ? dpGeburtsdatum.SelectedDate.Value.ToString("yyyy-MM-dd") : "",
                UrlaubsTageGesamt = urlaubsTageGesamt,
                RestUrlaub = restUrlaub
            };

            this.DialogResult = true;
            this.Close();
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
