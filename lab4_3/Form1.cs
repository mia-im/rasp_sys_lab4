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

namespace lab4_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        class SharedRes
        {
            public static Mutex mtx = new Mutex();
        }
        //Закрашивание в желтый
        private void Yellow()
        {
            // Получить мьютекс
            SharedRes.mtx.WaitOne();
            textBox1.Invoke((MethodInvoker)delegate ()
            {
                textBox1.BackColor = Color.Yellow;
            });          
            Thread.Sleep(500);
            // Освободить мьютекс
            SharedRes.mtx.ReleaseMutex();
        }
        //Закрашивание в голубой
        private void LightSkyBlue()
        {
            // Получить мьютекс
            SharedRes.mtx.WaitOne();
            textBox1.Invoke((MethodInvoker)delegate ()
            {
                textBox1.BackColor = Color.LightSkyBlue;
            });
            Thread.Sleep(500);
            // Освободить мьютекс
            SharedRes.mtx.ReleaseMutex();
        }
        //Закрашивание в черный
        private void Black()
        {
            // Получить мьютекс
            SharedRes.mtx.WaitOne();
            textBox1.Invoke((MethodInvoker)delegate ()
            {
                textBox1.BackColor = Color.Black;
            }); 
            Thread.Sleep(500);
            // Освободить мьютекс
            SharedRes.mtx.ReleaseMutex();
        }
        //Нажатие кнопки Запуск
        private void button1_Click(object sender, EventArgs e)
        {
            for(int i=0; i<100; i++)
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