using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkoodiKPS
{
    class KiviPaperiSakset
    {
        TekoAly tekoAly;
        private int voitot;
        private int haviot;
        private int tasapelit;

        private bool onKaynnissa;
        private int siirrot;

        public KiviPaperiSakset()
        {
            this.voitot = 0;
            this.haviot = 0;
            this.tasapelit = 0;
            this.siirrot = 0;
            this.onKaynnissa = true;

            this.tekoAly = new TekoAly();
        }

        private void paivitaTulos(string pelaajanSiirto, string tekoAlynSiirto)
        {
            this.siirrot++;
            if (tekoAlynSiirto.Equals(pelaajanSiirto))
            {
                this.tasapelit++;
            }
            else if (pelaajanSiirto.Equals("kivi") && tekoAlynSiirto.Equals("sakset"))
            {
                this.voitot++;
            }
            else if (pelaajanSiirto.Equals("kivi") && tekoAlynSiirto.Equals("paperi"))
            {
                this.haviot++;
            }
            else if (pelaajanSiirto.Equals("paperi") && tekoAlynSiirto.Equals("kivi"))
            {
                this.voitot++;
            }
            else if (pelaajanSiirto.Equals("paperi") && tekoAlynSiirto.Equals("sakset"))
            {
                this.haviot++;
            }
            else if (pelaajanSiirto.Equals("sakset") && tekoAlynSiirto.Equals("paperi"))
            {
                this.voitot++;
            }
            else if (pelaajanSiirto.Equals("sakset") && tekoAlynSiirto.Equals("kivi"))
            {
                this.haviot++;
            }
        }
        public void Pelaa()
        {
            Console.WriteLine("Tervetuloa KPS peliin. Tarkoituksena on saada mahdollisimman suuri voittoprosentti.");
            Console.WriteLine("Voit käyttää niin paljon vuoroja kuin haluat, mutta vähintään 10 vuoroa");
            Console.WriteLine("Palkinnoksi paras voittoprosentti pääsee tulostaulun johtoon!!");
            Console.WriteLine("Pelaa kirjoittamalla 'kivi, 'paperi' tai 'sakset' tai lopeta peli kirjoittamalla 'lopeta'");

            while (onKaynnissa)
            {
                string siirto = Console.ReadLine().ToLower();

                string tekoAlynSiirto = tekoAly.pelaa();
                tekoAly.muistaPelaajanSiirto(siirto);
                this.paivitaTulos(siirto, tekoAlynSiirto);
                statistiikka();

                if (siirto.Equals("lopeta"))
                {
                    if (siirrot <= 10)
                    {
                        Console.WriteLine("Pelaa vähintään 10 vuoroa");
                        continue;
                    }
                    Tietovarasto tv = new Tietovarasto();

                    Console.Write("Anna nimimerkki: ");
                    string nimimerkki = Console.ReadLine();
                    // Jos ei ole tulostaulua niin tekee sellaisen
                    if (tv.getTulostaulu() == null)
                    {
                        tv.luoTulostaulu();
                    }
                    tv.lisaaTulos(nimimerkki, voitot, haviot, tasapelit);

                    tulostaulu(tv);
                    this.onKaynnissa = false;
                }
                
            }
        }

        // Printtaa tulostaulun
        private void tulostaulu(Tietovarasto tv)
        {
            List<Pelaaja> tulostaulu = tv.getTulostaulu();
            
            if (tulostaulu != null)
            {
                // Vaihtaa järjestyksen, että korkein voitto % näytetään ensimmäisenä
                tulostaulu.Sort((x, y) => y.Voittoprosentti.CompareTo(x.Voittoprosentti));

                // Stringformatin ensimmäinen numero on string, joka tulee vuorojärjestyksessä
                // Toinen numero on paikkojen määrä joka varataan merkeille
                // :f2 tarkoittaa 2 desimaalin tarkkuutta floateille
                Console.WriteLine(String.Format("{0,12}   {1,12:f2}   {2,12}   {3,12}   {4,12}",
                        "Nimimerkki", "Voitto %", "Voitot", "Häviöt", "Tasapelit"));
                // Käy läpi jokaisen pelaajan, printaten statistiikat
                foreach (Pelaaja p in tulostaulu)
                {                                   //Varataan 12 paikkaa ja W%:lle 2 desimaalia, ettei tule loputonta deciä
                    Console.WriteLine(String.Format("{0,12} | {1,12:p2} | {2,12} | {3,12} | {4,12} ",
                        p.Nimi, p.Voittoprosentti, p.Voitot, p.Haviot, p.Tasapelit));
                }
            }
        }

        private void statistiikka()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("Voittoja: " + voitot + "\nHäviöitä: " + haviot + "\nTasapelit: " + tasapelit);
        }
    }
}
