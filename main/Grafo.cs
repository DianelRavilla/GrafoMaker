using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoGrafos
{
    class Grafo
    {
        private NodoDoble vPrimero = new NodoDoble();                           //TDA vertice incial
        private NodoDoble vUltimo = new NodoDoble();                            //TDA vertice final
        private NodoDoble aPrimero = new NodoDoble();                           //TDA arista incial
        private NodoDoble aUltimo = new NodoDoble();                            //TDA arista final
        private int nv;                                                         //Número de Vertices
        private int na;                                                         //Número de Aristas

        public Grafo()                                                                      //Constructor
        {
            Anula();
        }                                       
        public void Anula()                                                                 //Anula los vertices y aristas
        {
            vPrimero = null;
            vUltimo = null;
            aPrimero = null;
            aUltimo = null;
            nv = 0;
            na = 0;
        }
        public bool esVacio()                                                               //Devuelve TRUE si no hay vertices ni aristas
        {
            return (vPrimero == null && aPrimero == null);
        }
        public bool hayVertice()                                                            //Devuelve TRUE si no hay vertices
        {
            return (nv != 0);
        }
        public bool hayArista()                                                             //Devuelve TRUE si no hay aristas
        {
            return (na != 0);
        }
        public int Vertices()                                                               //Devuleve la cantidad de vertices
        {
            return nv;
        }
        public int Aristas()                                                                //Devuelve la cantidad de Aristas
        {
            return na;
        }
        public List<string> AdyacentesV(NodoDoble v)                                        //Devuelve una lista con la adyacencia de un nodo
        {
            List<string> lista = new List<string>();
            NodoDoble q = LocalizaVertice(v.Nombre);
            while (q != null)
            {
                q = q.VerticeAdyacente;
                if (q != null)
                    lista.Add(q.Nombre);
            }
            return lista;
        }
        public int Adyacentes(NodoDoble v)                                                  //Devuelve la cantidad de vertices adyacentes dado un vertice 
        {
            int n = 0;
            NodoDoble q = LocalizaVertice(v.Nombre);
            while (q != null)
            {
                q = q.VerticeAdyacente;
                n++;
            }
            return n;
        }
        public int Antecesores(NodoDoble v)                                                 //Devuelve la cantidad de vertices antecesores dado un vertice
        {
            int n = 0;
            NodoDoble q = LocalizaVertice(v.Nombre);
            while (q != null)
            {
                q = q.VerticeAntecesor;
                n++;
            }
            return n;
        }
        public void mostrarAntecesores(NodoDoble v, ListBox lbxMostrar)                     //Devuelve los vertices adyacentes dado un vertice
        {           
            int n = 0;
            NodoDoble q = LocalizaVertice(v.Nombre);
            string cadena = "";
            while (q != null)
            {
                if (n==0)
                    cadena = cadena + "||" + q.Nombre + "|| <-- ";
                else
                    cadena = cadena + q.Nombre + " <-- ";
                q = q.VerticeAdyacente;
                n++;
            }
            cadena = cadena + "null";
            lbxMostrar.Items.Add(cadena);
        } 
        public void mostrarAdyacentes(NodoDoble v, ListBox lbxMostrar)                      //Devuelve la cantidad de vertices antecesores dado un vertice
        {
            int n = 0;
            NodoDoble q = LocalizaVertice(v.Nombre);
            string cadena = "";
            while (q != null)
            {
                if (n == 0)
                    cadena = cadena + "||" + q.Nombre + "|| --> ";
                else
                    cadena = cadena + q.Nombre + " --> ";
                q = q.VerticeAntecesor;
                n++;
            }
            cadena = cadena + "null";
            lbxMostrar.Items.Add(cadena);
        }
        public bool LocalizaAdyacente(string vertice, string nombre)                        //Localiza dado un nombre de vertice
        {
            NodoDoble Act = LocalizaVertice(vertice);
            int n = 0;
            while (Act != null)
            {
                if (n == 0) { }
                else
                {
                    if (Act.Nombre.Equals(nombre))
                        return true;
                }         
                Act = Act.VerticeAntecesor;
                n++;
            }
            return false;
        }
        public void insertarVertice(NodoDoble v)                                            //Inserta un vertice dado su nombre
        {
            if (existeVertice(v))
                MessageBox.Show("Ya existe un vertice con el mismo nombre", "VERIFICACION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //INSERTANDO VERTICE EN LA LISTA VERTICES
                NodoDoble nuevo = v;
                if (vPrimero == null)
                {
                    vPrimero = nuevo;
                    vPrimero.Siguiente = null;
                    vPrimero.Anterior = null;
                    vUltimo = vPrimero;
                }
                else
                {
                    vUltimo.Siguiente = nuevo;
                    nuevo.Siguiente = null;
                    nuevo.Anterior = vUltimo;
                    vUltimo = nuevo;
                }
                nv++;
            }
        }
        public void insertarAristaDirigida(NodoDoble arista, NodoDoble a, NodoDoble s)      //Inserta una arista Dirigida dado 2 vertices
        {
            if (existeArista(arista))
            {
                MessageBox.Show("Ya existe esta arista", "VERIFICACION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);            
            }
            else
            {
                NodoDoble A = LocalizaVertice(s.Nombre);
                NodoDoble nuevoS = new NodoDoble();
                nuevoS.Nombre = a.Nombre;
                if (Adyacentes(A) == 0)
                    A.VerticeAdyacente = nuevoS;
                else
                {
                    nuevoS.VerticeAdyacente = A.VerticeAdyacente;
                    A.VerticeAdyacente = nuevoS;
                }

                NodoDoble S = LocalizaVertice(a.Nombre);
                NodoDoble nuevoA = new NodoDoble();
                nuevoA.Nombre = s.Nombre;
                if (Antecesores(S) == 0)
                    S.VerticeAntecesor = nuevoA;
                else
                {
                    nuevoA.VerticeAntecesor = S.VerticeAntecesor;
                    S.VerticeAntecesor = nuevoA;
                }

                //INSERTANDO ARISTA EN LA LISTA ARISTAS
                NodoDoble nuevo = new NodoDoble();
                nuevo = arista;
                if (aPrimero == null)
                {
                    aPrimero = nuevo;
                    aPrimero.Siguiente = null;
                    aPrimero.Anterior = null;
                    aUltimo = aPrimero;
                }
                else
                {
                    aUltimo.Siguiente = nuevo;
                    nuevo.Siguiente = null;
                    nuevo.Anterior = aUltimo;
                    aUltimo = nuevo;
                }
                na++;
            }           
        }
        public void insertarAristaNoDirigida(NodoDoble arista, NodoDoble a, NodoDoble s)    //Inserta una arista No Dirigida dado 2 vertices
        {
            if (existeArista(arista))
            {
                MessageBox.Show("Ya existe esta arista", "VERIFICACION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                NodoDoble A = LocalizaVertice(s.Nombre);
                NodoDoble nuevoA = new NodoDoble();
                nuevoA.Nombre = a.Nombre;
                if (Adyacentes(A) == 0)
                {
                    A.VerticeAdyacente = nuevoA;
                    A.VerticeAntecesor = nuevoA;
                }
                else
                {
                    nuevoA.VerticeAdyacente = A.VerticeAdyacente;
                    A.VerticeAdyacente = nuevoA;
                    nuevoA.VerticeAntecesor = A.VerticeAntecesor;
                    A.VerticeAntecesor = nuevoA;
                }

                NodoDoble S = LocalizaVertice(a.Nombre);
                NodoDoble nuevoS = new NodoDoble();
                nuevoS.Nombre = s.Nombre;
                if (Antecesores(S) == 0)
                {
                    S.VerticeAntecesor = nuevoS;
                    S.VerticeAdyacente = nuevoS;
                }
                else
                {
                    nuevoS.VerticeAntecesor = S.VerticeAntecesor;
                    S.VerticeAntecesor = nuevoS;
                    nuevoS.VerticeAdyacente = S.VerticeAdyacente;
                    S.VerticeAdyacente = nuevoS;
                }

                //INSERTANDO ARISTA EN LA LISTA ARISTAS
                NodoDoble nuevo = new NodoDoble();
                nuevo = arista;
                if (aPrimero == null)
                {
                    aPrimero = nuevo;
                    aPrimero.Siguiente = null;
                    aPrimero.Anterior = null;
                    aUltimo = aPrimero;
                }
                else
                {
                    aUltimo.Siguiente = nuevo;
                    nuevo.Siguiente = null;
                    nuevo.Anterior = aUltimo;
                    aUltimo = nuevo;
                }
                na++;
            }
        }
        public NodoDoble LocalizaVertice(string nombre)                                     //Localiza dado un nombre de vertice
        {
            NodoDoble Act = vPrimero;
            while (Act != null)
            {
                if (Act.Nombre.Equals(nombre))
                    return Act;
                Act = Act.Siguiente;
            }
            return null;
        }      
        public NodoDoble LocalizaArista(string nombre)                                      //Localiza dado un nombre de Arista
        {
            NodoDoble Act = aPrimero;
            while (Act != null)
            {
                if (Act.Nombre.Equals(nombre))
                    return Act;
                Act = Act.Siguiente;
            }
            return null;
        }
        public void eliminarVertice(NodoDoble v)                                            //Elimina un vertice dado su nombre
        {         
            NodoDoble actual = new NodoDoble();
            NodoDoble anterior = new NodoDoble();
            anterior = null;
            actual = vPrimero;
            bool encontrado = false;
            while (actual != null && encontrado == false)
            {
                if (actual.Nombre == v.Nombre)
                {
                    if (actual == vPrimero)
                    {
                        vPrimero = vPrimero.Siguiente;
                    }
                    else if (actual == vUltimo)
                    {
                        anterior.Siguiente = null;
                        vUltimo = anterior;
                    }
                    else
                    {
                        anterior.Siguiente = actual.Siguiente;
                        actual.Siguiente.Anterior = anterior;
                    }
                    encontrado = true;
                }
                anterior = actual;
                actual = actual.Siguiente;
            }
            if (encontrado == true)
                nv--;
        }
        public void eliminarArista(NodoDoble a)                                             //Elimina una arista dado dos vertices
        {
            NodoDoble actual = new NodoDoble();
            NodoDoble anterior = new NodoDoble();
            anterior = null;
            actual = aPrimero;
            bool encontrado = false;
            while (actual != null && encontrado == false)
            {
                if (actual.Nombre == a.Nombre)
                {
                    if (actual == aPrimero)
                    {
                        aPrimero = aPrimero.Siguiente;
                    }
                    else if (actual == aUltimo)
                    {
                        anterior.Siguiente = null;
                        aUltimo = anterior;
                    }
                    else
                    {
                        anterior.Siguiente = actual.Siguiente;
                        actual.Siguiente.Anterior = anterior;
                    }
                    encontrado = true;
                }
                anterior = actual;
                actual = actual.Siguiente;
                if (encontrado == true)
                    na--;
                }
            }      
        public bool existeVertice(NodoDoble v)                                              //Retorna TRUE si existe un vertice
        {
            NodoDoble buscado = LocalizaVertice(v.Nombre);
            if (buscado != null)
                return true;
            else
                return false;
        }               
        public NodoDoble existeAristaYVertices(NodoDoble ant, NodoDoble ady)                //Devuelve la arista si existe dado 2 vertices
        {
            string[] buscados = GetAristas();
            for (int i = 0; i < na; i++)
            {
                NodoDoble actual = LocalizaArista(buscados[i]);
                if (ant.Nombre == actual.VerticeAntecesor.Nombre && ady.Nombre == actual.VerticeAdyacente.Nombre)
                    return actual;
                else if (ady.Nombre == actual.VerticeAntecesor.Nombre && ant.Nombre == actual.VerticeAdyacente.Nombre)
                    return actual;              
            }
            return null; 
        }
        public bool existeArista(NodoDoble a)                                               //Retorna TRUE si exista una arista
        {
            NodoDoble buscado = LocalizaArista(a.Nombre);
            if (buscado != null)
            {
                return true;
            }
            else
                return false;
        }
        public void mostrarVertices(ListBox lbxComponentes)                                 //Muestra los vertices de la lista
        {
            if (hayVertice() == true)
            {
                lbxComponentes.Items.Clear();
                NodoDoble q = vPrimero;
                lbxComponentes.Items.Add("VERTICES:");
                while (q != null)
                {
                    lbxComponentes.Items.Add("- Vertice: " + q.Nombre + "\t█Pos(x): " + q.posX + "\t█Pos(y): " + q.posY);
                    q = q.Siguiente;
                }
            }
        }       
        public void mostrarAristas(ListBox lbxComponentes)                                  //Muestra las aristas de la lista
        {
            if (hayArista() == true)
            {
                lbxComponentes.Items.Clear();
                NodoDoble q = aPrimero;
                lbxComponentes.Items.Add("ARISTAS:");
                while (q != null)
                {
                    if (q.dirigido == true)
                        lbxComponentes.Items.Add("- Arista: " + q.Nombre + "\t█{" + q.VerticeAntecesor.Nombre + "," + q.VerticeAdyacente.Nombre + "}" + " Dirigido" + "\t\t█Peso{" + q.peso + "}");
                    else
                        lbxComponentes.Items.Add("- Arista: " + q.Nombre + "\t█{" + q.VerticeAntecesor.Nombre + "," + q.VerticeAdyacente.Nombre + "}" + " No Dirigido" + "\t█Peso{" + q.peso + "}");
                    q = q.Siguiente;
                }
            }
        }        
        public string[] GetVertices()                                                       //Devuelve el nombre de todos los vertices en un vector tipo string
        {
            string[] vecNombres = new string[nv];
            NodoDoble Actual = vPrimero;
            for(int i = 0; i < nv; i++)
            {
                vecNombres[i] = Actual.Nombre;
                Actual = Actual.Siguiente;
            }
            return vecNombres;
        }
        public string[] GetAristas()                                                        //Devuelve el nombre de todos las aristas en un vector tipo string
        {
            string[] vecNombres = new string[na];
            NodoDoble Actual = aPrimero;
            for (int i = 0; i < na; i++)
            {
                vecNombres[i] = Actual.Nombre;
                Actual = Actual.Siguiente;
            }
            return vecNombres;
        }

        public void BFS(ListBox lbx)
        {
            lbx.Items.Clear();
            List<string> listaVisitados = new List<string>();

            NodoDoble reco = new NodoDoble();

            reco = LocalizaVertice(vPrimero.Nombre);
            while (reco != null)
            {
                if (!listaVisitados.Contains(reco.Nombre))
                {                   
                    listaVisitados.Add(reco.Nombre);
                    lbx.Items.Add(reco.Nombre);                    
                }
                List<string> Ad = AdyacentesV(reco);
                for(int i = 0; i < Ad.Count; i++)
                {
                    if (!listaVisitados.Contains(Ad[i]))
                    {
                        listaVisitados.Add(Ad[i]);
                        lbx.Items.Add(Ad[i]);
                    }
                }            
                reco = reco.Siguiente;
            } 
        }

        public void DFS(ListBox lista)
        {
            lista.Items.Clear();
            List<string> listaVisitadosDFS = new List<string>();
            DFS(lista, listaVisitadosDFS, vPrimero);
        }
        private void DFS(ListBox lista, List<string>  lv, NodoDoble nodo)
        {
            if (nodo != null)
            {
                lv.Add(nodo.Nombre);
                lista.Items.Add(nodo.Nombre);

                List<string> Ad = AdyacentesV(nodo);
                for (int i = 0; i < Ad.Count; i++)
                {
                    if(!lv.Contains(Ad[i]))
                        DFS(lista, lv, LocalizaVertice(Ad[i]));
                }
            }
        }

        //public void Dijkastra(string nombre, string objetivo)
        //{
        //    NodoDoble nuev = new NodoDoble();
        //    nuev.Nombre = nombre;
        //    if(existeVertice(nuev))
        //    {
        //        DijkastraRecursivo(nuev, objetivo, 0);
        //    }
        //    else { MessageBox.Show("El vertice no existe", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        //}

        public int posicionVertice(string nombre)             //Retorna la posicion dado un nombre de vertice
        {
            int n = 0;
            NodoDoble Act = vPrimero;
            while (Act != null)
            {
                if (Act.Nombre.Equals(nombre))
                    return n;
                Act = Act.Siguiente;
                n++;
            }
            return -1;
        }
        public NodoDoble VerticeSegunPosicion(int n)          //Retorna el vertice dado una posicion
        {
            int contador = 0;
            NodoDoble Act = vPrimero;
            while (contador != n)
            {
                Act = Act.Siguiente;
                contador++;
            }
            return Act;
        }

        public void Dijkastra(string A, string B, ListBox L, TextBox T)
        {
            int inicio = posicionVertice(A);
            int final = posicionVertice(B);
            int distancia = 0;
            int n = 0;
            int cantNodos = nv;
            int actual = 0;
            int columna = 0;

            // CREANDO TABLA
            // 0 - Visitado
            // 1 - Distancia
            // 2 - Previo
            int[,] tabla = new int[cantNodos, 3];

            // Inicialzando la tabla
            for (n = 0; n < cantNodos; n++)
            {
                tabla[n, 0] = 0;
                tabla[n, 1] = int.MaxValue;
                tabla[n, 2] = 0;
            }
            tabla[inicio, 1] = 0;

            // Inicio Dijkastra
            actual = inicio;

            do
            {
                // Marcar nodo como visitado
                tabla[actual, 0] = 1;
                for (columna = 0; columna < cantNodos; columna++)
                {
                    
                    // Buscamos a quien se dirige
                    if (existeAristaYVertices(VerticeSegunPosicion(actual), VerticeSegunPosicion(columna)) != null)
                    {
                        // Calculamos la distancia
                        distancia = existeAristaYVertices(VerticeSegunPosicion(actual), VerticeSegunPosicion(columna)).peso + tabla[actual, 1];

                        // Colocamos las distancias
                        if (distancia < tabla[columna, 1])
                        {
                            tabla[columna, 1] = distancia;

                            // Colocamos la informacion del padre
                            tabla[columna, 2] = actual; 
                        }
                    }
                }

                // El nuevo actual es el nodo con la menor distancia que no ah sido visitado
                int indiceMenor = -1;
                int distanciaMenor = int.MaxValue;

                for (int x = 0; x < cantNodos; x++)
                {
                    if (tabla[x, 1] < distanciaMenor && tabla[x, 0] == 0)
                    {
                        indiceMenor = x;
                        distanciaMenor = tabla[x, 1];
                    }
                }                
                actual = indiceMenor;                 

            } while (actual != -1);

            // Obtenemos la ruta
            List<int> ruta = new List<int>();
            int nodo = final;

            while (nodo != inicio)
            {
                ruta.Add(nodo);
                nodo = tabla[nodo, 2];
            }
            ruta.Add(inicio);

            ruta.Reverse();

            int sumaDistancias = tabla[ruta.ElementAt(ruta.Count() - 1), 1];

            if (sumaDistancias == 0 || sumaDistancias == int.MaxValue)
            {
                L.Items.Add("No existe ruta disponible");
            }
            else 
            {
                foreach (int posicion in ruta)
                {
                    L.Items.Add("{" + VerticeSegunPosicion(posicion).Nombre + "}");
                    L.Items.Add("  ↓↓  ");                    
                }
                T.Text = Convert.ToString(sumaDistancias);
            }
        }
    }
}
