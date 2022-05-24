using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace TerytSearcher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ładowanie Danych...");
            //Etap ładowania danych
            Dictionary<string, List<string>> gminy = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> miejscowosci = new Dictionary<string, List<string>>();

            //Wczytanie gmin
            var reader = XmlReader.Create("database\\TERC_Urzedowy_2022-05-23.xml");
            string tempCode;
            
            do
            {
                reader.MoveToFirstAttribute();
                reader.ReadToFollowing("WOJ");
                tempCode = reader.ReadElementContentAsString();
                reader.ReadToFollowing("POW");
                tempCode += reader.ReadElementContentAsString();
                reader.ReadToFollowing("GMI");
                tempCode += reader.ReadElementContentAsString();
                reader.ReadToFollowing("RODZ");
                tempCode += reader.ReadElementContentAsString();
                if(tempCode.Length == 7)
                {
                    gminy.Add(tempCode, new List<string>());
                }
            } while (reader.ReadToFollowing("row"));

            Console.WriteLine("Wczytano gminy");

            //Wczytanie miejscowości
            reader = XmlReader.Create("database\\SIMC_Urzedowy_2022-05-23.xml");
            do
            {
                reader.MoveToFirstAttribute();
                reader.ReadToFollowing("WOJ");
                tempCode = reader.ReadElementContentAsString();
                reader.ReadToFollowing("POW");
                tempCode += reader.ReadElementContentAsString();
                reader.ReadToFollowing("GMI");
                tempCode += reader.ReadElementContentAsString();
                reader.ReadToFollowing("RODZ_GMI");
                tempCode += reader.ReadElementContentAsString();
                reader.ReadToFollowing("NAZWA");
                gminy[tempCode].Add(reader.ReadElementContentAsString());
                reader.ReadToFollowing("SYM");
                miejscowosci.Add(reader.ReadElementContentAsString(), new List<string>());
            } while (reader.ReadToFollowing("row"));

            Console.WriteLine("Wczytano miejscowości");

            //Wczytywanie ulic
            string tempStreet, tempName1;
            reader = XmlReader.Create("database\\ULIC_Urzedowy_2022-05-23.xml");
            do
            {
                reader.ReadToFollowing("SYM");
                tempCode = reader.ReadElementContentAsString();
                reader.ReadToFollowing("CECHA");
                tempStreet = reader.ReadElementContentAsString();
                reader.ReadToFollowing("NAZWA_1");
                tempName1 = reader.ReadElementContentAsString();
                reader.ReadToFollowing("NAZWA_2");
                tempStreet += " " + reader.ReadElementContentAsString();
                miejscowosci[tempCode].Add($"{tempStreet} {tempName1}");
            } while (reader.ReadToFollowing("row"));

            Console.WriteLine("Wczytano ulice\n");
            Console.WriteLine("Aby wyświetlić pomoc, wpisz komendę help.");
            bool work = true;
            while (work)
            {
                Console.Write(">");
                string teryt = Console.ReadLine();
                switch(teryt)
                {
                    case "help":
                        Console.WriteLine();
                        Console.WriteLine("Program wyświetla informacje na podstawie kodu TERYT. 2 typy kodu TERYT mogą zostać przyjęte przez program:");
                        Console.WriteLine(" - Kod Gminy, który po wpisaniu wyświetla wszystkie miejscowości w tej gminie,");
                        Console.WriteLine(" - Kod Miejscowości, który po wpisaniu wyświetla wszystkie ulice danej miejscowości.");
                        Console.WriteLine("Zamiast kodu TERYT można również wpisać jedną z komend: ");
                        Console.WriteLine("clear - czyści zawartość konsoli");
                        Console.WriteLine("help - wyświetla pomoc");
                        Console.WriteLine("quit - zamyka program");
                        Console.WriteLine();
                        break;
                    case "quit":
                        work = false;
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    default:
                        if (gminy.ContainsKey(teryt))
                        {
                            if (gminy[teryt].Count > 0)
                            {
                                foreach (string s in gminy[teryt])
                                {
                                    Console.WriteLine($"\t{s}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Wpisany kod TERYT gminy istnieje, lecz nie gmina do niego przypisana nie posiada miejscowości.");
                            }
                        }
                        else if (miejscowosci.ContainsKey(teryt))
                        {
                            if (miejscowosci[teryt].Count > 0)
                            {
                                foreach (string s in miejscowosci[teryt])
                                {
                                    Console.WriteLine($"\t{s}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Wpisany kod TERYT miejscowości istnieje, lecz nie miejscowość do niego przypisana nie posiada ulic.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wpisany kod nie istnieje w bazie danych");
                        }
                        break;
                }
            }
            
        }
    }
}
