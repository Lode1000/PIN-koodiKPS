using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkoodiKPS
{
    class TekoAly
    {
        ArrayList vastustajanHistoria;
        Random random;

        public TekoAly()
        {
            this.vastustajanHistoria = new ArrayList();
            this.random = new Random();
        }

        private String arvonta()
        {
            int arpa = random.Next(0, 2);
            if (arpa == 0)
            {
                return "sakset";
            }
            else if (arpa == 1)
            {
                return "paperi";
            }
            return "kivi";
        }

        // alkeellinen tekoäly, joka ottaa huomioon pelaajan historian
        public string pelaa()
        {
            int siirtojenmaara = this.vastustajanHistoria.Count;

            // Jos siirtoja yli 30 niin
            // Laskee pelaajan viimeisen 10 siirron KPS valintojen määrät
            // Ja valitsee sen mukaan mitä pelaaja on valinnut useasti aiemmin
            if (siirtojenmaara > 30)
            {
                int kivi = 0;
                int paperi = 0;
                int sakset = 0;
                for (int i = siirtojenmaara - 1; i > siirtojenmaara - 10; i--)
                {
                    if (this.vastustajanHistoria[i].Equals("kivi"))
                    {
                        kivi++;
                    }
                    if (this.vastustajanHistoria[i].Equals("paperi"))
                    {
                        paperi++;
                    }
                    if (this.vastustajanHistoria[i].Equals("sakset"))
                    {
                        sakset++;
                    }
                }
                // Ylläolevan laskemisen perusteella päättelee, 
                // että jos valitsee paljon jotain, niin kannattaa valita sitä vastaan
                if (kivi > paperi && kivi > sakset)
                {
                    return "paperi";
                }
                else if (paperi > kivi && paperi > sakset)
                {
                    return "sakset";
                }
                else if (sakset > kivi && sakset > paperi)
                {
                    return "kivi";
                }
                else
                {
                    return arvonta();
                }

            }
            // Valitsee siirron pelaajan siirroista, 
            // Esim jos pelaaja valitsee paljon saksia, niin tekoäly alkaa valita kiveä samassa suhteessa
            else if (siirtojenmaara > 3)
            {
                int arpa = random.Next(0, siirtojenmaara - 1);
                if (vastustajanHistoria[arpa].Equals("sakset"))
                {
                    return ("kivi");
                }
                if (vastustajanHistoria[arpa].Equals("kivi"))
                {
                    return ("paperi");
                }
                if (vastustajanHistoria[arpa].Equals("paperi"))
                {
                    return ("sakset");
                }
            }
            // Ensimmäiset kolme siirtoa täysin arvottu
            return arvonta();

        }

        // Muistaa pelaajan siirrot, jotta voi sopeutua pelaajan pelitapaan
        public void muistaPelaajanSiirto(String siirto)
        {
            this.vastustajanHistoria.Add(siirto);
        }

    }
}
