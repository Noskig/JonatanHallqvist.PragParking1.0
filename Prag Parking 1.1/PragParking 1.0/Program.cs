using System;
//Programmet heter fortfarande 1.0 på vissa ställen. Men detta är version 1.1

namespace PragParking_1._1
{
    class Program
    {
        static void Main()
        //System använder *M* för motorcykel. Motsvarande *B* för bil tillsammans med regnummer för identifikation. 
        //Platserna subtraheras in i, och adderas ut ur systemet. För att visa användaren motsvarande parkeringsplats(1-100) i huset
        //Användaren kan inte parkera ett regnummer som är längre än 10tecken. 

        //Kvitto skrivs ut vid parkering. Alt vid en fel-hantering av personal för att visa vilken plats fordonet faktiskt ska stå på systemet.
        {
            string[] parkhus = new string[100];
            parkhus[8] = "*B*ASD111";
            parkhus[14] = "*M*AAA111*M*AAA222";
            for (int i = 0; i < 99; i+=5)
            {
                parkhus[i] += "*M*aaa0"+ i;
            }

            while (1 > 0) ///hint till att det ska köras när datorn är på.
            {
                int val;

                Console.WriteLine("\n\tVÄLKOMMEN TILL PRAG PARKING!\n" +
                    "\nVad vill du göra?" +
                    "\n\n1. Parkera. \t\n2. Hämta ut fordon. \t\n3. Flytta till ny plats. \t\n4. Sök efter fordon.\t\n\n8. Visa Platser. \t\n9. Flytta ihop singel MCs");

                val = int.Parse(Console.ReadLine());

                switch (val)
                {
                    case 1: // Parkera
                        {
                            Parkera(parkhus);
                            break;
                        }
                    case 2: // Hämta ut
                        {
                            PlockaUr(parkhus);
                            break;
                        }
                    case 3: // flytta 
                        {
                            Flytta(parkhus);
                            break;
                        }
                    case 4: //sök
                        {
                            Sök(parkhus);
                            break;
                        }
                    case 8: //Visa platser
                        {
                            Visa(parkhus);
                            break;
                        }
                    case 9: // Flytta ihop singel MC's
                        {
                            OptimeraPlatserMC(parkhus);
                            break;
                        }
                }
            }
        }
        ///Methoder
        static void Park(string fordonsinfo, string[] parkhus) // Fyller en parkplats med en bil
        {
            string[] checkempty = new string[2];

            for (int i = 0; i < parkhus.Length; i++)
            {
                if (parkhus[i] == checkempty[0])
                {
                    parkhus[i] = fordonsinfo;
                    fordonsinfo = fordonsinfo.Remove(0, 3); //tar lokalt bort systemindikationen för fordonstyp för att skriva ut till consolen
                    Console.WriteLine("\nPlats {0} är ledig. Kör {1} dit", (i + 1), fordonsinfo);
                    break;
                }
                else if (i == 99) // det sista som kan hända i loopen
                {
                    Console.WriteLine("Tyvärr är parkeringarna fulla.");
                }
            }
        }
        static void Parkmc(string fordonsinfo, string[] parkhus)//Fylla en parkplats med en MC, så att det också kan stå två
        {
            string[] checkempty = new string[2];
            for (int i = 0; i < parkhus.Length; i++)
            {
                if (parkhus[i] == checkempty[0])
                {
                    parkhus[i] = fordonsinfo;
                    fordonsinfo = fordonsinfo.Remove(0, 3); //tar lokalt bort systemindikationen för fordonstyp för att skriva ut till consolen
                    Console.WriteLine("\nPlats {0} är ledig. Kör {1} dit", (i + 1), fordonsinfo);
                    break;
                }
                else if (parkhus[i].Substring(0, 3) == "*M*" && parkhus[i].LastIndexOf("*") < 4) //det står bara en motorcykel där
                {
                    parkhus[i] = parkhus[i] + fordonsinfo;
                    fordonsinfo = fordonsinfo.Remove(0, 3); //tar lokalt bort systemindikationen för fordonstyp för att skriva ut till consolen
                    Console.WriteLine("\nPlats {0} är ledig. Kör {1} dit", (i + 1), fordonsinfo);
                    break;
                }
                else if (i == 99) // det sista som kan hända i loopen. Det betyder alltså att..
                {
                    Console.WriteLine("Tyvärr är parkeringarna fulla.");
                }
            }
        }
        static int Find(string regnummer, string[] parkhus)//Söker efter regnummer... /// Det första sättet jag använde för att hitta regnummret. Snabbt. Men detta sättet hittade inte en andra mc.
        {
            string regnummerbil = "*B*" + regnummer;
            string regnummermc = "*M*" + regnummer;
            int platsinfo = Array.IndexOf(parkhus, regnummerbil); //Hittar vilken plats //söker först efter en bil

            if (platsinfo == -1)      //om ej bil                                                  
            {
                platsinfo = Array.IndexOf(parkhus, regnummermc); //kollar första mc:n
                if (platsinfo == -1)
                {
                    platsinfo = Contains(regnummer, parkhus); // hittas varken bilen eller mc:n Så kollas alla platser igenom om det står en andra mc. /// Detta var ett av mina större problemen i uppgiften att hitta en andra mc. men tyckte detta blev rätt bra tillslut. Känns som en hyfsat stabil söklogik för ett snabbt systemet. Hoppas du tycker det också.
                    if (platsinfo == -1)
                    {
                        Console.WriteLine("Hittar tyvärr inte regnummret...");
                        return 0;
                    }
                }
            }
            return platsinfo; // Ge tillbaka vilken plats den befinner sig på
        }
        static void Remove(int plats, string regnummer, string[] parkhus) // Tar bort regnummret från parkplatsen.
        {
            string mirror = "";                                                                                 /// gjorde en speglning av regnummret för min egen skull. Då för att jag kan läsa koden lättare
            if (parkhus[plats].Length < 13) // om den innehåller bara en bil eller mc. 
            {
                parkhus[plats] = null;
                Console.WriteLine("\nPakeringsplats: {0}. Blir nu tom.", plats + 1);
            }
            else if (parkhus[plats].Length >= 14) // om det står två motorcyklar parkerade                      ///Blir knas för regnummer som är kortare än 4 symboler. Som tur är tror jag inte det finns några sådana
            {
                mirror += "*M*" + regnummer;
                if (parkhus[plats].StartsWith(mirror))
                {
                    parkhus[plats] = parkhus[plats].Remove(0, mirror.Length);
                    Console.WriteLine("\n{0} har flyttats ur systemet.", regnummer);
                }
                else if (parkhus[plats].EndsWith(mirror))
                {
                    int pos = parkhus[plats].Length;
                    int pos2 = mirror.Length;
                    parkhus[plats] = parkhus[plats].Remove((pos - pos2), mirror.Length);
                    Console.WriteLine("\n{0} hämtas ut från plats {1}.", regnummer, plats + 1);
                }
            }
        }                                                                                                       /// Lite knepigt att ta bort rätt mc. Men hittade ett sätt efter lite klurande.
        static void Receipt(string regnummer, int plats) // Kvitto att ge till kund eller garantera vilken plats fordonet faktiskt står vid i systemet. 
        {
            Console.WriteLine("\n**Kvitto**" +
                "\n|| Fordon * {0} *  parkeras på plats: * {1} * ||\n" +
                "**Kvitto**", regnummer, plats + 1);
        }
        static void Move(int plats, int nyplats, string[] parkhus, string regnummer) //flyttar regnumret i systemet.
        {
            nyplats -= 1; // Användarsiffra fås in. Minus 1 för att matcha i systemet.
            string mirrorregnummer = "";
            string newmirror = parkhus[nyplats];
            bool ismc = false;
            ismc = CheckIfMc(regnummer, parkhus);

            if (parkhus[nyplats] == null)
            {
                if (ismc == true)
                {
                    mirrorregnummer = "*M*" + regnummer;
                }
                else
                {
                    mirrorregnummer = "*B*" + regnummer;
                }
                parkhus[nyplats] += mirrorregnummer;
                Remove(plats, regnummer, parkhus);
                Console.WriteLine("\nFordon ska flyttas från plats * {0} *. Till plats: * {1} *", plats + 1, nyplats + 1);
            }

            else if (ismc == true && newmirror.StartsWith("*M*") && newmirror.Length < 14) // då är det bara en motorcykel.
            {
                parkhus[nyplats] += "*M*" + regnummer;
                Remove(plats, regnummer, parkhus);
                Console.WriteLine("\nFordon ska flyttas från plats * {0} *. Till plats: * {1} *", plats + 1, nyplats + 1);
            }
            else
            {
                Console.WriteLine("\nDen platsen är tyvärr upptagen..");
            }

        }
        static int Contains(string regnummer, string[] parkhus) // Hittar platsen i systemet där regnummret är även en andra mc
        {
            bool plats = false;
            string[] checkempy = new string[2];                                         ///tom sträng för att kolla om den är null. Finns garanterat ett bättre sätt, men de va detta jag byggde när jag var inne i byggandet :P 

            for (int i = 0; i < parkhus.Length; i++)
            {
                if (parkhus[i] != checkempy[0])
                {
                    plats = parkhus[i].Contains(regnummer);
                    if (plats == true)
                    {
                        return i; // returnerar hittad plats.
                    }
                }
                else continue;
            }
            return -1; //hittar ej regnummret i systemet
        }
        static bool CheckIfMc(string regnummer, string[] parkhus) //kollar om regnummret är en mc. /// Om det 
        {
            string[] checkempty = new string[2];
            int plats = Contains(regnummer, parkhus);
            if (parkhus[plats].StartsWith("*B*") || parkhus[plats] == checkempty[0])
            {
                return false;
            }
            else return true;
        }
        static bool InputLength(string regnummer) //Självförklarande ;). ///Men om jag kontrollerar alla regnummer in till systemt.
        ///Så spelar den ingen större roll om de söker konstigt. Då räcker det med ett enkelt fel-meddelande att det inte hittades just där. Istället för att kontrollera alla användarinput.
        ///(just nu är det bara 10tecken, men här kan jag lägga lite tid på Regx)
        {
            if (regnummer.Length < 11)
            {
                return true;
            }
            else return false;
        }


