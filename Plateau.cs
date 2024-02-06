using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Mots_Glisses
{
    public class Plateau
    {
        private char[,] matriceLettres;  // pour stocker la grille de lettres
                                         //private int[] nombreMaxLettres; // pour définir le nombre maximal de lettres pour chaque lettre de l'alphabet


        public Plateau(char[,] matriceLettres /*int[] nombreMaxLettres*/ )
        {
            this.matriceLettres = matriceLettres;
            //this.nombreMaxLettres = nombreMaxLettres;

        }
        public char[,] MatriceLettres
        {
            get { return matriceLettres; }
            set { matriceLettres = value; }
        }




        public string toString()
        {
            string msg = "\n---------------------------------";
            for (int i = 0; i < matriceLettres.GetLength(0); i++)
            {
                msg += "\n";
                msg += "| ";
                for (int j = 0; j < matriceLettres.GetLength(1); j++)
                {
                    msg += Convert.ToString(matriceLettres[i, j]) + " | ";
                }
                msg += "\n";
                msg += "---------------------------------";
            }
            return msg;
        }
        public void ToFile(string nomFichier)//nomFIchier=Test1.csv
        {
            using (StreamWriter writer = new StreamWriter(nomFichier))
            {
                for (int i = 0; i < matriceLettres.GetLength(0); i++)
                {
                    for (int j = 0; j < matriceLettres.GetLength(1); j++)
                    {
                        writer.Write(matriceLettres[i, j] + ";");
                    }
                    writer.WriteLine();
                }
            }
        }


        public char[,] GénérationAléatoire(string nomFichier)//mettre le fichier Lettre.txt en nomFichier
        {
            Random r = new Random();
            List<char> lettres = new List<char>();//liste des lettres disponibles
            List<int> occurenceLettre = new List<int>();//tableau des occurences de chaque lettre
            try
            {
                string[] lignes = File.ReadAllLines(nomFichier);

                foreach (string line in lignes)
                {
                    string[] item = line.Split(',');
                    occurenceLettre.Add(Convert.ToInt32(item[1]));
                    lettres.Add(Convert.ToChar(item[0]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            for (int i = 0; i < matriceLettres.GetLength(0); i++)
            {
                for (int j = 0; j < matriceLettres.GetLength(1); j++)
                {
                    int a = r.Next(0, lettres.Count);
                    matriceLettres[i, j] = lettres[a];
                    occurenceLettre[a]--;
                    if (occurenceLettre[a] == 0)
                    {
                        lettres.RemoveAt(a);
                        occurenceLettre.RemoveAt(a);
                    }
                }
            }
            return matriceLettres;
        }
        public char[,] ToRead(string nomFichier = "Test1.csv")
        {//Appelez ici test1.csv
            try
            {
                string[] lignes = File.ReadAllLines(nomFichier);

                for (int i = 0; i < matriceLettres.GetLength(0); i++)
                {
                    string[] elements = lignes[i].Split(';');

                    for (int j = 0; j < matriceLettres.GetLength(1); j++)
                    {
                        char lettre = Convert.ToChar(elements[j]); // je converstie de la chaîne en caractère
                        matriceLettres[i, j] = lettre;
                    }
                }
                return matriceLettres;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;

        }


        public int[,] Recherche_Mot(string mot)
        {
            int[,] sauv = new int[mot.Length, 2];
            return Recherche_Mot_Recursive(mot.ToUpper(), 7, -1, 0, 1, 0, sauv, 0); // On fait appel à la méthode de recherche récursive
        }
        /*mot  = mot

          i = ligne

          j = colonne

          compt = savoir à quelle première case on est (ex: -1 , avec compt = 1, dit que la première lettre du mot est [7,2]

          index = savoir quand sauter les étapes, en fonction des recherches déjà faites précédemment

          sauv = tableau permettant de sauvegarder les positions des lettres

          NoRetour = variable pour s'assurer que une fois que le programme est vérifié la lettre de droite, il ne revienne pas en arrière sur la lettre de gauche

                     dans le cas où c'est la même*/
        private int[,] Recherche_Mot_Recursive(string mot, int i, int j, int l, int compt, int index, int[,] sauv, int NoRetour)
        {
            if (index < 1 && NoRetour != 1 && Recherche_Droite(mot, i, j, l) == true) // On vérifie que la recherche à droite est fructueuse
            {
                sauv[l, 0] = i;
                sauv[l, 1] = j + 1;  // On enregistre les positions de la lettre vérifiée
                if (mot.Length - 1 == l) { return sauv; } // On vérifie si il y a encore des lettres à trouver ou si le mot est complet
                if (l != 0) { NoRetour = 2; } // On s'assure avec la variable NoRetour que la recherche ne revienne pas à gauche
                return Recherche_Mot_Recursive(mot, i, j + 1, l + 1, compt, 0, sauv, NoRetour); // On relance la recherche pour la lettre suivante
            }
            else if (l != 0 && index < 2 && NoRetour != 2 && Recherche_Gauche(mot, i, j, l) == true) // La même pour la recherche de gauche
            {
                sauv[l, 0] = i;
                sauv[l, 1] = j - 1;
                if (mot.Length - 1 == l) { return sauv; }
                NoRetour = 1; // On s'assure avec la variable NoRetour que la recherche ne revienne pas à droite
                return Recherche_Mot_Recursive(mot, i, j - 1, l + 1, compt, 0, sauv, NoRetour);
            }
            else if (l != 0 && index < 3 && Recherche_Haut(mot, i, j, l) == true) // Recherche en haut cette fois-ci
            {
                sauv[l, 0] = i - 1;
                sauv[l, 1] = j;
                if (mot.Length - 1 == l) { return sauv; }
                NoRetour = 0;
                return Recherche_Mot_Recursive(mot, i - 1, j, l + 1, compt, 0, sauv, NoRetour);
            }
            else if (l != 0 && index < 4 && Recherche_Diago_Droite(mot, i, j, l) == true) // Recherche en haut à droite
            {
                sauv[l, 0] = i - 1;
                sauv[l, 1] = j + 1;
                if (mot.Length - 1 == l) { return sauv; }
                NoRetour = 0;
                return Recherche_Mot_Recursive(mot, i - 1, j + 1, l + 1, compt, 0, sauv, NoRetour);
            }
            else if (l != 0 && index < 5 && Recherche_Diago_Gauche(mot, i, j, l) == true) // Recherche en haut à gauche
            {
                sauv[l, 0] = i - 1;
                sauv[l, 1] = j - 1;
                if (mot.Length - 1 == l) { return sauv; }
                NoRetour = 0;
                return Recherche_Mot_Recursive(mot, i - 1, j - 1, l + 1, compt, 0, sauv, NoRetour);
            }
            else
            {
                if (l == 0 && j < 7) // On vérifie si on cherche la première lettre et si on a atteind le bout de la matrice
                {
                    return Recherche_Mot_Recursive(mot, 7, j + 1, 0, compt, 0, sauv, 0); // On relance la recherche suivante
                }
                else if (l == 0 && j == 7)
                {
                    int[,] faux = new int[1, 1];
                    return faux; // On a testé toute les possibilités, on retourne qu'on a pas trouvé le mot dans le tableau
                }
                else
                {
                    // On cherche de quelle direction vient la lettre précédente...
                    if (l - 2 >= 0)
                    {
                        if (sauv[l - 2, 1] == j - 1 && sauv[l - 2, 0] == i) //... de gauche...
                        {
                            index = 1; // Avec les index, on s'assure que l'ordinateur ne reprenne pas le même chemin
                            if (l == 2 || (l - 3 >= 0 && sauv[l - 3, 0] == i + 1)) { NoRetour = 0; }
                            else { NoRetour = 2; } // On s'assure que le programme ne revienne pas sur une lettre sur laquelle il est déjà passé
                        }
                        else if (sauv[l - 2, 1] == j + 1 && sauv[l - 2, 0] == i) //... de droite...
                        {
                            index = 2;
                            NoRetour = 0;
                        }
                        else if (sauv[l - 2, 1] == j && sauv[l - 2, 0] == i + 1) //... d'en bas...
                        {
                            index = 3;
                            NoRetour = 0;
                        }
                        else if (sauv[l - 2, 1] == j - 1 && sauv[l - 2, 0] == i + 1) //... d'en bas à gauche...
                        {
                            index = 4;
                            NoRetour = 0;
                        }
                        else if (sauv[l - 2, 1] == j + 1 && sauv[l - 2, 0] == i + 1)//... ou d'en bas à droite
                        {
                            index = 5;
                            NoRetour = 0;
                        }
                        return Recherche_Mot_Recursive(mot, sauv[l - 2, 0], sauv[l - 2, 1], l - 1, compt, index, sauv, NoRetour); // On relance le programme pour la lettre d'avant
                    }
                    else
                    {
                        return Recherche_Mot_Recursive(mot, 7, compt - 1, 0, compt + 1, 0, sauv, 0);
                    }
                }
            }
        }
        private bool Recherche_Gauche(string mot, int i, int j, int l) // On vérifie que la lettre que l'on cherche existe à gauche
        {
            if (j - 1 >= 0 && matriceLettres[i, j - 1] == mot[l]/*.ToString()*/)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool Recherche_Droite(string mot, int i, int j, int l) // De même pour la droite
        {
            if (j + 1 < 8 && matriceLettres[i, j + 1] == mot[l]/*.ToString()*/)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool Recherche_Haut(string mot, int i, int j, int l) // De même pour le haut

        {
            if (i - 1 >= 0 && j >= 0 && matriceLettres[i - 1, j] == mot[l]/*.ToString()*/)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool Recherche_Diago_Droite(string mot, int i, int j, int l) // De même pour en haut à droit

        {
            if (i - 1 >= 0 && j + 1 < 8 && matriceLettres[i - 1, j + 1] == mot[l]/*.ToString()*/)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool Recherche_Diago_Gauche(string mot, int i, int j, int l) // De même pour en haut à gauche
        {
            if (i - 1 >= 0 && j - 1 >= 0 && matriceLettres[i - 1, j - 1] == mot[l]/*.ToString()*/)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public char[,] Maj_Plateau(int[,] tab)
        {
            for (int i = tab.GetLength(0) - 1; i >= 0; i--)
            {
                int ligne = tab[i, 0];
                int colonne = tab[i, 1];

                // je supprime le caractère à l'indice correspondant à la ligne du plateau
                MatriceLettres[ligne, colonne] = ' ';

                // et je fais glisser les caractères du dessus vers le bas dans la colonne
                for (int j = ligne; j > 0; j--)
                {
                    MatriceLettres[j, colonne] = MatriceLettres[j - 1, colonne];
                }
                MatriceLettres[0, colonne] = ' ';
            }
            return MatriceLettres;
        }




        public bool FinJeu()
        {
            if (matriceLettres == null || matriceLettres.GetLength(0) == 0 || matriceLettres.GetLength(1) == 0)
            {
                return true;
            }
            return false;
        }
        





    }
}