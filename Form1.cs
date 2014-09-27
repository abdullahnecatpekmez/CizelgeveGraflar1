using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication9
{

    public partial class Form1 : Form
    {
        public static List<string> yazdir = new List<string>();
        SolidBrush b1 = new SolidBrush(Color.Red);
        SolidBrush b2 = new SolidBrush(Color.DarkGreen);
        Pen pen = new Pen(Color.Black);
        static Random ras;
        int i = 0, a = -1, kontrol = 0, sayac1;
        int[,] matr;
        List<Point> konum;
        List<int> dizi = new List<int>();
        public Form1()
        {

            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override void OnPaint(PaintEventArgs paintEvent)
        {
            if (a != -1)
            {

                if (kontrol == sayac1)
                {
                    konum = new List<Point>();
                }
                Point gecNok = new Point();
                int x, y, sayY = 1, sayX = 1, sayac = 0;
                double x1, y1;
                Graphics g = paintEvent.Graphics;
                foreach (int das in dizi)
                {
                    x1 = Math.Cos(Math.PI * das / 180);
                    y1 = Math.Sin(Math.PI * das / 180);

                    if ((x1 < 0) && (y1 > 0))
                    { sayY = -1; sayX = 1; }
                    else if ((x1 > 0) && (y1 < 0))
                    { sayY = -1; sayX = 1; }
                    else if ((x1 < 0) && (y1 < 0))
                    { sayY = -1; sayX = 1; }
                    else if ((x1 > 0) && (y1 > 0))
                    { sayY = -1; sayX = 1; }

                    x = (Convert.ToInt32(Math.Cos(Math.PI * das / 180) * 200) * sayX) + 210;
                    y = (Convert.ToInt32(Math.Sin(Math.PI * das / 180) * 200) * sayY) + 210;
                    g.DrawEllipse(pen, x, y, 15, 15);
                    g.DrawString(sayac.ToString(), new Font("Times New Roman", 9), b2, x, y);
                    /*if ((x1 < 0) && (y1 > 0))
                    { gecNok.X = x+13; gecNok.Y = y+13; }
                    else if ((x1 > 0) && (y1 < 0))
                    { gecNok.X = x; gecNok.Y = y; }
                    else if ((x1 < 0) && (y1 < 0))
                    { gecNok.X = x+13; gecNok.Y = y; }
                    else if ((x1 >= 0) && (y1 >= 0))
                    { gecNok.X = x; gecNok.Y = y + 13; } */
                    gecNok.X = x; gecNok.Y = y;
                    if (kontrol == sayac1)
                    {
                        gecNok.X = x; gecNok.Y = y;
                        konum.Add(gecNok);
                        sayac++;
                    }
                }

                if (kontrol == sayac1)
                {
                    ras = new Random();
                    int rnd = 0, deger;
                    for (int j = 0; j < konum.Count(); ++j)
                    {
                        rnd = ras.Next(konum.Count());
                        while (j == rnd || matr[rnd, j] >= 0)
                            rnd = ras.Next(konum.Count());
                        g.DrawLine(pen, konum.ElementAt(j), konum.ElementAt(rnd));
                        deger = ras.Next(100) + 1;
                        matr[j, rnd] = deger;
                        matr[rnd, j] = deger;
                        g.DrawString(deger.ToString(), new Font("Times New Roman", 10), b1, (konum.ElementAt(j).X + konum.ElementAt(rnd).X) / 2, (konum.ElementAt(j).Y + konum.ElementAt(rnd).Y) / 2 - 2);
                    }
                    a = -1;
                }
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            dizi.Add(i);
            i += a;
            Invalidate();
            sayac1++;
            if (kontrol == sayac1)
            { timer1.Enabled = false; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (i != 0) { i = 0; dizi.Clear(); kontrol = 0; sayac1 = 0; }
            if (textBox1.Text != "")
                kontrol = Convert.ToInt32(textBox1.Text);
            if ((textBox1.Text != "") && (kontrol > 0))
            {
                matr = new int[kontrol, kontrol];
                for (int v = 0; v < kontrol; v++)
                    for (int j = 0; j < kontrol; j++)
                        matr[v, j] = -1;
                a = 360 / kontrol;
                timer1.Enabled = true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            i = 0; dizi.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dizi.Count != 2)
            {
                dizi.RemoveAt(1);
                konum.Clear();
                i = 360;
                a = 0;
                Invalidate();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            yazdir.Clear();
            Kruskal K = new Kruskal(kontrol, matr);
            K.KruskalSolving();
            foreach (string s in yazdir)
                listBox3.Items.Add(s);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            yazdir.Clear();
            dfs df = new dfs(kontrol, matr);
            df.çıkar(Convert.ToInt32(textBox4.Text));
            foreach (string s in yazdir)
                listBox1.Items.Add(s);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            yazdir.Clear();
            Dijkstra D = new Dijkstra(kontrol, matr, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text));
            D.Run();
            foreach (string s in yazdir)
                listBox2.Items.Add(s);
        }
        public class Kruskal
        {
            private int derece = 0;
            private int[,] L;
            private int[] C;
            private int[,] tempL;
            private int satır;
            private int sutun;
            public int[,] sol;
            private int baglı;

            public int ilkatama()
            {
                int temp = Int32.MaxValue;
                for (int i = 0; i < derece; i++)
                {
                    for (int j = 0; j < derece; j++)
                    {
                        if (tempL[i, j] > 0 && tempL[i, j] < temp)
                        {
                            temp = tempL[i, j];
                            satır = i;
                            sutun = j;
                        }
                    }
                }
                tempL[satır, sutun] = tempL[sutun, satır] = -1;
                return temp;
            }
            public Kruskal(int der, int[,] d)
            {
                L = new int[der, der];
                C = new int[der];
                tempL = new int[der, der];
                sol = new int[der, der];
                derece = der;
                baglı = 0;
                for (int i = 0; i < derece; i++)
                {
                    for (int j = 0; j < derece; j++)
                    {
                        L[i, j] = d[i, j];
                        tempL[i, j] = d[i, j];
                        sol[i, j] = -1;
                    }
                }
                for (int i = 0; i < derece; i++)
                {
                    C[i] = i;
                }
            }
            public bool bittimi()
            {
                bool flag = true;
                for (int i = 0; i < derece; i++)
                {
                    for (int j = 0; j < derece; j++)
                    {
                        if (tempL[i, j] != -1)
                            flag = false;
                    }
                }
                if (flag) { return true; }
                for (int i = 0; i < derece; i++)
                {
                    if (C[i] != -1)
                    {
                        return false;
                    }
                }
                return true;
            }
            public void SetC(int a, int b)
            {
                for (int i = 0; i < derece; i++)
                {
                    if (C[i] == a)
                        C[i] = b;
                }
            }
            public void SetSolution(int r, int c, int dis)
            {
                sol[r, c] = dis;
                if (C[r] < 0 || C[c] < 0)
                {
                    if (C[r] < 0)
                    {
                        SetC(C[c], C[r]);
                        C[c] = C[r];
                    }
                    else
                    {
                        SetC(C[r], C[c]);
                        C[r] = C[c];
                    }
                }
                else
                {
                    C[r] = C[c] = (--baglı);
                }
                string ilk = r.ToString();
                string son = c.ToString();
                yazdir.Add(ilk + "'dan " + son + "'ye olan uzunluk: " + dis);
                Console.WriteLine("\t" + ilk + "'dan " + son + "'ye olan uzunluk: " + dis);
            }
            public void KruskalSolving()
            {
                int dis = ilkatama();
                SetSolution(satır, sutun, dis);
                while (!bittimi())
                {
                    dis = ilkatama();
                    if (C[satır] != C[sutun] || C[satır] >= 0)
                        SetSolution(satır, sutun, dis);
                }
            }
        }
        class Vertex
        {
            public int label;
            public bool wasVisited;
            // ------------------------------------------------------------
            public Vertex(int lab)   // constructor
            {
                label = lab;
                wasVisited = false;
            }
            // ------------------------------------------------------------
        }  // end class Vertex
        ////////////////////////////////////////////////////////////////
        class dfs
        {
            private Vertex[] vertexList; // list of vertices
            private int[,] adjMat;      // adjacency matrix
            private int nVerts;          // current number of vertices
            private Stack theStack;
            // ------------------------------------------------------------
            public dfs(int MAX_VERTS, int[,] dizi)               // constructor
            {
                vertexList = new Vertex[MAX_VERTS];                              // adjacency matrix
                adjMat = new int[MAX_VERTS, MAX_VERTS];
                nVerts = MAX_VERTS;
                adjMat = dizi;
                theStack = new Stack();
                for (int i = 0; i < MAX_VERTS; i++)
                    vertexList[i] = new Vertex(i);

            }  // end constructor
            // ------------------------------------------------------------


            public void displayVertex(int v)
            {
                yazdir.Add(vertexList[v].label.ToString());
            }
            // ------------------------------------------------------------
            public void çıkar(int d)  // depth-first search
            {                                 // begin at vertex 0
                vertexList[d].wasVisited = true;  // mark it
                displayVertex(d);                 // display it
                theStack.Push(d);                 // push it

                while (theStack.Count > 0)      // until stack empty,
                {
                    // get an unvisited vertex adjacent to stack top
                    int v = getAdjUnvisitedVertex((int)theStack.Peek());
                    if (v == -1)                    // if no such vertex,
                        theStack.Pop();
                    else                           // if it exists,
                    {
                        vertexList[v].wasVisited = true;  // mark it
                        displayVertex(v);                 // display it
                        theStack.Push(v);                 // push it
                    }
                }  // end while

                // stack is empty, so we're done
                for (int j = 0; j < nVerts; j++)          // reset flags
                    vertexList[j].wasVisited = false;
            }  // end dfs
            // ------------------------------------------------------------
            // returns an unvisited vertex adj to v
            public int getAdjUnvisitedVertex(int v)
            {
                for (int j = 0; j < nVerts; j++)
                    if (adjMat[v, j] != -1 && vertexList[j].wasVisited == false)
                        return j;
                return -1;
            }  // end getAdjUnvisitedVertex()
            // ------------------------------------------------------------
        }

        class Dijkstra
        {
            public int derece = 0;
            public int[,] L;
            public int[] C;
            public int[] D;
            public int bas, son;
            public int adım = 0;
            public Dijkstra(int der, int[,] d, int k, int s)
            {
                L = new int[der, der];
                C = new int[der];
                D = new int[der];
                derece = der;
                son = s;
                bas = k;
                L = d;
                for (int i = 0; i < derece; i++)
                {
                    C[i] = i;
                }
                C[bas] = -1;
                for (int i = 0; i < derece; i++)
                    D[i] = L[bas, i];

            }
            public void DijkstraSolving()
            {
                int mindeger = Int32.MaxValue;
                int minnokta = 0;
                for (int i = 0; i < derece; i++)
                {
                    if (C[i] == -1)
                        continue;
                    if (D[i] > 0 && D[i] < mindeger)
                    {
                        mindeger = D[i];
                        minnokta = i;
                    }
                }
                C[minnokta] = -1;
                for (int i = 0; i < derece; i++)
                {
                    if (L[minnokta, i] < 0)
                        continue;
                    if (D[i] < 0 && i != bas)
                    {
                        D[i] = mindeger + L[minnokta, i];
                        continue;
                    }
                    if ((D[minnokta] + L[minnokta, i]) < D[i])
                        D[i] = mindeger + L[minnokta, i];
                }
            }
            public void Run()
            {
                for (adım = 1; adım < derece; adım++)
                    DijkstraSolving();
                yazdir.Add(bas + " dan " + son + " ya olan uzunluk:" + D[son]);
            }
        }
    }
}
