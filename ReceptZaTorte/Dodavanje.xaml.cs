using Microsoft.Win32;
using MiodelLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
	/// Interaction logic for Dodavanje.xaml
	/// </summary>
	public partial class Dodavanje : Window{
        static BitmapImage s = new BitmapImage();
		static OpenFileDialog ofd = new OpenFileDialog();
		int idx;
		bool dodavanje = true;
		Recept rec;
		public Dodavanje(Recept recept, int idx){

			this.idx = idx; 
			if(recept != null){
				dodavanje = false;
			}
			InitializeComponent();
            fontfamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(t => t.Source);
            fontcolor.SelectedItem = Colors.Black;
            fontcolor.ItemsSource = typeof(Colors).GetProperties();
            for (int i = 6; i <= 22; i++) {
                fontsize.Items.Add(i);
            }
            if (recept == null){
				imetorte.Text = "Ime recepta...";
				imetorte.Foreground = Brushes.LightGray;

				osoba.Text = "Za koliko osoba...";
				osoba.Foreground = Brushes.LightGray;

				vreme.Text = "Vreme potrebno za tortu...";
				vreme.Foreground = Brushes.LightGray;
			} else {
				dugme.Content = "Izmeni";
				rec = recept;
				imetorte.Text = recept.ImeTorte;
				imetorte.Foreground = Brushes.Black;

				osoba.Text = recept.ZakolikoOsoba.ToString();
				osoba.Foreground = Brushes.Black;

				rok.SelectedDate = recept.RokTrajanja;

				vreme.Text = recept.VremePravljenja;
				vreme.Foreground = Brushes.Black;

				v.IsChecked = recept.VocnaCokoladna == true ? true : false;
				c.IsChecked = recept.VocnaCokoladna == false ? true : false;

				slika.Source = new BitmapImage(new Uri(recept.PrikazSlike));

				using (FileStream fileStream = new FileStream(recept.Putanja, FileMode.Open)){
					TextRange range = new TextRange(recepttekst.Document.ContentStart, recepttekst.Document.ContentEnd);
					range.Load(fileStream, DataFormats.Rtf);
				}
			}
		}
		private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e){
			this.DragMove();
		}
		private void Button_Click(object sender, RoutedEventArgs e){
			if (Validate()){
				if (dodavanje){
					bool d;
					if (v.IsChecked == true){
						d = true;
					} else {
						d= false;
                    }
					SaveFileDialog dlg = new SaveFileDialog();
					dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
					dlg.Title = "Snimanje tekstualne datoteke";

					if (dlg.ShowDialog() == true){
						using (FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create)){
							TextRange range = new TextRange(recepttekst.Document.ContentStart, recepttekst.Document.ContentEnd);
							range.Save(fileStream, DataFormats.Rtf);
							MainWindow.Baza.Add(new Recept(int.Parse(osoba.Text), imetorte.Text, rok.SelectedDate.Value, slika.Source.ToString(), dlg.FileName, vreme.Text, d));
							int b = MainWindow.Baza.Count;
							this.Close();
						}
					}
				} else {
					bool d;
					if (v.IsChecked == true) {
						d = true;
					} else {
						d = false;
                    }
					FileStream fs = new FileStream(rec.Putanja, FileMode.OpenOrCreate);
					TextRange tr = new TextRange(recepttekst.Document.ContentStart, recepttekst.Document.ContentEnd);
					tr.Save(fs, DataFormats.Rtf);
					fs.Close();
					MainWindow.Baza[idx] = new Recept(int.Parse(osoba.Text), imetorte.Text, rok.SelectedDate.Value, slika.Source.ToString(), rec.Putanja, vreme.Text, d);
					this.Close();
				}
			} else {
				MessageBox.Show("Polja nisu dobro popunjena!", "Greska!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		private void Button_Click_1(object sender, RoutedEventArgs e){
			this.Close();
		}

		private void ShowFileDialog(object sender, RoutedEventArgs e){
			ofd.FileName = "";
			ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			ofd.Title = "Chose image";
			ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

			if (ofd.ShowDialog() == true){
				slika.Source = new BitmapImage(new Uri(ofd.FileName));
				s = new BitmapImage(new Uri(ofd.FileName));
			}
		}

		private bool Validate(){
			bool val = true;

			if (imetorte.Text.Trim() == "" || imetorte.Text == ""){
				imevalidacija.Content = "Potrebno uneti\n ime recepta";
				imevalidacija.Foreground = Brushes.Red;
				val = false;
			} else {
				imevalidacija.Content = "";
			}
			int br1;
			if(int.TryParse(osoba.Text, out br1)){
				brosobavalidacija.Content = "";
			} else {
				brosobavalidacija.Content = "Potrebno uneti za koliko \n osoba je recept";
				brosobavalidacija.Foreground = Brushes.Red;
				val = false;
			}

			if (!rok.SelectedDate.HasValue){
				rokvalidacija.Content = "Odaberite rok\n trajanja torte";
				rokvalidacija.Foreground = Brushes.Red;
				val = false;
			} else {
				if (rok.SelectedDate <= DateTime.Now){
					rokvalidacija.Content = "Odaberite rok trajanja\n torte mladji od danas";
					rokvalidacija.Foreground = Brushes.Red;
					val = false;
				} else {
					rokvalidacija.Content = "";
				}
			}

			string[] niz = vreme.Text.Split(':');
			int br2, br3;
			if(!int.TryParse(niz[0],out br2) || !int.TryParse(niz[1],out br3)){
				trajanjevalidacija.Content = "Nepravilan format\n vremena";
				trajanjevalidacija.Foreground = Brushes.Red;
				val = false;
			} else {
				if(br2>=24 || br3 > 59) {
					trajanjevalidacija.Content = "Nepravilan unos \nsati i minuta";
					trajanjevalidacija.Foreground = Brushes.Red;
					val = false;
				} else {
					trajanjevalidacija.Content = "";
				}
			}

			TextRange spec = new TextRange(recepttekst.Document.ContentStart, recepttekst.Document.ContentEnd);
			if (string.IsNullOrWhiteSpace(spec.Text)){
				val = false;
				receptvalidacija.Content = "Morate napisati recept";
				receptvalidacija.Foreground = Brushes.Red;
			} else {
				receptvalidacija.Content = "";
			}

			if (null == slika.Source){
				val = false;
				slikavalidacija.Content = "Morate ubaciti sliku";
				slikavalidacija.Foreground = Brushes.Red;
			} else {
				slikavalidacija.Content = "";
			}

			return val;
		}

		private void imetorte_GotFocus(object sender, RoutedEventArgs e){
			if(imetorte.Text == "Ime recepta..."){
				imetorte.Text = "";
				imetorte.Foreground = Brushes.Black;
			}
		}

		private void imetorte_LostFocus(object sender, RoutedEventArgs e){
			if (imetorte.Text.Trim() == ""){
				imetorte.Text = "Ime recepta...";
				imetorte.Foreground = Brushes.LightGray;
			}
		}

		private void osoba_GotFocus(object sender, RoutedEventArgs e){
			if(osoba.Text== "Za koliko osoba..."){
				osoba.Text = "";
				osoba.Foreground = Brushes.Black;
			}
		}

		private void osoba_LostFocus(object sender, RoutedEventArgs e){
			if(osoba.Text.Trim() == ""){
				osoba.Text = "Za koliko osoba...";
				osoba.Foreground = Brushes.LightGray;
			}
		}

		private void vreme_GotFocus(object sender, RoutedEventArgs e){
			if(vreme.Text== "Vreme potrebno za tortu..."){
				vreme.Text = "";
				vreme.Foreground = Brushes.Black;
			}
		}

		private void vreme_LostFocus(object sender, RoutedEventArgs e){
			if(vreme.Text.Trim() == ""){
				vreme.Text = "Vreme potrebno za tortu...";
				vreme.Foreground = Brushes.LightGray;
			}
		}

		private void fontfamily_SelectionChanged(object sender, SelectionChangedEventArgs e){
			if (fontfamily.SelectedItem != null){
				recepttekst.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontfamily.SelectedItem);
				fontfamily.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
			}
		}

		private void fontsize_SelectionChanged(object sender, SelectionChangedEventArgs e){
			if (fontsize.SelectedItem != null){
				recepttekst.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontsize.SelectedItem.ToString());
				fontsize.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
			}
		}

		private void fontcolor_SelectionChanged(object sender, SelectionChangedEventArgs e){
			string c = fontcolor.SelectedItem.ToString().Substring(fontcolor.SelectedItem.ToString().LastIndexOf(" ") + 1);
			BrushConverter conv = new BrushConverter();
			SolidColorBrush brush = conv.ConvertFromString(c) as SolidColorBrush;
			recepttekst.Selection.ApplyPropertyValue(Inline.ForegroundProperty, brush);
			fontcolor.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
		}

		private void recepttekst_SelectionChanged(object sender, RoutedEventArgs e){
			object temp = recepttekst.Selection.GetPropertyValue(Inline.FontWeightProperty);
			boldBtn.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

			temp = recepttekst.Selection.GetPropertyValue(Inline.FontStyleProperty);
			italicBtn.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

			temp = recepttekst.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
			underlineBtn.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

			if (fontfamily.SelectedItem == null) {
				fontfamily.SelectedItem = new FontFamily("Arial");
			}

            if (fontfamily.SelectedItem != null && !recepttekst.Selection.IsEmpty) {
                recepttekst.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontfamily.SelectedItem);
            }	//Vezbe4 bug...
            //recepttekst.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontfamily.SelectedItem);

			if (fontsize.SelectedItem == null){
				fontsize.SelectedItem = 11;
			}

			if (fontsize.SelectedItem != null && !recepttekst.Selection.IsEmpty) {
				recepttekst.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, double.Parse(fontsize.SelectedItem.ToString()));
			}   //Vezbe4 bug...
            //recepttekst.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, double.Parse(fontsize.SelectedItem.ToString()));


            BrushConverter c = new BrushConverter();
            if (fontcolor.SelectedItem == null) {
                fontcolor.SelectedItem = Colors.Black;
            }
            if (fontcolor.SelectedItem != null && !recepttekst.Selection.IsEmpty) {
                SolidColorBrush brush = c.ConvertFromString(fontcolor.SelectedItem.ToString().Substring(fontcolor.SelectedItem.ToString().LastIndexOf(" ") + 1)) as SolidColorBrush;
                recepttekst.Selection.ApplyPropertyValue(Inline.ForegroundProperty, brush);
            }

        }
		private void recepttekst_TextChanged(object sender, TextChangedEventArgs e){
			string spec = (new TextRange(recepttekst.Document.ContentStart, recepttekst.Document.ContentEnd)).Text;
			MatchCollection wordColl = Regex.Matches(spec, @"[\W]+");
			brReci.Text = wordColl.Count.ToString();
			TextRange range = new TextRange(recepttekst.Document.ContentStart, recepttekst.Document.ContentEnd);
			if (range.Text.Trim() == ""){
				brReci.Text = "0";
			}
		}		
	}
}
