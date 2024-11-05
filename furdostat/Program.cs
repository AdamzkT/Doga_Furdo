using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furdostat
{
    internal class Program
    {
        struct Adatok
        {
            public string azon { get; set; }
            public string reszleg_azon { get; set; }
            public int be_ki { get; set; }
            public int ora { get; set; }
            public int perc { get; set; }
            public int masodperc { get; set; }
            public Adatok(string sor)
            {
                string[] s = sor.Split(' ');
                azon = s[0];
                reszleg_azon = s[1];
                be_ki = Convert.ToInt32(s[2]);
                ora = Convert.ToInt32(s[3]);
                perc = Convert.ToInt32(s[4]);
                masodperc = Convert.ToInt32(s[5]);
            }
        }

        static List<Adatok> adatok = new List<Adatok>();
        static void Main(string[] args)
        {
            feladat1();
            feladat2();
            feladat3();
            feladat4();
            feladat5();
            feladat6();
            feladat7();

            Console.ReadKey();
        }

        private static void feladat1()
        {
            string[] f = File.ReadAllLines("furdoadat.txt",Encoding.UTF8);
            foreach (string sor in f) { adatok.Add(new Adatok(sor)); }
        }
        private static void feladat2()
        {
            Console.WriteLine("2. feladat:");

            int min_ora = 19;
            int min_perc = 59;
            int min_mperc = 59;
            int max_ora = 6;
            int max_perc = 0;
            int max_mperc = 0;

            foreach (var adat in adatok) {
                if(adat.be_ki == 1 && adat.reszleg_azon == "0")
                {
                    if (adat.ora < min_ora) { min_ora = adat.ora; }
                    if (adat.ora > max_ora) { max_ora = adat.ora; }
                }
            }
            foreach (var adat in adatok)
            {
                if (adat.be_ki == 1 && adat.reszleg_azon == "0")
                {
                    if (adat.ora == min_ora && adat.perc < min_perc) { min_perc = adat.perc; }
                    if (adat.ora == max_ora && adat.perc > max_perc) { max_perc = adat.perc; }
                }
            }
            foreach (var adat in adatok)
            {
                if (adat.be_ki == 1 && adat.reszleg_azon == "0")
                {
                    if (adat.ora == min_ora && adat.perc == min_perc && adat.masodperc < min_mperc) { min_mperc = adat.masodperc; }
                    if (adat.ora == max_ora && adat.perc == max_perc && adat.masodperc > max_mperc) { max_mperc = adat.masodperc; }
                }
            }

            Console.WriteLine($"Az első vendég {min_ora}:{min_perc}:{min_mperc}-kor lépett ki az öltözőből.");
            Console.WriteLine($"Az utolsó vendég {max_ora}:{max_perc}:{max_mperc}-kor lépett ki az öltözőből.");
        }
        private static void feladat3()
        {
            Console.WriteLine("\n3. feladat:");
            Dictionary<string, int> reszleg_belepes = new Dictionary<string, int>();
            foreach (var adat in adatok) {
                if(adat.reszleg_azon != "0" && adat.be_ki == 0)
                {
                    if (reszleg_belepes.ContainsKey(adat.azon)) { reszleg_belepes[adat.azon]++; }
                    else { reszleg_belepes[adat.azon] = 1; }
                }
            }
            int egy_belepes = 0;
            foreach (var szemely in reszleg_belepes) {
                if(szemely.Value == 1) { egy_belepes++; }
            }
            Console.WriteLine($"A fürdőben {egy_belepes} vendég járt csak egy részlegen.");
        }
        private static void feladat4()
        {
            Console.WriteLine("\n4. feladat:");
            Dictionary<string, int> furdo_ido_be = new Dictionary<string, int>();
            Dictionary<string, int> furdo_ido_ki = new Dictionary<string, int>();
            foreach (var adat in adatok)
            {
                int ido = adat.ora * 60 * 60 + adat.perc * 60 + adat.masodperc;
                if (furdo_ido_be.ContainsKey(adat.azon))
                {
                    if (furdo_ido_be[adat.azon] > ido) { furdo_ido_be[adat.azon] = ido; }
                }
                else { furdo_ido_be[adat.azon] = ido; }
                if (furdo_ido_ki.ContainsKey(adat.azon))
                {
                    if (furdo_ido_ki[adat.azon] < ido) { furdo_ido_ki[adat.azon] = ido; }
                }
                else { furdo_ido_ki[adat.azon] = ido; }
            }
            int legtobb_ido = 0;
            string legtobb_azon = "";
            foreach (var azon in furdo_ido_be.Keys) {
                int ido = furdo_ido_ki[azon] - furdo_ido_be[azon];
                if (ido > legtobb_ido) { legtobb_ido = ido; legtobb_azon = azon; }
            }
            int legtobb_ora = legtobb_ido / 3600;
            int legtobb_perc = (legtobb_ido % 3600)/60;
            int legtobb_mperc = (legtobb_ido % 3600)%60;
            Console.WriteLine($"A legröbb időt eltöltő vendég:\n{legtobb_azon}. vendég {legtobb_ora}:{legtobb_perc}:{legtobb_mperc}");
        }
        private static void feladat5()
        {
            Console.WriteLine("\n5. feladat:");
            Dictionary<string, int> statisztika = new Dictionary<string, int>();
            statisztika["6-9"] = 0;
            statisztika["9-16"] = 0;
            statisztika["16-20"] = 0;
            foreach(var adat in adatok)
            {
                if(adat.be_ki == 1 && adat.reszleg_azon == "0")
                {
                    if(adat.ora >= 6 && adat.ora < 9) { statisztika["6-9"]++; }
                    if(adat.ora >= 9 && adat.ora < 16) { statisztika["9-16"]++; }
                    if(adat.ora >= 16 && adat.ora < 20) { statisztika["16-20"]++; }
                }
            }
            foreach (var i in statisztika) { Console.WriteLine($"{i.Key} óra között {i.Value} vendég"); }
        }
        private static void feladat6()
        {
            Dictionary<string, int> szauna = new Dictionary<string, int>();
            for(int i = 0; i < adatok.Count; i++)
            {
                if (adatok[i].reszleg_azon == "2" && adatok[i].be_ki == 0)
                {
                    int ido = 0;
                    for (int j = i+1; j < adatok.Count; j++)
                    {
                        if (adatok[j].reszleg_azon == "2" && adatok[j].be_ki == 1) {
                            ido = (adatok[j].ora * 3600 + adatok[j].perc * 60 + adatok[j].masodperc) - (adatok[i].ora * 3600 + adatok[i].perc * 60 + adatok[i].masodperc);
                            break;
                        }
                    }
                    if (szauna.ContainsKey(adatok[i].azon)) { szauna[adatok[i].azon] += ido; }
                    else { szauna[adatok[i].azon] = ido; }
                }
            }
            File.WriteAllText("szauna.txt", "");
            foreach (var i in szauna) {
                int ora = i.Value / 3600;
                int perc = (i.Value % 3600)/60;
                int mperc = (i.Value % 3600)%60;
                File.AppendAllText("szauna.txt", $"{i.Key} {ora:00}:{perc:00}:{mperc:00}\n");
            }
        }
        private static void feladat7()
        {
            Console.WriteLine("\n7. feladat:");
            Dictionary<string, int> uszoda_db = new Dictionary<string, int>();
            Dictionary<string, int> szauna_db = new Dictionary<string, int>();
            Dictionary<string, int> gyogyviz_db = new Dictionary<string, int>();
            Dictionary<string, int> strand_db = new Dictionary<string, int>();
            foreach (var adat in adatok) {
                switch (adat.reszleg_azon) {
                    case "1": uszoda_db[adat.azon] = 1; break;
                    case "2": szauna_db[adat.azon] = 1; break;
                    case "3": gyogyviz_db[adat.azon] = 1; break;
                    case "4": strand_db[adat.azon] = 1; break;
                    default: break;
                }
            }
            Console.WriteLine("Uszoda: " + uszoda_db.Keys.Count());
            Console.WriteLine("Szaunák: " + szauna_db.Keys.Count());
            Console.WriteLine("Gyógyvizes medencék: " + gyogyviz_db.Keys.Count());
            Console.WriteLine("Strand: " + strand_db.Keys.Count());
        }
    }
}
