using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace lab4_2
{
    public partial class Form1 : Form
    {
        const float PI = (float)Math.PI;
        GraphicsPath sinusoid;
        int n1 = 0;
        int n2 = 0;
        int k = 0; //количество нажатий на кнопку Старт
        private object lockOn = new object();
        Thread thr1;
        Thread thr2;

        public Form1()
        {
            InitializeComponent();
            button1.Click += button1_Click;
            pictureBox1.Paint += pictureBox1_Paint;
            timer1.Tick += tmr1_Tick;
            timer2.Tick += tmr2_Tick;
            sinusoid = new GraphicsPath();
            thr1 = new Thread(Eq);
            thr2 = new Thread(Eq);
        }    
        //действия timer1
        void tmr1_Tick(object sender, EventArgs e)
        {
            n1++;
            if (++n1 >= sinusoid.PointCount) n1 = 0;
            if (n1 == 10) timer2.Start();
            pictureBox1.Refresh();
        }        
        //действия timer2
        void tmr2_Tick(object sender, EventArgs e)
        {
            n2++;
            if (++n2 >= sinusoid.PointCount) n2 = 0;
            pictureBox1.Refresh();
        }        
        //Используемый потоком метод
        private void Eq(object o)
        {
            string name = Convert.ToString(o);
            if (name == "Red")
            {
                while (true)
                {
                    RedCircle(true);
                }    
            }
            else
            {
                while (true)
                {
                    BlueCircle(true);
                }
            }
        }        
        //Прорисовка красного шарика
        private void RedCircle(bool running)
        {
            lock (lockOn)
            {
                if (!running)
                {
                    // Остановить шарик
                    Monitor.Pulse(lockOn);
                    return;
                }
                pictureBox1.Invoke((MethodInvoker)delegate ()
                {
                    PointF pt = sinusoid.PathPoints[n1];
                    Graphics g = pictureBox1.CreateGraphics();
                    g.ScaleTransform(1, -1);
                    g.TranslateTransform(5, -pictureBox1.Height / 2);
                    g.FillEllipse(Brushes.Red, RectangleF.FromLTRB(pt.X - 20, pt.Y - 20, pt.X + 20, pt.Y + 20));
                });
                // Разрешить выполнение метода BlueCircle()
                Monitor.Pulse(lockOn);
                // Ожидать завершение BlueCircle()
                Monitor.Wait(lockOn);
            }
        }        
        //Прорисовка синего шарика
        private void BlueCircle(bool running)
        {
            lock (lockOn)
            {
                if (!running)
                {
                    // Остановить шарик
                    Monitor.Pulse(lockOn);
                    return;
                }
                pictureBox1.Invoke((MethodInvoker)delegate ()
                {
                    PointF pt = sinusoid.PathPoints[n2];
                    Graphics g = pictureBox1.CreateGraphics();
                    g.ScaleTransform(1, -1);
                    g.TranslateTransform(5, -pictureBox1.Height / 2);
                    g.FillEllipse(Brushes.Blue, RectangleF.FromLTRB(pt.X - 20, pt.Y - 20, pt.X + 20, pt.Y + 20));
                });
                // Разрешить выполнение метода RedCircle()
                Monitor.Pulse(lockOn);
                // Ожидать завершение RedCircle()
                Monitor.Wait(lockOn);
            }
        }        
        //Создание синусоиды
        private void CreateSinusoid(GraphicsPath gp, float MaxX, Size size)
        {
            gp.Reset();
            PointF[] points = new PointF[1] { PointF.Empty };
            for (float i = 0; i <= MaxX; i += 0.4f)
            {
                points[points.GetUpperBound(0)] = new PointF(i, (float)Math.Sin(i));
                Array.Resize<PointF>(ref points, points.Length + 1);
            }
            Array.Resize<PointF>(ref points, points.Length - 1);
            gp.AddCurve(points);
            Matrix m = new Matrix();
            m.Scale((float)(size.Width / MaxX), 0.4f * size.Height);
            gp.Transform(m);
        }        
        //Прорисовка синусоиды
        void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (sinusoid.PointCount == 0) return;
            e.Graphics.ScaleTransform(1, -1);
            e.Graphics.TranslateTransform(5, -pictureBox1.Height / 2);
            e.Graphics.DrawPath(Pens.Red, sinusoid);
        }      
        //Нажатие кнопки Старт
        void button1_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            CreateSinusoid(sinusoid, 10f * PI, pictureBox1.ClientSize);
            if (k == 0)
            {
                timer1.Start();
                thr1.Start("Red");
                thr2.Start("Blue");
            }
            else
            {
                timer1.Start();
                timer2.Start();
            }
            k++;
        }        
        //Нажатие кнопки стоп
        private void button2_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
        }        
        //Код при закрытии формы
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}