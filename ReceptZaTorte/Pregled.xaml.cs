using System;
using System.Collections.Generic;
using System.IO;
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

namespace ReceptZaTorte
{
	/// <summary>
	/// Interaction logic for Pregled.xaml
	/// </summary>
	public partial class Pregled : Window{
		public Pregled(int zakolikoOsoba, string imeTorte, DateTime rokTrajanja, string prikazSlike, string putanja, string vremePravljenja, bool vocnaCokoladna){
			InitializeComponent();
			Uri uri = new Uri(prikazSlike);
			slika.Source = new BitmapImage(uri);
			imetorte.Content = imeTorte;
			osoba.Content = zakolikoOsoba;
			rok.Content = $"do {rokTrajanja.Day} . {rokTrajanja.Month} . {rokTrajanja.Year}";
			vreme.Content = vremePravljenja + "(hh:mm)";
			vc.Content = vocnaCokoladna == true ? "Vocna" : "Cokoladna";
			using (FileStream fileStream = new FileStream(putanja, FileMode.Open)){
				TextRange range = new TextRange(recept.Document.ContentStart, recept.Document.ContentEnd);
				range.Load(fileStream, DataFormats.Rtf);
			}
		}

		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e){
			this.DragMove();
		}

		private void Button_Click(object sender, RoutedEventArgs e){
			this.Close();
		}
	}
}
