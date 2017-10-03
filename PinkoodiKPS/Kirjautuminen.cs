using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PinkoodiKPS
{
    class Kirjautuminen
    {

        private string pukKoodi;
        private string pinKoodi;
       
        // Epäonnistuneet kirjautumisyritykset eri koodeille
        private int pinYritysKerta;
        private int pukYritysKerta;

        // Jos koodit on käytetty, niin laite lukitaan
        private bool onLukittu;
        private Tietovarasto tietovarasto;

        public Kirjautuminen(string pukKoodi)
        {
            this.tietovarasto = new Tietovarasto();
            // Ensimmäisessä käynnistyksessä määritetään PIN-koodi käyttäjälle tallentaen sen 
            // Salattuna tietokantaan, ja joka myöhemmin haetaan sieltä
            if (tietovarasto.getPinPuk() == null)
            {
                this.pukKoodi = SecurePasswordHasher.Hash(pukKoodi);
                tietovarasto.luoKirjautumisTaulu();
                vaihdaPin();
                tietovarasto.lisaaKirjautumisRivi(this.pinKoodi, this.pukKoodi);
            }
            this.pinKoodi = tietovarasto.getPinPuk()[0];
            this.pukKoodi = tietovarasto.getPinPuk()[1];
            
            
            this.pinYritysKerta = 3;
            this.pukYritysKerta = 3;
            this.onLukittu = false;
        }

        
        private void kirjauduPukKoodilla()
        {
            Console.Write("Anna PUK-koodi: ");
            String kirjautumisPuk = turvallinenKirjautuminen();

            if (tarkistaKirjautumiskoodinOikeellisuus(kirjautumisPuk, 8))
            {
                Console.WriteLine("Puk väärän muotoinen. Kirjautuminen keskeytyy!");
                return;
            }
            else if (!SecurePasswordHasher.Verify(kirjautumisPuk, this.pukKoodi))
            {
                this.pukYritysKerta--;
                Console.WriteLine("Väärä PUK-koodi. kirjautusmisyrityksiä jäljellä: " + this.pukYritysKerta);
                return;
            }

            this.pukYritysKerta = 3;
            this.pinYritysKerta = 3;
        }

        public void kirjaudu(string komento)
        {
            // Jos kaikki koodit käytetty niin lukitus
            if (pukYritysKerta <= 0)
            {
                this.onLukittu = true;
            }

            if (onLukittu)
            {
                Console.WriteLine("--------------------");
                Console.WriteLine("LAITE LUKITU! OTA YHTEYS TIETOHALLINTOON!");
                return;
            }

            if (pinYritysKerta > 0)
            {
                Console.Write("Kirjaudu PIN-koodilla: ");
                String kirjautumisKoodi = turvallinenKirjautuminen();
                if (tarkistaKirjautumiskoodinOikeellisuus(kirjautumisKoodi, 4))
                {
                    Console.WriteLine("Pin väärän muotoinen. Kirjautuminen keskeytyy!");
                }
                else if (!SecurePasswordHasher.Verify(kirjautumisKoodi, this.pinKoodi))
                {
                    this.pinYritysKerta--;
                    Console.WriteLine("Väärä PIN-koodi. kirjautusmisyrityksiä jäljellä: " + this.pinYritysKerta);
                }
                else if (SecurePasswordHasher.Verify(kirjautumisKoodi, this.pinKoodi) && komento.Equals("1"))
                {
                    KiviPaperiSakset kps = new KiviPaperiSakset();
                    kps.Pelaa();
                }
                else if (SecurePasswordHasher.Verify(kirjautumisKoodi, this.pinKoodi) && komento.Equals("2"))
                {
                    vaihdaPin();
                }
            }
            else if (pukYritysKerta > 0)
            {
                kirjauduPukKoodilla();
            }
        }

        private void vaihdaPin()
        {
            string uusiPin = "";

            while (true)
            {
                Console.Write("Anna uusi PIN-koodi: ");
                uusiPin = turvallinenKirjautuminen();

                if (tarkistaKirjautumiskoodinOikeellisuus(uusiPin, 4))
                {
                    continue;
                }
                else
                {
                    this.pinKoodi = SecurePasswordHasher.Hash(uusiPin);
                    if (tietovarasto.getPinPuk() != null)
                    {
                        tietovarasto.muutaPinPuk(this.pinKoodi, this.pukKoodi);
                    }
                    
                    this.pinYritysKerta = 3;
                    break;
                }

            }

        }
        //-------Apumetodilaakso-------

        // Piilottaa mitä kirjoittaa ****** merkkien sisälle ja palauttaa takaisin selkomuotoisen salasanan
        private string turvallinenKirjautuminen() {
            ConsoleKeyInfo key;
            String uusiPin = "";
            do
            {
                key = Console.ReadKey(true);

                // Jos näppäin ei ole Backspace tai Enter, niin lisää charin  pinKoodiin ja kirjoittaa *
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    uusiPin += key.KeyChar;
                    Console.Write("*");
                }
                // Jos painaa Backspacea ja pin on yli 0 pitkä, niin poistaa merkin ja backspacea Writessä
                else
                {
                    if (key.Key == ConsoleKey.Backspace && uusiPin.Length > 0)
                    {
                        uusiPin = uusiPin.Substring(0, (uusiPin.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Kun painetaan enter niin poistuu
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return uusiPin;
        }

        // Palauttaa stringin mitä tyyppiä halutaan pin vai puk
        // Apumetodi tarkistamiseen, onko koodi oikeantyyppinen
        private string pinVaiPuk(int pituus)
        {
            if (pituus == 4)
            {
                return "pin";
            }
            else if (pituus == 8)
            {
                return "puk";
            }
            return "";
        }

        // Käy läpi jokaisen merkin ja palauttuu true, mikäli löytyy jotain muuta kuin numero
        private bool tarkistaKirjautumiskoodinOikeellisuus(string kirjautumisKoodi, int haluttuPituus)
        {

            // Testaa onko pin tasan 4 pitkä
            if (kirjautumisKoodi.Length != haluttuPituus)
            {
                Console.WriteLine("Anna tasan " + haluttuPituus + " numeroa pitkä " + pinVaiPuk(haluttuPituus) + "-koodi!");
                return true;
            }

            // Laittaa merkit taulukkoon ja testaa onko jotain muuta merkkejä, kuin numeroita
            char[] cPin = kirjautumisKoodi.ToCharArray();

            foreach (char c in cPin)
            {
                if (!char.IsDigit(c))
                {
                    Console.WriteLine("anna ainoastaan numeroita!");
                    return true;
                }

            }

            return false;
        }
    }
}

