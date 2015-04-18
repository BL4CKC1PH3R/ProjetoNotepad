using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;

namespace Notepad___
{
    public partial class Form1 : Form
    {
        private Font verdana10Font;
        private StreamReader reader;
        String bufferArchive = "";
        bool saved;

        public Form1()
        {
            InitializeComponent();
            DirectoryInfo dir = new DirectoryInfo(@"C:\Users\Jonata\AppData\Roaming\Notepad+++");
            dir.Create();
           
        }

        private void novoDocumentoDeTextoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePrint();
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void Salvar() //SALVAR ARQUIVO.
        {
            try
            {
                if (bufferArchive.Equals(""))
                {

                    saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.ShowDialog();


                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(rich_MainText.Text);
                    sw.Close();
                    fs.Close();
                }
                else
                {

                    StreamWriter sw = new StreamWriter(bufferArchive);
                    sw.Write(rich_MainText.Text);
                    sw.Close();

                }

                saved = true;

            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
            }
        }

        private void abrirArquivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {                

                openFileDialog1 = new OpenFileDialog();
                openFileDialog1.ShowDialog();

                bufferArchive = openFileDialog1.FileName;//Salvar caminho.

                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamReader sr = new StreamReader(fs);

                rich_MainText.Text = sr.ReadToEnd();
                sr.Close();
                fs.Close();
            }
            catch {}

          
        }
//IMPRIMIR-------------------------------------------------------------------------
        private void imprimirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePrint();
            string filename = @"C:\Users\Jonata\AppData\Roaming\Notepad+++\imprimir.txt";
            //Create a StreamReader object
            reader = new StreamReader(filename);
            //Create a Verdana font with size 10
            verdana10Font = new Font("System", 10);
            //Create a PrintDocument object
            PrintDocument pd = new PrintDocument();
            //Add PrintPage event handler
            pd.PrintPage += new PrintPageEventHandler(this.PrintTextFileHandler);
            //Call Print Method
            pd.Print();
            //Close the reader
            if (reader != null)
                reader.Close();
            DelPrintTxt();
        }

        private void PrintTextFileHandler(object sender, PrintPageEventArgs ppeArgs)
        {
            //Get the Graphics object
            Graphics g = ppeArgs.Graphics;
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            //Read margins from PrintPageEventArgs
            float leftMargin = ppeArgs.MarginBounds.Left;
            float topMargin = ppeArgs.MarginBounds.Top;
            string line = null;
            //Calculate the lines per page on the basis of the height of the page and the height of the font
            linesPerPage = ppeArgs.MarginBounds.Height /
            verdana10Font.GetHeight(g);
            //Now read lines one by one, using StreamReader
            while (count < linesPerPage &&
            ((line = reader.ReadLine()) != null))
            {
                //Calculate the starting position
                yPos = topMargin + (count *
                verdana10Font.GetHeight(g));
                //Draw text
                g.DrawString(line, verdana10Font, Brushes.Black,
                leftMargin, yPos, new StringFormat());
                //Move to next line
                count++;
            }
            //If PrintPageEventArgs has more pages to print
            if (line != null)
            {
                ppeArgs.HasMorePages = true;
            }
            else
            {
                ppeArgs.HasMorePages = false;
            }

           
        }

        private void DelPrintTxt()
        {
            File.Delete(@"C:\Users\Jonata\AppData\Roaming\Notepad+++\imprimir.txt");
        }

        private void SavePrint()
        {
            StreamWriter sw = new StreamWriter(@"C:\Users\Jonata\AppData\Roaming\Notepad+++\imprimir.txt");
            sw.Write(rich_MainText.Text);
            sw.Close();
        }
//-----------------------------------------------------------------------------
        private void salvarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.ShowDialog();
               

                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(fs);
                sw.Write(rich_MainText.Text);
                sw.Close();
                fs.Close();

                saved = true;
            }
            catch { 
            }
        }

        private void fecharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                Salvar();
            }
            else
            {
                Close();
                Application.Exit();
            }

            Directory.Delete(@"C:\Users\Jonata\AppData\Roaming\Notepad+++");
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("a");
            if (!saved)
            {
                Salvar();
            }
            else
            {
                Close();
            }
        }

       
    }
}
