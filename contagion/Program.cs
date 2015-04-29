using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace contagion
{
    class Program
    {
        public static int N = 7;
        public static int MIN = 1;
        public static char VIDE = ' ';
        public static char ROND = 'O';
        public static char CROIX = 'X';

        // Déclaration d'un tableau en 2 dimensions pour stocker les elements du plateau ou de la matrice

        public static char[,] m = new char[7, 7];

        static void Main(string[] args)
        {
            int score1;
            int score2;

            Menu();

            initialiser(m);
            afficher(m);
            chacunSonTourDeJeu(m);
            calculScore(m, out score1, out score2);
            afficheGagnant(score1, score2);
        }

        static void Menu()
        {
            Console.WriteLine("Bonjour cher joueur vous allez enfin pouvoir jouer à mon Jeu");
            Console.WriteLine("     -----------------------------------------------");
            Console.WriteLine("    |       Contagion by TRAORE Oumar               |");
            Console.WriteLine("    |                                               |");
            Console.WriteLine("     -----------------------------------------------");
            Console.WriteLine("Ce jeu a ete Developpe par moi Traore Oumar" + Environment.NewLine);
        }

        // Fait en sorte que l'utilisateur entre bien un chiffre

        static void saisie(out int i)
        {
            bool valide = false;
            int temp = 0;
            while (!valide)
            {
                string saisissez = Console.ReadLine();
                if (int.TryParse(saisissez, out temp))
                    valide = true;
                else
                {
                    valide = false;
                    Console.WriteLine("Ceci n'est pas un chiffre ...");
                }
            }
            i = temp;
        }



        // Pour initialiser mon tableau au vide a l'exception des 4 cases du coin

        static void initialiser(char[,] m)
        {
            int i, j;
            for (i = 0; i < N; i++ )
            {
                for (j = 0; j < N; j++ )
                {
                    m[i, j] = VIDE;
                }
            }
            m[0, 0] = m[6, 6] = CROIX;
            m[0, 6] = m[6, 0] = ROND;
        }

        // Affiche le tableau

        static void afficher(char[,] m)
        {
            int i, j;
            Console.WriteLine("    1   2   3   4   5   6   7");
            for (i = 0; i < N; i++ )
            {
                Console.WriteLine("  ---------------------");
                Console.Write(i + 1);
                for (j = 0; j < N; j++)
                {
                    if (m[i, j] == ROND)
                    {
                        Console.Write("| " + m[i, j]);
                    }
                    else if (m[i, j] == CROIX)
                    {
                        Console.Write("| " + m[i, j]);
                    }
                    else Console.Write("| " + m[i, j]);
                }
                Console.WriteLine("| ");
            }
            Console.WriteLine("  ---------------------");

        }

        // Verifie si un deplacement est possible

        static bool veriDeplacer(int i, int j, int x, int y)
        {
            bool vDep = false;
            if ((i == x - 1 && j == y - 2) || (i == x && j == y - 2) || (i == x + 1 && j == y - 2) || (i == x + 2 && j == y - 2)) vDep = true;
            if ((i == x + 2 && j == y - 1) || (i == x + 2 && j == y) || (i == x + 2 && j == y + 1) || (i == x + 2 && j == y + 2)) vDep = true;
            if ((i == x + 1 && j == y + 2) || (i == x && j == y + 2) || (i == x - 1 && j == y + 2) || (i == x - 2 && j == y + 2)) vDep = true;
            if ((i == x - 2 && j == y + 1) || (i == x - 2 && j == y) || (i == x - 2 && j == y - 1) || (i == x - 2 && j == y - 2)) vDep = true;
            return vDep;
        }

        // Verifie l'accessibilite d'une case avant de jouer une copie ou un deplacement

        static bool caseAcces(char[,] m, int x, int y)
        {
            return (m[x, y] == VIDE);
        }

        // verifie si toutes les cases sont pleines pour marquer la fin du jeu

        static bool estTermineJeu(char[,] m)
        {
            int i, j;
            int cpt = 0;
            for (i = 0; i < N ; i++)
            {
                for (j = 0; j < N; j++)
                {
                    if (m[i, j] == VIDE) cpt++;
                }
            }
            return (cpt == 0);
        }

        // Verifie si l'utilisateur a entre un nombre entre 1 et 7 

        static bool veriChoix(int x, int y)
        {
            bool veri = true;
            if ((x < 0 || x >= N || y < 0 || y >= N)) veri = false;
            return veri;
        }

        // Contamine les cases Adverses

        static void contaminer(char[,] m, int x, int y)
        {
            char val = m[x, y];
            if (x > 0)
            {
                if (y > 0)
                {
                    if (m[x - 1, y - 1] != VIDE) m[x - 1, y - 1] = val;
                }
                if(m[x-1, y] != VIDE) m[x-1, y]=val;
                if( y < 6)
                {
                    if(m[x-1, y+1] != VIDE) m[x-1, y+1]=val;
                }
            }

            if(y > 0)
            {
                if(m[x, y-1] != VIDE) m[x, y-1]=val;
            }
            if( y < 6)
            {
                if(m[x, y+1] != VIDE) m[x, y+1]=val;
            }
            if(x < 6)
            {
                if(y > 0)
                {
                    if(m[x+1, y-1] != VIDE) m[x+1, y-1]=val;
                }
                if(m[x+1, y] != VIDE) m[x+1, y]=val;
                if( y < 6)
                {
                    if(m[x+1, y+1] != VIDE) m[x+1, y+1]=val;
                }
            }
        }
        
        // Pour choisir les coordonnees de depart

        static void selectionPositionDepart(char[,] m, out int xInitial, out int yInitial, int tour)
        {
            xInitial = 0;
            yInitial = 0;
            if (tour%2==0)
            {
                do 
                {
                    Console.WriteLine("COORDONNES DEPART" + Environment.NewLine);
                    Console.WriteLine("Donner la ligne et la colonne de depart:");
                    saisie(out xInitial);
                    saisie(out yInitial);
                }while(m[xInitial-1, yInitial-1]!= CROIX);
            }
            if (tour%2!=0) // La variable tour permet de passer d'un joueur à l'autre
            {
                do 
                {
                    Console.WriteLine("COORDONNES DEPART" + Environment.NewLine);
                    Console.Write("Donner la ligne : ");
                    saisie(out xInitial);
                    Console.Write("Donner la colonne : ");
                    saisie(out yInitial);
                }while(m[xInitial-1, yInitial-1] != ROND);
            }
            while (((veriChoix(xInitial-1, yInitial-1))==false) || (caseAcces(m, xInitial-1, yInitial-1)==true))
            {
                Console.WriteLine("\nVous avez fait une mauvaise saisie, reesayez" + Environment.NewLine);
                Console.WriteLine("COORDONNES DEPART" + Environment.NewLine);
                Console.Write("Donner la ligne : ");
                saisie(out xInitial);
                Console.Write("Donner la colonne : ");
                saisie(out yInitial);
            }
            (xInitial)--;
            (yInitial)--;
        }

        // Pour choisir les coordonnees d'arrivee

        static void selectionPositionFinal(char[,] m, out int xFinal, out int yFinal)
        {
            Console.WriteLine("COORDONNES ARRIVEE" + Environment.NewLine);
            Console.Write("Donner la ligne : ");
            saisie(out xFinal);
            Console.Write("Donner la colonne : ");
            saisie(out yFinal);
            while (((veriChoix(xFinal-1, yFinal-1))==false) || (caseAcces(m, xFinal-1, yFinal-1)==false))
            {
                Console.WriteLine(Environment.NewLine + "Vous avez fait une mauvaise saisie, reesayez" + Environment.NewLine);
                Console.WriteLine("COORDONNES ARRIVEE" + Environment.NewLine);
                Console.Write("Donner la ligne : ");
                saisie(out xFinal);
                Console.Write("Donner la colonne : ");
                saisie(out yFinal);
            }
            (xFinal)--; // On décrémente parce que le tableau commence à l'indice 0
            (yFinal)--;
        }

        // Pour effectuer une copie par la Machine

        static void copier(char[,] m, int i, int j, int x, int y)
        {
            m[x, y]=m[i, j];
            contaminer(m, x, y);
            afficher(m);
        }

        // Pour effectuer un déplacement par la Machine

        static void deplacer(char[,] m, int i, int j, int x, int y)
        {
            m[x, y] = m[i, j];
            m[i, j] = VIDE;
            contaminer(m, x, y);
            afficher(m);
        }

        // Copie d'un pion par l'utilisateur

        static void jouerCopie(char[,] m, int tour)
        {

            int i, j, x, y;
            selectionPositionDepart(m, out i, out j, tour);
            selectionPositionFinal(m, out x, out y);
            while (!((x - 1 <= i) && (x <= i + 1) && (y - 1 <= j) && (y <= j + 1)))
            {
                Console.Write(Environment.NewLine + "Coordonnees non valides pour une copie, reesayez" + Environment.NewLine);
                selectionPositionFinal(m, out x, out y);
            }
            copier(m, i, j, x, y);
        }

        // Afin que l'ordinateur effectue son jeu en choisissant les coordonees de depart et d'arrivee

        static void jouerHasard(char[,] m, int tour)
        {
	        int i,j,k,l;
            int x, y;
            char couleur;
            int[] I = new int[7];
            int[] J = new int[7];
            int taille = 0;

            if(!peutJouer(m,tour)) return;

            if(tour%2 == 0)
            {
                couleur = CROIX;
            }
            else
            {
                couleur = ROND;
            }

            for(i = 0; i<N; i++ )
            {
 	            for(j = 0; j<N ; j++ )
                {
   		            if(peutCopier(m, i, j, out x, out y,couleur) || peutDeplacer(m, i, j, out x, out y,couleur))
                    {
      	                I[taille] = i; 
                        J[taille]=j;
        	            taille++;
                    }
                }
            }

            // Selection au hasard

            k = new Random().Next(0, 2) + 1; // un nombre entre 1:copie et 2:deplacement
            l = new Random().Next(0, 500) % taille; // indice du pion pris au hasard parmi les pions pouvant jouer
            i = I[l];
            j = J[l];
            if(k == 1)
            {// k==1:copie
  	            if(peutCopier(m, i, j, out x, out y,couleur))
                {
    	            copier(m, i, j, x, y);
		        }
                else 
                {
                    peutDeplacer(m, i, j, out x, out y,couleur);
                    deplacer(m, i, j, x, y);
                }
            }
            else
            { // k==2:deplacement
  	            if(peutDeplacer(m, i, j, out x, out y,couleur))
                {
    	            deplacer(m, i, j, x, y);
		        }
                else 
                {
                    peutCopier(m, i, j, out x, out y,couleur);
                    copier(m,i,j,x,y);
                }
            }

        }

        // Deplacement d'un pion par l'utilisateur

        static void jouerDeplacement(char[,] m, int tour)
        {
            int i, j, x, y;
            selectionPositionDepart(m, out i, out j, tour);
            selectionPositionFinal(m, out x, out y);
            while (veriDeplacer(i, j, x, y) == false)
            {
                Console.WriteLine("\nCoordonnees non valides pour un deplacement, reesayez" + Environment.NewLine);
                selectionPositionFinal(m, out x, out y);
            }
            m[x, y] = m[i, j];
            m[i, j] = VIDE;
            contaminer(m, x, y);
            afficher(m);
        }

        // Pour choisir une action: Copie ou deplacer

        static void typeJeu(char[,] m, int tour)
        {
            int k;
            Console.WriteLine("Votre choix, Tapez: " + Environment.NewLine);
            Console.WriteLine("1 pour  Copier");
            Console.WriteLine("2 pour Deplacer");
            saisie(out k);
            while (!(k == 1 || k == 2))
            {
                Console.WriteLine("Faite le bon choix, Tapez");
                Console.WriteLine("1 pour  Copier");
                Console.WriteLine("2 pour Deplacer");
                saisie(out k);
            }
            if (k == 1)
            {
                jouerCopie(m, tour);
            }
            else
            {
                jouerDeplacement(m, tour);
            }
        }

        // Teste si a partir un pion selectionner au depart si on peut le copier

        static bool peutCopier(char[,] m, int i, int j, out int x, out int y, char couleur)
        {
            bool ok = false;
            if (m[i, j] == couleur)
            {

                if (veriChoix(i + 1, j + 1) && m[i + 1, j + 1] == VIDE)
                {
                    x = i + 1;
                    y = j + 1;
                    return true;
                }
                if (veriChoix(i - 1, j - 1) && m[i - 1, j - 1] == VIDE)
                {
                    x = i - 1;
                    y = j - 1;
                    return true;
                }
                if (veriChoix(i, j + 1) && m[i, j + 1] == VIDE)
                {
                    x = i;
                    y = j + 1;
                    return true;
                }
                if (veriChoix(i + 1, j) && m[i + 1, j] == VIDE)
                {
                    x = i + 1;
                    y = j;
                    return true;
                }
                if (veriChoix(i, j - 1) && m[i, j - 1] == VIDE)
                {
                    x = i;
                    y = j - 1;
                    return true;
                }
                if (veriChoix(i - 1, j) && m[i - 1, j] == VIDE)
                {
                    x = i - 1;
                    y = j;
                    return true;
                }
                if (veriChoix(i + 1, j - 1) && m[i + 1, j - 1] == VIDE)
                {
                    x = i + 1;
                    y = j - 1;
                    return true;
                }
                if (veriChoix(i - 1, j + 1) && m[i - 1, j + 1] == VIDE)
                {
                    x = i - 1;
                    y = j + 1;
                    return true;
                }

            }
            x = 1;
            y = 1;
            return ok;
        }

        // Teste si a partir d'un pion selectionner si on peut le deplacer

        static bool peutDeplacer(char[,] m, int i, int j, out int x, out int y, char couleur)
        {
            if (m[i, j] == couleur)
            {
                if (veriChoix(i + 2, j + 2) && m[i + 2, j + 2] == VIDE)
                {
                    x = i + 2;
                    y = j + 2;
                    return true;
                }
                if (veriChoix(i - 2, j - 2) && m[i - 2, j - 2] == VIDE)
                {
                    x = i - 2;
                    y = j - 2;
                    return true;
                }
                if (veriChoix(i - 2, j + 2) && m[i - 2, j + 2] == VIDE)
                {
                    x = i - 2;
                    y = j + 2;
                    return true;
                }
                if (veriChoix(i + 2, j - 2) && m[i + 2, j - 2] == VIDE)
                {
                    x = i + 2;
                    y = j - 2;
                    return true;
                }
                if (veriChoix(i, j - 2) && m[i, j - 2] == VIDE)
                {

                    x = i;
                    y = j - 2;
                    return true;
                }
                if (veriChoix(i - 2, j) && m[i - 2, j] == VIDE)
                {
                    x = i - 2;
                    y = j;
                    return true;
                }
                if (veriChoix(i + 2, j) && m[i + 2, j] == VIDE)
                {
                    x = i + 2;
                    y = j;
                    return true;
                }
                if (veriChoix(i, j + 2) && m[i, j + 2] == VIDE)
                {
                    x = i;
                    y = j + 2;
                    return true;
                }
                if (veriChoix(i - 1, j - 2) && m[i - 1, j - 2] == VIDE)
                {
                    x = i - 1;
                    y = j - 2;
                    return true;
                }
                if (veriChoix(i - 1, j + 2) && m[i - 1, j + 2] == VIDE)
                {
                    x = i - 1;
                    y = j + 2;
                    return true;
                }
                if (veriChoix(i + 1, j - 2) && m[i + 1, j - 2] == VIDE)
                {
                    x = i + 1;
                    y = j - 2;
                    return true;
                }
                if (veriChoix(i + 1, j + 2) && m[i + 1, j + 2] == VIDE)
                {
                    x = i + 1;
                    y = j + 2;
                    return true;
                }
                if (veriChoix(i - 2, j - 1) && m[i - 2, j - 1] == VIDE)
                {
                    x = i - 2;
                    y = j - 1;
                    return true;
                }
                if (veriChoix(i - 2, j + 1) && m[i - 2, j + 1] == VIDE)
                {
                    x = i - 2;
                    y = j + 1;
                    return true;
                }
                if (veriChoix(i + 2, j - 1) && m[i + 2, j - 1] == VIDE)
                {
                    x = i + 2;
                    y = j - 1;
                    return true;
                }
                if (veriChoix(i + 2, j + 1) && m[i + 2, j + 1] == VIDE)
                {
                    x = i + 2;
                    y = j + 1;
                    return true;
                }
            }
            x = 1;
            y = 1;
            return false;
        }

        /* Teste si un joueur a un pion libre et qui peut soit être copie
        soit être deplacer dans le tableau de jeu avant de lui donner
        l'autorisation de jouer sinon il perd */

        static bool peutJouer(char[,] m, int tour)
        {

            int i, j, x, y;
            char couleur;
            if (tour % 2 == 0)
            {
                couleur = CROIX;
            }
            else
            {
                couleur = ROND;
            }
            for (i = 0; i < N; i++)
            {
                for (j = 0; j < N; j++)
                {
                    // verification qu'une copie est possible
                    if (peutCopier(m, i, j, out x, out y, couleur))
                        return true;
                    // verification qu'un deplacement est possible
                    if (peutDeplacer(m, i, j, out x, out y, couleur))
                        return true;
                }
            }
            return false;

        }

        /* Attribue le reste des cases au joueur adverse si celui dont le tour est arrive ne peut plus jouer.
        parcequ'il ne peut effectuer aucune action ou que tous ces pions sont contaminées */
        /* C'est aussi une autre condition pour mettre fin à la partie */
        /* Au cas ou la partie fini plus tot */

        static void attribuer(char[,] m, int tour)
        {
            int i, j;
            char couleur;
            if (tour % 2 == 0)
            {
                couleur = CROIX;
            }
            else
            {
                couleur = ROND;
            }
            for (i = 0; i < N; i++)
            {
                for (j = 0; j < N; j++)
                {
                    if (m[i, j] == VIDE)
                    {
                        m[i, j] = couleur;
                    }
                }
            }
        }

        /* Pour jouer a 2 en choisissant le mode jusqu'a la fin de la partie */
        /* Contre la Machine: Jouer contre jouer: Machine vs Machine */

        static void chacunSonTourDeJeu(char[,] m)
        {
            int tour;
            tour = 0;
            int k;

            Console.WriteLine("Faite Votre Choix:");
            Console.WriteLine("taper 1 pour jouer à deux");
            Console.WriteLine("taper 2 pour Jouer contre l'Ordinateur");
            Console.WriteLine("taper 3 Pour Machine Vs machine");
            Console.Write("votre choix: ");
            saisie(out k);
            while (!(k == 1 || k == 2 || k == 3))
            {
                Console.Write("choisissez un jeu: ");
                saisie(out k);
            }
            while (!estTermineJeu(m))
            {
                if (k == 3)
                {
                    jouerHasard(m, tour);
                    tour++;
                }
                if (k == 2)
                {
                    if (tour % 2 == 0)
                    {
                        if (peutJouer(m, tour))
                        {
                            jouerHasard(m, tour);
                        }
                        else
                        {
                            attribuer(m, 1);
                        }
                    }
                    else
                    {
                        if (peutJouer(m, tour))
                        {
                            Console.WriteLine("Player c'est a vous de jouer");
                            typeJeu(m, tour);
                        }
                        else
                        {
                            attribuer(m, 2);
                        }
                    }
                    tour++;
                }
                if (k == 1)
                {
                    if (tour % 2 == 0)
                    {
                        if (peutJouer(m, tour))
                        {
                            Console.WriteLine("Joueur 1 c'est a vous de jouer");
                            typeJeu(m, tour);
                        }
                        else
                        {
                            attribuer(m, 1);
                        }
                    }
                    else
                    {
                        if (peutJouer(m, tour))
                        {
                            Console.WriteLine("Joueur 2 c'est a vous de jouer");
                            typeJeu(m, tour);
                        }
                        else
                        {
                            attribuer(m, 2);
                        }
                    }
                    tour++;
                }
            }
        }


        // Calcul le nombre de croix et de rond a la fin du jeu pour determiner le gagnant

        static void calculScore(char[,] m, out int score1, out int score2)
        {
            int i, j;
            score1 = 0;
            score2 = 0;
            i = 0;
            while (i < N)
            {
                j = 0;
                while (j < N)
                {
                    if (m[i, j] == CROIX)
                    {
                        score1 = score1 + 1;
                    }
                    if (m[i, j] == ROND)
                    {
                        score2 = score2 + 1;
                    }
                    j++;
                }
                i++;
            }
        }

        // Afficher le gagnant avec son nombre de pion et celui de son adversaire de pion

        static void afficheGagnant(int score1, int score2)
        {
            if (score1 > score2)
            {
                Console.WriteLine("Le gagnant est le joueur1 avec " + score1 + " de pions contre %d de pions pour le joueur2");
            }
            else if (score1 < score2)
            {
                Console.Write("Le gagnant est le joueur2 avec " + score2 + " de pions contre " + score2 + " de pions pour le joueur1");
            }
            else
                Console.WriteLine("Match nul !");
        }   

    }
}