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
    /// Interaction logic for FudbalskiTim.xaml
    /// </summary>
    public partial class FudbalskiTim : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FudbalskiTim()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtIme.Focus();
            PopuniListu();
        }
        public FudbalskiTim(bool azuriraj, DataRowView pomocniRed) : this()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtIme.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            PopuniListu();
        }
        private void PopuniListu()
        {

            try
            {
                konekcija.Open();
            
                string vratiLigu = @"select ligaID, ime from dbo.Liga";
                DataTable dtLiga = new DataTable();
                SqlDataAdapter daLiga = new SqlDataAdapter(vratiLigu, konekcija);
                daLiga.Fill(dtLiga);

                // Postavljanje ComboBox-a
                cbLiga.ItemsSource = dtLiga.DefaultView;
                cbLiga.DisplayMemberPath = "ime"; // Postavljanje imena polja koje će se prikazivati
                cbLiga.SelectedValuePath = "ligaID"; // Postavljanje vrednosti za odabrani element

                // Oslobađanje resursa
                dtLiga.Dispose();
                daLiga.Dispose();
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
                cmd.Parameters.Add(@"imeTima", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add(@"godinaOsnivanja", SqlDbType.Int).Value = txtGodinaOsnivanja.Text;
                cmd.Parameters.Add(@"nadimak", SqlDbType.NVarChar).Value = txtNadimak.Text;
                cmd.Parameters.Add(@"vrednost", SqlDbType.Int).Value = txtVrednost.Text;

                cmd.Parameters.Add(@"ligaID", SqlDbType.Int).Value = cbLiga.SelectedValue;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update dbo.tblFudbalskiTim
                                       set
                                        imeTima = @imeTima,
                                        nadimak = @nadimak,
                                        godinaOsnivanja = @godinaOsnivanja,
                                        vrednostKluba = @vrednost,
                                        ligaID = @ligaID
                                        where fudbalskiTimID = @id";

                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.tblFudbalskiTim(nadimak, godinaOsnivanja, vrednostKluba, imeTima, ligaID)
                                    values(@nadimak, @godinaOsnivanja, @vrednost, @imeTima, @ligaID)";
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
        



