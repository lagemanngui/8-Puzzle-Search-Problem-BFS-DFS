using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmoDeBuscaTurbo
{
    class Program
    {
        public static int[,] inicial = new int[3, 3]{
            {2,1,3 },
            {4,5,0},
            {7,6,8}
        };

        public static int[,] meta = new int[3, 3]{
            {0,1,2},
            {3,4,5},
            {6,7,8}
        };

        static void Main(string[] args)
        {

            Nodo _ini = new Nodo(inicial, null);
            Nodo _met = new Nodo(meta, null);

            Busca busca = new Busca(_ini, _met, 5);
            busca.buscaLargura();

            printArvore(busca.solucao);

            if(!busca.achouMeta)
            {
                Console.WriteLine("Não encontrou o resultado.");
            }

            Console.ReadKey(true);
        }
        static void printArvore(List<Nodo> lista)
        {
            int i = lista.Count-1;
            foreach (Nodo n in lista)
            {
                Console.Write("Estado: " + i + "\n");
                n.mostraValores();
                Console.Write("\n");
                i--;
            }
        }
    }
}
