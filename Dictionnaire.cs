using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mots_Glisses
{
    public class Dictionnaire
    {
      private string fileName; //jsp quoi mettre dans le constructeur et ici donc j'ai mis le nom du fichier ou y'a le dictionnaire si y'a un bug avec l'utilisation du fichier on remet les "string fileName = ..." je les ai laisser en commentaire
        private List<string[]> motsParLettre; //chaque ligne de la list est un tableau de mots qui commencent par la même lettre
        public Dictionnaire(/*List<string[]> motsParLettre*/)
        {
            this.fileName = "Mots_Français.txt";
            this.motsParLettre = LireMots("Mots_Français.txt");
        }
        public List<string[]> MotsParLettre
        {
            get { return motsParLettre; }
            set { motsParLettre = value; }
        }

        public List<string[]> LireMots(string fileName)//créé une liste avec tout les mots
        {
            List<string[]> motsParLettre = new List<string[]>();

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] mots = line.Split(' ');
                        motsParLettre.Add(mots);
                    }
                }

                return motsParLettre;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string toString()
        {
            // string fileName = "Mots_Fr.txt";
            string msg = "";

            try
            {
                string[] lignes = File.ReadAllLines(fileName);
                foreach (string ligne in lignes)
                {
                    string[] item = ligne.Split(' ');

                    msg += item[0][0] + " : " + item.Length + "\n";

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            msg += "Le dictionnaire est en français.";
            return msg;
        }
        //Début de la méthode de tri :
        public List<string[]> Tri_QuickSort()
        {
            if (motsParLettre != null && motsParLettre.Count > 0)
            {
                foreach (string[] mots in motsParLettre) //"mots" c'est une ligne de "motsParLettre" et donc un tableau de mot commencant par la meme lettre et trié par ordre alphabétique
                {
                    QuickSort(mots, 0, mots.Length - 1);

                    //AfficherMots(mots);
                }

                return motsParLettre;
            }
            else
            {

                return null;
            }

        }



        static void QuickSort(string[] mots, int min, int max)
        {
            if (min < max)
            {
                int IndexEchange = Echange(mots, min, max);

                QuickSort(mots, min, IndexEchange - 1); //on trie les deux moitié du tableau en même temps 
                QuickSort(mots, IndexEchange + 1, max);
            }
        }

        static int Echange(string[] mots, int min, int max)
        {
            string temp = "";
            string pivot = mots[max];
            int i = min - 1; // l'index i qui sera utilisé pour suivre la position du dernier élément inférieur au pivot

            for (int j = min; j < max; j++)
            {
                if (string.Compare(mots[j], pivot) <= 0) //compare chaque élément avec le pivot. Si l'élément est inférieur ou égal au pivot, il est échangé avec l'élément à l'index i et i est incrémenté.
                {
                    i++;
                    temp = mots[i];
                    mots[i] = mots[j];
                    mots[j] = temp;
                }
            }

            temp = mots[i + 1];
            mots[i + 1] = mots[max];
            mots[max] = temp;
            return i + 1;
        }


        //Fin de la méthode de tri*/




        //Debut de la méthode de recherche
        public bool RechDichoRecursif(string mot)
        {
            foreach (string[] ligne in motsParLettre)
            {
                if (ComparRecursif(ligne, mot, 0, ligne.Length - 1) == true)
                {
                    return true;
                }
            }

            return false;
        }

        static bool ComparRecursif(string[] mots, string mot, int debut, int fin)
        {
            if (debut <= fin)
            {
                int milieu = (debut + fin) / 2;
                int comparaison = string.Compare(mot, mots[milieu]);
                if (comparaison == 0)
                {
                    // Le mot a été trouvé
                    return true;
                }
                else if (comparaison < 0)
                {
                    // Le mot se trouve dans la moitié gauche
                    return ComparRecursif(mots, mot, debut, milieu - 1);
                }
                else
                {
                    // Le mot se trouve dans la moitié droite
                    return ComparRecursif(mots, mot, milieu + 1, fin);
                }
            }

            // Le mot n'a pas été trouvé
            return false;
        }

    }
}
