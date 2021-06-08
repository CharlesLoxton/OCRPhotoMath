using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronOcr;

namespace IronOCR
{
    public partial class Form1 : Form
    {
        string equation;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Ocr = new IronTesseract();
            using (var Input = new OcrInput(pictureBox1.Image))
            {
                Input.DeNoise();
                Input.ToGrayScale();
                var Result = Ocr.Read(Input);
                textBox1.Text = Result.Text;
                string equation0 = Result.Text;
                equation = equation0.ToLower();
                checkOperator(removeWhiteSpace(equation));
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF",
                Title = "Browse Images",

                CheckFileExists = true,
                CheckPathExists = true,

                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Image myimage = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = myimage;
            }
        }






        //Calculations
        public string removeWhiteSpace(string word)
        {
            string removed = word.Replace(" ", "");
            return removed;
        }

        public void checkOperator(string word)
        {
            int plusOp = word.IndexOf("+");
            int minusOp = word.IndexOf("-");

            if (plusOp > 0)
            {
                solvePlus();
            }
            else if (minusOp > 0)
            {
                solveMinus();
            }
            else
            {
                noOperator();
            }
        }

        public int getEqual(string word)
        {
            int equals = word.IndexOf("=");
            return equals;
        }
        public void solvePlus()
        {
            string equation1 = removeWhiteSpace(equation);
            int plus = equation1.IndexOf("+");
            int equals = getEqual(equation1);
            string number = equation1.Substring(plus + 1, equals - plus - 1);

            textBox1.Text += "\r\n" + getX(equation1) + " = " + afterEqual(equation1) + " - " +  number;
            textBox1.Text += "\r\nx = " + "(" + afterEqual(equation1) + " - " + number + ")" + " / " + getX(equation1).Substring(0, getX(equation1).IndexOf("x"));

            double answer = (double.Parse(afterEqual(equation1)) - double.Parse(number)) / double.Parse(getX(equation1).Substring(0, getX(equation1).IndexOf("x")));

            textBox1.Text += "\r\nx = " + answer;
        }

        public void solveMinus()
        {
            string equation1 = removeWhiteSpace(equation);
            int plus = equation1.IndexOf("-");
            int equals = getEqual(equation1);
            string number = equation1.Substring(plus + 1, equals - plus - 1);

            textBox1.Text += "\r\n" + getX(equation1) + " = " + afterEqual(equation1) + " + " + number;
            textBox1.Text += "\r\nx = " + "(" + afterEqual(equation1) + " + " + number + ")" + " / " + getX(equation1).Substring(0, getX(equation1).IndexOf("x"));

            double answer = (double.Parse(afterEqual(equation1)) + double.Parse(number)) / double.Parse(getX(equation1).Substring(0, getX(equation1).IndexOf("x")));

            textBox1.Text += "\r\nx = " +  answer;
        }

        public void noOperator()
        {
            try
            {
                string equation1 = removeWhiteSpace(equation);

                textBox1.Text += "\r\nx = " + afterEqual(equation1) + "/" + getX(equation1).Substring(0, getX(equation1).IndexOf("x"));

                double answer = double.Parse(afterEqual(equation1)) / double.Parse(getX(equation1).Substring(0, getX(equation1).IndexOf("x")));

                textBox1.Text += "\r\nx = " + answer;
            }
            catch(Exception e)
            {
                textBox1.Text = "No operator found";
            }
        }

        public string getX(string word)
        {
            int x = word.IndexOf("x");
            string beforeX;
            if (x == 0)
            {
                beforeX = "1x";
            }
            else
            {
                beforeX = word.Substring(0, x + 1);
            }
            return beforeX;
        }

        public string afterEqual(string word)
        {
            string afterEqual = word.Substring(getEqual(word) + 1, word.Length - getEqual(word) - 1);
            return afterEqual;
        }
    }
}
