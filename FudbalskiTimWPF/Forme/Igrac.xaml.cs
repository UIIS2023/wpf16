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
    /// Interaction logic for Igrac.xaml
    /// </summary>
    public partial class Igrac : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public Igrac()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtImeIgraca.Focus();
            PopuniListu();
        }
        public Igrac(bool azuriraj, DataRowView pomocniRed) : this()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtImeIgraca.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            PopuniListu();
        }
        private void PopuniListu()
        {
            try
            {
                konekcija.Open();

                string vratiTim = @"select fudbalskiTimID, imeTima from dbo.tblFudbalskiTim";
                DataTable dtTim = new DataTable();
                SqlDataAdapter daTim = new SqlDataAdapter(vratiTim, konekcija);
                daTim.Fill(dtTim);

                // Postavljanje ComboBox-a
                cbFK.ItemsSource = dtTim.DefaultView;
                cbFK.DisplayMemberPath = "imeTima"; // Postavljanje imena polja koje će se prikazivati
                cbFK.SelectedValuePath = "fudbalskiTimID"; // Postavljanje vrednosti za odabrani element

                // Oslobađanje resursa
                dtTim.Dispose();
                daTim.Dispose();
            }

            catch (SqlException)
            {
                MessageBox.Show("Padajuce liste nisu popunjene", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }

        }
        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add(@"imeIgraca", SqlDbType.NVarChar).Value = txtImeIgraca.Text;
                cmd.Parameters.Add(@"prezimeIgraca", SqlDbType.NVarChar).Value = txtPrezimeIgraca.Text;
                cmd.Parameters.Add(@"godineIgraca", SqlDbType.Int).Value = txtGodineIgraca.Text;
                cmd.Parameters.Add(@"visinaIgraca", SqlDbType.Int).Value = txtVisinaIgraca.Text;
                cmd.Parameters.Add(@"nacionalnostIgraca", SqlDbType.NVarChar).Value = txtNacionalnostIgraca.Text;
                cmd.Parameters.Add(@"plataIgraca", SqlDbType.Decimal).Value = txtPlataIgraca.Text;
                cmd.Parameters.Add(@"pozicijaIgraca", SqlDbType.NVarChar).Value = txtPozicijaIgraca.Text;

                cmd.Parameters.Add(@"fudbalskiTimID", SqlDbType.Int).Value = cbFK.SelectedValue; 

                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update dbo.Igrac
                                        set ime =@imeIgraca, prezime = @prezimeIgraca, godine = @godineIgraca, visina =@visinaIgraca, nacionalnost = @nacionalnostIgraca, plata = @plataIgraca, pozicija = @pozicijaIgraca, fudbalskiTimID = @fudbalskiTimID
                                        where igracID = @id";

                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.Igrac(ime, prezime, godine, visina, nacionalnost, plata, pozicija, fudbalskiTimID)
                                    values(@imeIgraca, @prezimeIgraca, @godineIgraca, @visinaIgraca, @nacionalnostIgraca, @plataIgraca, @pozicijaIgraca, @fudbalskiTimID)";
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

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
