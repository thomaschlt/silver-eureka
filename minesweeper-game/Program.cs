using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CHASSE_AU_TRESOR__
{
    class Program
    {
        static void Main(string[] args)

        {
            int nbLignes, nbColonnes;
            int ligneChoisie = -1, colonneChoisie = -1;
            int nbTrésors = 0;
            int finPartie = 1;
            string reponseRefairePartie = "O";

            direBienvenue();
            afficherRegles();


            while (reponseRefairePartie.Equals("O"))
            {
                Console.Clear();
                Console.WriteLine(" ============================= Début de la partie ! ============================= ");
                Thread.Sleep(1000);
                afficherTexteAnimé("\t\t ~~ Dimension de la grille à définir ~~ ");

                do
                {
                    afficherTexteAnimé("Nombre de lignes: ");
                    nbLignes = Convert.ToInt32(Console.ReadLine());
                    afficherTexteAnimé("Nombre de colonnes : ");
                    nbColonnes = Convert.ToInt32(Console.ReadLine());
                } while (nbLignes <= 1 || nbColonnes <= 1); //s'arrête lorsque lignes > 1 et colonnes > 1

                /**
                 * Création des grilles 
                 */

                string[,] gamingGride = new string[nbLignes, nbColonnes];
                int[,] cheatGride = new int[nbLignes, nbColonnes];

                //premier tour
                premierCoup(cheatGride, gamingGride, ref ligneChoisie, ref colonneChoisie, ref nbTrésors, ref finPartie);

                while (finPartie != 0 && nbTrésors > 0)
                {
                    devoilerCaseRécursif(cheatGride, gamingGride, ligneChoisie, colonneChoisie);
                    displayStringGrid(gamingGride);
                    choisirCoup(cheatGride, ref ligneChoisie, ref colonneChoisie, ref nbTrésors, ref finPartie);
                }
                if (nbTrésors == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("C'est gagné !");
                    Console.ReadLine();
                }
                else if (finPartie == 0)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Aïe ! C'est perdu !");
                    Console.ReadLine();
                }
                Console.ResetColor();
                afficherTexteAnimé("Voulez-vous refaire une partie ? (O/N) ");
                do
                {
                    reponseRefairePartie = Console.ReadLine().ToUpper();
                } while (!reponseRefairePartie.Equals("O") && !reponseRefairePartie.Equals("N")); //s'arrete lorsque egale à O ou N 
            }
            Console.Clear();
            afficherTexteAnimé(@"
  ___    _     _            _        _     _ 
 / _ \  | |   (_)          | |      | |   | |
/ /_\ \ | |__  _  ___ _ __ | |_ ___ | |_  | |
|  _  | | '_ \| |/ _ \ '_ \| __/ _ \| __| | |
| | | | | |_) | |  __/ | | | || (_) | |_  |_|
\_| |_/ |_.__/|_|\___|_| |_|\__\___/ \__| (_)
                                             
                                             ", 2);

        }

        static void direBienvenue()
        {
            afficherTexteAnimé(@"
  ____   _                                               _ 
 |  _ \ (_)                                             | |
 | |_) | _   ___  _ __ __   __ ___  _ __   _   _   ___  | |
 |  _ < | | / _ \| '_ \\ \ / // _ \| '_ \ | | | | / _ \ | |
 | |_) || ||  __/| | | |\ V /|  __/| | | || |_| ||  __/ |_|
 |____/ |_| \___||_| |_| \_/  \___||_| |_| \__,_| \___| (_)", 2);

            Console.WriteLine("\n\n");
            Thread.Sleep(2000);
            Console.Clear();

            afficherTexteAnimé(" Appuyez sur n'importe quelle touche pour démarrer le jeu ", 2);
            Console.ReadKey(true);
            Console.Clear();
        }

        static void afficherRegles()
        {
            afficherTexteAnimé("Voulez-vous lire les règles du jeu ? (O/N)");

            string reponse = Console.ReadLine().ToUpper();


            while (!reponse.Equals("O") && !reponse.Equals("N"))
            {
                afficherTexteAnimé("Entrez une réponse correcte !");
                reponse = Convert.ToString(Console.ReadLine()).ToUpper();
            }

            if (reponse.Equals("O"))
            {
                Console.Clear();
                Console.WriteLine(" ============================= Règles du jeu : ============================= \n\n");
                string premierMessage = "Vous allez devoir choisir des dimensions pour votre grille. Le nombre de lignes et " +
                    "de colonnes devra être supérieur à 1. \nVous devrez alors choisir une ligne et une colonne sur laquelle jouer. " +
                    "\nLe but est d'éviter de tomber sur une mine et de ramasser tous les trésors présents dans la grille." +
                    "\nUne partie est gagnée quand toutes les cases non-minées ont été révélées par le joueur et que tous les trésors ont été trouvés" +
                    "\nAu contraire, une partie sera considérée comme perdue lorsque le joueur aura révélé une case piégée";
                afficherTexteAnimé(premierMessage);
            }
            else
            {
                Console.Clear();
                afficherTexteAnimé("Très bien ! On va pouvoir commencer ! ");
            }
            Thread.Sleep(2000);
        }

        static void choisirCoup(int[,] grid1, ref int ligneChoisie, ref int colonneChoisie, ref int nbTrésors, ref int finPartie)
        {
            Console.ResetColor();
            afficherTexteAnimé("Quelle ligne voulez-vous jouer ? (entre 1 et " + grid1.GetLength(0) + ") : ");
            ligneChoisie = Convert.ToInt32(Console.ReadLine()) - 1;

            afficherTexteAnimé("Quelle colonne voulez-vous jouer ? (entre 1 et " + grid1.GetLength(1) + ") : ");
            colonneChoisie = Convert.ToInt32(Console.ReadLine()) - 1;

            if (grid1[ligneChoisie, colonneChoisie] == (int)etatCase.tresor)
                nbTrésors--;
            else if (grid1[ligneChoisie, colonneChoisie] == (int)etatCase.bombe)
                finPartie = 0;

        }

        static void afficherTexteAnimé(string text, int speed = 20)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
            Console.WriteLine();
        }

        static void devoilerCaseRécursif(int[,] grid1, string[,] grid2, int ligneChoisie, int colonneChoisie)
        {
            bool estJouable = grid1[ligneChoisie, colonneChoisie] == (int)etatCase.vide || grid1[ligneChoisie, colonneChoisie] == (int)etatCase.tresor;

            if (grid1[ligneChoisie, colonneChoisie] > 0)
            {
                if (grid1[ligneChoisie, colonneChoisie] == (int)etatCase.tresor)
                {
                    grid2[ligneChoisie, colonneChoisie] = " T";
                }

                else
                {
                    grid2[ligneChoisie, colonneChoisie] = " " + grid1[ligneChoisie, colonneChoisie];
                    grid1[ligneChoisie, colonneChoisie] = (int)etatCase.vérifiée;
                }
            }
            else if (grid1[ligneChoisie, colonneChoisie] != (int)etatCase.vérifiée && estJouable)
            {
                if (grid1[ligneChoisie, colonneChoisie] == (int)etatCase.tresor)
                {
                    grid2[ligneChoisie, colonneChoisie] = " T";
                }
                else
                    grid2[ligneChoisie, colonneChoisie] = "  ";

                grid1[ligneChoisie, colonneChoisie] = (int)etatCase.vérifiée;

                if (ligneChoisie - 1 >= 0 && grid1[ligneChoisie - 1, colonneChoisie] != (int)etatCase.vérifiée) //NORD
                {
                    devoilerCaseRécursif(grid1, grid2, ligneChoisie - 1, colonneChoisie);

                    if (colonneChoisie + 1 < grid1.GetLength(0) && grid1[ligneChoisie, colonneChoisie + 1] != (int)etatCase.vérifiée)
                        devoilerCaseRécursif(grid1, grid2, ligneChoisie - 1, colonneChoisie + 1); //NORD-EST

                    if (colonneChoisie - 1 >= 0 && grid1[ligneChoisie, colonneChoisie - 1] != (int)etatCase.vérifiée)
                        devoilerCaseRécursif(grid1, grid2, ligneChoisie - 1, colonneChoisie - 1); //NORD-OUEST
                }

                if (colonneChoisie + 1 < grid1.GetLength(1) && grid1[ligneChoisie, colonneChoisie + 1] != (int)etatCase.vérifiée)//EST
                    devoilerCaseRécursif(grid1, grid2, ligneChoisie, colonneChoisie + 1);

                if (ligneChoisie + 1 < grid1.GetLength(0) && grid1[ligneChoisie + 1, colonneChoisie] != (int)etatCase.vérifiée)//SUD
                {
                    devoilerCaseRécursif(grid1, grid2, ligneChoisie + 1, colonneChoisie);
                    if (colonneChoisie + 1 < grid1.GetLength(1) && grid1[ligneChoisie + 1, colonneChoisie + 1] != (int)etatCase.vérifiée)
                        devoilerCaseRécursif(grid1, grid2, ligneChoisie + 1, colonneChoisie + 1);//SUD-EST
                    if (colonneChoisie - 1 >= 0 && grid1[ligneChoisie + 1, colonneChoisie - 1] != (int)etatCase.vérifiée)
                        devoilerCaseRécursif(grid1, grid2, ligneChoisie, colonneChoisie); //SUD-OUEST
                }
                if (colonneChoisie - 1 >= 0 && grid1[ligneChoisie, colonneChoisie - 1] != (int)etatCase.vérifiée)//OUEST
                    devoilerCaseRécursif(grid1, grid2, ligneChoisie, colonneChoisie - 1);

            }



        }

        public enum etatCase
        {
            bombe = -2,
            tresor,
            vide,
            choisie,
            vérifiée = -9,
        }

        static void premierCoup(int[,] grid1, string[,] grid2, ref int ligneChoisie, ref int colonneChoisie, ref int nbTrésors, ref int finPartie)
        {
            choisirCoup(grid1, ref ligneChoisie, ref colonneChoisie, ref nbTrésors, ref finPartie);
            //initialisation des grilles
            initializeGrid(grid1, grid2, grid1.GetLength(0), grid1.GetLength(1), ligneChoisie, colonneChoisie, ref nbTrésors);
            placerChiffre(grid1);
        }

        static void initializeGrid(int[,] demineur1, string[,] demineur2, int nbLigne, int nbColonne, int ligneChoisie, int colonneChoisie, ref int nbTrésors)
        {
            Random rnd = new Random();

            //placement des bombes dans la grille cheatée

            placerBombes(demineur1, nbLigne, nbColonne, ligneChoisie, colonneChoisie, rnd);

            //placement des trésors dans la grille cheatée

            nbTrésors = placerTresors(demineur1, ligneChoisie, colonneChoisie, rnd);

            //création de la grille joueur
            for (int i = 0; i < demineur1.GetLength(0); i++)
            {
                for (int j = 0; j < demineur1.GetLength(1); j++)
                {
                    demineur2[i, j] = "ND";
                }
            }


        }

        static void placerBombes(int[,] grid1, int nbLigne, int nbColonne, int ligne, int colonne, Random r)
        {
            int nbMines = r.Next(((nbLigne + 1) / 2), ((nbLigne + 1 * nbColonne + 1) / 2) + 1);
            int ligneAleatoire, colonneAleatoire;

            do
            {
                int x = r.Next(0, grid1.GetLength(0));
                int y = r.Next(0, grid1.GetLength(1));

                if (x == ligne || y == colonne)
                {
                    x = r.Next(0, grid1.GetLength(0));
                    y = r.Next(0, grid1.GetLength(1));
                }
                else //s'exécute lorsque x != ligne et y != colonne
                {
                    grid1[x, y] = (int)etatCase.bombe;
                    nbMines--;
                }

            } while (nbMines > 0); //s'arrête lorsque nbMines == 0

        }

        static int placerTresors(int[,] grid1, int ligne, int colonne, Random r)
        {
            int nbTrésors = r.Next(1, 4);
            int nbTrésorsDec = nbTrésors;
            Console.WriteLine("Nombre de trésors à trouver " + nbTrésors);
            do
            {
                int x = r.Next(0, grid1.GetLength(0));
                int y = r.Next(0, grid1.GetLength(1));
                if ((x == ligne && y == colonne) || grid1[x, y] == (int)etatCase.bombe) //contraposée : on veut placer un trésor sur la case choisie ou sur une case mine
                {
                    x = r.Next(0, grid1.GetLength(0));
                    y = r.Next(0, grid1.GetLength(1));
                }
                else //contraposée : la case n'est pas celle choisie par le joueur et n'est pas une mine
                {
                    grid1[x, y] = (int)etatCase.tresor;
                    nbTrésorsDec--;
                }

            } while (nbTrésorsDec != 0); //s'arrête lorsque nbTrésorsDec == 0;
            return nbTrésors;
        }

        //permet d'afficher une grille Int
        static void displayIntGrid(int[,] grid)
        {
            //Console.Clear();
            for (int row = 0; row < grid.GetLength(0); row++)
            {

                for (int column = 0; column < grid.GetLength(1); column++)
                {
                    if (grid[row, column] >= 0)
                        Console.Write("| " + grid[row, column] + "  |");
                    else if (grid[row, column] < 0)
                        Console.Write("| " + grid[row, column] + " |");
                }
                Console.WriteLine();
            }
        }

        static void displayStringGrid(string[,] grid)
        {
            //Console.Clear();
            Console.WriteLine(" ============================= Votre tour ============================= ");
            Console.WriteLine("\n");
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                Console.ResetColor();
                Console.Write("[" + (row + 1) + "] : |");
                for (int column = 0; column < grid.GetLength(1); column++)
                {
                    if (grid[row, column] == " T")
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (grid[row, column] == " 1" || grid[row, column] == " 2")
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if (grid[row, column] == " 3" || grid[row, column] == " 4")
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    else
                        Console.ResetColor();

                    Console.Write(grid[row, column] + "|");
                }
                Console.WriteLine("\n");
            }
        }


        static void placerChiffre(int[,] grid)
        {

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == (int)etatCase.vide || grid[i, j] == (int)etatCase.choisie)
                        grid[i, j] = compteurCase(grid, i, j);
                }
            }
        }

        static int compteurCase(int[,] grid, int x, int y)
        {
            int cpt = 0;

            for (int row = x - 1; row <= x + 1; row++)
            {
                for (int column = y - 1; column <= y + 1; column++)
                {
                    if (row >= 0 && column >= 0 && row < grid.GetLength(0) && column < grid.GetLength(1)) //s'arrête lorsque row négatif ou supérieur au nombre de ligne ou column négatif ou supérieur au nombre de colonne
                    {
                        if (grid[row, column] == (int)etatCase.bombe)
                            cpt++;

                        if (grid[row, column] == (int)etatCase.tresor)
                            cpt = cpt + 2;
                    }
                }
            }
            return cpt;
        }
    }

}


