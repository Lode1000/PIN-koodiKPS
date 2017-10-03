using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinkoodiKPS
{
    class Tietovarasto
    {
        private string connectionString;

        public Tietovarasto()
        {
            this.connectionString = "Data Source=(localdb)\\v11.0;" + "Initial Catalog=KPS;" + "Trusted_Connection = Yes;";
        }

        public void lisaaTulos(string nimi, int voitot, int haviot, int tasapelit) {

            using (SqlConnection conn = new SqlConnection())
            {
                // Create the connectionString
                // Trusted_Connection is used to denote the connection uses Windows Authentication
                conn.ConnectionString = this.connectionString;
                try
                {                   
                    conn.Open();
                    // Määrittää mihin laitetaan, numerot edustavat sarakkeita joiden täytyy olla samasa järjestyksessä kuin sarakkaiden nimet
                    SqlCommand insertCommand = new SqlCommand("INSERT INTO tulostaulu (nimi, voittoprosentti, voitot, haviot, tasapelit) VALUES (@0, @1, @2, @3, @4)", conn);

                    // Laitetaan parametreistä saadut muuttujat tietokantaan oikeille sarakkeille
                    insertCommand.Parameters.Add(new SqlParameter("0", nimi));
                    insertCommand.Parameters.Add(new SqlParameter("1", (double)voitot / ((double)haviot + (double)voitot + (double) tasapelit)));
                    insertCommand.Parameters.Add(new SqlParameter("2", voitot));
                    insertCommand.Parameters.Add(new SqlParameter("3", haviot));
                    insertCommand.Parameters.Add(new SqlParameter("4", tasapelit));

                    insertCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ei saatu yhteyttä tietokantaan" + e.Message);
                }
                    
            }

        }

        public void muutaPinPuk(string pin, string puk)
        {

            using (SqlConnection conn = new SqlConnection())
            {

                conn.ConnectionString = this.connectionString;
                try
                {
                    conn.Open();
                    // Määrittää mihin päivitetään mitäkin tietoa
                    SqlCommand insertCommand = new SqlCommand("update kirjautumistiedot set pin=@pin, puk=@puk where tiedot=@t", conn);

                    // Laitetaan parametreistä saadut muuttujat tietokantaan oikeille sarakkeille
                    insertCommand.Parameters.Add(new SqlParameter("@pin", pin));
                    insertCommand.Parameters.Add(new SqlParameter("@puk", puk));
                    insertCommand.Parameters.Add(new SqlParameter("@t", "tiedot"));
                    insertCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ei saatu yhteyttä tietokantaan" + e.Message);
                }

            }

        }

        public void lisaaKirjautumisRivi(string pin, string puk)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = this.connectionString;
                try
                {
                    conn.Open();
                    // Määrittää mihin laitetaan, numerot edustavat sarakkeita joiden täytyy olla samasa järjestyksessä kuin sarakkaiden nimet
                    SqlCommand insertCommand = new SqlCommand("INSERT INTO kirjautumistiedot (tiedot, pin, puk) VALUES (@0, @1, @2)", conn);

                    // Laitetaan parametreistä saadut muuttujat tietokantaan oikeille sarakkeille
                    insertCommand.Parameters.Add(new SqlParameter("0", "tiedot"));
                    insertCommand.Parameters.Add(new SqlParameter("1", pin));
                    insertCommand.Parameters.Add(new SqlParameter("2", puk));

                    insertCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ei saatu yhteyttä tietokantaan" + e.Message);
                }
            }

        }

        public void luoKirjautumisTaulu()
        {
            using (SqlConnection conn = new SqlConnection())
            {

                try
                {
                    conn.ConnectionString = this.connectionString;
                    conn.Open();

                    using (SqlCommand command = new SqlCommand("CREATE TABLE kirjautumistiedot(tiedot varchar(100) NOT NULL, pin varchar(100) NOT NULL,puk varchar(100) NOT NULL);", conn))
                        command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ei saatu yhteyttä tietokantaan" + e.Message);  
                }
            }
        }

        public void luoTulostaulu()
        {
            using (SqlConnection conn = new SqlConnection())
            {

                try
                {
                    conn.ConnectionString = this.connectionString;
                    conn.Open();

                    using (SqlCommand command = new SqlCommand("CREATE TABLE tulostaulu(nimi varchar(12) NOT NULL," 
                        + "voittoprosentti float NOT NULL, voitot int NOT NULL,"
                        + "haviot int NOT NULL, tasapelit int NOT NULL);", conn))
                        command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ei saatu yhteyttä tietokantaan" + e.Message);
                }
            }
        }

        // Hakee PIN ja PUK koodin taulukkoon jossa [0] on PIN ja [1] on PUK
        public string[] getPinPuk()
        {

            using (SqlConnection conn = new SqlConnection())
            {
                try
                {
                    conn.ConnectionString = this.connectionString;
                    conn.Open();

                    SqlCommand command = new SqlCommand("SELECT * FROM kirjautumistiedot WHERE tiedot = @0", conn);
                    command.Parameters.Add(new SqlParameter("0", "tiedot"));
                    string[] pinPuk = new string[2];
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pinPuk[0] = (string)reader[1];
                            pinPuk[1] = (string)reader[2];
                        }
                    }
                    return pinPuk;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ei saatu yhteyttä tietokantaan" + e.Message);
                    return null;
                }
            }
        }

        public List<Pelaaja> getTulostaulu()
        {

            using (SqlConnection conn = new SqlConnection())
            {
                try
                {
                    conn.ConnectionString = this.connectionString;
                    conn.Open();
                    // Pelaajat laitetaan tulostauluun, jonka järjestystä voi muuttaa
                    List<Pelaaja> tulosTaulu = new List<Pelaaja>();
                    SqlCommand command = new SqlCommand("SELECT * FROM tulostaulu", conn);
                
                // Jos haluaisi etsiä nimellä yksittäistä tulosta
                    // SqlCommand command = new SqlCommand("SELECT * FROM tulostaulu WHERE nimi = @0", conn);
                    // command.Parameters.Add(new SqlParameter("0", "Anna"));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            tulosTaulu.Add(new Pelaaja((string) reader[0], (double) reader[1], (int) reader[2], (int) reader[3], (int) reader[4]));
                            /*
                             *  Printtaisi tulokset, OBSOLETE laitettu oliomuotooon listaan
                             *  jotta voisi käsitellä parhaan tuloksen ensimmäiseksi
                            
                            Console.WriteLine(String.Format("{0} \t\t | {1} \t\t | {2} \t\t | {3} \t\t | {4} \t ",
                                reader[0], reader[1], reader[2], reader[3], reader[4]));
                             * 
                             */
                        }
                    }
                    return tulosTaulu;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ei saatu yhteyttä tietokantaan" + e.Message);
                    return null;
                }

            }
        }

    }
}
