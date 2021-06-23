using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoGrafos
{  
    public partial class Form1 : Form
    {
        Random random = new Random();
        Validacion v = new Validacion();
        Grafo OBJ = new Grafo();
        public int contadorGlobalV = 0;
        public int contadorGlobalA = 0;

        //Elementos de grafico
        private Bitmap bmp;
        private Pen lapiz;
        private Graphics g;

        public Form1()
        {
            InitializeComponent();
            txtNombreVertice.Text = "V0";
            txtNombreArista.Text = "A0";
            InicializarBitmapYGraficos();
            pnlComponentes.BringToFront();
        }

        //BOTONES_HERRAMIENTAS
        private void BtnComponentes_Click(object sender, EventArgs e)
        {
            pnlComponentes.BringToFront();
        }
        private void BtnRepresentar_Click(object sender, EventArgs e)
        {
            pnlRepresentar.BringToFront();
        }
        private void BtnGraficoAvanzado_Click(object sender, EventArgs e)
        {
            pnlPropiedadesDeGrafico.BringToFront();
        }
        private void btnRecorridos_Click(object sender, EventArgs e)
        {
            pnlRecorridos.BringToFront();
        }

        //BOTONES//
        private void BtnAgregar_Click(object sender, EventArgs e)
        {        
            if (tabComponentes.SelectedTab.Text == "VERTICES")
            {
                try
                {
                    string nombre = txtNombreVertice.Text;
                    nombre = nombre.Trim();
                    int posX;
                    int posY;
                    if (txtPosX.Text == "(Aleatorio)" || txtPosX.Text == "")
                        posX = random.Next(20, 765);
                    else
                        posX = Int32.Parse(txtPosX.Text) + 20;
                    if (txtPosY.Text == "(Aleatorio)" || txtPosY.Text == "")
                        posY = random.Next(20, 615);
                    else
                        posY = Int32.Parse(txtPosY.Text) + 30;
                    string color = cbxColorVertice.Text;
                    int tamaño = Int32.Parse(cbxTamaño.Text);

                    if (nombre != "" && posX <= 765 && posY <= 610 && tamaño >= 10 && tamaño <= 50)
                    {
                        NodoDoble vertice = new NodoDoble();
                        vertice.Nombre = nombre;
                        vertice.posX = posX;
                        vertice.posY = posY;
                        vertice.color = color;
                        vertice.grosor = Convert.ToInt32(GrosorUpDown.Value);
                        vertice.tamaño = tamaño;

                        OBJ.insertarVertice(vertice);
                        dibujarVertice(nombre, color, tamaño, posX, posY);
                        ClearText();
                        if (OBJ.existeVertice(vertice))
                        {
                            contadorGlobalV += 1;
                            txtNombreVertice.Text = "V" + contadorGlobalV;
                        }
                    }
                    else
                    {
                        int n = Convert.ToInt32(cbxColorVertice.Text);     //Error provocado a drede
                    }
                }
                catch
                {
                    MessageBox.Show("GRAFO MAKER: PROPIEDADES DEL VERTICE INVALIDAS:\n" +
                        "\n" +
                        "1.- Los vertices deben tener nombre o etiqueta\n" +
                        "2.- La posición X es un número entero positvo (entre 0 y 765)\n" +
                        "3.- La posición Y es un número entero positivo (entre 0 y 610)\n" +
                        "4.- Se debe haber seleccionado al menos un color\n" +
                        "5.- Se debe haber seleccionado al menos un tamaño, entre 10 y 50", "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (tabComponentes.SelectedTab.Text == "ARISTAS")
            {
                try
                {
                    string nombre = txtNombreArista.Text;
                    nombre = nombre.Trim();
                    string color = cbxColorArista.Text;
                    string conexion1 = cbxConexionVertice1.Text;
                    conexion1 = conexion1.Trim();
                    string conexion2 = cbxConexionVertice2.Text;
                    conexion2 = conexion2.Trim();
                    int peso = Convert.ToInt32(txtPeso.Text);

                    if (nombre != "" && conexion1 != "" && conexion2 != "")
                    {
                        NodoDoble nuevo = new NodoDoble();
                        nuevo.Nombre = txtNombreArista.Text;
                        nuevo.color = color;
                        nuevo.grosor = Convert.ToInt32(GrosorUpDown2.Value);
                        nuevo.peso = Convert.ToInt32(txtPeso.Text);
                        if (chkbxDirigido.Checked == true)
                            nuevo.dirigido = true;
                        else
                            nuevo.dirigido = false;

                        NodoDoble antecesor = OBJ.LocalizaVertice(conexion1);
                        NodoDoble adyacente = OBJ.LocalizaVertice(conexion2);
                        nuevo.VerticeAdyacente = adyacente;
                        nuevo.VerticeAntecesor = antecesor;

                        if (antecesor != null && adyacente != null)
                        {
                            if (OBJ.existeAristaYVertices(antecesor, adyacente) != null)
                            {
                                MessageBox.Show("Ya existe una arista con estos vertices", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Question);
                            }
                            else
                            {     
                                if (conexion1 == conexion2)
                                {
                                    dibujarLazo(nombre, color, antecesor.posX, antecesor.posY);
                                    OBJ.insertarAristaDirigida(nuevo, antecesor, adyacente);
                                }
                                else
                                {
                                    if (nuevo.dirigido == true)
                                    {
                                        OBJ.insertarAristaDirigida(nuevo, antecesor, adyacente);
                                        dibujarAristaDirigida(nombre, peso, color, antecesor.posX, antecesor.posY, adyacente.posX, adyacente.posY);
                                    }
                                    else
                                    {
                                        OBJ.insertarAristaNoDirigida(nuevo, antecesor, adyacente);
                                        dibujarAristaNoDirigida(nombre, peso, color, antecesor.posX, antecesor.posY, adyacente.posX, adyacente.posY);
                                    } 
                                }
                                ClearText();
                                contadorGlobalA += 1;
                                txtNombreArista.Text = "A" + contadorGlobalA;
                            }

                        }
                        else
                        {
                            MessageBox.Show("No se ha encontrado uno de los vertices", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        }
                    }
                    else
                    {
                        int n = Convert.ToInt32(cbxColorVertice.Text);     //Error provocado a drede
                    }
                }
                catch
                {
                    MessageBox.Show("GRAFO MAKER: PROPIEDADES DE LA ARISTA INVALIDAS:\n" +
                        "\n" +
                        "1.- Las aristas deben tener nombre o etiqueta\n" +
                        "2.- Las aristas deben tener peso (por defecto 1)\n" +
                        "3.- El peso de las aristas es de tipo entero\n" +
                        "4.- Se debe haber seleccionado al menos un color\n" +
                        "5.- Se debe colocar el nombre de los vertices que se desean conectar", "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }                              
            }
            BtnActualizarGrafico_Click(sender, e);
            ActualizarDatos();
            RecorrerListBox();
        }
        private void BtnLimpiarPanel_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("Estas Seguro?", "Confirmación de Limpieza", MessageBoxButtons.YesNoCancel);
            if (resultado == DialogResult.Yes)
            {
                contadorGlobalV = 0;
                contadorGlobalA = 0;
                lblAristas.Text = "ARISTAS: " + contadorGlobalA;
                lblVertices.Text = "VERTICES: " + contadorGlobalV;
                txtNombreVertice.Text = "V0";
                txtNombreArista.Text = "A0";
                cbxConexionVertice1.Items.Clear();
                cbxConexionVertice2.Items.Clear();
                lbxComponentesVertices.Items.Clear();
                lbxComponentesAristas.Items.Clear();
                lbxDetallesAristas.Items.Clear();
                lbxDetallesVertices.Items.Clear();
                txtCantidadDeVertices.Text = "V = {}";
                txtCantidadDeAristas.Text = "A = {}";
                lbxListaAdyacencia.Items.Clear();
                lbxListaAntecesor.Items.Clear();
                dgvTabla.Rows.Clear();
                dgvTabla.Columns.Clear();
                OBJ.Anula();
                InicializarBitmapYGraficos();
            }
            else if (resultado == DialogResult.No)
            {
                return;
            }
            else
            {
                return;
            }         
        }
        private void BtnActualizarGrafico_Click(object sender, EventArgs e)
        {
            g.Clear(Color.Transparent);
            int NumeroDeVertices = OBJ.Vertices();
            int NumeroDeAristas = OBJ.Aristas();
            string[] NombresVertices = OBJ.GetVertices();
            string[] NombresAristas = OBJ.GetAristas();

            for (int i = 0; i < NumeroDeVertices; i++)
            {
                NodoDoble actual = OBJ.LocalizaVertice(NombresVertices[i]);
                OBJ.mostrarVertices(lbxComponentesVertices);
                OBJ.mostrarVertices(lbxDetallesVertices);
                dibujarVertice(actual.Nombre, actual.color, actual.tamaño, actual.posX, actual.posY);
            }
            for(int i = 0; i < NumeroDeAristas; i++)
            {
                NodoDoble actual = OBJ.LocalizaArista(NombresAristas[i]);
                OBJ.mostrarVertices(lbxComponentesVertices);
                OBJ.mostrarVertices(lbxDetallesVertices);
                if (OBJ.existeVertice(actual.VerticeAdyacente) && OBJ.existeVertice(actual.VerticeAntecesor))
                {
                    dibujarAristaDirigida(actual.Nombre, actual.peso, actual.color, actual.VerticeAdyacente.posX, actual.VerticeAdyacente.posY, actual.VerticeAntecesor.posX, actual.VerticeAntecesor.posY);
                }
            }
            RecorrerListBox();
        }
        private void btnActualizarGrafico2_Click(object sender, EventArgs e)
        {
            BtnActualizarGrafico_Click(sender, e);
        }
        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            string nombre = cbxEliminarComponente.Text;
            NodoDoble verOari = new NodoDoble();
            verOari.Nombre = nombre;

            if(OBJ.existeVertice(verOari))
            {
                OBJ.eliminarVertice(verOari);
                string[] aristas = OBJ.GetAristas();
                for(int i = OBJ.Aristas() - 1; i >= 0; i--)
                {
                    NodoDoble actual = OBJ.LocalizaArista(aristas[i]);
                    if (actual.VerticeAdyacente.Nombre == nombre || actual.VerticeAntecesor.Nombre == nombre)
                    {
                        OBJ.eliminarArista(actual);                       
                    }
                }
                contadorGlobalV--;
            }
            else if (OBJ.existeArista(verOari))
            {
                OBJ.eliminarArista(verOari);
                contadorGlobalA--;
            }
            else
            {
                MessageBox.Show("Este elemento no existe en el grafo", "ELEMENTO NO ENCONTRADO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ActualizarDatos();
            RecorrerListBox();
            cbxEliminarComponente.Text = "";
            BtnActualizarGrafico_Click(sender, e);
        }
        private void BtnCalcularListaAdyacencia_Click(object sender, EventArgs e)
        {
            lbxListaAdyacencia.Items.Clear();
            lbxListaAntecesor.Items.Clear();
            try
            {
                string[] vertices = OBJ.GetVertices();
                lbxListaAdyacencia.Items.Add("ADYACENTES A:");
                for (int i = 0; i < OBJ.Vertices(); i++)
                {
                    OBJ.mostrarAdyacentes(OBJ.LocalizaVertice(vertices[i]), lbxListaAdyacencia);
                }
                lbxListaAntecesor.Items.Add("ANTECESORES A:");
                for (int i = 0; i < OBJ.Vertices(); i++)
                {
                    OBJ.mostrarAntecesores(OBJ.LocalizaVertice(vertices[i]), lbxListaAntecesor);
                }
            }
            catch
            {
                MessageBox.Show("No existen elementos en el grafo", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnCalcularMatrizAdyacencia_Click(object sender, EventArgs e)
        {
            dgvTabla.Rows.Clear();
            dgvTabla.Columns.Clear();
            string[] NombreDeVertices = OBJ.GetVertices();
            dgvTabla.Columns.Add("VERTIECES", "");
            for (int i = 0; i < OBJ.Vertices(); i++)
            {
                dgvTabla.Columns.Add(NombreDeVertices[i], NombreDeVertices[i]);
                dgvTabla.Rows.Add(NombreDeVertices[i]);
                dgvTabla.Columns[i].Width = 40;
                dgvTabla.Columns[i+1].Width = 40;
            }

            for (int i = 1; i <= OBJ.Vertices(); i++)
            {
                for (int j = 1; j <= OBJ.Vertices(); j++)
                {
                    string vertice1 = dgvTabla.Rows[i-1].Cells[0].Value.ToString();
                    NodoDoble ver = OBJ.LocalizaVertice(vertice1);

                    if (OBJ.LocalizaAdyacente(ver.Nombre, dgvTabla.Columns[j].Name.ToString()) == true)
                    {
                        try
                        { dgvTabla.Rows[i - 1].Cells[j].Value = (OBJ.existeAristaYVertices(OBJ.LocalizaVertice(vertice1), OBJ.LocalizaVertice(dgvTabla.Columns[j].Name.ToString())).peso); }
                        catch { dgvTabla.Rows[i - 1].Cells[j].Value = 0; }
                    }
                    else
                        dgvTabla.Rows[i - 1].Cells[j].Value = "0";
                }
            }
        }
        private void btnMostrarBFS_Click(object sender, EventArgs e)
        {
            try{ OBJ.BFS(ColaBFS); }
            catch { MessageBox.Show("El grafo esta vacio", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void btnMostrarDFS_Click(object sender, EventArgs e)
        {
            try { OBJ.DFS(ColaDFS); }
            catch { MessageBox.Show("El grafo esta vacio", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        //DIBUJANDO//
        private void InicializarBitmapYGraficos()
        {
            bmp = new Bitmap(pnlGrafo.Width, pnlGrafo.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pnlGrafo.BackColor = Color.Black;
            pnlGrafo.Image = bmp;
        }
        private void dibujarVertice(string nombre, string color,int tamaño, int posX, int posY)
        {
            switch (color)
            {
                case "Blanco":
                    lapiz = new Pen(Color.White, (float)GrosorUpDown.Value);
                    break;
                case "Naranja":
                    lapiz = new Pen(Color.Orange, (float)GrosorUpDown.Value);
                    break;
                case "Rojo":
                    lapiz = new Pen(Color.Red, (float)GrosorUpDown.Value);
                    break;
                case "Azul":
                    lapiz = new Pen(Color.Blue, (float)GrosorUpDown.Value);
                    break;
                case "Azul Oscuro":
                    lapiz = new Pen(Color.DarkBlue, (float)GrosorUpDown.Value);
                    break;
                case "Azul Claro":
                    lapiz = new Pen(Color.LightBlue, (float)GrosorUpDown.Value);
                    break;
                case "Cafe":
                    lapiz = new Pen(Color.Brown, (float)GrosorUpDown.Value);
                    break;
                case "Amarillo":
                    lapiz = new Pen(Color.Yellow, (float)GrosorUpDown.Value);
                    break;
                case "Verde":
                    lapiz = new Pen(Color.Green, (float)GrosorUpDown.Value);
                    break;
                case "Verde Oscuro":
                    lapiz = new Pen(Color.DarkGreen, (float)GrosorUpDown.Value);
                    break;
                case "Verde Claro":
                    lapiz = new Pen(Color.LightGreen, (float)GrosorUpDown.Value);
                    break;
                case "Plomo":
                    lapiz = new Pen(Color.Silver, (float)GrosorUpDown.Value);
                    break;
                case "Rosado":
                    lapiz = new Pen(Color.HotPink, (float)GrosorUpDown.Value);
                    break;
                case "Morado":
                    lapiz = new Pen(Color.Purple, (float)GrosorUpDown.Value);
                    break;
            }

            //Escribir nombres
            SolidBrush s = new SolidBrush(Color.White);
            FontFamily ff = new FontFamily("Consolas");
            Font font = new Font(ff, 15);

            g = pnlGrafo.CreateGraphics();
            g.DrawArc(lapiz, new Rectangle(posX, posY, tamaño, tamaño), 0, 360);
            g.DrawString(nombre, font, s, posX - 10, posY - 30);
        }
        private void dibujarAristaDirigida(string nombre, int peso, string color, int xi, int yi, int xf, int yf)
        {
            switch (color)
            {
                case "Blanco":
                    lapiz = new Pen(Color.White, (float)GrosorUpDown2.Value);
                    break;
                case "Naranja":
                    lapiz = new Pen(Color.Orange, (float)GrosorUpDown2.Value);
                    break;
                case "Rojo":
                    lapiz = new Pen(Color.Red, (float)GrosorUpDown2.Value);
                    break;
                case "Azul":
                    lapiz = new Pen(Color.Blue, (float)GrosorUpDown2.Value);
                    break;
                case "Azul Oscuro":
                    lapiz = new Pen(Color.DarkBlue, (float)GrosorUpDown2.Value);
                    break;
                case "Azul Claro":
                    lapiz = new Pen(Color.LightBlue, (float)GrosorUpDown2.Value);
                    break;
                case "Cafe":
                    lapiz = new Pen(Color.Brown, (float)GrosorUpDown2.Value);
                    break;
                case "Amarillo":
                    lapiz = new Pen(Color.Yellow, (float)GrosorUpDown2.Value);
                    break;
                case "Verde":
                    lapiz = new Pen(Color.Green, (float)GrosorUpDown2.Value);
                    break;
                case "Verde Oscuro":
                    lapiz = new Pen(Color.DarkGreen, (float)GrosorUpDown2.Value);
                    break;
                case "Verde Claro":
                    lapiz = new Pen(Color.LightGreen, (float)GrosorUpDown2.Value);
                    break;
                case "Plomo":
                    lapiz = new Pen(Color.Silver, (float)GrosorUpDown2.Value);
                    break;
                case "Rosado":
                    lapiz = new Pen(Color.HotPink, (float)GrosorUpDown2.Value);
                    break;
                case "Morado":
                    lapiz = new Pen(Color.Purple, (float)GrosorUpDown2.Value);
                    break;
            }

            lapiz.StartCap = LineCap.Flat;
            lapiz.EndCap = LineCap.ArrowAnchor;

            //Escribir nombres
            SolidBrush s = new SolidBrush(Color.Azure);
            FontFamily ff = new FontFamily("Consolas");
            Font font = new Font(ff, 15);

            g = pnlGrafo.CreateGraphics();
            g.DrawLine(lapiz, xi+5, yi+6, xf+5, yf+6);
            g.DrawString(nombre + "(" + peso + ")", font, s, (xi+xf)/2, (yi+yf)/2);
        }
        private void dibujarAristaNoDirigida(string nombre,int peso, string color, int xi, int yi, int xf, int yf)
        {
            switch (color)
            {
                case "Blanco":
                    lapiz = new Pen(Color.White, (float)GrosorUpDown2.Value);
                    break;
                case "Naranja":
                    lapiz = new Pen(Color.Orange, (float)GrosorUpDown2.Value);
                    break;
                case "Rojo":
                    lapiz = new Pen(Color.Red, (float)GrosorUpDown2.Value);
                    break;
                case "Azul":
                    lapiz = new Pen(Color.Blue, (float)GrosorUpDown2.Value);
                    break;
                case "Azul Oscuro":
                    lapiz = new Pen(Color.DarkBlue, (float)GrosorUpDown2.Value);
                    break;
                case "Azul Claro":
                    lapiz = new Pen(Color.LightBlue, (float)GrosorUpDown2.Value);
                    break;
                case "Cafe":
                    lapiz = new Pen(Color.Brown, (float)GrosorUpDown2.Value);
                    break;
                case "Amarillo":
                    lapiz = new Pen(Color.Yellow, (float)GrosorUpDown2.Value);
                    break;
                case "Verde":
                    lapiz = new Pen(Color.Green, (float)GrosorUpDown2.Value);
                    break;
                case "Verde Oscuro":
                    lapiz = new Pen(Color.DarkGreen, (float)GrosorUpDown2.Value);
                    break;
                case "Verde Claro":
                    lapiz = new Pen(Color.LightGreen, (float)GrosorUpDown2.Value);
                    break;
                case "Plomo":
                    lapiz = new Pen(Color.Silver, (float)GrosorUpDown2.Value);
                    break;
                case "Rosado":
                    lapiz = new Pen(Color.HotPink, (float)GrosorUpDown2.Value);
                    break;
                case "Morado":
                    lapiz = new Pen(Color.Purple, (float)GrosorUpDown2.Value);
                    break;
            }

            lapiz.StartCap = LineCap.ArrowAnchor;
            lapiz.EndCap = LineCap.ArrowAnchor;

            //Escribir nombres
            SolidBrush s = new SolidBrush(Color.Azure);
            FontFamily ff = new FontFamily("Consolas");
            Font font = new Font(ff, 15);

            g = pnlGrafo.CreateGraphics();
            g.DrawLine(lapiz, xi + 5, yi + 6, xf + 5, yf + 6);
            g.DrawString(nombre + "(" + peso +")", font, s, (xi + xf) / 2, (yi + yf) / 2);
        }
        private void dibujarLazo(string nombre, string color, int x, int y)
        {
            switch (color)
            {
                case "Blanco":
                    lapiz = new Pen(Color.White, (float)GrosorUpDown2.Value);
                    break;
                case "Naranja":
                    lapiz = new Pen(Color.Orange, (float)GrosorUpDown2.Value);
                    break;
                case "Rojo":
                    lapiz = new Pen(Color.Red, (float)GrosorUpDown2.Value);
                    break;
                case "Azul":
                    lapiz = new Pen(Color.Blue, (float)GrosorUpDown2.Value);
                    break;
                case "Azul Oscuro":
                    lapiz = new Pen(Color.DarkBlue, (float)GrosorUpDown2.Value);
                    break;
                case "Azul Claro":
                    lapiz = new Pen(Color.LightBlue, (float)GrosorUpDown2.Value);
                    break;
                case "Cafe":
                    lapiz = new Pen(Color.Brown, (float)GrosorUpDown2.Value);
                    break;
                case "Amarillo":
                    lapiz = new Pen(Color.Yellow, (float)GrosorUpDown2.Value);
                    break;
                case "Verde":
                    lapiz = new Pen(Color.Green, (float)GrosorUpDown2.Value);
                    break;
                case "Verde Oscuro":
                    lapiz = new Pen(Color.DarkGreen, (float)GrosorUpDown2.Value);
                    break;
                case "Verde Claro":
                    lapiz = new Pen(Color.LightGreen, (float)GrosorUpDown2.Value);
                    break;
                case "Plomo":
                    lapiz = new Pen(Color.Silver, (float)GrosorUpDown2.Value);
                    break;
                case "Rosado":
                    lapiz = new Pen(Color.HotPink, (float)GrosorUpDown2.Value);
                    break;
                case "Morado":
                    lapiz = new Pen(Color.Purple, (float)GrosorUpDown2.Value);
                    break;
            }

            lapiz.StartCap = LineCap.Flat;
            lapiz.EndCap = LineCap.ArrowAnchor;

            //Escribir nombres
            SolidBrush s = new SolidBrush(Color.Azure);
            FontFamily ff = new FontFamily("Consolas");
            Font font = new Font(ff, 15);

            g = pnlGrafo.CreateGraphics();
            g.DrawArc(lapiz, new Rectangle(x-30, y-15, 40, 40), 40, 300);
            g.DrawString(nombre, font, s, x - 45, y - 50);
        }
        private void PnlGrafo_MouseDown(object sender, MouseEventArgs e)
        {
            if (chkbxManualmente.Checked == true)
            {
                g = pnlGrafo.CreateGraphics();
                string color = cbxColorVertice.Text;
                string nombre = txtNombreVertice.Text;
                switch (color)
                {
                    case "Blanco":
                        lapiz = new Pen(Color.White, (float)GrosorUpDown.Value);
                        break;
                    case "Naranja":
                        lapiz = new Pen(Color.Orange, (float)GrosorUpDown.Value);
                        break;
                    case "Rojo":
                        lapiz = new Pen(Color.Red, (float)GrosorUpDown.Value);
                        break;
                    case "Azul":
                        lapiz = new Pen(Color.Blue, (float)GrosorUpDown.Value);
                        break;
                    case "Azul Oscuro":
                        lapiz = new Pen(Color.DarkBlue, (float)GrosorUpDown.Value);
                        break;
                    case "Azul Claro":
                        lapiz = new Pen(Color.LightBlue, (float)GrosorUpDown.Value);
                        break;
                    case "Cafe":
                        lapiz = new Pen(Color.Brown, (float)GrosorUpDown.Value);
                        break;
                    case "Amarillo":
                        lapiz = new Pen(Color.Yellow, (float)GrosorUpDown.Value);
                        break;
                    case "Verde":
                        lapiz = new Pen(Color.Green, (float)GrosorUpDown.Value);
                        break;
                    case "Verde Oscuro":
                        lapiz = new Pen(Color.DarkGreen, (float)GrosorUpDown.Value);
                        break;
                    case "Verde Claro":
                        lapiz = new Pen(Color.LightGreen, (float)GrosorUpDown.Value);
                        break;
                    case "Plomo":
                        lapiz = new Pen(Color.Silver, (float)GrosorUpDown.Value);
                        break;
                    case "Rosado":
                        lapiz = new Pen(Color.HotPink, (float)GrosorUpDown.Value);
                        break;
                    case "Morado":
                        lapiz = new Pen(Color.Purple, (float)GrosorUpDown.Value);
                        break;
                }

                //Escribir nombres
                SolidBrush s = new SolidBrush(Color.White);
                FontFamily ff = new FontFamily("Consolas");
                Font font = new Font(ff, 15);

                g.DrawArc(lapiz, new Rectangle(e.X, e.Y, Convert.ToInt32(cbxTamaño.Text), Convert.ToInt32(cbxTamaño.Text)), 0, 360);
                g.DrawString(nombre, font, s, e.X - 10, e.Y - 30);

                txtPosX.Text = Convert.ToString(e.X);
                txtPosY.Text = Convert.ToString(e.Y);
                InsertarSiManualmente(e.X, e.Y);
                RecorrerListBox();
            }
            else
            {

            }
            BtnActualizarGrafico_Click(sender, e);
        }

        //FUNCIONES Y PROCEDIMIENTOS AUXILIARES//
        private void InsertarSiManualmente(int x, int y)
        {
            try
            {
                string nombre = txtNombreVertice.Text;
                nombre = nombre.Trim();                             
                //string antecede = txtAntecede.Text;
                //string sucede = txtSucede.Text;

                if (nombre != "" && x <= 765 && y <= 610)
                {
                    NodoDoble vertice = new NodoDoble();
                    vertice.Nombre = nombre;
                    vertice.posX = x;
                    vertice.posY = y;
                    vertice.color = cbxColorVertice.Text;
                    vertice.grosor = Convert.ToInt32(GrosorUpDown.Value);
                    vertice.tamaño = Convert.ToInt32(cbxTamaño.Text);

                    if (!OBJ.existeVertice(vertice))
                    {
                        contadorGlobalV += 1;                        
                        ClearText();
                        txtNombreVertice.Text = "V" + contadorGlobalV;
                    }
                    OBJ.insertarVertice(vertice);
                    OBJ.mostrarVertices(lbxComponentesVertices);
                    OBJ.mostrarAristas(lbxComponentesAristas);
                    OBJ.mostrarVertices(lbxDetallesVertices);
                    OBJ.mostrarAristas(lbxDetallesAristas);
                    ActualizarDatos();
                }
                else
                {
                    int n = Convert.ToInt32(cbxColorVertice.Text);     //Error provocado a drede
                }
            }
            catch
            {
                MessageBox.Show("GRAFO MAKER: PROPIEDADES DEL VERTICE INVALIDAS:\n" +
                    "\n" +
                    "1.- Los vertices deben tener nombre o etiqueta\n" +
                    "2.- La posición X es un número entero positvo (máximo 765)\n" +
                    "3.- La posición Y es un número entero positivo (máximo 610)\n" +
                    "4.- Se debe haber seleccionado al menos un color\n" +
                    "5.- Se debe haber seleccionado al menos un tamaño, entre 10 y 50", "ERROR ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void ActualizarDatos()
        {
            OBJ.mostrarVertices(lbxComponentesVertices);
            OBJ.mostrarVertices(lbxDetallesVertices);
            OBJ.mostrarAristas(lbxComponentesAristas);
            OBJ.mostrarAristas(lbxDetallesAristas);

            cbxConexionVertice1.Items.Clear();
            cbxConexionVertice2.Items.Clear();
            cbxEliminarComponente.Items.Clear();
            string[] vertices = OBJ.GetVertices();
            string[] aristas = OBJ.GetAristas();
            for (int i = 0; i < OBJ.Vertices(); i++)
            {
                cbxConexionVertice1.Items.Add(vertices[i]);
                cbxConexionVertice2.Items.Add(vertices[i]);
                cbxEliminarComponente.Items.Add(vertices[i]);
            }
            for (int i = 0; i < OBJ.Aristas(); i++)
            {
                cbxEliminarComponente.Items.Add(aristas[i]);
            }


            string[] names = OBJ.GetVertices();
            string cadena = "V = {";
            for (int i = 0; i < OBJ.Vertices(); i++)
            {
                if (i + 1 == OBJ.Vertices())
                {
                    cadena = cadena + names[i];
                }
                else
                {
                    cadena = cadena + names[i] + ", ";
                }
            }
            cadena = cadena + "}";

            string[] names2 = OBJ.GetAristas();
            string cadena2 = "A = {";
            for (int i = 0; i < OBJ.Aristas(); i++)
            {
                if (i + 1 == OBJ.Aristas())
                {
                    cadena2 = cadena2 + names2[i];
                }
                else
                {
                    cadena2 = cadena2 + names2[i] + ", ";
                }
            }
            cadena2 = cadena2 + "}";

            txtCantidadDeVertices.Text = cadena;
            txtCantidadDeAristas.Text = cadena2;

            lblAristas.Text = "ARISTAS: " + contadorGlobalA;
            lblVertices.Text = "VERTICES: " + contadorGlobalV;
        }
        private void RecorrerListBox()
        {
            lbxComponentesVertices.SelectedIndex = lbxComponentesVertices.Items.Count - 1;
            lbxComponentesVertices.SelectedIndex = -1;
            lbxComponentesAristas.SelectedIndex = lbxComponentesAristas.Items.Count - 1;
            lbxComponentesAristas.SelectedIndex = -1;
            lbxDetallesVertices.SelectedIndex = lbxDetallesVertices.Items.Count - 1;
            lbxDetallesVertices.SelectedIndex = -1;
            lbxDetallesAristas.SelectedIndex = lbxDetallesAristas.Items.Count - 1;
            lbxDetallesAristas.SelectedIndex = -1;
        }
        private void ClearText()
        {
            cbxConexionVertice1.Text = "";
            cbxConexionVertice2.Text = "";
            txtPosX.Text = "(Aleatorio)";
            txtPosY.Text = "(Aleatorio)";
        }     

        //VALIDACIONES PARA EL USUARIO//VALIDACIONES PARA EL USUARIO//VALIDACIONES PARA EL USUARIO//VALIDACIONES PARA EL USUARIO//
        private void TxtPosX_Click(object sender, EventArgs e)
        {
            txtPosX.Clear();
        }
        private void TxtPosY_Click(object sender, EventArgs e)
        {
            txtPosY.Clear();
        }
        private void TxtPosX_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.SoloNumeros(e);
        }
        private void TxtPosY_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.SoloNumeros(e);
        }
        private void CbxColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.NoSePuedeEscribir(e);
        }
        private void CbxTamaño_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.NoSePuedeEscribir(e);
        }
        private void GrosorUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.NoSePuedeEscribir(e);
        }
        private void GrosorUpDown2_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.NoSePuedeEscribir(e);
        }
        private void TxtCantidadDeVertices_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.NoSePuedeEscribir(e);
        }
        private void TxtCantidadDeAristas_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.NoSePuedeEscribir(e);
        }
        private void RichTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.NoSePuedeEscribir(e);
        }
        private void GrosorUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(GrosorUpDown.Value) > 10)
            {
                GrosorUpDown.Value = 10;
            }
        }
        private void GrosorUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(GrosorUpDown2.Value) > 10)
            {
                GrosorUpDown2.Value = 10;
            }
        }        
        private void PnlGrafo_MouseMove(object sender, MouseEventArgs e)
        {
            lblLocalizacion.Text = ("X " + e.X + " , Y " + e.Y);
            if (chkbxManualmente.Checked == true)
            {
                txtPosX.Text = Convert.ToString(e.X);
                txtPosY.Text = Convert.ToString(e.Y);
            }
        }
        private void TxtPeso_KeyPress(object sender, KeyPressEventArgs e)
        {
            v.SoloNumeros(e);
        }
        private void CbxConexionVertice2_TextChanged(object sender, EventArgs e)
        {
            string uno = cbxConexionVertice1.Text;
            string dos = cbxConexionVertice2.Text;
            if (uno == dos)
            {
                chkbxDirigido.Enabled = false;
                chkbxDirigido.Checked = false;
            }              
            else
                chkbxDirigido.Enabled = true;
        }
        private void CbxConexionVertice1_TextChanged(object sender, EventArgs e)
        {
            string uno = cbxConexionVertice1.Text;
            string dos = cbxConexionVertice2.Text;
            if (uno == dos)
            {
                chkbxDirigido.Enabled = false;
                chkbxDirigido.Checked = false;
            }
            else
                chkbxDirigido.Enabled = true;
        }
        private void ChkbxDirigido_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxDirigido.Checked == true)
            {
                cbxColorArista.Text = "Rojo";
            }
            else
            {
                cbxColorArista.Text = "Rosado";
            }
        }
        private void chkbxGrafoPonderado_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxGrafoPonderado.Checked == true)
            {
                txtPeso.Text = "1";
                txtPeso.Show();
                txtPESO_N.Show();
            }
            else
            {
                txtPeso.Text = "1";
                txtPeso.Hide();
                txtPESO_N.Hide();
            }
        }
        private void chkbxAristaDirigida_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxAristaDirigida.Checked == true)
            {
                chkbxDirigido.Checked = false;
                chkbxDirigido.Show();
            }
            else
            {
                chkbxDirigido.Checked = false;
                chkbxDirigido.Hide();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbxRuta.Items.Clear();
            txtCostoRecorrido.Text = "";
            try
            {
                if (txtDE.Text != "" && txtA.Text != "")
                {
                    OBJ.Dijkastra(txtDE.Text, txtA.Text, lbxRuta, txtCostoRecorrido);
                }
                else
                { MessageBox.Show("Debes colocar el inicio y destino del recorrido", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            catch { MessageBox.Show("No existe tal vertice o el grafo esta vacio", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}
