using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace lab4_3_S_
{
    public partial class Form1 : Form
    {
        private static Semaphore _pool;

        public Form1()
        {
            InitializeComponent();
        }
        //Закрашивание в желтый
        private void Yellow()
        {
            // Получить семафор
            _pool.WaitOne();
            textBox1.Invoke((MethodInvoker)delegate ()
            {
                textBox1.BackColor = Color.Yellow;
            });
            Thread.Sleep(500);
            // Освободить семафор
            _pool.Release();
        }
        //Закрашивание в голубой
        private void LightSkyBlue()
        {
            // Получить семафор
            _pool.WaitOne();
            textBox1.Invoke((MethodInvoker)delegate ()
            {
                textBox1.BackColor = Color.LightSkyBlue;
            });
            Thread.Sleep(500);
            // Освободить семафор
            _pool.Release();
        }
        //Закрашивание в черный
        private void Black()
        {
            // Получить семафор
            _pool.WaitOne();
            textBox1.Invoke((MethodInvoker)delegate ()
            {
                textBox1.BackColor = Color.Black;
            });
            Thread.Sleep(500);
            // Освободить семафор
            _pool.Release();
        }
        //Нажатие кнопки Запуск
        private void button1_Click_1(object sender, EventArgs e)
        {
            _pool = new Semaphore(1, 1);
            for (int i = 0; i < 100; i++)
            {
                Thread thread1 = new Thread(Yellow);
                thread1.Start();
                Thread.Sleep(1);
                Thread thread2 = new Thread(LightSkyBlue);
                thread2.Start();
                Thread.Sleep(1);
                Thread thread3 = new Thread(Black);
                thread3.Start();
                Thread.Sleep(1);
            }
        }
        //Код при закрытии формы
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(1);
        }
    }
}