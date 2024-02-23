using FudbalskiTimWPF.Forme;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace FudbalskiTimWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ucitanaTabela;
        bool azuriraj;
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        private DataRowView red;
        #region Select upiti
        private static string IgraciSelect = @"select IgracID as ID, ime, prezime , godine, visina, nacionalnost, plata, dbo.tblFudbalskiTim.imeTima as ImeKluba from dbo.Igrac join dbo.tblFudbalskiTim on dbo.Igrac.fudbalskiTimID = dbo.tblFudbalskiTim.fudbalskiTimID";
        private static string DresSelect = @"select dresID as ID, boje, proizvodjac, dbo.tblFudbalskiTim.imeTima as ImeKluba from dbo.Dres join dbo.tblFudbalskiTim on dbo.Dres.fudbalskiTimID = dbo.tblFudbalskiTim.fudbalskiTimID";
        private static string LigaSelect = @"select ligaID as ID, ime, bodovi, mesto, kvalifikacijeZaEvropu from dbo.Liga";
        private static string NavijacSelect = @"select navijacID as ID, ime, godine, clanskaKarta, dbo.tblFudbalskiTim.imeTima as ImeKluba from dbo.Navijac join dbo.tblFudbalskiTim on dbo.Navijac.fudbalskiTimID = dbo.tblFudbalskiTim.fudbalskiTimID";
        private static string SezonaSelect = @"select sezonaID as ID, godina, titula, bodovi, evropa, dbo.tblFudbalskiTim.imeTima as ImeKluba from dbo.Sezona join dbo.tblFudbalskiTim on dbo.Sezona.fudbalskiTimID = dbo.tblFudbalskiTim.fudbalskiTimID";
        private static string SponzorSelect = @"select sponzorID as ID, ime as ImeSponzora, oblast from dbo.Sponzor";
        private static string StadionSelect = @"select stadionID as ID, imeStadiona, kapacitetMesta, dbo.tblFudbalskiTim.imeTima as ImeKluba from dbo.Stadion join dbo.tblFudbalskiTim on dbo.Stadion.fudbalskiTimID = dbo.tblFudbalskiTim.fudbalskiTimID";
        private static string TrenerSelect = @"select trenerID as ID, ime, prezime, godine, plata, iskustvo, titule, nacionalnost from dbo.Trener";
        private static string TrofejSelect = @"select trofejID as ID, nazivTrofeja, godinaOsvajanja, dbo.tblFudbalskiTim.imeTima as ImeKluba from dbo.Trofej join dbo.tblFudbalskiTim on dbo.Trofej.fudbalskiTimID = dbo.tblFudbalskiTim.fudbalskiTimID";
        private static string FudbalskiTimSelect = @"select fudbalskiTimID as ID, nadimak, godinaOsnivanja, vrednostKluba, imeTima, dbo.Liga.ime as ImeLige from dbo.tblFudbalskiTim  join dbo.Liga on dbo.tblFudbalskiTim.ligaID = dbo.Liga.ligaID";
        #endregion
        #region Select sa uslovom
        private static string selectUslovIgrac = @"select * from dbo.Igrac where igracID=";
        private static string selectUslovDres = @"select * from dbo.Dres where dresID=";
        private static string selectUslovLiga = @"select * from dbo.Liga where ligaID=";
        private static string selectUslovNavijac = @"select * from dbo.Navijac where navijacID=";
        private static string selectUslovSezona = @"select * from dbo.Sezona where sezonaID=";
        private static string selectUslovSponzor = @"select * from dbo.Sponzor where sponzorID=";
        private static string selectUslovStadion = @"select * from dbo.Stadion where stadionID=";
        private static string selectUslovTrener = @"select * from dbo.Trener where trenerID=";
        private static string selectUslovTrofej = @"select * from dbo.Trofej where trofejID=";
        private static string selectUslovFudbalskiTim = @"select * from dbo.tblFudbalskiTim where fudbalskiTimID=";
        #endregion

        #region Delete upiti
        private static string IgracDelete = @"Delete from dbo.Igrac where igracID =";
        private static string DresDelete = @"Delete from dbo.Dres where dresID =";
        private static string LigaDelete = @"Delete from dbo.Liga where ligaID =";
        private static string NavijacDelete = @"Delete from dbo.Navijac where navijacID =";
        private static string SezonaDelete = @"Delete from dbo.Sezona where sezonaID =";
        private static string SponzorDelete = @"Delete from dbo.Spoznor where sponzorID =";
        private static string StadionDelete = @"Delete from dbo.Stadion where stadionID =";
        private static string TrenerDelete = @"Delete from dbo.Trener where trenerID =";
        private static string TrofejDelete = @"Delete from dbo.Trofej where trofejID =";
        private static string FudbalskiTimDelete = @"Delete from dbo.FudbalskiTim where fudbalskiTimID =";
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke(IgraciSelect);
            //difoltna tabela kad se prikaze mainwindow
        }
        private void UcitajPodatke(string selectUpit)
        {
            {
                try
                {
                    konekcija.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(selectUpit, konekcija);
                    DataTable dt = new DataTable();

                    dataAdapter.Fill(dt);
                    if (dataGridCentralni != null)
                    {
                        dataGridCentralni.ItemsSource = dt.DefaultView;

                    }
                    ucitanaTabela = selectUpit;
                    dt.Dispose();
                    dataAdapter.Dispose();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Neuspesno ucitani podaci.\n " + ex.Message,
                                    "Greska", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
                finally
                {
                    if (konekcija != null)
                        konekcija.Close();
                }
            }
        }
        private void btnIgraci_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(IgraciSelect);
        }

        private void btnLiga_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(LigaSelect);
        }

        private void btnNavijac_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(NavijacSelect);
        }

        private void btnSezona_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(SezonaSelect);
        }

        private void btnSpoznor_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(SponzorSelect);
        }

        private void btnStadion_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(StadionSelect);
        }

        private void btnDres_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(DresSelect);
        }

        private void btnFudbalskiTim_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(FudbalskiTimSelect);
        }
        private void btnTrofej_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(TrofejSelect);
        }
        private void btnTrener_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(TrenerSelect);
        }
        void PopuniFormu(string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                DataRowView red = (DataRowView)dataGridCentralni.SelectedItems[0];
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                cmd.CommandText = selectUslov + "@id";
                SqlDataReader citac = cmd.ExecuteReader();
                cmd.Dispose();
                if (citac.Read())
                {
                    if (ucitanaTabela.Equals(IgraciSelect))
                    {
                        Igrac prozorIgrac = new Igrac(azuriraj, red);
                        prozorIgrac.txtImeIgraca.Text = citac["Ime"].ToString();
                        prozorIgrac.txtPrezimeIgraca.Text = citac["Prezime"].ToString();
                        prozorIgrac.txtGodineIgraca.Text = citac["Godine"].ToString();
                        prozorIgrac.txtVisinaIgraca.Text = citac["Visina"].ToString();
                        prozorIgrac.txtPlataIgraca.Text = citac["Plata"].ToString();
                        prozorIgrac.txtPozicijaIgraca.Text = citac["Pozicija"].ToString();
                        prozorIgrac.txtNacionalnostIgraca.Text = citac["nacionalnost"].ToString();
                        prozorIgrac.cbFK.SelectedValue = citac["fudbalskiTimID"].ToString();

                        prozorIgrac.ShowDialog();

                    }
                    else if (ucitanaTabela.Equals(DresSelect))
                    {
                        Dres prozorDres = new Dres(azuriraj, red);
                        prozorDres.txtProizvodjac.Text = citac["proizvodjac"].ToString();
                        prozorDres.txtBoja.Text = citac["boje"].ToString();
                        prozorDres.cbFK.SelectedValue = citac["fudbalskiTimID"].ToString();

                        prozorDres.ShowDialog();

                    }
                    else if (ucitanaTabela.Equals(LigaSelect))
                    {
                        Liga prozorLiga = new Liga(azuriraj, red);
                        prozorLiga.txtIme.Text = citac["Ime"].ToString();
                        prozorLiga.txtBodovi.Text = citac["Bodovi"].ToString();
                        prozorLiga.txtMesto.Text = citac["Mesto"].ToString();
                        prozorLiga.cbxEvropa.IsChecked = (Boolean)citac["kvalifikacijeZaEvropu"];

                        prozorLiga.ShowDialog();


                    }
                    else if (ucitanaTabela.Equals(NavijacSelect))
                    {
                        Navijac prozorNavijac = new Navijac(azuriraj, red);
                        prozorNavijac.txtIme.Text = citac["Ime"].ToString();
                        prozorNavijac.txtGodine.Text = citac["Godine"].ToString();
                        prozorNavijac.cbxCK.IsChecked = (Boolean)citac["clanskaKarta"];

                        prozorNavijac.cbFK.SelectedValue = citac["fudbalskiTimID"].ToString();
                        prozorNavijac.ShowDialog();


                    }
                    else if (ucitanaTabela.Equals(SezonaSelect))
                    {
                        Sezona prozorSezona = new Sezona(azuriraj, red);
                        prozorSezona.txtTitula.Text = citac["Titula"].ToString();
                        prozorSezona.txtGodina.Text = citac["Godina"].ToString();
                        prozorSezona.txtBodovi.Text = citac["Bodovi"].ToString();
                        prozorSezona.cbxEvropa.IsChecked = (Boolean)citac["Evropa"];

                        prozorSezona.cbFK.SelectedValue = citac["fudbalskiTimID"].ToString();
                        prozorSezona.ShowDialog();


                    }
                    else if (ucitanaTabela.Equals(TrenerSelect))
                    {
                        Trener prozorTrener = new Trener(azuriraj, red);
                        prozorTrener.txtIme.Text = citac["Ime"].ToString();
                        prozorTrener.txtPrezime.Text = citac["Prezime"].ToString();
                        prozorTrener.txtGodine.Text = citac["Godine"].ToString();
                        prozorTrener.txtPlata.Text = citac["Plata"].ToString();
                        prozorTrener.txtIskustvo.Text = citac["Iskustvo"].ToString();
                        prozorTrener.txtTitule.Text = citac["Titule"].ToString();
                        prozorTrener.txtNacionalnost.Text = citac["Nacionalnost"].ToString();

                        prozorTrener.ShowDialog();


                    }
                    else if (ucitanaTabela.Equals(FudbalskiTimSelect))
                    {
                        FudbalskiTim ft = new FudbalskiTim(azuriraj, red);
                        ft.txtNadimak.Text = citac["Nadimak"].ToString();
                        ft.txtIme.Text = citac["imeTima"].ToString();
                        ft.txtGodinaOsnivanja.Text = citac["godinaOsnivanja"].ToString();
                        ft.txtVrednost.Text = citac["vrednostKluba"].ToString();

                        ft.cbLiga.SelectedValue = citac["ligaID"].ToString();
                        ft.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(SponzorSelect))
                    {
                        Sponzor ft = new Sponzor(azuriraj, red);
                        ft.txtIme.Text = citac["Ime"].ToString();
                        ft.txtOblast.Text = citac["Oblast"].ToString();

                        ft.cbDres.SelectedValue = citac["dresID"].ToString();
                        ft.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(StadionSelect))
                    {
                        Stadion ft = new Stadion(azuriraj, red);
                        ft.txtIme.Text = citac["ImeStadiona"].ToString();
                        ft.txtKapacitet.Text = citac["KapacitetMesta"].ToString();

                        ft.cbFK.SelectedValue = citac["fudbalskiTimID"].ToString();
                        ft.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(TrofejSelect))
                    {
                        Trofej ft = new Trofej(azuriraj, red);
                        ft.txtNaziv.Text = citac["NazivTrofeja"].ToString();
                        ft.txtGodina.Text = citac["GodinaOsvajanja"].ToString();

                        ft.cbFK.SelectedValue = citac["fudbalskiTimID"].ToString();
                        ft.ShowDialog();
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("Niste selektovali red!" + ex.Message, "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
                azuriraj = false;
            }
        }
        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;
            if (ucitanaTabela.Equals(IgraciSelect))
            {
                prozor = new Igrac();
                prozor.ShowDialog();
                UcitajPodatke(IgraciSelect);
            }
            else if (ucitanaTabela.Equals(DresSelect))
            {
                prozor = new Dres();
                prozor.ShowDialog();
                UcitajPodatke(DresSelect);
            }
            else if (ucitanaTabela.Equals(LigaSelect))
            {
                prozor = new Liga();
                prozor.ShowDialog();
                UcitajPodatke(LigaSelect);
            }
            else if (ucitanaTabela.Equals(NavijacSelect))
            {
                prozor = new Navijac();
                prozor.ShowDialog();
                UcitajPodatke(NavijacSelect);
            }
            else if (ucitanaTabela.Equals(SezonaSelect))
            {
                prozor = new Sezona();
                prozor.ShowDialog();
                UcitajPodatke(SezonaSelect);
            }
            else if (ucitanaTabela.Equals(TrenerSelect))
            {
                prozor = new Trener();
                prozor.ShowDialog();
                UcitajPodatke(TrenerSelect);
            }
            else if (ucitanaTabela.Equals(FudbalskiTimSelect))
            {
                prozor = new FudbalskiTim();
                prozor.ShowDialog();
                UcitajPodatke(FudbalskiTimSelect);
            }
            else if (ucitanaTabela.Equals(SponzorSelect))
            {
                prozor = new Sponzor();
                prozor.ShowDialog();
                UcitajPodatke(SponzorSelect);
            }
            else if (ucitanaTabela.Equals(StadionSelect))
            {
                prozor = new Stadion();
                prozor.ShowDialog();
                UcitajPodatke(StadionSelect);
            }
            else if (ucitanaTabela.Equals(TrofejSelect))
            {
                prozor = new Trofej();
                prozor.ShowDialog();
                UcitajPodatke(TrofejSelect);
            }
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(IgraciSelect))
            {
                PopuniFormu(selectUslovIgrac);
                UcitajPodatke(IgraciSelect);
            }
            else if (ucitanaTabela.Equals(DresSelect))
            {
                PopuniFormu(selectUslovDres);
                UcitajPodatke(DresSelect);
            }
            else if (ucitanaTabela.Equals(LigaSelect))
            {
                PopuniFormu(selectUslovLiga);
                UcitajPodatke(LigaSelect);
            }
            else if (ucitanaTabela.Equals(NavijacSelect))
            {
                PopuniFormu(selectUslovNavijac);
                UcitajPodatke(NavijacSelect);
            }
            else if (ucitanaTabela.Equals(SezonaSelect))
            {
                PopuniFormu(selectUslovSezona);
                UcitajPodatke(SezonaSelect);
            }
            else if (ucitanaTabela.Equals(SponzorSelect))
            {
                PopuniFormu(selectUslovSponzor);
                UcitajPodatke(SponzorSelect);
            }
            else if (ucitanaTabela.Equals(StadionSelect))
            {
                PopuniFormu(selectUslovStadion);
                UcitajPodatke(StadionSelect);
            }
            else if (ucitanaTabela.Equals(FudbalskiTimSelect))
            {
                PopuniFormu(selectUslovFudbalskiTim);
                UcitajPodatke(FudbalskiTimSelect);
            }
            else if (ucitanaTabela.Equals(TrenerSelect))
            {
                PopuniFormu(selectUslovTrener);
                UcitajPodatke(TrenerSelect);
            }
            else if (ucitanaTabela.Equals(TrofejSelect))
            {
                PopuniFormu(selectUslovTrofej);
                UcitajPodatke(TrofejSelect);
            }
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(IgraciSelect))
            {
                ObrisiIspis(IgracDelete);
                UcitajPodatke(IgraciSelect);
            }
            else if (ucitanaTabela.Equals(DresSelect))
            {
                ObrisiIspis(DresDelete);
                UcitajPodatke(DresSelect);
            }
            else if (ucitanaTabela.Equals(LigaSelect))
            {
                ObrisiIspis(LigaDelete);
                UcitajPodatke(LigaSelect);
            }
            else if (ucitanaTabela.Equals(NavijacSelect))
            {
                ObrisiIspis(NavijacDelete);
                UcitajPodatke(NavijacSelect);
            }
            else if (ucitanaTabela.Equals(SezonaSelect))
            {
                ObrisiIspis(SezonaDelete);
                UcitajPodatke(SezonaSelect);
            }
            else if (ucitanaTabela.Equals(SponzorSelect))
            {
                ObrisiIspis(SponzorDelete);
                UcitajPodatke(SponzorSelect);
            }
            else if (ucitanaTabela.Equals(StadionSelect))
            {
                ObrisiIspis(StadionDelete);
                UcitajPodatke(StadionSelect);
            }
            else if (ucitanaTabela.Equals(FudbalskiTimSelect))
            {
                ObrisiIspis(FudbalskiTimDelete);
                UcitajPodatke(FudbalskiTimSelect);
            }
            else if (ucitanaTabela.Equals(TrenerSelect))
            {
                ObrisiIspis(TrenerDelete);
                UcitajPodatke(TrenerSelect);
            }
            else if (ucitanaTabela.Equals(TrofejSelect))
            {
                ObrisiIspis(TrofejDelete);
                UcitajPodatke(TrofejSelect);
            }
        }
        private void ObrisiIspis(string deleteUpit)
        {
            try
            {
                konekcija.Open();
                red = (DataRowView)dataGridCentralni.SelectedItems[0];
                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezultat == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = konekcija
                    };
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = deleteUpit + "@id";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u drugim tabelama", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }

        }
    }
}
