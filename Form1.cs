using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace Dzielnice
{
    //Ulica [skrócona nazwa];Ulica [pełna nazwa];Numery;Sektor;Dzielnica
    public struct ulica {
        public string skrot;
        public string ulica_r;
        public string ulica_l;
        public string uwagi;
        public string dzielnica;
        public string sektor;
    }

    public partial class Form1 : Form
    {
        ArrayList ulice;

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ulice == null)
            {

            }
            else
            {
                listView1.BeginUpdate();
                listView1.Items.Clear();
                string text = textBox1.Text.ToLower();
                foreach (ulica ulicaElem in ulice)
                {
                    if (ulicaElem.ulica_l.Contains(text))
                    {
                        ListViewItem item = new ListViewItem(new string[] { ulicaElem.skrot, ulicaElem.ulica_r, ulicaElem.uwagi, ulicaElem.dzielnica, ulicaElem.sektor });
                        listView1.Items.Add(item);
                    }
                }
                // foreach(ListView1.)
                foreach (ColumnHeader column in listView1.Columns)
                {
                    column.Width = -1;
                }
                listView1.EndUpdate();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void spis_laduj()
        {
            ulice = new ArrayList();
            StreamReader reader = new StreamReader("dzielnice.csv");
            while (reader.Peek() != -1)
            {
                string[] ulicaLine = reader.ReadLine().Split(';');
                ulica ulicaElem = new ulica();
                ulicaElem.skrot = ulicaLine[0];
                ulicaElem.ulica_r = ulicaLine[1];
                ulicaElem.ulica_l = ulicaLine[1].ToLower();
                ulicaElem.uwagi = ulicaLine[2];
                ulicaElem.sektor = ulicaLine[3];
                ulicaElem.dzielnica = ulicaLine[4];
                ulice.Add(ulicaElem);

            }
            reader.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("dzielnice.csv") == false)
            {
                MessageBox.Show("Brak pliku z dzielnicami. Aktualizuj spis!");
            }
            else
            {
                spis_laduj();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/ideaconnect/gdansk-dzielnice/master/csv/dzielnice.csv.sha1");
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                string response = reader.ReadToEnd();
                reader.Close();
                string sum = "";
                if (File.Exists("dzielnice.csv.sha1") == true)
                {
                    reader = new StreamReader("dzielnice.csv.sha1");
                    sum = reader.ReadToEnd();
                    reader.Close();
                }

                if (sum == response)
                {
                    MessageBox.Show("Posiadasz aktualny spis!");
                }
                else
                {
                    if (File.Exists("dzielnice.csv"))
                    {
                        File.Delete("dzielnice.csv");
                    }
                    if (File.Exists("dzielnice.csv.sha1"))
                    {
                        File.Delete("dzielnice.csv.sha1");
                    }
                    StreamWriter writer = new StreamWriter("dzielnice.csv");
                    request = (HttpWebRequest)WebRequest.Create("https://raw.githubusercontent.com/ideaconnect/gdansk-dzielnice/master/csv/dzielnice.csv");
                    reader = new StreamReader(request.GetResponse().GetResponseStream());
                    string dzielniceResponse = reader.ReadToEnd();
                    reader.Close();
                    writer.Write(dzielniceResponse);
                    writer.Close();

                    writer = new StreamWriter("dzielnice.csv.sha1");
                    writer.Write(response);
                    writer.Close();
                    MessageBox.Show("Zaktualizowano!");
                    spis_laduj();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Nie można było pobrać pliku. Sprawdź swoje połączenie z siecią!");
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ideaconnect/gdansk-wyszukiwarka-dzielnic");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://idct.pl");
        }
    }
}
