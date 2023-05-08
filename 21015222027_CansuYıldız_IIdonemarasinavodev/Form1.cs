using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace _21015222027_CansuYıldız_IIdonemarasinavodev
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Showlist();
        }
        public List<Araba> arabas = new List<Araba>()
        {
            new Araba()
            {
                Plaka = "34cns74",
                Marka = "Mitsubishi",
                Model = "Lancer",
                Yakıt ="LPG",
                Renk = "Beyaz",
                Vites = "otomatik",
                KasaTipi ="Sedan",
                Açıklama = "temiz araç kazası yoktur",
            },
             new Araba()
            {
                Plaka = "34cf890",
                Marka = "Hyundai",
                Model = "i30",
                Yakıt ="LPG",
                Renk = "lacivert",
                Vites = "otomatik",
                KasaTipi ="Sedan",
                Açıklama = "temiz araç kazası yoktur",
            }
        };
        
        public void Showlist()
        {
            listView1.Items.Clear();
            foreach (Araba araba in arabas)
            {
                AddArabaToListView(araba);
            }
        }
        public void AddArabaToListView(Araba araba)
        {
            ListViewItem item=
                    new ListViewItem(new string[]
                {
                 araba.Plaka,
                 araba.Marka,
                 araba.Model,
                 araba.Yakıt,
                 araba.Renk,
                 araba.Vites,
                 araba.KasaTipi,
                 araba.Açıklama
                });
            item.Tag = araba;
            listView1.Items.Add(item);
        }
        void EditArabaOnlistView(ListViewItem aItem, Araba araba)
        {
            aItem.SubItems[0].Text = araba.Plaka;
            aItem.SubItems[1].Text = araba.Marka;
            aItem.SubItems[2].Text = araba.Model;
            aItem.SubItems[3].Text = araba.Yakıt;
            aItem.SubItems[4].Text = araba.Renk;
            aItem.SubItems[5].Text = araba.Vites;
            aItem.SubItems[6].Text = araba.KasaTipi;
            aItem.SubItems[7].Text = araba.Açıklama;
            aItem.Tag = araba;
        }
        private void AracEkle(object sender, EventArgs e)
        {
            Frmcar frm = new Frmcar()
            {
                Text = "Araç Ekle",
                StartPosition= FormStartPosition.CenterParent,
                araba = new Araba()
            };
            if(frm.ShowDialog()==DialogResult.OK)
            {
                arabas.Add(frm.araba);
                AddArabaToListView(frm.araba);
            }
        }

        private void ArabaDuzenle(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            
                return;
                ListViewItem aItem = listView1.SelectedItems[0];
            Araba secili = aItem.Tag as Araba;
            Frmcar frm = new Frmcar()
            {
                Text = "Araba Düzenle",
                StartPosition = FormStartPosition.CenterParent,
                araba = Clone (secili),

            };
              if (frm.ShowDialog() == DialogResult.OK)
            {
                secili = frm.araba;
                EditArabaOnlistView(aItem, secili);
            }
        }
        Araba Clone(Araba araba)
        {
            return new Araba()
            {
                id = araba.ID,
                Plaka = araba.Plaka,
                Marka = araba.Marka,
                Model = araba.Model,
                Yakıt = araba.Yakıt,
                Renk = araba.Renk,
                Vites = araba.Vites,
                KasaTipi = araba.KasaTipi,
                Açıklama = araba.Açıklama
            };
        }

        private void aracSil(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)

                return;
            ListViewItem aItem = listView1.SelectedItems[0];
            Araba secili = aItem.Tag as Araba;

          var sonuc =  MessageBox.Show($"Seçili araba silinsin mi? \n\n{secili.Plaka}", 
                "Silmeyi Onayla", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            if(sonuc == DialogResult.Yes)
            {
                arabas.Remove(secili);
                listView1.Items.Remove(aItem);
            }
        }

        private void save(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog()
            {
                Filter = "Json Formatı |*.json|Xml Formatı|*.xml",
            };
            if(sf.ShowDialog() == DialogResult.OK)
            {
               if (sf.FileName.EndsWith("json"))
                {
                  string data  = JsonSerializer.Serialize(arabas);
                    File.WriteAllText(sf.FileName, data);
                   
                }else if (sf.FileName.EndsWith("xml"))
                {
                    StreamWriter sw = new StreamWriter(sf.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Araba>));
                    serializer.Serialize(sw, arabas);
                    sw.Close();
                }
            }
        }
    }
    [Serializable]
    public class Araba
    {
        public string id;
        public string ID
        {
            get { if (id == null)
                    id = Guid.NewGuid().ToString();
                return id;
            }
            set { id = value; }
        }

        public string Plaka { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Yakıt { get; set; }
        public string Renk { get; set; }
        public string Vites { get; set; }
        public string KasaTipi { get; set; }
        public string Açıklama { get; set; }
    }
}
