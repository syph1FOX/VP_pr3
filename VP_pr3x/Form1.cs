using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Net.Http.Headers;

namespace VP_pr3x
{

    public partial class Form1 : Form
    {
        List<GNode> gList = null;
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ImageList images = new ImageList();
            //изображения грызунов
            images.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\img\\polevka.jpg"));
            images.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\img\\belka.jpg"));
            images.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\img\\kapibara.jpg"));
            images.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\img\\shinshila.jpg"));
            images.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\img\\tushkanchik.jpg"));
            images.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\img\\dikobraz.jpg"));
            images.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\img\\homyak.jpg"));
            //первое изображение
            images.Images.Add(Image.FromFile(Directory.GetCurrentDirectory() + "\\img\\megakapibara.jpg"));

            try
            {
                string path = Directory.GetCurrentDirectory() + "\\glist.json";
                string text = File.ReadAllText(path);
                gList = JsonSerializer.Deserialize<List<GNode>>(text, options: options);
            }
            catch
                {
                MessageBox.Show("Не удалось прочитать json файл", "Ошибка");
                this.Close();
                }


            //добавление семейств грызунов(при первом запуске). можно еще добавить при желании
            //gList = new List<GNode>();
            //GNode temp;
            //temp = new GNode("Полевки", "Мелкие вредители", "Повсюду", Directory.GetCurrentDirectory() + "\\img\\polevka.jpg", 2, 15);
            //gList.Add(temp);
            //temp = new GNode("Белки", "Пух", "Лес", Directory.GetCurrentDirectory() + "\\img\\belka.jpg", 3, 10);
            //gList.Add(temp);
            //temp = new GNode("Капибара", "Милейшие создания", "Оазисы", Directory.GetCurrentDirectory() + "\\img\\kapibara.jpg", 10, 65);
            //gList.Add(temp);
            //temp = new GNode("Шиншиллы", "Милашки", "Южная  Америка", Directory.GetCurrentDirectory() + "\\img\\shinshila.jpg", 5, 10);
            //gList.Add(temp);
            //temp = new GNode("Тушканчики", "Микро кенгурушки", "Азия и Северная Африка", Directory.GetCurrentDirectory() + "\\img\\tushkanchik.jpg", 6, 5);
            //gList.Add(temp);
            //temp = new GNode("Дикобразы", "Колючки", "Южная и Северная Америка", Directory.GetCurrentDirectory() + "\\img\\dikobraz.jpg", 15, 18);
            //gList.Add(temp);
            //temp = new GNode("Хомяки", "Пухлячки", "Повсюду", Directory.GetCurrentDirectory() + "\\img\\homyak.jpg", 3, 10);
            //gList.Add(temp);

            int i = 0, j;
            this.treeView1.ImageList = images;
            this.treeView1.Nodes[0].ImageIndex = images.Images.Count - 1;
            this.treeView1.Nodes[0].SelectedImageIndex = images.Images.Count - 1;
            foreach (var g in gList)
            {
                j = 0;
                TreeNode treeNode = new TreeNode(g.name);
                treeNode.Name = g.name;
                treeNode.ImageIndex = i;
                treeNode.SelectedImageIndex = i;
                this.treeView1.Nodes[0].Nodes.Add(treeNode);
                this.treeView1.Nodes[0].Nodes[i].Nodes.Add("","Описание", i, i);
                this.treeView1.Nodes[0].Nodes[i].Nodes[j++].Nodes.Add("", g.description, i, i);
                this.treeView1.Nodes[0].Nodes[i].Nodes.Add("", "Среда обитания", i, i);
                this.treeView1.Nodes[0].Nodes[i].Nodes[j++].Nodes.Add("", g.habitat, i, i);
                this.treeView1.Nodes[0].Nodes[i].Nodes.Add("", "Длительность жизни", i, i);
                if(g.maxAge > 4)
                    this.treeView1.Nodes[0].Nodes[i].Nodes[j++].Nodes.Add("", g.maxAge.ToString() + " лет", i, i);
                else
                    this.treeView1.Nodes[0].Nodes[i].Nodes[j++].Nodes.Add("", g.maxAge.ToString() + " года", i ,i);
                this.treeView1.Nodes[0].Nodes[i].Nodes.Add("", "Максимальный вес", i, i);
                this.treeView1.Nodes[0].Nodes[i].Nodes[j++].Nodes.Add("", g.maxWeight.ToString() + " кг", i, i);
                this.treeView1.Nodes[0].Nodes[i].Nodes.Add("", "Изображение", i, i);
                this.treeView1.Nodes[0].Nodes[i].Nodes[j++].Nodes.Add("", g.path, i, i);
                i++;
            }
            this.treeView1.Nodes[0].Expand();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (gList == null) { return; }
            string path = Directory.GetCurrentDirectory() + "\\glist.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize<List<GNode>>(fstream, gList, options);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string key = this.textBox1.Text;
            if (key == string.Empty)
                return;
            TreeNode[] temp = this.treeView1.Nodes[0].Nodes.Find(key, true);
            if (temp.Length > 0)
            {
                this.treeView1.Nodes[0].Collapse(false);
                this.treeView1.Nodes[0].Expand();
                temp[0].Expand();
                this.textBox1.Text = string.Empty;
                return;
            }
            MessageBox.Show("Ничего не найдено", "Ошибка поиска");
        }
    }

    [Serializable]
    class GNode
    {
        public string name { get; set; }
        public string description {  get; set; }

        public string habitat {  get; set; }

        public string path {  get; set; }
        

        public int maxAge { get; set; }

        public int maxWeight { get; set; }
        public GNode()
        {
        }
        public GNode(string name, string description, string habitat, string path, int maxAge, int maxWeight)
        {
            this.name = name;
            this.description = description;
            this.habitat = habitat;
            this.path = path;
            this.maxAge = maxAge;
            this.maxWeight = maxWeight;
        }
    }
}
