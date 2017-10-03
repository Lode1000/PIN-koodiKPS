using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkoodiKPS
{
    class Program
    {
        static void Main(string[] args)
        {
            Kirjautuminen kirjautuminen = new Kirjautuminen("00000000");

            bool onPaalla = true;
            while (onPaalla)
            {
                Console.WriteLine("'1' Pelataksesi KPS pelin\n'2' vaihtaaksesi PIN-koodin\n'3' lopettaaksesi");
                string komento = Console.ReadLine();
                if (komento.Equals("1"))
                {
                    kirjautuminen.kirjaudu(komento);
                }
                if (komento.Equals("2"))
                {
                    kirjautuminen.kirjaudu(komento);
                }
                if (komento.Equals("3"))
                {
                    onPaalla = false;
                }

            }

        }
    }
}