        ///Meny Val
        public static void Sök(string[] parkhus)
        {
            string regnummer;
            Console.Write("\nVilket regnummer vill du söka efter?" +
                                        "\nRegnummer: ");
            regnummer = Console.ReadLine();
            if (Contains(regnummer, parkhus) >= 0)
            {
                Console.WriteLine("\nFordonet står på plats: {0}\n", (Contains(regnummer, parkhus) + 1));

            }
            else { Console.WriteLine("\nHittar inte regnummret i systemet"); }
        }
        static void Flytta(string[] parkhus)
        {
            string regnummer;
            int plats;
            int nyplats;
            Console.WriteLine("\nVilket fordon vill du flytta?\n" +
                               "Regnummer: ");
            regnummer = Console.ReadLine();
            plats = Find(regnummer, parkhus);
            Console.WriteLine("\nFordonet står på plats: {0}\n" +
                             "\nVilken plats vill du flytta den till?" +
                             "\nTill plats: ", plats + 1);
            nyplats = int.Parse(Console.ReadLine());
            Move(plats, nyplats, parkhus, regnummer);
            Receipt(regnummer, Find(regnummer, parkhus));

        }
        static void PlockaUr(string[] parkhus)
        {
            string regnummer;
            Console.Write("\nVilket Regnummer vill du hitta: ");
            regnummer = Console.ReadLine();
            Console.WriteLine("\nFordonet finns på plats: {0}.\n\n | Kör ut den till kunden |\n", (Contains(regnummer, parkhus) + 1));
            Remove(Contains(regnummer, parkhus), regnummer, parkhus);
        }
        static void Parkera(string[] parkhus)
        {

            int val = 0;
            string fordonsinfo = "";
            string regnummer = "";

            string mc = "*M*";
            string bil = "*B*";

            Console.WriteLine("\nÄr det en:\n\n 1. Bil\n 2. Motorcykel");
            val = int.Parse(Console.ReadLine());
            switch (val)
            {
                case 1: // bil
                    {
                        fordonsinfo += bil;                                         //rest av hur jag började bygga systemet. Hade jag gjort om det, så hade detta hanterats av method
                        Console.Write("\nRegnummmer: ");
                        regnummer = Console.ReadLine();
                        if (InputLength(regnummer))
                        {
                            fordonsinfo += regnummer;
                            Park(fordonsinfo, parkhus);
                            Receipt(regnummer, Find(regnummer, parkhus));
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Felaktigt regnummer");
                            break;
                        }
                    }
                case 2: // mc
                    {
                        fordonsinfo += mc;
                        Console.Write("\nRegnummmer: ");
                        regnummer = Console.ReadLine();
                        if (InputLength(regnummer))
                        {
                            fordonsinfo += regnummer;
                            Parkmc(fordonsinfo, parkhus);
                            Receipt(regnummer, (Contains(regnummer, parkhus)));
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Felaktigt regnummer");
                            break;
                        }
                    }
            }
        }
        static void Visa(string[] parkhus) //Ett qvickt och lätt sätt att visa vad som finns i platserna för användaren
        {
            int counter = 0;

            Console.WriteLine("\n*B* är en BIL.\n*M* är en motorcykel");

            foreach (string plats in parkhus)
            {
                counter += 1;
                Console.Write("\n[{0}:", counter);
                if (plats != null)
                {
                    int fylld = plats.Length;
                    if (fylld > 0)
                    {
                        Console.Write("{0}]", plats);
                    }
                }
                else Console.Write("tom] ");
                if (counter % 5 == 0)
                {
                    Console.WriteLine("\t");
                }
            }
        }
        static void OptimeraPlatserMC(string[]parkhus) //Hittar singel MCs och parar ihop dom. 
        {
            string mc = "*M*";
            string mirrorregnummer;

            for (int i = 99; i >= 0; i--) 
            {
                if (parkhus[i] == null)
                {
                    continue;
                }
                if (parkhus[i].StartsWith(mc) && parkhus[i].Length < 14) //kollar om det finns en singel MC
                {
                    mirrorregnummer = parkhus[i].Remove(0,3);

                    for (int j = 0; j < parkhus.Length; j++)
                    {
                        if (parkhus[j] == null)
                        {
                            continue;
                        }
                        else if (parkhus[j].StartsWith(mc) && parkhus[j].Length < 14)
                        {
                            Move(i, j+1, parkhus, mirrorregnummer);
                            break;
                        }
                        else if (j == parkhus.Length) // Om den kommer till slutet
                        {
                            Console.WriteLine("Går ej att flytta mer tyvärr");
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }
}