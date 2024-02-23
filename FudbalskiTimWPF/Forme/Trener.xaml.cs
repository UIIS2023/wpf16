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
using System.Windows.Media.Animation;

namespace FudbalskiTimWPF.Forme
{
    /// <summary>
    /// Interaction logic for Trener.xaml
    /// </summary>
    public partial class Trener : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public Trener()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtIme.Focus();
        }
        public Trener(bool azuriraj, DataRowView pomocniRed) : this()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtIme.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
        }
        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bntSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add(@"Ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add(@"Prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                cmd.Parameters.Add(@"Godine", SqlDbType.Int).Value = txtGodine.Text;
                cmd.Parameters.Add(@"Plata", SqlDbType.Decimal).Value = txtPlata.Text;
                cmd.Parameters.Add(@"Iskustvo", SqlDbType.NVarChar).Value = txtIskustvo.Text;
                cmd.Parameters.Add(@"Titule", SqlDbType.Int).Value = txtTitule.Text;
                cmd.Parameters.Add(@"Nacionalnost", SqlDbType.NVarChar).Value = txtNacionalnost.Text;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update dbo.Trener
                                        set ime =@Ime, 
                                        godine = @godine, 
                                        prezime= @Prezime, 
                                        plata = @Plata, 
                                        iskustvo = @Iskustvo, 
                                        titule = @Titule, 
                                        nacionalnost = @Nacionalnost
                                        where trenerID = @id";

                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.Trener(ime, prezime, godine, plata, iskustvo, titule, nacionalnost)
                                    values(@Ime, @Prezime, @Godine, @Plata, @Iskustvo, @Titule, @Nacionalnost)";
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
    }
}
