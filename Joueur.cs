using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mots_Glisses
{
    public class Joueur
    {
      private string nom_joueur; //Le nom du joueur
        private List<string> liste_mot = new List<string>(); //la liste des mots déjà trouvés par le joueur
        private int score; //le score du joueur
        public Joueur(string nom_joueur, List<string> liste_mot, int score = 0)
        {
            this.nom_joueur = nom_joueur;
            this.liste_mot = liste_mot;
            this.score = score;

        }
        public string Nom_joueur
        {
            get { return nom_joueur; }
            set { nom_joueur = value; }
        }
        public List<string> Liste_nom
        {
            get { return liste_mot; }
            set { liste_mot = value; }
        }
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public void Add_mot(string mot)
        {
            liste_mot.Add(mot); //ajoute un mot dans la liste de mots trouvés
        }
        public string toString()
        {
            string chaine_mot = ""; //la chaine de caractère regroupant tout les mots trouvés par je joueur
            foreach (string element in liste_mot)
            {
                chaine_mot += element + "  ";
            }
            string chaine_joueur = "nom : " + nom_joueur + "\nmots trouvés : " + chaine_mot + "\nscore" + score; //chaine de caractère qui décrit un joueur
            return chaine_joueur;
        }
        public int Add_score(int val)
        {
            score += val;
            return score;
        }
        public bool Contient(string mot)  //si le mot trouvé par le joueur est déjà dans la liste Contient retourne false sinon Contient retourne true
        {
            if (liste_mot == null)
            {
                liste_mot.Add("");
                return true;
            }
            foreach (string element in liste_mot)
            {
                if (element == mot) return false;
            }
            return true;
        }
    }
}