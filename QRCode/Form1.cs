using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using QRCoder;

namespace QRCode_Front
{
    public partial class Form1 : Form
    {
        QRCodeGenerator gera = new QRCodeGenerator();

        string selected;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            ListViewItem listViewItem;
            FileInfo fileInfo;
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowser.SelectedPath;
                foreach (var item in Directory.GetFiles(textBox1.Text))
                {
                    fileInfo = new FileInfo(item);
                    if (fileInfo.Extension == ".xlsx")
                    {
                        listViewItem = new ListViewItem();
                        listViewItem.Text = fileInfo.Name;
                        listViewItem.SubItems.Add(fileInfo.FullName);
                        listViewItem.ImageIndex = 0;
                        listView1.Items.Add(listViewItem);
                    }

                    else if (fileInfo.Extension == ".pdf")
                    {
                        listViewItem = new ListViewItem();
                        listViewItem.Text = fileInfo.Name;
                        listViewItem.SubItems.Add(fileInfo.FullName);
                        listViewItem.ImageIndex = 1;
                        listView1.Items.Add(listViewItem);
                    }
                    if (fileInfo.Extension == ".docx")
                    {
                        listViewItem = new ListViewItem();
                        listViewItem.Text = fileInfo.Name;
                        listViewItem.SubItems.Add(fileInfo.FullName);
                        listViewItem.ImageIndex = 0;
                        listView1.Items.Add(listViewItem);
                    }
                }
                if(listView1.Items.Count==0)
                    MessageBox.Show("Não foram encontrados tipos de arquivo suportado!","Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            selected = listView1.SelectedItems[0].SubItems[1].Text;
        }

        private void Gerar(string url)
        {
            richTextBox1.Clear();
            /*QRCodeData dataCode = gera.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode codigo = new QRCode(dataCode);
            Bitmap imageCode = codigo.GetGraphic(20);

            /*
            MemoryStream ms = new MemoryStream();
            imageCode.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            Byte[] arquivoDesfeito = ms.ToArray();

            pictureBox1.Image = Image.FromStream(ms);
            richTextBox1.Text = Convert.ToBase64String(arquivoDesfeito);
            Stream stream = new MemoryStream(arquivoDesfeito);
            pictureBox2.Image = Image.FromStream(stream);*/

            //Convertendo o arquivo selecionado em stream
            FileStream file = new FileStream(url,FileMode.Open,FileAccess.Read);
            MemoryStream stream = new MemoryStream();
            file.CopyTo(stream);
  
            
            Byte[] arquivoByte = stream.ToArray();
            richTextBox1.Text = Convert.ToBase64String(arquivoByte);

            //Convertendo o arquivo em Byte
            //pictureBox2.Image = Image.FromStream(stream);

            //Gerando o QR Code com a url
            QRCodeData dataCode = gera.CreateQrCode(url,QRCodeGenerator.ECCLevel.Q);
            QRCode codigo = new QRCode(dataCode);
            Bitmap imageCode = codigo.GetGraphic(20);

            MemoryStream ms = new MemoryStream();
            imageCode.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            pictureBox1.Image = Image.FromStream(ms);
            //pictureBox2.Image = Image.FromStream(stream);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selected != "")
                Gerar(selected);
        }
    }
}
