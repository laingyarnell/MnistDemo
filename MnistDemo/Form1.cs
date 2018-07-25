using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MnistDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private const int ImageSize = 28;
        private Model.Mnist model;
        private Graphics graphics;
        private Point startPoint;

        private void Form1_Load(object sender, EventArgs e)
        {
            model = new Model.Mnist();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            pictureBox1.Invalidate();
            label1.Text = string.Empty;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Pen penStyle = new Pen(Color.Black, 40);
                penStyle.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                penStyle.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                graphics.DrawLine(penStyle, startPoint, e.Location);
                pictureBox1.Invalidate();
                startPoint = e.Location;

            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Bitmap clonedMap = new Bitmap(pictureBox1.Image, ImageSize, ImageSize);
                var image = new List<float>(ImageSize * ImageSize);

                for (var y = 0; y < ImageSize; y++)
                {
                    for (var x = 0; x < ImageSize; x++)
                    {
                        var color = clonedMap.GetPixel(x, y);
                        var value = (float)(0.5 - (color.R + color.G + color.B) / (3.0 * 255));
                        image.Add(value);
                    }

                }

                var text = model.Infer(new List<IEnumerable<float>> { image }).First().First().ToString();
                label1.Text = text;
            }
        }
    }
}
