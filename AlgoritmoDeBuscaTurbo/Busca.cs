using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmoDeBuscaTurbo
{
    class Busca
    {
        public int profMax;
        public int testados;
        public int _inversoes;

        public int _largAux = 0;

        public bool achouMeta = false;

        public Nodo inicial;
        public Nodo meta;
        public Nodo _current;

        public List<Nodo> aberto = new List<Nodo> { };
        public List<Nodo> fechado = new List<Nodo> { };
        public List<Nodo> solucao = new List<Nodo> { };

        public Busca(Nodo _ini, Nodo _meta, int pMax)
        {
            inicial = _ini;
            meta = _meta;
            profMax = pMax;
        }
        public void buscaLargura()
        {
            //1. Verifica se o problema é solúvel
            if (!isSoluvel(inicial))
            {
                Console.WriteLine("->INSOLÚVEL\n-->INVERSÕES: " + _inversoes);
                return;
            }

            Console.WriteLine("->SOLÚVEL\n-->INVERSÕES: " + _inversoes + "\n");

            _current = inicial;

            aberto.Add(_current);

            while(aberto.Count > 0 && !achouMeta)
            {
                Console.WriteLine("Profundidade atual: " + _current.prof);
                if(isMeta(_current))
                {
                    break;
                }

                if(aberto.Count > 0)
                {
                    expande(_current);
                }

                aberto.Remove(aberto.First());

                if (aberto.Count > 0)
                    _current = aberto.First();
            }

            //9. Se houver resultado é adicionado a cadeia de "pais" a uma pilha
            while (_current != null && achouMeta)
            {
                solucao.Add(_current);
                _current = _current._parent;
            }

            //10. Termina
            //Console.WriteLine("->SOLÚVEL\n-->INVERSÕES: " + _inversoes + "\n");


        }
        public void buscaProfundidade()
        {
            Console.WriteLine("Buscando profundidade máxima: " + profMax);


            //1. Verifica se o problema é solúvel
            if(!isSoluvel(inicial))
            {
                Console.WriteLine("->INSOLÚVEL\n-->INVERSÕES: " + _inversoes);
                return;
            }

            //2. Define o estado atual
            _current = inicial;

            //3. Adiciona o estado atual na pilha
            aberto.Add(_current);

            //4. Enquanto tiverem estados abertos ou a meta não for encontrada
            while(aberto.Count > 0 && !achouMeta)
            {    
                //5. Checa se o estado é meta                  
                if(isMeta(_current))
                {
                    break;
                }

                //6. Se tiverem estados abertos, remove o ultimo
                if (aberto.Count > 0)
                    aberto.Remove(aberto.Last());
         
                //7. Se a profundidade máxima não foi alcançada: expande o nó
                if(_current.prof < profMax)
                {
                    expande(_current);
                }

                //8. Se tiverem estados abertos seta o topo da pilha como estado atual
                if(aberto.Count > 0)
                {
                    _current = aberto.Last();
                }
            }

            if(achouMeta == false && profMax < 30)
            {
                profMax++;
                _current = inicial;
                aberto.Clear();
                solucao.Clear();
                fechado.Clear();
                buscaProfundidade();
            }

            //9. Se houver resultado é adicionado a cadeia de "pais" a uma pilha
            while(_current != null && achouMeta)
            {
                solucao.Add(_current);
                _current = _current._parent;
            }

            //10. Termina
            
        }
        public bool isSoluvel(Nodo n)
        {
            List<int> curr = new List<int> { };
            int invers = 0;

            /**
			 * Posiciona os valores da matriz em um vetor temporário
			 * */
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    curr.Add(n._state[i, j]);
                }
            }


            //Conta o numero de inversoes
            for (int i = 0; i < curr.Count; i++)
            {
                if (curr[i] != 0)
                {
                    for (int j = i; j < curr.Count; j++)
                    {
                        if (curr[j] != 0)
                        {
                            if (curr[i] > curr[j])
                            {
                                invers++;
                            }
                        }
                    }
                }
            }

            _inversoes = invers;

            //Checa se é PAR ou IMPAR
            if (invers % 2 == 0)
            {
                return true;
            }

            return false;
        }
        public bool isMeta(Nodo n)
        {
            testados++;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (n._state[i, j] != meta._state[i, j])
                    {
                        return false;
                    }
                }
            }

            achouMeta = true;
            return true;
        }
        public bool expande(Nodo curr)
        {
            bool _achou = false;

            Nodo _novoUp = new Nodo(curr._state, curr);
            Nodo _novoDown = new Nodo(curr._state, curr);
            Nodo _novoRight = new Nodo(curr._state, curr);
            Nodo _novoLeft = new Nodo(curr._state, curr);

            //Branco move pra cima
            if (_novoUp.moveUp())
            {
                if (equalPrevious(_novoUp, curr) == false)
                {
                    _achou = true;
                    aberto.Add(_novoUp);
                }
            }

            //Branco move pra baixo
            if (_novoDown.moveDown())
            {
                if (equalPrevious(_novoDown, curr) == false)
                {
                    _achou = true;
                    aberto.Add(_novoDown);
                }
            }

            //Branco move para direita
            if (_novoRight.moveRight())
            {
                if (equalPrevious(_novoRight, curr) == false)
                {
                    _achou = true;
                    aberto.Add(_novoRight);
                }
            }

            //Branco move para a esquerda
            if (_novoLeft.moveLeft())
            {
                if (equalPrevious(_novoLeft, curr) == false)
                {
                    _achou = true;
                    aberto.Add(_novoLeft);
                }
            }
            return _achou;
        }
        /*
		 * Checa se o estado novo é igual ao estado anterior
		 * */
        public bool equalPrevious(Nodo n, Nodo curr)
        {
            if (curr._parent == null)
            {
                return false;
            }

            Nodo _tmp = curr._parent;
            bool result = true;
            while (_tmp != null)
            {
                result = true;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (n._state[i, j] != curr._parent._state[i, j])
                        {
                            result = false;
                        }
                    }
                }

                _tmp = _tmp._parent;

            }

            return result;
        }
    }
}
