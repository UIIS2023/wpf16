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
    /// Interaction logic for Sponzor.xaml
    /// </summary>
    public partial class Sponzor : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public Sponzor()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtIme.Focus();
            PopuniListu();
        }
        public Sponzor(bool azuriraj, DataRowView pomocniRed) : this()
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

                string vratiDres = @"select dresID, boje from dbo.Dres";
                DataTable dtDres = new DataTable();
                SqlDataAdapter daDres = new SqlDataAdapter(vratiDres, konekcija);
                daDres.Fill(dtDres);

                // Postavljanje ComboBox-a
                cbDres.ItemsSource = dtDres.DefaultView;
                cbDres.DisplayMemberPath = "boje"; // Postavljanje imena polja koje će se prikazivati
                cbDres.SelectedValuePath = "dresID"; // Postavljanje vrednosti za odabrani element

                // Oslobađanje resursa
                dtDres.Dispose();
                daDres.Dispose();
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
                cmd.Parameters.Add(@"ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add(@"oblast", SqlDbType.NVarChar).Value = txtOblast.Text;


                cmd.Parameters.Add(@"dresID", SqlDbType.Int).Value = cbDres.SelectedValue;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update dbo.Sponzor
                                       set
                                        ime = @ime,
                                        oblast = @oblast,
                                        dresID = @dresID
                                        
                                        where sponzorID = @id";

                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.Sponzor(ime, oblast, dresID)
                                    values(@ime, @oblast, @dresID)";
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
