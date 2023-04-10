using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace INF1008_tp1
{
    public class Labyrinthe
    {
        List<Node> nodes;
        List<Arc> arcs;

        public Labyrinthe(int x, int y)
        {
            nodes = new();
            arcs = new();
            CreateLabyrinthe(x, y);
            Complexity.Stocker = Complexity.Counter;
            Complexity.Counter = 0;
            RandomArcs(x, y);
        }

        private void CreateLabyrinthe(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    nodes.Add(new Node(i, j));
                    Complexity.Counter++;
                }
            }
        }
        private void RandomArcs(int x, int y)
        {
            int pointer = 0;
            Random ran = new Random();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Node n = nodes[pointer];
                    if (j < y - 1)
                    {
                        n.AddRelate(nodes[pointer + 1], ran.Next(1, 11));
                        Complexity.Counter++;
                    }
                    if (i < x - 1)
                    {
                        n.AddRelate(nodes[pointer + y], ran.Next(1, 11));
                        Complexity.Counter++;
                    }
                    if (j > 0)
                    {
                        Complexity.Counter++;
                        n.AddRelate(nodes[pointer - 1], 0);
                    }
                    if (i > 0)
                    {
                        n.AddRelate(nodes[pointer - y], 0);
                        Complexity.Counter++;
                    }
                    nodes[pointer] = n;
                    pointer++;
                }
            }
            foreach (Node n in nodes)
            {
                foreach (Arc a in n.Relates)
                {
                    if (!arcs.Contains(a))
                    {
                        arcs.Add(a);
                        Complexity.Counter++;
                    }
                }
            }
        }

        public bool IsLegal()
        {
            return nodes.Count > 0;
        }

        public List<Arc> Prim()
        {
            List<Node> nodeparsed = new() { nodes[0] };//Déjà passé le noeud initial (0, 0)
            List<Node> nodeleft = nodes.Skip(1).ToList();//A l'exception du noeud initial (0, 0), les autres noeuds n'ont pas encore passé
            List<Arc> arcparsed = new();//Toutes les arêtes ne sont pas encore passées
            //Classer toutes les arêtes adjacentes aux noeuds qui sont déjà passés dans l'ordre croissant de leur poids.
            List<Arc> arcadj = nodes[0].Relates.OrderBy(n => n.Weight).ToList();//la méthode OrderBy utilise un algorithme de tri rapide O(n*logn)
            while (nodeleft.Count > 0)
            {
                arcparsed.Add(arcadj[0]);//Sélectionner la plus petite arête adjacente O(1)
                nodeparsed.Add(arcparsed.Last().Relates.Single(n => nodeleft.Contains(n)));//Passer le noeud adjacent à l'arête O(n)
                nodeleft.Remove(nodeparsed.Last());//Le noeud a été passé O(n)
                //Ajouter les arêtes adjacentes du noeud à la liste des arêtes adjacentes par ordre.
                foreach (Arc a in nodeparsed.Last().Relates)
                {//Nombre constant de cycles avec un maximum de 4
                    var index = arcadj.BinarySearch(a);//O(logn)
                    if (index < 0) index = ~index;
                    arcadj.Insert(index, a);//O(n)
                }
                //Supprimer toutes les arêtes qui rejoignent des noeuds qui ont déjà été passés.
                arcadj.RemoveAll(n => nodeparsed.Contains(n.Relates[0]) && nodeparsed.Contains(n.Relates[1]));//O(n)
            }
            return arcparsed;
        }

        public void Affiche()
        {
            int pointer = 0;
            int x = nodes.Max(n => n.X) + 1;
            int y = nodes.Max(n => n.Y) + 1;
            for (int i = 0; i < x; i++)//O(n)
            {
                string stringbuffer = "";
                for (int j = 0; j < y; j++)//O(n)*O(n)
                {
                    stringbuffer += String.Format("{0,4},{1,-4:N0}", nodes[pointer].X, nodes[pointer].Y);
                    Complexity.Counter++;
                    if (j < y - 1)
                    {
                        int w = nodes[pointer].Relates.First(n => n.Orien.Equals(true)).Weight;
                        stringbuffer += String.Format("{2,2}---{0,3}{1,2}---{3,2}", w, "", "", "");//O(n^2)
                        Complexity.Counter++;
                    }
                    pointer++;
                }
                Console.WriteLine(stringbuffer);

                if (i < x - 1)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        stringbuffer = "";
                        string w = "|";
                        if (k == 0 || k == 2 || k == 4 || k == 6) w = "";
                        for (int j = 0; j < y; j++)//O(n)*O(n)
                        {
                            if (k == 3) w = nodes[pointer + j].Relates.Last(n => n.Orien.Equals(false)).Weight.ToString();
                            stringbuffer += String.Format("{0,5}{1,4}", w, "");
                            Complexity.Counter++;
                            if (j < y - 1)
                            {
                                stringbuffer += String.Format("{0,8}{1,7}", "", "");
                                Complexity.Counter++;
                            }
                        }

                        Console.WriteLine(stringbuffer);
                    }
                }
            }
            Console.WriteLine("\r\nLe labyrinthe a été généré avec succès");
        }

        public override string ToString()
        {
            string result = "";
            int pointer = 0;
            List<Arc> arcparsed = Prim();
            int x = nodes.Max(n => n.X) + 1;
            int y = nodes.Max(n => n.Y) + 1;
            for (int i = 0; i < x; i++)
            {
                string stringbuffer = "";
                for (int j = 0; j < y; j++)
                {
                    stringbuffer += String.Format("{0,4},{1,-4:N0}", nodes[pointer].X, nodes[pointer].Y);
                    Complexity.Counter++;
                    if (j < y - 1)
                    {
                        Arc hor = nodes[pointer].Relates.First(n => n.Orien.Equals(true));
                        if (arcparsed.Contains(hor))
                        {
                            stringbuffer += String.Format("-----{0,3}{1,2}-----", hor.Weight, "");
                            Complexity.Counter++;
                        }
                        else
                        {
                            stringbuffer += String.Format("{0,8}{1,7}", hor.Weight, "");
                            Complexity.Counter++;
                        }

                    }
                    pointer++;
                }
                result += "\r\n" + stringbuffer;

                if (i < x - 1)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        stringbuffer = "";
                        for (int j = 0; j < y; j++)
                        {
                            Arc ver = nodes[pointer + j].Relates.Last(n => n.Orien.Equals(false));
                            string w = "|";
                            if (!arcparsed.Contains(ver))
                            {
                                w = "";
                            }
                            if (k == 3) w = ver.Weight.ToString();
                            stringbuffer += String.Format("{0,5}{1,4}", w, "");
                            Complexity.Counter++;
                            if (j < y - 1)
                            {
                                stringbuffer += String.Format("{0,8}{1,7}", "", "");
                                Complexity.Counter++;
                            }
                        }

                        result += "\r\n" + stringbuffer;
                    }
                }
            }
            return result;
        }
    }
}
