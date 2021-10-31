﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace lab4_1
{
    public partial class Form1 : Form
    {
        Thread thread1;
        Thread thread2;
        Thread thread3;

        public Form1()
        {
            InitializeComponent();
            thread1 = new Thread(new ThreadStart(DrawRectangle));
            thread2 = new Thread(new ThreadStart(DrawEllipse));
            thread3 = new Thread(new ThreadStart(RandomNumber));
        }
        //Прорисовка прямоугольников
        private void DrawRectangle()
        {
            try
            {
                Random rnd = new Random();
                Graphics g = panel1.CreateGraphics();
                while (true)
                {
                    Thread.Sleep(50);
                    g.DrawRectangle(Pens.Pink, 0, 0, rnd.Next(this.Width),
                    rnd.Next(this.Height));
                }
            }
            catch (Exception ex) { }
        }
        //Прорисовка элипсов
        private void DrawEllipse()
        {
            try
            {
                Random rnd = new Random();
                Graphics g = panel2.CreateGraphics();
                while (true)
                {
                    Thread.Sleep(50);
                    g.DrawEllipse(Pens.Yellow, 0, 0, rnd.Next(this.Width),
                    rnd.Next(this.Height));
                }
            }
            catch (Exception ex) { }
        }
        //Запись чисел
        private void RandomNumber()
        {
            try
            {
                Random rnd = new Random();
                Parallel.For(0, 500, i =>
                {
                    textBox1.Invoke((MethodInvoker)delegate ()
                    {
                        textBox1.Text += rnd.Next().ToString();
                    });
                });
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        //Кнопка запуска всех трех методов
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                thread1.Start();
                thread2.Start();
                thread3.Start();
            }
            catch (Exception ex) { }
        }
        //Запуск метода прорисовки прямоугольников
        private void button1_Click(object sender, EventArgs e)
        {
            thread1.Start();
        }
        //Запуск метода прорисовки элипсов
        private void button2_Click(object sender, EventArgs e)
        {
            thread2.Start();
        }
        //Запуск метода записи чисел
        private void button3_Click(object sender, EventArgs e)
        {
            thread3.Start();
        }
        //Приостановка потоков
        private void button5_Click(object sender, EventArgs e)
        {
            thread1.Interrupt();
            thread2.Interrupt();
            thread3.Interrupt();
        }
        //Код при закрытии формы
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}
