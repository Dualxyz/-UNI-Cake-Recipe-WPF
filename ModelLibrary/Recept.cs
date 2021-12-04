using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiodelLibrary
{
	public class Recept
	{
		private int zakolikoOsoba;
		private string imeTorte;
		private DateTime rokTrajanja;
		string prikazSlike;
		private string putanja;
		private string vremePravljenja;
		private bool vocnaCokoladna;
		private static int id = 0;

		public Recept(int zakolikoOsoba, string imeTorte, DateTime rokTrajanja, string prikazSlike, string putanja, string vremePravljenja, bool vocnaCokoladna)
		{
			id++;
			this.ZakolikoOsoba = zakolikoOsoba;
			this.ImeTorte = imeTorte;
			this.RokTrajanja = rokTrajanja;
			this.PrikazSlike = prikazSlike;
			this.Putanja = putanja;
			this.VremePravljenja = vremePravljenja;
			this.VocnaCokoladna = vocnaCokoladna;		
		}

		public int ZakolikoOsoba { get => zakolikoOsoba; set => zakolikoOsoba = value; }
		public string ImeTorte { get => imeTorte; set => imeTorte = value; }
		public DateTime RokTrajanja { get => rokTrajanja; set => rokTrajanja = value; }
		public string PrikazSlike { get => prikazSlike; set => prikazSlike = value; }
		public string Putanja { get => putanja; set => putanja = value; }
		public string VremePravljenja { get => vremePravljenja; set => vremePravljenja = value; }
		public bool VocnaCokoladna { get => vocnaCokoladna; set => vocnaCokoladna = value; }
	}
}
