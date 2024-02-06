using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mots_Glisses
{
    public class Jeu
    {
     private Dictionnaire dictionnaire;
        private Plateau plateau;
        private List<Joueur> joueurs;
        private TimeSpan tempsTour;
        private TimeSpan tempsTotal;

        public Jeu(Dictionnaire dictionnaire, Plateau plateau, List<Joueur> joueurs, TimeSpan tempsTour, TimeSpan tempsTotal)
        {
            this.dictionnaire = dictionnaire;
            this.plateau = plateau;
            this.joueurs = joueurs;
            this.tempsTour = tempsTour;
            this.tempsTotal = tempsTotal;
        }
        public int CalculerScore(string mot)
        {
            try
            {
                string[] lignes = File.ReadAllLines("Lettre.txt");
                int score = 0;

                foreach (string line in lignes)
                {
                    string[] item = line.Split(',');
                    for (int i = 0; i < mot.Length; i++)
                    {
                        if (Convert.ToChar(item[0]) == mot[i])
                        {
                            score += Convert.ToInt32(item[2]);
                        }
                    }

                }
                return score;



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;

        }

        public void Jouer()
        {
            bool partieTerminee = false;
            DateTime debutPartie = DateTime.Now;

            dictionnaire.MotsParLettre = dictionnaire.Tri_QuickSort();
            int variableArret = 0;

            while (!partieTerminee)
            {
                

                foreach (Joueur joueur in joueurs)
                {
                    Console.WriteLine();
                    Console.WriteLine(joueur.Nom_joueur + " doit jouer !\n");


                    DateTime debutTour = DateTime.Now;
                    TimeSpan tempsEcoule = DateTime.Now - debutTour;//temps ecouler depuis le début de la partie
                    TimeSpan tempsRestant = tempsTotal - (DateTime.Now - debutPartie);
                    // Vérifier si la partie est terminée
                    if (plateau.FinJeu() || tempsRestant.TotalSeconds <= 0) // Verifie les conditions de fin du jeu
                    {
                        partieTerminee = true;
                        break;
                    }

                    if (tempsRestant.TotalSeconds > tempsTour.TotalSeconds )
                    {
                        Console.WriteLine(tempsTour.TotalSeconds + " secondes pour jouer");
                    }
                    else
                    {
                        Console.WriteLine(tempsRestant.TotalSeconds + " secondes pour jouer");
                    }

                    Console.WriteLine(plateau.toString());
                    Console.WriteLine("Si vous souhaitez arrêter de jouer, les deux joueurs doivent entrer "+"!\n"+"Entrez un mot : ");
                    while (tempsEcoule < tempsTour)
                    {
                        tempsEcoule = DateTime.Now - debutTour;
                        // Vérifier si la partie est terminée
                        if (plateau.FinJeu() || tempsRestant.TotalSeconds <= 0) // Verifie les conditions de fin du jeu
                        {
                            partieTerminee = true;
                            break;
                        }

                        tempsEcoule = DateTime.Now - debutTour;
                        // Logique de jeu pour le tour du joueur

                        if (Console.KeyAvailable == true)
                        {
                            string motPropose = Console.ReadLine();
                            while(motPropose == null||motPropose.Length==0)
                            {
                                Console.WriteLine("Mot invalide, réessayez.");
                                motPropose = Console.ReadLine();
                            }
                            if(motPropose == "!")
                            {
                                variableArret++;
                                if (variableArret >= 2)
                                {
                                    partieTerminee = true;
                                    break;
                                }
                                break;
                            }
                            if(variableArret > 0) variableArret--;

                            

                            motPropose = motPropose.ToUpper();

                            // Vérifier si le mot est valide sur le plateau et dans le dictionnaire
                            bool a = true;
                            if (plateau.Recherche_Mot(motPropose) == null || plateau.Recherche_Mot(motPropose).GetLength(0) == 1) { a = false; }

                            bool motValide = a && dictionnaire.RechDichoRecursif(motPropose);

                            if (motValide && joueur.Contient(motPropose) == true)
                            {
                                joueur.Add_mot(motPropose); // Ajouter le mot à la liste des mots du joueur


                                // Mettre à jour le plateau après avoir trouvé un mot valide
                                plateau.Maj_Plateau(plateau.Recherche_Mot(motPropose));
                                int Scorejoueur = joueur.Add_score(CalculerScore(motPropose)); // Calculer et ajouter le score du mot au joueur

                                Console.WriteLine("bien vu mgl ! Ton score est de : " + Scorejoueur);
                                break; // Sortir de la boucle de jeu du joueur si le mot est valide
                            }
                            else
                            {
                                Console.WriteLine("Mot invalide ou déjà trouvé. Réessayez !");
                            }
                        }

                        
                        // Mettre à jour le temps écoulé pour le tour
                        tempsEcoule = DateTime.Now - debutTour;
                        
                        
                    }
                    Console.Write("\nChangement de joueur !");

                    if (partieTerminee==true)
                    {
                        Console.WriteLine("La partie est terminée !");
                        break;
                    }

                }

            }

            // Afficher le résultat final, scores, etc.
            Console.WriteLine("Resultat final :");
            Console.WriteLine(joueurs[0].Nom_joueur + " a " + joueurs[0].Score + " points.");
            Console.WriteLine(joueurs[1].Nom_joueur + " a " + joueurs[1].Score + " points.");
            if (joueurs[0].Score > joueurs[1].Score)
            {
                Console.WriteLine(joueurs[0].Nom_joueur + " a gagné !");
            }
            else if (joueurs[0].Score < joueurs[1].Score)
            {
                Console.WriteLine(joueurs[1].Nom_joueur + " a gagné !");
            }
            else
            {
                Console.WriteLine("Egalité !");
            }


        }
    }
}
