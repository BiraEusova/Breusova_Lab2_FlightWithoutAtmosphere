using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlightWithoutAtmosphere
{
    public partial class Form1 : Form
    {
        const double dt = 0.01;
        const double g = 9.81;
        bool pause = false;

        double a;
        double v0, v0x, v0y;
        double y0;
        double t;
        double x, y;
        double outputT;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void startButton_Click(object sender, EventArgs e)
        {
            if (!pause)
            {
                a = (double)angelNumericUpDown.Value * Math.PI / 180;
                v0 = (double)speedNumericUpDown.Value;
                y0 = (double)heightNumericUpDown.Value;

                v0x = v0 * Math.Cos(a);
                v0y = v0 * Math.Sin(a);

                t = 0;
                outputT = 0;
                x = 0;
                y = y0;

                double xAdd = v0x * (Math.Sqrt(v0y * v0y + 2 * g * y0) - v0y) / g;
                // Если тело брошено с какой-то высоты y0, то обычная формула расчета 
                // максимальной дальности полета не подойдет (данная формула основана
                // на времени подъема тела до самой высокой точки), т.к. при падении относительно самой высокой точки полета
                // тело по оси оХ преодолевает большее расстояние, чем до этой точки.
                // Следовательно, надо прибавить расстояние, 
                // пройденное телом по оси оХ после достижения уровня высоты у0 при падении.
                // Если y0 = 0, то xAdd также = 0.


                double xMax = v0 * v0 * Math.Sin(2 * a) / g + xAdd;
                double yMax = y0 + v0 * v0 * Math.Sin(a) * Math.Sin(a) / (2 * g);

                chart1.ChartAreas[0].AxisX.Maximum = xMax + 0.5;
                chart1.ChartAreas[0].AxisY.Maximum = yMax + 0.5;

                chart1.Series[0].Points.Clear();
                chart1.Series[0].Points.AddXY(x, y);
                outputTimeLabel.Text = t.ToString() + " sec";
            }

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pause = false;
            t += dt;
            x = v0x * t;
            y = y0 + v0y * t - g * t * t / 2;
            chart1.Series[0].Points.AddXY(x, y);

            outputT += Math.Round(t, 3);
            outputTimeLabel.Text = outputT.ToString() + " sec";

            if (y <= 0) timer1.Stop();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            pause = true;
            timer1.Stop();
        }
    }
}
