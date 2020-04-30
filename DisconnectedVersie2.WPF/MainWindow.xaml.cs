using System;
using System.Collections.Generic;
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
using DisconnectedVersie2.LIB.Services;
using DisconnectedVersie2.LIB.Entities;

namespace DisconnectedVersie2.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        bool isNieuw;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            XMLHelper.GetData();
            cmbSoorten.ItemsSource = XMLHelper.addressTypes;
            lstAdressen.ItemsSource = XMLHelper.addresses;
            MaakLeeg();
            grpAdressen.IsEnabled = true;
            grpGegevens.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;

        }
        private void MaakLeeg()
        {
            txtNaam.Text = "";
            txtAdres.Text = "";
            txtPost.Text = "";
            txtGemeente.Text = "";
            txtLand.Text = "";
            cmbSoorten.SelectedIndex = -1;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            XMLHelper.SaveData();
        }

        private void lstAdressen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MaakLeeg();
            if (lstAdressen.SelectedIndex == -1) return;

            Address address = (Address)lstAdressen.SelectedItem;
            txtNaam.Text = address.Naam;
            txtAdres.Text = address.Adres;
            txtPost.Text = address.Post;
            txtGemeente.Text = address.Gemeente;
            txtLand.Text = address.Land;
            int indeks = 0;
            foreach(AddressType addressType in cmbSoorten.Items)
            {
                if(addressType.ID == address.Soort_ID)
                {
                    cmbSoorten.SelectedIndex = indeks;
                    break;
                }
                indeks++;
            }


        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            isNieuw = true;
            grpAdressen.IsEnabled = false;
            grpGegevens.IsEnabled = true;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
            MaakLeeg();
            txtNaam.Focus();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            isNieuw = false;
            grpAdressen.IsEnabled = false;
            grpGegevens.IsEnabled = true;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
            txtNaam.Focus();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtNaam.Text.Trim() == "")
            {
                MessageBox.Show("Naam invoeren !", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNaam.Focus();
                return;
            }
            if (cmbSoorten.SelectedIndex == -1)
            {
                MessageBox.Show("Adressoort selecteren !", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbSoorten.Focus();
                return;

            }
            Address address;
            if(isNieuw)
            {
                address = new Address();
                address.ID = Guid.NewGuid().ToString();
            }
            else
            {
                address = (Address)lstAdressen.SelectedItem;
            }
            address.Naam = txtNaam.Text;
            address.Adres = txtAdres.Text;
            address.Post = txtPost.Text;
            address.Gemeente = txtGemeente.Text;
            address.Land = txtLand.Text;

            AddressType addressType = (AddressType)cmbSoorten.SelectedItem;
            address.Soort_ID = addressType.ID;

            if (isNieuw)
                XMLHelper.addresses.Add(address);

            grpAdressen.IsEnabled = true;
            grpGegevens.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;

            lstAdressen.ItemsSource = null;
            lstAdressen.Items.Clear();
            lstAdressen.ItemsSource = XMLHelper.addresses;

            int indeks = 0;
            foreach(Address zoekadres in lstAdressen.Items)
            {
                if(zoekadres == address)
                {
                    lstAdressen.SelectedIndex = indeks;
                    break;
                }
                indeks++;

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            grpAdressen.IsEnabled = true;
            grpGegevens.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;
            lstAdressen_SelectionChanged(null, null);
            lstAdressen.Focus();
        }
    }
}
