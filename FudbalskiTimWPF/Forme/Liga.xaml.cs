using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Shapes;

namespace FudbalskiTimWPF.Forme
{
    /// <summary>
    /// Interaction logic for Liga.xaml
    /// </summary>
    public partial class Liga : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public Liga()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtIme.Focus();
        }
        public Liga(bool azuriraj, DataRowView pomocniRed) : this()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtIme.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
        }
        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add(@"imeLige", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add(@"bodovi", SqlDbType.Int).Value = txtBodovi.Text;
                cmd.Parameters.Add(@"mesto", SqlDbType.Int).Value = txtMesto.Text;
                cmd.Parameters.Add(@"kvalifikacija", SqlDbType.Bit).Value = Convert.ToInt32(cbxEvropa.IsChecked);
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update dbo.Liga
                                        set ime = @imeLige, bodovi = @bodovi, mesto = @mesto, kvalifikacijeZaEvropu = @kvalifikacija
                                        where ligaID = @id";

                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.Liga(ime, bodovi, mesto, kvalifikacijeZaEvropu)
                                    values(@imeLige, @bodovi, @mesto, @kvalifikacija)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos nije uspesan!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (FormatException)
            {
                MessageBox.Show("Greska prilikom konverzije podataka!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
