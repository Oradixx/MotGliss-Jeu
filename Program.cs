using System;
using System.Collections.Generic;
using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


namespace Mots_Glisses
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Jeu de Mots Glissés de Jules et Clément";
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            
            Console.WriteLine("Bienvenue Joueurs passionnés, prenez place \n");

            Console.WriteLine("Comment voulez vous jouez : \n - 1 Plateau Prédéfini \n - 2 Plateau Aléatoire");
            int choix;
            string choixString = Console.ReadLine();
            while(choixString == null || choixString.Length == 0||(choixString != "1" && choixString != "2"))
            {
                Console.WriteLine("Veuillez choisir un nombre entre 1 et 2");
                choixString = Console.ReadLine();
            }

             choix = Convert.ToInt32(choixString);

            // Création du dictionnaire

            Dictionnaire dictionnaire = new Dictionnaire();
            char[,] matriceLettres = new char[8, 8];
            Plateau plateau;
            if (choix == 1)
            {
                plateau = new Plateau(matriceLettres);
                matriceLettres = plateau.ToRead();
                plateau = new Plateau(matriceLettres);
            }
            else if (choix == 2)
            {
                plateau = new Plateau(matriceLettres);
                matriceLettres = plateau.GénérationAléatoire("Lettre.txt");
                plateau = new Plateau(matriceLettres);
            }
            else
            {
                Console.WriteLine("Attention, le plateau n'est pas bien chargé, le plateau predefini a été chargeé à la place");
                plateau = new Plateau(matriceLettres);
                matriceLettres = plateau.ToRead();
                plateau = new Plateau(matriceLettres);
            }




            // Création du plateau
            // Créez la matrice de lettres selon vos besoins
            Console.WriteLine("Nom du joueur 1 : ");
            string nom_joueur1 = Console.ReadLine();
            nom_joueur1=verif(nom_joueur1);

            Console.WriteLine("Nom du joueur 2 : ");
            string nom_joueur2 = Console.ReadLine();
            nom_joueur2 = verif(nom_joueur2);
            /*while (nom_joueur2 == null || nom_joueur2.Length == 0)
            {
                Console.WriteLine("Nom invalide, réessayez.");
                nom_joueur2 = Console.ReadLine();
            }*/

            Console.WriteLine("Bonjour, " + nom_joueur1 + " et " + nom_joueur2 + " !");

            // Création des joueurs
            List<string> listMot = new List<string>();
            Joueur joueur1 = new Joueur(nom_joueur1, listMot);
            Joueur joueur2 = new Joueur(nom_joueur2, listMot);
            List<Joueur> joueurs = new List<Joueur> { joueur1, joueur2 };
            int tempsPartie;//temps total de la partie en minute
            int tempTour;//temps de chaques tours en secondes
            
            Console.WriteLine("\nCombien de temps voulez vous avoir pour jouer cette partie? (en minutes)");
            string tempsPartieStr = Console.ReadLine();
            
            tempsPartieStr=verif(tempsPartieStr);
            tempsPartie = Convert.ToInt32(tempsPartieStr);
            while (tempsPartie < 0|| tempsPartie > 15)
            {
                Console.WriteLine("Veuillez choisir un nombre entier supérieur à 0 et inférieur à 15");
                tempsPartieStr = Console.ReadLine();
                tempsPartieStr = verif(tempsPartieStr);
                tempsPartie = Convert.ToInt32(tempsPartieStr);
            }
            
            Console.WriteLine("\nCombien de temps voulez vous avoir pour jouer chaque tours ? (en secondes)");
            string tempTourStr=Console.ReadLine();
            tempTourStr=verif(tempTourStr);
            tempTour = Convert.ToInt32(tempTourStr);
            while (tempTour < 0|| tempTour > 120)
            {
                Console.WriteLine("Veuillez choisir un nombre entier supérieur à 0 et inférieur à 120");
                tempTourStr = Console.ReadLine();
                tempTourStr = verif(tempTourStr);
                tempTour = Convert.ToInt32(tempTourStr);
            }
            while (tempsPartie * 60 < tempTour||(tempsPartie < 0 || tempsPartie > 15)) 
            {
                    Console.WriteLine("Le temps de partie n'est pas valide. Choisissez-en un différent (en minutes).");
                tempsPartieStr = Console.ReadLine();
                tempsPartieStr = verif(tempsPartieStr);
                tempsPartie = Convert.ToInt32(tempsPartieStr);
            } 
            // Création du jeu
            TimeSpan tempsTour = TimeSpan.FromSeconds(tempTour); // Définissez le temps par tour
            TimeSpan tempsTotal = TimeSpan.FromMinutes(tempsPartie); // Définissez la durée totale de la partie
            Jeu jeu = new Jeu(dictionnaire, plateau, joueurs, tempsTour, tempsTotal);

            // Lancez le jeu
            jeu.Jouer();

            Console.ReadLine(); // Pour que la console ne se ferme pas directement

        }
        static string verif(string msg) //Cette méthode permet de vérifier si les texte écrits par l'utilisateur sont valides.
        {
            while (msg == null || msg.Length == 0)
            {
                Console.WriteLine("La saisie n'est pas valide. Réessayez.");
                msg = Console.ReadLine();
            }
            return msg;
        }
       
    }   
}