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
    /// Interaction logic for Trofej.xaml
    /// </summary>
    public partial class Trofej : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public Trofej()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtNaziv.Focus();
            PopuniListu();
        }
        public Trofej(bool azuriraj, DataRowView pomocniRed) : this()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtNaziv.Focus();
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
        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add(@"naziv", SqlDbType.NVarChar).Value = txtNaziv.Text;
                cmd.Parameters.Add(@"godina", SqlDbType.Int).Value = txtGodina.Text;


                cmd.Parameters.Add(@"fudbalskiTimID", SqlDbType.Int).Value = cbFK.SelectedValue;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update dbo.Trofej
                                       set
                                        nazivTrofeja = @naziv,
                                        godinaOsvajanja = @godina,
                                        fudbalskiTimID = @fudbalskiTimID,
                                        
                                        where trofejID = @id";

                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.Trofej(nazivTrofeja, godinaOsvajanja, fudbalskiTimID)
                                    values(@naziv, @godina, @fudbalskiTimID)";
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
