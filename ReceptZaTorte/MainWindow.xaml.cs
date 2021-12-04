using MiodelLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ReceptZaTorte
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window{
		private static BindingList<Recept> baza;

		public static BindingList<Recept> Baza { get => baza; set => baza = value; }

		public MainWindow(){
			InitializeComponent();
			Baza = new BindingList<Recept>();
			DataContext = this;
		}

		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e){
			this.DragMove();
		}

		private void btn_prikazi_Click(object sender, RoutedEventArgs e){
			int zakolikoOsoba = Baza[bazagrid.SelectedIndex].ZakolikoOsoba;
			string imeTorte = Baza[bazagrid.SelectedIndex].ImeTorte;
			DateTime rokTrajanja = Baza[bazagrid.SelectedIndex].RokTrajanja;
			string prikazSlike = Baza[bazagrid.SelectedIndex].PrikazSlike;
			string putanja = Baza[bazagrid.SelectedIndex].Putanja;
			string vremePravljenja = Baza[bazagrid.SelectedIndex].VremePravljenja;
			bool vocnaCokoladna = Baza[bazagrid.SelectedIndex].VocnaCokoladna;

			Pregled p = new Pregled(zakolikoOsoba,imeTorte,rokTrajanja,prikazSlike,putanja,vremePravljenja,vocnaCokoladna);
			p.ShowDialog();
		}

		private void btn_izmeni_Click(object sender, RoutedEventArgs e){
			Dodavanje d = new Dodavanje(Baza[bazagrid.SelectedIndex], bazagrid.SelectedIndex);
			//d.Show();
			d.ShowDialog();
		}

		private void btn_obrisi_Click(object sender, RoutedEventArgs e){
			MessageBoxResult result = MessageBox.Show("Da li želite da obrišete recept?", "Brisanje recepta", MessageBoxButton.YesNo);
			switch (result){
				case MessageBoxResult.Yes:
					Baza.RemoveAt(bazagrid.SelectedIndex);
					//MessageBox.Show($"{bazagrid.SelectedIndex}");
					bazagrid.Items.Refresh();
					break;
				case MessageBoxResult.No:
					break;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e){
			Dodavanje d = new Dodavanje(null,-1);
			//d.Show();
			d.ShowDialog();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e){
			this.Close();
		}
	}
}
