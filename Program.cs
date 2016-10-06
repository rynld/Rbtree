using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RBTREE
{
    class Program
    {
        static void Main(string[] args)
        {
            RedBlackTree tree = new RedBlackTree();
            int limite = 10000;
            int[] ar = new int[] { 8, 5, 14, 5, 13, 5, 9, 6, 11, 5 };
            //for (int i = 0; i < ar.Length; i++)
            //{
            //    tree.Insert(ar[i]);

            //}
            //IEnumerable<int> lista = tree.Lowers(8);
            //foreach (var item in lista)
            //{
            //    Console.WriteLine(item);
            //    Console.WriteLine();
            //}
            //Console.ReadLine();

            Stopwatch crono = new Stopwatch();

            crono.Start();
            for (int i = 0; i < 1000000; i++)
            {
                tree.Insert(i);
            }
            crono.Stop();
            Console.WriteLine(crono.ElapsedMilliseconds);
            Console.ReadLine();
            crono.Reset();
            crono.Start();
            for (int i = 0; i < 1000000; i++)
            {
                tree.Delete(i);
            }
            crono.Stop();
            Console.WriteLine(crono.ElapsedMilliseconds);
            Console.ReadLine();
            //try
            //{
            //    ar = new int[limite];
            //    while (true)
            //    {

            //        Random r = new Random();
            //        for (int i = 0; i < ar.Length; i++)
            //        {
            //            ar[i] = r.Next(1, 15);
            //        }
            //        for (int i = 0; i < ar.Length; i++)
            //        {
            //            tree.Insert(ar[i]);

            //        }
            //        //tree.Sum(7, 7);
            //        Correctness(tree);
            //        for (int i = 0; i < ar.Length; i++)
            //        {
            //            if (r.Next(10000) % 3 == 0)
            //            {
            //                tree.Delete(ar[i]);
            //                Correctness(tree);
            //            }
            //        }

            //    }
            //}
            //catch (Exception)
            //{
            //    int[] x = ar;
            //    throw;
            //}






        }

        static void Correctness(RedBlackTree tree)
        {
            IEnumerable<int> nodos = tree.root.EntreOrden();
            List<int> lista = new List<int>();
            foreach (var item in nodos)
            {
                lista.Add(item);
            }


            if (!IsCorrect(tree, lista))//Correctitud
            {
            }

            //Predecesor y sucesor
            for (int i = 0; i < lista.Count; i++)
            {
                int pre = tree.Predecessor(lista[i]);
                int suc = tree.Succesor(lista[i]);
                if (!PredCorrect(tree.root, lista[i], pre, lista) || !SucCorrect(tree.root, lista[i], suc, lista))
                {
                }
            }

            //Sum en un intervalo
            Random r = new Random();
            for (int i = 0; i < 10000; i++)
            {
                int a;
                int b;
                try
                {
                    a = r.Next(lista[0], lista[lista.Count - 1]);
                    b = r.Next(lista[0], lista[lista.Count - 1]);
                    if (a <= b)
                    {
                        int sum = tree.Sum(a, b);
                        if (!SumInterval(tree.root, a, b, sum, lista))
                        {
                        }
                    }

                    int pred = tree.Predecessor(a);
                    int pred1 = tree.Predecessor(b);
                    if (!PredCorrect(tree.root, a, pred, lista) || !PredCorrect(tree.root, b, pred1, lista))
                    {
                    }

                    int suc = tree.Succesor(a);
                    int suc1 = tree.Succesor(b);
                    if (!SucCorrect(tree.root, a, suc, lista) || !SucCorrect(tree.root, b, suc1, lista))
                    {
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        static bool IsSort(RedBlackTree tree, List<int> lista)
        {
            for (int i = 0; i < lista.Count - 1; i++)
            {
                if (lista[i] > lista[i + 1]) return false;
            }
            return true;

        }

        static bool RedProperty(Node tree)
        {
            bool l = true; bool r = true;
            if (tree.ColorBlack == false && tree.Left.ColorBlack == false || tree.ColorBlack == false && tree.Right.ColorBlack == false) return false;
            if (!tree.Left.Sentinel)
                l = RedProperty(tree.Left);

            if (!tree.Right.Sentinel)
                r = RedProperty(tree.Right);
            return l && r;
        }

        static bool HeightProperty(Node tree)
        {
            return true;
        }

        static bool IsCorrect(RedBlackTree tree, List<int> lista)
        {
            if (IsSort(tree, lista) && RedProperty(tree.root)) return true;
            return false;
        }

        static bool PredCorrect(Node tree, int current, int pred, List<int> lista)
        {
            if (pred == int.MaxValue || pred == int.MinValue) return true;

            int best = int.MinValue / 2;
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i] > current) break;
                if (Math.Abs(current - lista[i]) < Math.Abs(current - best) && lista[i] != current)
                    best = lista[i];
            }

            return best == pred;
        }

        static bool SucCorrect(Node tree, int current, int pred, List<int> lista)
        {
            if (pred == int.MaxValue || pred == int.MinValue) return true;

            int best = int.MinValue / 2;
            for (int i = lista.Count - 1; i >= 0; i--)
            {
                if (lista[i] < current) break;
                if (Math.Abs(current - lista[i]) < Math.Abs(best - current) && lista[i] != current)
                    best = lista[i];
            }

            return best == pred;
        }

        static bool SumInterval(Node tree, int a, int b, int res, List<int> lista)
        {
            int sum = 0;
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i] >= a && lista[i] <= b) sum += lista[i];
            }
            return sum == res;

        }
    }

    public class RedBlackTree
    {
        public Node root;

        public Node nil;

        public RedBlackTree()
        {
            nil = new Node(int.MinValue, true) { Sentinel = true, Cant = 0, Rep = 0 };
        }

        public RedBlackTree(int value)
        {
            root = new Node(value);
        }

        public void Insert(int value)
        {
            if (root == null)
                root = new Node(value, nil, nil, true);
            else
                Inserta(value);

        }
        #region//Insertion

        void Inserta(int node)
        {
            Node x = root;
            Node padre = root;

            while (!x.Sentinel)
            {
                x.Cant++;
                x.Sum += node;

                padre = x;

                if (node < x.Value)
                    x = x.Left;
                else if (node > x.Value)
                    x = x.Right;
                else
                { x.Rep++; return; }
            }

            Node nuevo = new Node(node, nil, padre);

            if (nuevo.Value > padre.Value)
                padre.Right = nuevo;
            else
                padre.Left = nuevo;

            FixedUp(nuevo);
        }

        void FixedUp(Node z)
        {
            while (!z.Father.ColorBlack)
            {
                if (z.Father == z.Father.Father.Left)
                {
                    Node y = z.Father.Father.Right;
                    if (!y.ColorBlack)
                    {
                        y.ColorBlack = z.Father.ColorBlack = true;
                        z.Father.Father.ColorBlack = false;
                        z = z.Father.Father;
                    }
                    else
                    {
                        if (z == z.Father.Right)
                        {
                            z = z.Father;
                            LeftRotate(z);
                        }
                        z.Father.ColorBlack = true;
                        z.Father.Father.ColorBlack = false;
                        RightRotate(z.Father.Father);
                    }
                }
                else
                {
                    Node y = z.Father.Father.Left;
                    if (!y.ColorBlack)
                    {
                        y.ColorBlack = z.Father.ColorBlack = true;
                        z.Father.Father.ColorBlack = false;
                        z = z.Father.Father;
                    }
                    else
                    {
                        if (z == z.Father.Left)
                        {
                            z = z.Father;
                            RightRotate(z);
                        }
                        z.Father.ColorBlack = true;
                        z.Father.Father.ColorBlack = false;
                        LeftRotate(z.Father.Father);
                    }
                }
            }

            root.ColorBlack = true;
        }

        void LeftRotate(Node x)
        {
            x.Sum += x.Right.Left.Sum - x.Right.Sum;//Modify the sum of the trees
            x.Right.Sum += x.Sum - x.Right.Left.Sum;

            x.Cant += x.Right.Left.Cant - x.Right.Cant;//Modify the cant of elements of the trees
            x.Right.Cant += x.Cant - x.Right.Left.Cant;

            Node y = x.Right;
            x.Right = y.Left;

            if (!y.Left.Sentinel)
                y.Left.Father = x;

            y.Father = x.Father;

            if (x.Father.Sentinel)
                root = y;
            else if (x.Value == x.Father.Left.Value)
                x.Father.Left = y;
            else
                x.Father.Right = y;

            y.Left = x;
            x.Father = y;
        }

        public void RightRotate(Node x)
        {
            x.Sum += x.Left.Right.Sum - x.Left.Sum;//Modify the sum of the trees
            x.Left.Sum += x.Sum - x.Left.Right.Sum;

            x.Cant += x.Left.Right.Cant - x.Left.Cant;//Modify the cant of elements of the trees
            x.Left.Cant += x.Cant - x.Left.Right.Cant;

            Node y = x.Left;
            x.Left = y.Right;

            if (!y.Right.Sentinel)
                y.Right.Father = x;

            y.Father = x.Father;

            if (x.Father.Sentinel)
                root = y;
            else if (x.Value == x.Father.Right.Value)
                x.Father.Right = y;
            else
                x.Father.Left = y;

            y.Right = x;
            x.Father = y;

        }

        #endregion

        public bool Delete(int numero)
        {
            Node x = root;

            if (Find(numero) == null) return false;

            while (!x.Sentinel)
            {
                x.Cant--;
                x.Sum -= numero;

                if (x.Value < numero)
                    x = x.Right;
                else if (x.Value > numero)
                    x = x.Left;
                else
                {
                    if (x.Rep > 1)
                        x.Rep--;
                    else
                        Delete(x);
                    return true;
                }
            }
            return true;
        }
        #region//Delete Arreglar
        void Delete(Node z)//FALTA ARREGLAR LO DE LA SUMA Y LA CANTIDAD DE ELEMENTOS
        {
            Node y = z;
            Node x;
            bool original = y.ColorBlack;

            if (z.Left.Sentinel && z.Right.Sentinel)
            {
                x = z.Left;
                Transplant(z, x);
            }

            else if (z.Left.Sentinel && !z.Right.Sentinel)
            {
                x = z.Right;
                Transplant(z, z.Right);
            }
            else if (z.Right.Sentinel && !z.Left.Sentinel)
            {
                x = z.Left;
                Transplant(z, z.Left);
            }
            else
            {
                y = Minimum(z.Right);
                DeleteMinimum(y.Father, z, y.Value);

                original = y.ColorBlack;
                x = y.Right;
                if (y.Father == z)
                    x.Father = y;
                else
                {
                    Transplant(y, y.Right);
                    y.Right = z.Right;
                    y.Right.Father = y;
                }
                Transplant(z, y);
                y.Left = z.Left;
                y.Left.Father = y;
                y.ColorBlack = z.ColorBlack;
            }

            if (original)
                DeleteFixedUp(x);


        }

        void DeleteFixedUp(Node x)
        {
            Node w;
            while (x != root && x.ColorBlack)
            {
                if (x == x.Father.Left)
                {
                    w = x.Father.Right;
                    if (!w.ColorBlack)
                    {
                        w.ColorBlack = true;
                        x.Father.ColorBlack = false;
                        LeftRotate(x.Father);
                        w = x.Father.Right;
                    }
                    if (w.Left.ColorBlack && w.Right.ColorBlack)
                    {
                        w.ColorBlack = false;
                        x = x.Father;
                    }
                    else
                    {
                        if (w.Right.ColorBlack)
                        {
                            w.Left.ColorBlack = true;
                            w.ColorBlack = false;
                            RightRotate(w);
                            w = x.Father.Right;
                        }

                        w.ColorBlack = x.Father.ColorBlack;
                        x.Father.ColorBlack = true;
                        w.Right.ColorBlack = true;
                        LeftRotate(x.Father);
                        x = root;
                    }
                }
                else
                {
                    w = x.Father.Left;
                    if (!w.ColorBlack)
                    {
                        w.ColorBlack = true;
                        x.Father.ColorBlack = false;
                        RightRotate(x.Father);
                        w = x.Father.Left;
                    }
                    if (w.Right.ColorBlack && w.Left.ColorBlack)
                    {
                        w.ColorBlack = false;
                        x = x.Father;
                    }
                    else
                    {
                        if (w.Left.ColorBlack)
                        {
                            w.Right.ColorBlack = true;
                            w.ColorBlack = false;
                            LeftRotate(w);
                            w = x.Father.Left;
                        }

                        w.ColorBlack = x.Father.ColorBlack;
                        w.Left.ColorBlack = x.Father.ColorBlack = true;
                        RightRotate(x.Father);
                        x = root;
                    }
                }
            }
            x.ColorBlack = true;
        }

        void Transplant(Node u, Node v)
        {
            if (u.Father.Sentinel)
                root = v;
            else if (u == u.Father.Left)
                u.Father.Left = v;
            else
                u.Father.Right = v;

            v.Father = u.Father;
        }

        void DeleteMinimum(Node begin, Node stop, int value)
        {
            while (begin != stop)
            {
                begin.Sum -= value;
                begin = begin.Father;
            }

        }
        #endregion


        /// <summary>
        /// Determines if an element belongs to the tree
        /// </summary>
        /// <param name="nodo">The element to find</param>
        /// <returns></returns>
        public bool Contains(int nodo)
        {
            var aux = Find(nodo);
            if (aux == null)
                return false;

            return true;
        }
        Node Find(int nodo)
        {
            var raiz = root;

            while (!raiz.Sentinel)
            {
                if (raiz.Value.CompareTo(nodo) < 0)
                    raiz = raiz.Right;
                else if (raiz.Value.CompareTo(nodo) > 0)
                    raiz = raiz.Left;
                else
                    return raiz;
            }

            return null;
        }

        Node Minimum(Node node)
        {
            if (node.Sentinel) return node;
            if (!node.Left.Sentinel)
                return Minimum(node.Left);
            else
                return node;

        }

        Node Maximum(Node node)
        {
            if (node.Sentinel) return node;
            if (!node.Right.Sentinel)
                return Maximum(node.Right);
            else
                return node;

        }

        /// <summary>
        /// Return the greater of the smaller elements.
        /// </summary>
        /// <param name="value">The pivot element</param>
        /// <returns></returns>
        public int Predecessor(int value)
        {
            int res = Pred(value).Value;
            if (res == int.MinValue) throw new InvalidOperationException();

            return res;
        }
        /// <summary>
        /// Return the node that is the predeccesor if this value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Node Pred(int value)
        {
            var x = this.root;
            Node closest = new Node();
            closest.Value = int.MinValue;

            while (!x.Sentinel)
            {
                if (x.Value >= value)
                    x = x.Left;
                else
                {
                    if (x.Value > closest.Value && x.Value < value)
                        closest = x;
                    x = closest.Right;
                }
            }
            return closest;

        }


        /// <summary>
        /// Return the smaller of the greaters elements.
        /// </summary>
        /// <param name="value">The pivot element</param>
        /// <returns></returns>
        public int Succesor(int value)
        {
            int res = Suc(value).Value;
            if (res == int.MinValue) throw new InvalidOperationException();
            return res;
        }
        /// <summary>
        /// Return the node that is the successor of this value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Node Suc(int value)
        {
            var x = this.root;
            Node closest = new Node();

            while (!x.Sentinel)
            {
                if (x.Value <= value)
                    x = x.Right;
                else
                {
                    if (x.Value < closest.Value && x.Value > value)
                        closest = x;
                    x = closest.Left;
                }
            }

            return closest;
        }

        /// <summary>
        /// Return the sum in a interval
        /// </summary>
        /// <param name="a">The begin of the interval</param>
        /// <param name="b">The end of the interval</param>
        /// <returns></returns>
        public int Sum(int a, int b)
        {
            int total = 0;

            var aux = this.root;

            while (aux != null && (aux.Value > b || aux.Value < a))
            {
                if (aux.Value > b)
                    aux = aux.Left;
                else if (aux.Value < a)
                    aux = aux.Right;
            }
            if (aux == null) return 0;
            total += aux.Value;
            //In this position in aux is the lca

            var path_izq = aux.Left;

            while (!path_izq.Sentinel)//Down for the left path
            {
                if (path_izq.Value < a)
                    path_izq = path_izq.Right;
                else
                {
                    if (!path_izq.Right.Sentinel)
                        total += path_izq.Right.Sum;
                    total += path_izq.Value;

                    if (path_izq.Value == a) break;

                    path_izq = path_izq.Left;
                }
            }

            var path_right = aux.Right;

            while (!path_right.Sentinel)//Down for the right path
            {
                if (path_right.Value > b)
                    path_right = path_right.Left;
                else
                {
                    if (!path_right.Left.Sentinel)
                        total += path_right.Left.Sum;

                    total += path_right.Value;

                    if (path_right.Value == a) break;

                    path_right = path_right.Right;
                }
            }

            return total;
        }

        /// <summary>
        /// Return the cant of elements in a interval
        /// </summary>
        /// <param name="a">The begin of the interval</param>
        /// <param name="b">The end of the interval</param>
        /// <returns></returns>
        public int Cant(int a, int b)
        {
            int total = 0;

            var aux = this.root;

            while (aux != null && (aux.Value > b || aux.Value < a))
            {
                if (aux.Value > b)
                    aux = aux.Left;
                else if (aux.Value < a)
                    aux = aux.Right;
            }
            if (aux == null) return 0;
            total++;
            //In this position in aux is the lca

            var path_izq = aux.Left;

            while (!path_izq.Sentinel)//Down for the left path
            {
                if (path_izq.Value < a)
                    path_izq = path_izq.Right;
                else
                {
                    if (!path_izq.Right.Sentinel)
                        total += path_izq.Right.Cant;
                    total++;

                    if (path_izq.Value == a) break;

                    path_izq = path_izq.Left;
                }
            }

            var path_right = aux.Right;

            while (!path_right.Sentinel)//Down for the right path
            {
                if (path_right.Value > b)
                    path_right = path_right.Left;
                else
                {
                    if (!path_right.Left.Sentinel)
                        total += path_right.Left.Cant;

                    total++;

                    if (path_right.Value == a) break;

                    path_right = path_right.Right;
                }
            }

            return total;
        }

        public IEnumerable<int> Lowers(int value)
        {
            var x = this.root;

            while (!x.Sentinel)
            {
                if (x.Value <= value)
                {
                    if (!x.Left.Sentinel)
                        foreach (var item in x.Left.EntreOrden())
                            yield return item;
                    if (x.Value == value)
                        break;
                    else
                        yield return x.Value;
                    x = x.Right;
                }
                else
                    x = x.Left;
            }
        }

        public IEnumerable<int> Greaters(int value)
        {
            var x = this.root;

            while (!x.Sentinel)
            {
                if (x.Value <= value)
                {
                    if (!x.Right.Sentinel)
                        foreach (var item in x.Right.EntreOrden())
                            yield return item;
                    if (x.Value == value)
                        break;
                    else
                        yield return x.Value;
                    x = x.Left;
                }
                else
                    x = x.Right;
            }
        }

        public int this[int pos]
        {
            get
            {
                if (pos < 0 || pos >= root.Cant) throw new ArgumentOutOfRangeException();

                var r = this.root;
                while (!r.Sentinel)
                {

                    if (pos >= r.Left.Cant && pos < r.Left.Cant + r.Rep)
                        return r.Value;

                    if (pos > r.Left.Cant)
                    {
                        pos -= (r.Left.Cant + r.Rep);
                        r = r.Right;
                    }
                    else
                        r = r.Left;
                }
                return 0;
            }
        }
    }

    public class Node
    {

        public bool ColorBlack { get; set; }
        public Node Father { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public int Value { get; set; }

        /// <summary>
        /// Gets or set the total of elements of this tree
        /// </summary>
        public int Cant { get; set; }

        /// <summary>
        /// Gets or set the sum of the elements of this tree
        /// </summary>
        public int Sum { get; set; }

        /// <summary>
        /// Gets or set if this node is a sentinel
        /// </summary>
        public bool Sentinel { get; set; }

        /// <summary>
        /// Gets or set how many elements are repited
        /// </summary>
        public int Rep { get; set; }

        /// <summary>
        /// Gets or set the distence from the root.
        /// </summary>
        public int Level { get; set; }

        public Node(int value, bool color)
        {
            this.ColorBlack = color;
            this.Value = this.Sum = value;
            this.Rep = this.Cant = 1;
        }

        public Node(int value)
        {
            this.Value = this.Sum = value;
        }

        public Node(int value, Node sen, Node pa)
        {
            this.Value = this.Sum = value;
            this.Left = this.Right = sen;
            this.Father = pa;
        }

        public Node(int value, Node sen, Node pa, bool color)
        {
            this.Value = this.Sum = value;
            this.Left = this.Right = sen;
            this.Father = pa;
            this.ColorBlack = color;
        }

        public Node()
        {
            this.Rep = this.Cant = 1;
        }

        public IEnumerable<int> EntreOrden()
        {
            if (Left != null && !Left.Sentinel)
                foreach (var item in Left.EntreOrden())
                    yield return item;

            yield return Value;

            if (Right != null && !Right.Sentinel)
                foreach (var item in Right.EntreOrden())
                    yield return item;
        }
    }

    public enum Colores
    { Black, Red }
}

