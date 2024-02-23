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
    /// Interaction logic for Dres.xaml
    /// </summary>
    public partial class Dres : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public Dres()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtProizvodjac.Focus();
            PopuniListu();
        }
        public Dres(bool azuriraj, DataRowView pomocniRed) : this()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtProizvodjac.Focus();
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
                cmd.Parameters.Add("@proizvodjacDresa", System.Data.SqlDbType.NVarChar).Value
                    = txtProizvodjac.Text;
                cmd.Parameters.Add("@bojaDresa", System.Data.SqlDbType.NVarChar).Value
                    = txtBoja.Text;

                cmd.Parameters.Add("@fk", System.Data.SqlDbType.Int).Value
                  = cbFK.SelectedValue;

                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update dbo.Dres
                                        set proizvodjac = @proizvodjacDresa, boje = @bojaDresa, fudbalskiTimID = @fk 
                                        Where dresID = @id";
                    this.pomocniRed = null;
                }
                else
                {
                    cmd.CommandText = @"insert into dbo.Dres(boje, proizvodjac, fudbalskiTimID)
                                        values(@bojaDresa, @proizvodjacDresa, @fk)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch(SqlException ex)
            {
                MessageBox.Show("Unos nije uspesan!" + ex.Message, "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
