//INF1008-TP1-Main.cs
//Jiadong Jin JINJ86100000
using INF1008_tp1;

bool menu_continue = true;
Labyrinthe l = new(0, 0);
while (menu_continue)
{
    Console.WriteLine("\r\nMenu");
    Console.WriteLine("Veuillez entrer un numéro pour sélectionner la fonction que vous voulez utiliser:");
    Console.WriteLine("(1) Générer un labyrinthe d'une taille spécifiée");
    Console.WriteLine("(2) Utiliser l'algorithme de Prim pour résoudre le labyrinthe.");
    Console.WriteLine("(3) Quitter l'application.");
    switch (Console.ReadKey().KeyChar)
    {
        case '1':
            l = new(0, 0);
            Console.WriteLine("\r\nVeuillez entrer le nombre de colonnes du labyrinthe.:");
            if (!int.TryParse(Console.ReadLine(), out int y))
            {
                Console.WriteLine("Caractère non valide.");
                break;
            }
            Console.WriteLine("\r\nVeuillez entrer le nombre de lignes du labyrinthe.:");
            if (!int.TryParse(Console.ReadLine(), out int x))
            {
                Console.WriteLine("Caractère non valide.");
                break;
            }
            //Initialisation un compteur
            //pour compter le nombre d'opérations de base utilisées dans la génération des labyrinthes et des listes d'adjacence.
            Complexity.Counter = 0;
            //Générer un labyrinthe d'une taille donnée à partir du nombre de lignes et de colonnes saisies par l'utilisateur.
            l = new Labyrinthe(x, y);
            Console.WriteLine("Le nombre d'opérations élémentaires (l'initialisation de la liste d'adjacence):" + Complexity.Stocker);
            Complexity.Stocker = 0;
            Console.WriteLine("Le nombre d'opérations élémentaires (la génération du labyrinthe):" + Complexity.Counter + "\r\n");
            Complexity.Counter = 0;
            if (l.IsLegal())
            {
                //Afficher le labyrinthe terminé et le nombre des opérations de base.
                l.Affiche();
                Console.WriteLine("Le nombre d'opérations élémentaires (l'affichage du labyrinthe):" + Complexity.Counter);
                Complexity.Counter = 0;
            }
            else
            {
                Console.WriteLine("Nombre non valide.");
            }
            break;
        case '2':
            if (l.IsLegal())
            {
                //Utilisation de l'algorithme prim pour résoudre des labyrinthes et afficher le nombre des opérations de base
                Console.WriteLine("\r\nL'arbre couvrant de poids minimal de ce labyrinthe est :");
                Console.WriteLine(l.ToString());
                Console.WriteLine("\r\nLe nombre d'opérations élémentaires (l'affichage du labyrinthe):" + Complexity.Counter);
                Complexity.Counter = 0;
            }
            else
            {
                Console.WriteLine("\r\nLe labyrinthe n'est pas encore initialisé.");
            }
            break;
        case '3':
            menu_continue = false;
            break;
        default:
            Console.WriteLine("\r\n" + "Caractère non valide.\r\n");
            break;
    }
}


