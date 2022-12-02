using gyak9.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gyak9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Population = GetPopulation(@"C:\Temp\nép-teszt.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");
        }

        List<Person> Females = new List<Person>();
        List<Person> Males = new List<Person>();

        private void Start()
        {
            for (int year = 2005; year < 2025; year++)
            {
                for (int i = 0; i < Population.Count(); i++)
                {
                    SimStep(year, Population[i]);
                }

                int NumberOfMales = (from x in Population
                                     where x.Gender == Gender.Male && x.IsAlive
                                     select x).Count();
                int NumberOfFemales = (from x in Population
                                       where x.Gender == Gender.Female && x.IsAlive
                                       select x).Count();

                Console.WriteLine(
                    string.Format("Év: {0} Fiúk: {1} Lányok: {2}", year, NumberOfMales, NumberOfFemales));

            }
        }

        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;
            int age = (year - person.BirthYear);

            var pDeath = (from x in DeathProbabilities
                          where x.Gender == person.Gender && x.Age == age
                          select x.Probability).FirstOrDefault();


            if (rng.NextDouble() < pDeath) person.IsAlive = false;
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                var pBirth = (from x in BirthProbabilities
                              where x.Age == age
                              select x.Probability).FirstOrDefault();
                if (rng.NextDouble() <= pBirth)
                {
                    var newBorn = new Person
                    {
                        BirthYear = year,
                        IsAlive = true,
                        NumberOfChildren = 0,
                        Gender = (Gender)(rng.Next(1, 3))
                    };
                    Population.Add(newBorn);
                }
            }
        }



        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        Random rng = new Random(1234);
        public List<Person> GetPopulation(string csvpath)
        {

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    Population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NumberOfChildren = int.Parse(line[2])
                    });
                }

            }
            return Population;

        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {


            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    BirthProbabilities.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NumberOfChildren = int.Parse(line[1]),
                        Probability = double.Parse(line[2])
                    });
                }

            }
            return BirthProbabilities;

        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {


            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    DeathProbabilities.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        Probability = double.Parse(line[2])
                    });
                }

            }
            return DeathProbabilities;

        }
    }
}
