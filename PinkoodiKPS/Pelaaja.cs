using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkoodiKPS
{
    class Pelaaja
    {
        private string nimi;
        private double voittoprosentti;
        private int voitot;
        private int haviot;
        private int tasapelit;

        public Pelaaja(String nimi, double voittoprosentti, int voitot, int haviot, int tasapelit){
            this.nimi = nimi;
            this.voittoprosentti = voittoprosentti;
            this.voitot = voitot;
            this.haviot = haviot;
            this.tasapelit = tasapelit;
        }

        public string Nimi
        {
            get { return nimi; }
            set { nimi = value; }
        }

        public double Voittoprosentti
        {
            get { return voittoprosentti; }
            set { voittoprosentti = value; }
        }

        public int Voitot
        {
            get { return voitot; }
            set { voitot = value; }
        }

        public int Haviot
        {
            get { return haviot; }
            set { haviot = value; }
        }

        public int Tasapelit
        {
            get { return tasapelit; }
            set { tasapelit = value; }
        }
    }
}
