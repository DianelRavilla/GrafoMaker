using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoGrafos
{
    class NodoDoble
    {
        //Declarando Autopropiedades
        public string Nombre { set; get; }
        public int posX { set; get; }
        public int posY { set; get; }
        public string color { set; get; }
        public int tamaño { set; get; }
        public int grosor { set; get; }
        public int peso { set; get; }
        public bool dirigido { set; get; }
        public NodoDoble Anterior { set; get; }
        public NodoDoble VerticeAntecesor { set; get; }
        public NodoDoble VerticeAdyacente { set; get; }
        public NodoDoble Siguiente { set; get; }    
        
        public NodoDoble()      ////Constructor
        {
            Nombre = null;
            posX = 0;
            posY = 0;
            color = "";
            tamaño = 0;
            grosor = 0;
            peso = 0;
            Anterior = null;
            VerticeAdyacente = null;
            VerticeAntecesor = null;
            Siguiente = null;
        }
    }
}
