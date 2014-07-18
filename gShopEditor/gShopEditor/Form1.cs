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
using gShopEditor.Structure;

namespace gShopEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Elements element = new Elements();
        public gShop_126 gshop_126 = new gShop_126();
        public gShop_14x_Client gshop_14x_client = new gShop_14x_Client();
        gShop_14x_Server gshop14x_server = new gShop_14x_Server();
        ElementsReader reader = new ElementsReader();

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog()
            {
                InitialDirectory = "",
                Filter = "gshop.data|*gshop*.data|Все файлы|*.*",
                Title = "Загрузка gshop.data"
            };
            if (open.ShowDialog() != DialogResult.Cancel)
            {
                using (BinaryReader read = new BinaryReader(new FileStream(open.FileName, FileMode.Open, FileAccess.Read), Encoding.Unicode))
                {
                    if (radioButton6.Checked)
                        gShop126Read(read, gshop_126);
                    else if (radioButton7.Checked)
                        gShop14xRead(read, gshop_14x_client);
                    read.Close();
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                int count = 0;
                listBox2.Items.Clear();
                if (radioButton6.Checked)
                {
                    for (int i = 0; i < gshop_126.cats[listBox1.SelectedIndex].sub_cat_count; i++)
                    {
                        count = 0;
                        for (int j = 0; j < gshop_126.item_count; j++)
                            if (gshop_126.items[j].main_type == listBox1.SelectedIndex && gshop_126.items[j].sub_type == i)
                                count++;
                        string[] name = getDecoding(gshop_126.cats[listBox1.SelectedIndex].subcat[i].sub_cat_name, "Unicode").Split('\0');
                        listBox2.Items.Add(name[0] + " (" + count + ")");
                    }
                    string[] names = listBox1.SelectedItem.ToString().Split(' ');
                    textBox13.Text = names[0];
                }
                else if (radioButton7.Checked)
                {
                    for (int i = 0; i < gshop_14x_client.cats[listBox1.SelectedIndex].sub_cat_count; i++)
                    {
                        count = 0;
                        for (int j = 0; j < gshop_14x_client.item_count; j++)
                            if (gshop_14x_client.items[j].main_type == listBox1.SelectedIndex && gshop_14x_client.items[j].sub_type == i)
                                count++;
                        string[] name = getDecoding(gshop_14x_client.cats[listBox1.SelectedIndex].subcat[i].sub_cat_name, "Unicode").Split('\0');
                        listBox2.Items.Add(name[0] + " (" + count + ")");
                    }
                    string[] names = listBox1.SelectedItem.ToString().Split(' ');
                    textBox13.Text = names[0];
                }
            }                    
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex > -1)
            {
                int count = 0;
                dataGridView1.Rows.Clear();
                if (radioButton6.Checked)
                {
                    for (int i = 0; i < gshop_126.item_count; i++)
                    {
                        if (gshop_126.items[i].main_type == listBox1.SelectedIndex && gshop_126.items[i].sub_type == listBox2.SelectedIndex)
                        {
                            count++;
                            string[] name = getDecoding(gshop_126.items[i].name, "Unicode").Split('\0');
                            dataGridView1.Rows.Add(count, gshop_126.items[i].local_id, name[0]);
                        }
                    }
                    string[] tmp_name = listBox2.SelectedItem.ToString().Split(' ');
                    textBox14.Text = tmp_name[0];
                }
                else if (radioButton7.Checked)
                {
                    for (int i = 0; i < gshop_14x_client.item_count; i++)
                    {
                        if (gshop_14x_client.items[i].main_type == listBox1.SelectedIndex && gshop_14x_client.items[i].sub_type == listBox2.SelectedIndex)
                        {
                            count++;
                            string[] name = getDecoding(gshop_14x_client.items[i].name, "Unicode").Split('\0');
                            dataGridView1.Rows.Add(count, gshop_14x_client.items[i].local_id, name[0]);
                        }
                    }
                    string[] tmp_name = listBox2.SelectedItem.ToString().Split(' ');
                    textBox14.Text = tmp_name[0];
                }
            }
        }

        #region functions

        public string getDecoding(byte[] text, string encoding)
        {
            return Encoding.GetEncoding(encoding).GetString(text);
        }

        public byte[] getEncoding(string text, string encoding)
        {
            return Encoding.GetEncoding(encoding).GetBytes(text);
        }

        private string getConvertTimestampToString(int time)
        {
            return DateTime.Parse("01.01.1970").AddSeconds(time).ToShortDateString();
        }

        private int getConvertStringToTimestamp(string date)
        {
            return DateTime.Parse(date).Second;
        }

        private string getConvertSecondsToString(int time)
        {
            int days = time / 86400;
            time -= days * 86400;
            int hours = time / 3600;
            time -= hours * 3600;
            int minutes = time * 60;
            time -= minutes * 60;
            int seconds = time - minutes * 60;
            return (days.ToString("D2") + "-" + hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2"));
        }

        private int getConvertStringToSeconds(string time)
        {
            try
            {
                string[] arr = time.Split(new char[] { '-', ':' });
                return ((Int32.Parse(arr[0]) * 86400) + (Int32.Parse(arr[1]) * 3600) + (Int32.Parse(arr[2]) * 60) + Int32.Parse(arr[3]));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void gShop126Read(BinaryReader read, gShop_126 gshop)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            gshop.timestamp = read.ReadInt32();
            gshop.item_count = read.ReadInt32();
            gshop.items = new List<Items>(gshop.item_count);
            for (int i = 0; i < gshop.item_count; i++)
            {
                gshop.items.Add(new Items());
                gshop.items[i].local_id = read.ReadInt32();
                gshop.items[i].main_type = read.ReadInt32();
                gshop.items[i].sub_type = read.ReadInt32();
                string[] tmp_icon = getDecoding(read.ReadBytes(128), "GBK").Split('\0');
                gshop.items[i].icon = getEncoding(tmp_icon[0], "GBK");
                gshop.items[i].item_id = read.ReadUInt32();
                gshop.items[i].item_count = read.ReadUInt32();
                gshop.items[i].sell_options = new List<Sell_options>(4);
                for (int j = 0; j < 4; j++)
                {
                    gshop.items[i].sell_options.Add(new Sell_options());
                    gshop.items[i].sell_options[j].price = read.ReadInt32();
                    gshop.items[i].sell_options[j].until_time = read.ReadInt32();
                    gshop.items[i].sell_options[j].time = read.ReadInt32();
                }
                gshop.items[i].props = read.ReadUInt32();
                string[] tmp_desc = getDecoding(read.ReadBytes(1024), "Unicode").Split('\0');
                gshop.items[i].desc = getEncoding(tmp_desc[0], "Unicode");
                string[] tmp_name = getDecoding(read.ReadBytes(64), "Unicode").Split('\0');
                gshop.items[i].name = getEncoding(tmp_name[0], "Unicode");
            }
            gshop.cats = new List<Category>(8);
            for (int i = 0; i < 8; i++)
            {
                gshop.cats.Add(new Category());
                string[] tmp_cat_name = getDecoding(read.ReadBytes(128), "Unicode").Split('\0');
                gshop.cats[i].cat_name = getEncoding(tmp_cat_name[0], "Unicode");
                gshop.cats[i].sub_cat_count = read.ReadInt32();

                gshop.cats[i].subcat = new List<SubCat_Sett>(8);
                for (int j = 0; j < gshop.cats[i].sub_cat_count; j++)
                {
                    gshop.cats[i].subcat.Add(new SubCat_Sett());
                    string[] tmp_sub_cat_name = getDecoding(read.ReadBytes(128), "Unicode").Split('\0');
                    gshop.cats[i].subcat[j].sub_cat_name = getEncoding(tmp_sub_cat_name[0], "Unicode");
                }
                listBox1.Items.Add(Encoding.Unicode.GetString(gshop.cats[i].cat_name).Replace("\0", string.Empty) + " (" + gshop.cats[i].sub_cat_count + ")");
            }
        }

        private void gShop14xRead(BinaryReader read, gShop_14x_Client gshop)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            gshop.timestamp = read.ReadInt32();
            gshop.item_count = read.ReadInt32();
            gshop.items = new List<Items_1>(gshop.item_count);
            for (int i = 0; i < gshop.item_count; i++)
            {
                gshop.items.Add(new Items_1());
                gshop.items[i].local_id = read.ReadInt32();
                gshop.items[i].main_type = read.ReadInt32();
                gshop.items[i].sub_type = read.ReadInt32();
                string[] tmp_icon = getDecoding(read.ReadBytes(128), "GBK").Split('\0');
                gshop.items[i].icon = getEncoding(tmp_icon[0], "GBK");
                gshop.items[i].item_id = read.ReadUInt32();
                gshop.items[i].item_count = read.ReadUInt32();
                gshop.items[i].sell_options = new List<Sell_options_1>(4);
                for (int j = 0; j < 4; j++)
                {
                    gshop.items[i].sell_options.Add(new Sell_options_1());
                    gshop.items[i].sell_options[j].price = read.ReadUInt32();
                    gshop.items[i].sell_options[j].end_time = read.ReadUInt32();
                    gshop.items[i].sell_options[j].time = read.ReadUInt32();
                    gshop.items[i].sell_options[j].start_time = read.ReadUInt32();
                    gshop.items[i].sell_options[j].type = read.ReadInt32();
                    gshop.items[i].sell_options[j].day = read.ReadUInt32();
                    gshop.items[i].sell_options[j].status = read.ReadUInt32();
                    gshop.items[i].sell_options[j].flag = read.ReadUInt32();
                }
                string[] tmp_desc = getDecoding(read.ReadBytes(1024), "Unicode").Split('\0');
                gshop.items[i].desc = getEncoding(tmp_desc[0], "Unicode");
                string[] tmp_name = getDecoding(read.ReadBytes(64), "Unicode").Split('\0');
                gshop.items[i].name = getEncoding(tmp_name[0], "Unicode");
                gshop.items[i].idGift = read.ReadInt32();
                gshop.items[i].iGiftNum = read.ReadInt32();
                gshop.items[i].iGiftTime = read.ReadInt32();
                gshop.items[i].iLogPrice = read.ReadInt32();
            }
            gshop.cats = new List<Category>(8);
            for (int i = 0; i < 8; i++)
            {
                gshop.cats.Add(new Category());
                string[] tmp_cat_name = getDecoding(read.ReadBytes(128), "Unicode").Split('\0');
                gshop.cats[i].cat_name = getEncoding(tmp_cat_name[0], "Unicode");
                gshop.cats[i].sub_cat_count = read.ReadInt32();

                gshop.cats[i].subcat = new List<SubCat_Sett>(8);
                for (int j = 0; j < gshop.cats[i].sub_cat_count; j++)
                {
                    gshop.cats[i].subcat.Add(new SubCat_Sett());
                    string[] tmp_sub_cat_name = getDecoding(read.ReadBytes(128), "Unicode").Split('\0');
                    gshop.cats[i].subcat[j].sub_cat_name = getEncoding(tmp_sub_cat_name[0], "Unicode");
                }
                listBox1.Items.Add(Encoding.Unicode.GetString(gshop.cats[i].cat_name).Replace("\0", string.Empty) + " (" + gshop.cats[i].sub_cat_count + ")");
            }
        }

        public DataGridView getGrid()
        {
            return this.dataGridView1;
        }

        private void elementsRead(BinaryReader read, Elements element)
        {  
            #region elements.data
            element = reader.getElementsReader(element, read);
            /*
            element.version = read.ReadInt16();
            element.unk = read.ReadInt16();
            element.unk2 = read.ReadInt32();
            element.list1_count = read.ReadInt32();
            getChangePos(read, element.list1_count, 84);
            element.list2_count = read.ReadInt32();
            getChangePos(read, element.list2_count, 68);
            element.list3_count = read.ReadInt32();
            getChangePos(read, element.list3_count, 356);
            element.list4_count = read.ReadInt32();
            element.weapons = new List<ListToRead>(element.list4_count);
            for (int i = 0; i < element.list4_count; i++)
            {
                element.weapons.Add(new ListToRead());
                element.weapons[i].id = read.ReadInt32();
                read.BaseStream.Position += 8;
                element.weapons[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 388;
                element.weapons[i].surface = read.ReadBytes(128);
                if (element.version == 12)
                    read.BaseStream.Position += 812;
                else if (element.version > 12 && element.version <= 101)
                    read.BaseStream.Position += 828;
            }
            element.list5_count = read.ReadInt32();
            getChangePos(read, element.list5_count, 68);
            element.list6_count = read.ReadInt32();
            getChangePos(read, element.list6_count, 72);
            element.list7_count = read.ReadInt32();
            element.armor = new List<ListToRead>(element.list7_count);
            for (int i = 0; i < element.list7_count; i++)
            {
                element.armor.Add(new ListToRead());
                element.armor[i].id = read.ReadInt32();
                read.BaseStream.Position += 8;
                element.armor[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 160;
                element.armor[i].surface = read.ReadBytes(128);
                if (element.version == 12)
                    read.BaseStream.Position += 740;
                else if (element.version > 12 && element.version <= 101)
                    read.BaseStream.Position += 764;
            }
            element.list8_count = read.ReadInt32();
            getChangePos(read, element.list8_count, 68);
            element.list9_count = read.ReadInt32();
            getChangePos(read, element.list9_count, 72);
            element.list10_count = read.ReadInt32();
            element.ornaments = new List<ListToRead>(element.list10_count);
            for (int i = 0; i < element.list10_count; i++)
            {
                element.ornaments.Add(new ListToRead());
                element.ornaments[i].id = read.ReadInt32();
                read.BaseStream.Position += 8;
                element.ornaments[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 256;
                element.ornaments[i].surface = read.ReadBytes(128);
                if (element.version == 12)
                    read.BaseStream.Position += 696;
                else if (element.version > 12 && element.version <= 101)
                    read.BaseStream.Position += 708;
            }
            element.list11_count = read.ReadInt32();
            getChangePos(read, element.list11_count, 68);
            element.list12_count = read.ReadInt32();
            getChangePos(read, element.list12_count, 68);
            element.list13_count = read.ReadInt32();
            element.remedies = new List<ListToRead>(element.list13_count);
            for (int i = 0; i < element.list13_count; i++)
            {
                element.remedies.Add(new ListToRead());
                element.remedies[i].id = read.ReadInt32();
                read.BaseStream.Position += 8;
                element.remedies[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.remedies[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 44;
            }
            element.list14_count = read.ReadInt32();
            getChangePos(read, element.list14_count, 68);
            element.list15_count = read.ReadInt32();
            getChangePos(read, element.list15_count, 68);
            element.list16_count = read.ReadInt32();
            element.materials = new List<ListToRead>(element.list16_count);
            for (int i = 0; i < element.list16_count; i++)
            {
                element.materials.Add(new ListToRead());
                element.materials[i].id = read.ReadInt32();
                read.BaseStream.Position += 8;
                element.materials[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.materials[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 36;
            }
            element.list17_count = read.ReadInt32();
            getChangePos(read, element.list17_count, 68);
            element.list18_count = read.ReadInt32();
            element.atk_hierogr = new List<ListToRead>(element.list18_count);
            for (int i = 0; i < element.list18_count; i++)
            {
                element.atk_hierogr.Add(new ListToRead());
                element.atk_hierogr[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.atk_hierogr[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.atk_hierogr[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 36;
            }
            element.list19_count = read.ReadInt32();
            getChangePos(read, element.list19_count, 68);
            element.list20_count = read.ReadInt32();
            element.def_hierogr = new List<ListToRead>(element.list20_count);
            for (int i = 0; i < element.list20_count; i++)
            {
                element.def_hierogr.Add(new ListToRead());
                element.def_hierogr[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.def_hierogr[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.def_hierogr[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 296;
            }
            if (element.version == 12)
                read.BaseStream.Position += 19;
            else if (element.version == 60)
                read.BaseStream.Position += 23;
            element.list21_count = read.ReadInt32();
            getChangePos(read, element.list21_count, 68);
            element.list22_count = read.ReadInt32();
            element.skills = new List<ListToRead>(element.list22_count);
            for (int i = 0; i < element.list22_count; i++)
            {
                element.skills.Add(new ListToRead());
                element.skills[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.skills[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.skills[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 20;
            }
            element.list23_count = read.ReadInt32();
            element.flyes = new List<ListToRead>(element.list23_count);
            for (int i = 0; i < element.list23_count; i++)
            {
                element.flyes.Add(new ListToRead());
                element.flyes[i].id = read.ReadInt32();
                element.flyes[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 256;
                element.flyes[i].surface = read.ReadBytes(128);
                if (element.version == 12)
                    read.BaseStream.Position += 64;
                else if (element.version == 60)
                    read.BaseStream.Position += 72;
            }
            element.list24_count = read.ReadInt32();
            getChangePos(read, element.list24_count, 488);
            element.list25_count = read.ReadInt32();
            getChangePos(read, element.list25_count, 348);
            element.list26_count = read.ReadInt32();
            getChangePos(read, element.list26_count, 4);
            element.list27_count = read.ReadInt32();
            element.key_items = new List<ListToRead>(element.list27_count);
            for (int i = 0; i < element.list27_count; i++)
            {
                element.key_items.Add(new ListToRead());
                element.key_items[i].id = read.ReadInt32();
                element.key_items[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.key_items[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 28;
            }
            element.list28_count = read.ReadInt32();
            getChangePos(read, element.list28_count, 348);
            element.list29_count = read.ReadInt32();
            element.quest_items = new List<ListToRead>(element.list29_count);
            for (int i = 0; i < element.list29_count; i++)
            {
                element.quest_items.Add(new ListToRead());
                element.quest_items[i].id = read.ReadInt32();
                element.quest_items[i].name = read.ReadBytes(64);
                element.quest_items[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 12;
            }
            element.list30_count = read.ReadInt32();
            getChangePos(read, element.list30_count, 888);
            element.list31_count = read.ReadInt32();
            getChangePos(read, element.list31_count, 68);
            element.list32_count = read.ReadInt32();
            element.ammo = new List<ListToRead>(element.list32_count);
            for (int i = 0; i < element.list32_count; i++)
            {
                element.ammo.Add(new ListToRead());
                element.ammo[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.ammo[i].name = read.ReadBytes(192);
                read.BaseStream.Position += 128;
                element.ammo[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 436;
            }
            element.list33_count = read.ReadInt32();
            getChangePos(read, element.list33_count, 68);
            element.list34_count = read.ReadInt32();
            getChangePos(read, element.list34_count, 340);
            element.list35_count = read.ReadInt32();
            getChangePos(read, element.list35_count, 68);
            element.list36_count = read.ReadInt32();
            element.soulgems = new List<ListToRead>(element.list36_count);
            for (int i = 0; i < element.list36_count; i++)
            {
                element.soulgems.Add(new ListToRead());
                element.soulgems[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.soulgems[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.soulgems[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 108;
            }
            element.list37_count = read.ReadInt32();
            if (element.version <= 60)
                getChangePos(read, element.list37_count, 84);
            element.list38_count = read.ReadInt32();
            if (element.version <= 60)
                getChangePos(read, element.list38_count, 196);
            element.list39_count = read.ReadInt32();
            if (element.version <= 12)
                getChangePos(read, element.list39_count, 1500);
            else if (element.version == 60)
                getChangePos(read, element.list39_count, 1552);
            element.list40_count = read.ReadInt32();
            if (element.version <= 60)
                getChangePos(read, element.list40_count, 72);
            element.list41_count = read.ReadInt32();
            if (element.version <= 12)
                getChangePos(read, element.list41_count, 1224);
            else if (element.version == 60)
                getChangePos(read, element.list41_count, 2280);
            element.list42_count = read.ReadInt32();
            if (element.version <= 60)
                getChangePos(read, element.list42_count, 72);
            element.list43_count = read.ReadInt32();
            if (element.version <= 60)
                getChangePos(read, element.list43_count, 72);
            element.list44_count = read.ReadInt32();
            if (element.version <= 60)
                getChangePos(read, element.list44_count, 200);
            element.list45_count = read.ReadInt32();
            if (element.version <= 60)
                getChangePos(read, element.list45_count, 200);
            element.list46_count = read.ReadInt32();
            if (element.version <= 12)
                getChangePos(read, element.list46_count, 196);
            else if (element.version == 60)
                getChangePos(read, element.list46_count, 1092);
            element.list47_count = read.ReadInt32();
            getChangePos(read, element.list47_count, 196);
            element.list48_count = read.ReadInt32();
            getChangePos(read, element.list48_count, 644);
            element.list49_count = read.ReadInt32();
            getChangePos(read, element.list49_count, 584);
            element.list50_count = read.ReadInt32();
            getChangePos(read, element.list50_count, 72);
            element.list51_count = read.ReadInt32();
            getChangePos(read, element.list51_count, 460);
            element.list52_count = read.ReadInt32();
            getChangePos(read, element.list52_count, 68);
            element.list53_count = read.ReadInt32();
            getChangePos(read, element.list53_count, 1228);
            element.list54_count = read.ReadInt32();
            getChangePos(read, element.list54_count, 68);
            element.list55_count = read.ReadInt32();
            getChangePos(read, element.list55_count, 1228);
            element.list56_count = read.ReadInt32();
            getChangePos(read, element.list56_count, 72);
            element.list57_count = read.ReadInt32();
            getChangePos(read, element.list57_count, 68);
            element.list58_count = read.ReadInt32();
            getChangePos(read, element.list58_count, 848);
            element.list59_count = read.ReadInt32();
            read.BaseStream.Position += 3294300;
            element.list60_count = read.ReadInt32();
            getChangePos(read, element.list60_count, 476);
            element.list61_count = read.ReadInt32();
            getChangePos(read, element.list61_count, 348);
            element.list62_count = read.ReadInt32();
            getChangePos(read, element.list62_count, 4);
            element.list63_count = read.ReadInt32();
            getChangePos(read, element.list63_count, 336);
            element.list64_count = read.ReadInt32();
            getChangePos(read, element.list64_count, 468);
            element.list65_count = read.ReadInt32();
            getChangePos(read, element.list65_count, 340);
            element.list66_count = read.ReadInt32();
            getChangePos(read, element.list66_count, 208);
            element.list67_count = read.ReadInt32();
            getChangePos(read, element.list67_count, 204);
            element.list68_count = read.ReadInt32();
            getChangePos(read, element.list68_count, 68);
            element.list69_count = read.ReadInt32();
            getChangePos(read, element.list69_count, 68);
            element.list70_count = read.ReadInt32();
            getChangePos(read, element.list70_count, 404);
            element.list71_count = read.ReadInt32();
            getChangePos(read, element.list71_count, 196);
            element.list72_count = read.ReadInt32();
            getChangePos(read, element.list72_count, 160);
            element.list73_count = read.ReadInt32();
            getChangePos(read, element.list73_count, 612);
            element.list74_count = read.ReadInt32();
            getChangePos(read, element.list74_count, 488);
            element.list75_count = read.ReadInt32();
            getChangePos(read, element.list75_count, 404);
            element.list76_count = read.ReadInt32();
            element.quest_rewards = new List<ListToRead>(element.list76_count);
            for (int i = 0; i < element.list76_count; i++)
            {
                element.quest_rewards.Add(new ListToRead());
                element.quest_rewards[i].id = read.ReadInt32();
                element.quest_rewards[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.quest_rewards[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 20;

            }
            element.list77_count = read.ReadInt32();
            getChangePos(read, element.list77_count, 340);
            element.list78_count = read.ReadInt32();
            getChangePos(read, element.list78_count, 668);
            element.list79_count = read.ReadInt32();
            getChangePos(read, element.list79_count, 68);
            element.list80_count = read.ReadInt32();
            element.resources = new List<ListToRead>(element.list80_count);
            for (int i = 0; i < element.list80_count; i++)
            {
                element.resources.Add(new ListToRead());
                element.resources[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.resources[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 380;
            }
            element.list81_count = read.ReadInt32();
            getChangePos(read, element.list81_count, 72);
            element.list82_count = read.ReadInt32();
            getChangePos(read, element.list82_count, 68);
            element.list83_count = read.ReadInt32();
            getChangePos(read, element.list83_count, 72);
            element.list84_count = read.ReadInt32();
            element.fashion = new List<ListToRead>(element.list84_count);
            for (int i = 0; i < element.list84_count; i++)
            {
                element.fashion.Add(new ListToRead());
                element.fashion[i].id = read.ReadInt32();
                read.BaseStream.Position += 8;
                element.fashion[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 160;
                element.fashion[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 40;
            }
            element.list85_count = read.ReadInt32();
            getChangePos(read, element.list85_count, 68);
            element.list86_count = read.ReadInt32();
            getChangePos(read, element.list86_count, 68);
            element.list87_count = read.ReadInt32();
            getChangePos(read, element.list87_count, 488);
            element.list88_count = read.ReadInt32();
            getChangePos(read, element.list88_count, 68);
            element.list89_count = read.ReadInt32();
            getChangePos(read, element.list89_count, 68);
            element.list90_count = read.ReadInt32();
            getChangePos(read, element.list90_count, 2412);
            element.list91_count = read.ReadInt32();
            getChangePos(read, element.list91_count, 292);
            element.list92_count = read.ReadInt32();
            getChangePos(read, element.list92_count, 68);
            element.list93_count = read.ReadInt32();
            getChangePos(read, element.list93_count, 344);
            element.list94_count = read.ReadInt32();
            getChangePos(read, element.list94_count, 68);
            element.list95_count = read.ReadInt32();
            getChangePos(read, element.list95_count, 480);
            element.list96_count = read.ReadInt32();
            element.pet_eggs = new List<ListToRead>(element.list96_count);
            for (int i = 0; i < element.list96_count; i++)
            {
                element.pet_eggs.Add(new ListToRead());
                element.pet_eggs[i].id = read.ReadInt32();
                element.pet_eggs[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.pet_eggs[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 304;
            }
            element.list97_count = read.ReadInt32();
            element.pet_food = new List<ListToRead>(element.list97_count);
            for (int i = 0; i < element.list97_count; i++)
            {
                element.pet_food.Add(new ListToRead());
                element.pet_food[i].id = read.ReadInt32();
                element.pet_food[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.pet_food[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 36;
            }
            element.list98_count = read.ReadInt32();
            getChangePos(read, element.list98_count, 344);
            element.list99_count = read.ReadInt32();
            element.fireworks = new List<ListToRead>(element.list99_count);
            for (int i = 0; i < element.list99_count; i++)
            {
                element.fireworks.Add(new ListToRead());
                element.fireworks[i].id = read.ReadInt32();
                element.fireworks[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.fireworks[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 156;
            }
            element.list100_count = read.ReadInt32();
            getChangePos(read, element.list100_count, 344);
            read.BaseStream.Position += 95;
            element.list101_count = read.ReadInt32();
            getChangePos(read, element.list101_count, 148);
            element.list102_count = read.ReadInt32();
            getChangePos(read, element.list102_count, 1092);
            element.list103_count = read.ReadInt32();
            getChangePos(read, element.list103_count, 368);
            element.list104_count = read.ReadInt32();
            getChangePos(read, element.list104_count, 76);
            element.list105_count = read.ReadInt32();
            getChangePos(read, element.list105_count, 584);
            element.list106_count = read.ReadInt32();
            getChangePos(read, element.list106_count, 76);
            element.list107_count = read.ReadInt32();
            element.potions = new List<ListToRead>(element.list107_count);
            for (int i = 0; i < element.list107_count; i++)
            {
                element.potions.Add(new ListToRead());
                element.potions[i].id = read.ReadInt32();
                element.potions[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.potions[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 32;
            }
            element.list108_count = read.ReadInt32();
            element.refining = new List<ListToRead>(element.list108_count);
            for (int i = 0; i < element.list108_count; i++)
            {
                element.refining.Add(new ListToRead());
                element.refining[i].id = read.ReadInt32();
                element.refining[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.refining[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 112;
            }
            element.list109_count = read.ReadInt32();
            getChangePos(read, element.list109_count, 344);
            element.list110_count = read.ReadInt32();
            getChangePos(read, element.list110_count, 76);
            element.list111_count = read.ReadInt32();
            getChangePos(read, element.list111_count, 76);
            element.list112_count = read.ReadInt32();
            getChangePos(read, element.list112_count, 76);
            element.list113_count = read.ReadInt32();
            element.heaven_books = new List<ListToRead>(element.list113_count);
            for (int i = 0; i < element.list113_count; i++)
            {
                element.heaven_books.Add(new ListToRead());
                element.heaven_books[i].id = read.ReadInt32();
                element.heaven_books[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.heaven_books[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 60;
            }
            element.list114_count = read.ReadInt32();
            element.chat_speakers = new List<ListToRead>(element.list114_count);
            for (int i = 0; i < element.list114_count; i++)
            {
                element.chat_speakers.Add(new ListToRead());
                element.chat_speakers[i].id = read.ReadInt32();
                element.chat_speakers[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.chat_speakers[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 24;
            }
            element.list115_count = read.ReadInt32();
            element.mp_hierogr = new List<ListToRead>(element.list115_count);
            for (int i = 0; i < element.list115_count; i++)
            {
                element.mp_hierogr.Add(new ListToRead());
                element.mp_hierogr[i].id = read.ReadInt32();
                element.mp_hierogr[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.mp_hierogr[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 32;
            }
            element.list116_count = read.ReadInt32();
            element.hp_hierogr = new List<ListToRead>(element.list116_count);
            for (int i = 0; i < element.list116_count; i++)
            {
                element.hp_hierogr.Add(new ListToRead());
                element.hp_hierogr[i].id = read.ReadInt32();
                element.hp_hierogr[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.hp_hierogr[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 32;
            }
            element.list117_count = read.ReadInt32();
            element.multi_exp = new List<ListToRead>(element.list117_count);
            for (int i = 0; i < element.list117_count; i++)
            {
                element.multi_exp.Add(new ListToRead());
                element.multi_exp[i].id = read.ReadInt32();
                element.multi_exp[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.multi_exp[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 24;
            }
            element.list118_count = read.ReadInt32();
            element.teleport = new List<ListToRead>(element.list118_count);
            for (int i = 0; i < element.list118_count; i++)
            {
                element.teleport.Add(new ListToRead());
                element.teleport[i].id = read.ReadInt32();
                element.teleport[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.teleport[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 20;
            }
            element.list119_count = read.ReadInt32();
            element.dyes = new List<ListToRead>(element.list119_count);
            for (int i = 0; i < element.list119_count; i++)
            {
                element.dyes.Add(new ListToRead());
                element.dyes[i].id = read.ReadInt32();
                element.dyes[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.dyes[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 44;
            }
            */
            #endregion      
        }

        private void getChangePos(BinaryReader read, int count, long position)
        {
            read.BaseStream.Position += position * count;
        }

        #endregion

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (radioButton6.Checked)
            {
                textBox1.Text = gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_id.ToString();
                textBox2.Text = getDecoding(gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].name, "Unicode");
                textBox3.Text = gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_count.ToString();
                textBox4.Text = getDecoding(gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].icon, "GBK");
                textBox5.Text = gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].price.ToString();
                textBox6.Text = getConvertTimestampToString(gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].until_time);
                textBox7.Text = getConvertSecondsToString(gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].time);
                richTextBox1.Text = getDecoding(gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].desc, "Unicode");
                switch (gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props)
                {
                    case 0:
                        {
                            radioButton1.Checked = true;
                            break;
                        }
                    case 1:
                        {
                            radioButton2.Checked = true;
                            break;
                        }
                    case 2:
                        {
                            radioButton3.Checked = true;
                            break;
                        }
                    case 3:
                        {
                            radioButton4.Checked = true;
                            break;
                        }
                    case 4:
                        {
                            radioButton5.Checked = true;
                            break;
                        }
                }
            }
            else if (radioButton7.Checked)
            {
                textBox1.Text = gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_id.ToString();
                textBox2.Text = getDecoding(gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].name, "Unicode");
                textBox3.Text = gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_count.ToString();
                textBox4.Text = getDecoding(gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].icon, "GBK");
                textBox5.Text = gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].price.ToString();
                textBox6.Text = getConvertTimestampToString((int)gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].end_time);
                textBox7.Text = getConvertSecondsToString((int)gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].start_time);
                richTextBox1.Text = getDecoding(gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].desc, "Unicode");
                switch (gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].status)
                {
                    case 0:
                        {
                            radioButton1.Checked = true;
                            break;
                        }
                    case 1:
                        {
                            radioButton2.Checked = true;
                            break;
                        }
                    case 2:
                        {
                            radioButton3.Checked = true;
                            break;
                        }
                    case 3:
                        {
                            radioButton4.Checked = true;
                            break;
                        }
                    case 4:
                        {
                            radioButton5.Checked = true;
                            break;
                        }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton6.Checked)
                {
                    for (int i = 0; i < gshop_126.item_count; i++)
                    {
                        if (gshop_126.items[i].main_type == listBox1.SelectedIndex && gshop_126.items[i].sub_type == listBox2.SelectedIndex)
                            gshop_126.items[i].sell_options[0].price = Convert.ToInt32(Single.Parse(textBox9.Text) * 100);
                    }
                }
                else if (radioButton7.Checked)
                {
                    for (int i = 0; i < gshop_14x_client.item_count; i++)
                    {
                        if (gshop_14x_client.items[i].main_type == listBox1.SelectedIndex && gshop_14x_client.items[i].sub_type == listBox2.SelectedIndex)
                            gshop_14x_client.items[i].sell_options[0].price = Convert.ToUInt32(Single.Parse(textBox9.Text) * 100);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog()
            {
                InitialDirectory = "",
                Filter = "gshop.data|*gshop*.data|Все файлы|*.*",
                Title = "Сохранение gshop.data"
            };
            if (save.ShowDialog() != DialogResult.Cancel)
            {
                using (BinaryWriter write = new BinaryWriter(new FileStream(save.FileName, FileMode.Create, FileAccess.Write), Encoding.Unicode))
                {
                    if (radioButton6.Checked)
                    {
                        write.Write(gshop_126.timestamp);
                        write.Write(gshop_126.item_count);
                        for (int i = 0; i < gshop_126.item_count; i++)
                        {
                            write.Write(gshop_126.items[i].local_id);
                            write.Write(gshop_126.items[i].main_type);
                            write.Write(gshop_126.items[i].sub_type);
                            write.Write(gshop_126.items[i].icon);
                            getWriteNull(write, gshop_126.items[i].icon.Length, 128);
                            write.Write(gshop_126.items[i].item_id);
                            write.Write(gshop_126.items[i].item_count);
                            for (int j = 0; j < 4; j++)
                            {
                                write.Write(gshop_126.items[i].sell_options[j].price);
                                write.Write(gshop_126.items[i].sell_options[j].until_time);
                                write.Write(gshop_126.items[i].sell_options[j].time);
                            }
                            write.Write(gshop_126.items[i].props);
                            write.Write(gshop_126.items[i].desc);
                            getWriteNull(write, gshop_126.items[i].desc.Length, 1024);
                            write.Write(gshop_126.items[i].name);
                            getWriteNull(write, gshop_126.items[i].name.Length, 64);
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            write.Write(gshop_126.cats[i].cat_name);
                            getWriteNull(write, gshop_126.cats[i].cat_name.Length, 128);
                            write.Write(gshop_126.cats[i].sub_cat_count);
                            for (int j = 0; j < gshop_126.cats[i].sub_cat_count; j++)
                            {
                                write.Write(gshop_126.cats[i].subcat[j].sub_cat_name);
                                getWriteNull(write, gshop_126.cats[i].subcat[j].sub_cat_name.Length, 128);
                            }
                        }
                    }
                    else if (radioButton7.Checked)
                    {
                        write.Write(gshop_14x_client.timestamp);
                        write.Write(gshop_14x_client.item_count);
                        for (int i = 0; i < gshop_14x_client.item_count; i++)
                        {
                            write.Write(gshop_14x_client.items[i].local_id);
                            write.Write(gshop_14x_client.items[i].main_type);
                            write.Write(gshop_14x_client.items[i].sub_type);
                            write.Write(gshop_14x_client.items[i].icon);
                            getWriteNull(write, gshop_14x_client.items[i].icon.Length, 128);
                            write.Write(gshop_14x_client.items[i].item_id);
                            write.Write(gshop_14x_client.items[i].item_count);
                            for (int j = 0; j < 4; j++)
                            {
                                write.Write(gshop_14x_client.items[i].sell_options[j].price);
                                write.Write(gshop_14x_client.items[i].sell_options[j].end_time);
                                write.Write(gshop_14x_client.items[i].sell_options[j].time);
                                write.Write(gshop_14x_client.items[i].sell_options[j].start_time);
                                write.Write(gshop_14x_client.items[i].sell_options[j].type);
                                write.Write(gshop_14x_client.items[i].sell_options[j].day);
                                write.Write(gshop_14x_client.items[i].sell_options[j].status);
                                write.Write(gshop_14x_client.items[i].sell_options[j].flag);
                            }
                            write.Write(gshop_14x_client.items[i].desc);
                            getWriteNull(write, gshop_14x_client.items[i].desc.Length, 1024);
                            write.Write(gshop_14x_client.items[i].name);
                            getWriteNull(write, gshop_14x_client.items[i].name.Length, 64);
                            write.Write(gshop_14x_client.items[i].idGift);
                            write.Write(gshop_14x_client.items[i].iGiftNum);
                            write.Write(gshop_14x_client.items[i].iGiftTime);
                            write.Write(gshop_14x_client.items[i].iLogPrice);
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            write.Write(gshop_14x_client.cats[i].cat_name);
                            getWriteNull(write, gshop_14x_client.cats[i].cat_name.Length, 128);
                            write.Write(gshop_14x_client.cats[i].sub_cat_count);
                            for (int j = 0; j < gshop_14x_client.cats[i].sub_cat_count; j++)
                            {
                                write.Write(gshop_14x_client.cats[i].subcat[j].sub_cat_name);
                                getWriteNull(write, gshop_14x_client.cats[i].subcat[j].sub_cat_name.Length, 128);
                            }
                        }
                    }
                    write.Close();
                }
            }
        }

        private void getWriteNull(BinaryWriter write, int data_length, int max_length)
        {
            if (data_length < max_length)
                for (int i = data_length; i < max_length; i++)
                    write.Write((byte)0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                    {
                        if (radioButton6.Checked)
                            gshop_126.items[Convert.ToInt32(dataGridView1[1, i].Value)].sell_options[0].price = Convert.ToInt32(Single.Parse(textBox9.Text) * 100);
                        else if (radioButton7.Checked)
                            gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, i].Value)].sell_options[0].price = Convert.ToUInt32(Single.Parse(textBox9.Text) * 100);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog()
            {
                InitialDirectory = "",
                Filter = "gshop.data|*gshop*.data|Все файлы|*.*",
                Title = "Загрузка gshop.data"
            };
            if (open.ShowDialog() != DialogResult.Cancel)
                textBox11.Text = open.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog()
            {
                InitialDirectory = "",
                Filter = "elements.data|*elements*.data|Все файлы|*.*",
                Title = "Загрузка elements.data"
            };
            if (open.ShowDialog() != DialogResult.Cancel)
                textBox12.Text = open.FileName;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (BinaryReader read = new BinaryReader(new FileStream(textBox11.Text, FileMode.Open, FileAccess.Read), Encoding.Unicode))
            {
                if (radioButton6.Checked)
                    gShop126Read(read, gshop_126);
                else if (radioButton7.Checked)
                    gShop14xRead(read, gshop_14x_client);
                else
                    MessageBox.Show("Не выбрана версия для загрузки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                read.Close();
            }
            using (BinaryReader read = new BinaryReader(new FileStream(textBox12.Text, FileMode.Open, FileAccess.Read), Encoding.Unicode))
            {
                elementsRead(read, element);
                read.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count >= 8)
                MessageBox.Show("Невозможно добавить категорию.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                listBox1.Items.Add("New");
                if (radioButton6.Checked)
                    gshop_126.cats.Add(new Category());
                else if (radioButton7.Checked)
                    gshop_14x_client.cats.Add(new Category());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            if (radioButton6.Checked)
                gshop_126.cats.RemoveAt(listBox1.SelectedIndex);
            else if (radioButton7.Checked)
                gshop_14x_client.cats.RemoveAt(listBox1.SelectedIndex);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                listBox1.Items[listBox1.SelectedIndex] = textBox13.Text + " (" + gshop_126.cats[listBox1.SelectedIndex].sub_cat_count + ")";
                gshop_126.cats[listBox1.SelectedIndex].cat_name = getEncoding(textBox13.Text, "Unicode");
            }
            else if (radioButton7.Checked)
            {
                listBox1.Items[listBox1.SelectedIndex] = textBox13.Text + " (" + gshop_14x_client.cats[listBox1.SelectedIndex].sub_cat_count + ")";
                gshop_14x_client.cats[listBox1.SelectedIndex].cat_name = getEncoding(textBox13.Text, "Unicode");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (radioButton6.Checked)
            {
                for (int i = 0; i < gshop_126.item_count; i++)
                    if (gshop_126.items[i].main_type == listBox1.SelectedIndex && gshop_126.items[i].sub_type == listBox2.SelectedIndex)
                        count++;
                gshop_126.cats[listBox1.SelectedIndex].subcat[listBox2.SelectedIndex].sub_cat_name = getEncoding(textBox14.Text, "Unicode");
            }
            else if (radioButton7.Checked)
            {
                for (int i = 0; i < gshop_14x_client.item_count; i++)
                    if (gshop_14x_client.items[i].main_type == listBox1.SelectedIndex && gshop_14x_client.items[i].sub_type == listBox2.SelectedIndex)
                        count++;
                gshop_14x_client.cats[listBox1.SelectedIndex].subcat[listBox2.SelectedIndex].sub_cat_name = getEncoding(textBox14.Text, "Unicode");
            }
            listBox2.Items[listBox2.SelectedIndex] = textBox14.Text + " (" + count + ")";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                if (gshop_126.cats[listBox1.SelectedIndex].sub_cat_count >= 8)
                    MessageBox.Show("Невозможно добавить подкатегорию.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    listBox2.Items.Add("New");
                    gshop_126.cats[listBox1.SelectedIndex].subcat.Add(new SubCat_Sett());
                    gshop_126.cats[listBox1.SelectedIndex].subcat[gshop_126.cats[listBox1.SelectedIndex].sub_cat_count].sub_cat_name = getEncoding("New", "Unicode");
                    gshop_126.cats[listBox1.SelectedIndex].sub_cat_count++;
                }
            }
            else if (radioButton7.Checked)
            {
                if (gshop_14x_client.cats[listBox1.SelectedIndex].sub_cat_count >= 8)
                    MessageBox.Show("Невозможно добавить подкатегорию.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    listBox2.Items.Add("New");
                    gshop_14x_client.cats[listBox1.SelectedIndex].subcat.Add(new SubCat_Sett());
                    gshop_14x_client.cats[listBox1.SelectedIndex].subcat[gshop_14x_client.cats[listBox1.SelectedIndex].sub_cat_count].sub_cat_name = getEncoding("New", "Unicode");
                    gshop_14x_client.cats[listBox1.SelectedIndex].sub_cat_count++;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                for (int i = 0; i < gshop_126.item_count; i++)
                {
                    if (gshop_126.items[i].main_type == listBox1.SelectedIndex && gshop_126.items[i].sub_type == listBox2.SelectedIndex)
                    {
                        gshop_126.items.RemoveAt(i);
                        gshop_126.item_count--;
                    }
                }
                gshop_126.cats[listBox1.SelectedIndex].subcat.RemoveAt(listBox2.SelectedIndex);
                gshop_126.cats[listBox1.SelectedIndex].sub_cat_count--;
            }
            else if (radioButton7.Checked)
            {
                for (int i = 0; i < gshop_14x_client.item_count; i++)
                {
                    if (gshop_14x_client.items[i].main_type == listBox1.SelectedIndex && gshop_14x_client.items[i].sub_type == listBox2.SelectedIndex)
                    {
                        gshop_14x_client.items.RemoveAt(i);
                        gshop_14x_client.item_count--;
                    }
                }
                gshop_14x_client.cats[listBox1.SelectedIndex].subcat.RemoveAt(listBox2.SelectedIndex);
                gshop_14x_client.cats[listBox1.SelectedIndex].sub_cat_count--;
            }
            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                int max_id = gshop_126.item_count;
                gshop_126.items.Add(new Items());
                gshop_126.items[max_id].local_id = max_id;
                gshop_126.items[max_id].main_type = listBox1.SelectedIndex;
                gshop_126.items[max_id].sub_type = listBox2.SelectedIndex;
                gshop_126.items[max_id].icon = new byte[128];
                gshop_126.items[max_id].item_id = 0;
                gshop_126.items[max_id].item_count = 0;
                gshop_126.items[max_id].sell_options = new List<Sell_options>(4);
                for (int j = 0; j < 4; j++)
                {
                    gshop_126.items[max_id].sell_options.Add(new Sell_options());
                    gshop_126.items[max_id].sell_options[j].price = 0;
                    gshop_126.items[max_id].sell_options[j].until_time = 0;
                    gshop_126.items[max_id].sell_options[j].time = 0;
                }
                gshop_126.items[max_id].props = 1;
                gshop_126.items[max_id].desc = new byte[1024];
                gshop_126.items[max_id].name = getEncoding("New", "Unicode");
                gshop_126.item_count++;
                dataGridView1.Rows.Add(dataGridView1.Rows.Count, gshop_126.items[max_id].local_id, getDecoding(gshop_126.items[max_id].name, "Unicode"));
            }
            else if (radioButton7.Checked)
            {
                int max_id = gshop_14x_client.item_count;
                gshop_14x_client.items.Add(new Items_1());
                gshop_14x_client.items[max_id].local_id = max_id;
                gshop_14x_client.items[max_id].main_type = listBox1.SelectedIndex;
                gshop_14x_client.items[max_id].sub_type = listBox2.SelectedIndex;
                gshop_14x_client.items[max_id].icon = new byte[128];
                gshop_14x_client.items[max_id].item_id = 0;
                gshop_14x_client.items[max_id].item_count = 0;
                gshop_14x_client.items[max_id].sell_options = new List<Sell_options_1>(4);
                for (int j = 0; j < 4; j++)
                {
                    gshop_14x_client.items[max_id].sell_options.Add(new Sell_options_1());
                    gshop_14x_client.items[max_id].sell_options[j].price = 0;
                    gshop_14x_client.items[max_id].sell_options[j].end_time = 0;
                    gshop_14x_client.items[max_id].sell_options[j].time = 0;
                    gshop_14x_client.items[max_id].sell_options[j].start_time = 0;
                    gshop_14x_client.items[max_id].sell_options[j].type = 0;
                    gshop_14x_client.items[max_id].sell_options[j].day = 0;
                    gshop_14x_client.items[max_id].sell_options[j].status = 1;
                    gshop_14x_client.items[max_id].sell_options[j].flag = 0;
                }
                gshop_14x_client.items[max_id].desc = new byte[1024];
                gshop_14x_client.items[max_id].name = getEncoding("New", "Unicode");
                gshop_14x_client.item_count++;
                dataGridView1.Rows.Add(dataGridView1.Rows.Count, gshop_14x_client.items[max_id].local_id, getDecoding(gshop_14x_client.items[max_id].name, "Unicode"));
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value);
            int pos = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            if (radioButton6.Checked)
            {
                gshop_126.items.RemoveAt(index);
                gshop_126.item_count--;
                for (int i = index; i < gshop_126.item_count; i++)
                    gshop_126.items[i].local_id--;
                for (int i = pos; i < dataGridView1.Rows.Count - 1; i++)
                {
                    int number = Convert.ToInt32(dataGridView1[0, i].Value);
                    int value = Convert.ToInt32(dataGridView1[1, i].Value);
                    dataGridView1[0, i].Value = number - 1;
                    dataGridView1[1, i].Value = value - 1;
                }
            }
            else if (radioButton7.Checked)
            {
                gshop_14x_client.items.RemoveAt(index);
                gshop_14x_client.item_count--;
                for (int i = index; i < gshop_14x_client.item_count; i++)
                    gshop_14x_client.items[i].local_id--;
                for (int i = pos; i < dataGridView1.Rows.Count - 1; i++)
                {
                    int number = Convert.ToInt32(dataGridView1[0, i].Value);
                    int value = Convert.ToInt32(dataGridView1[1, i].Value);
                    dataGridView1[0, i].Value = number - 1;
                    dataGridView1[1, i].Value = value - 1;
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1 && listBox2.SelectedIndex > -1)
            {
                Form2 frm = new Form2();
                frm.cat_index = listBox1.SelectedIndex;
                frm.sub_cat_index = listBox2.SelectedIndex;
                frm.type = radioButton6.Checked ? 1 : 2;
                if (radioButton6.Checked)
                    frm.shop_126 = gshop_126;
                else if (radioButton7.Checked)
                    frm.shop_14x_client = gshop_14x_client;
                frm.elem = element;
                frm.ShowDialog();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_id = UInt32.Parse(textBox1.Text);
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_id = UInt32.Parse(textBox1.Text);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_count = UInt32.Parse(textBox3.Text);
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_count = UInt32.Parse(textBox3.Text);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_count = UInt32.Parse(textBox3.Text);
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_count = UInt32.Parse(textBox3.Text);
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].desc = getEncoding(richTextBox1.Text, "Unicode");
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].desc = getEncoding(richTextBox1.Text, "Unicode");
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].icon = getEncoding(textBox4.Text, "GBK");
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].icon = getEncoding(textBox4.Text, "GBK");
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].price = Int32.Parse(textBox5.Text);
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].price = UInt32.Parse(textBox5.Text);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].time = getConvertStringToTimestamp(textBox6.Text);
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].time = (uint)getConvertStringToTimestamp(textBox6.Text);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].until_time = getConvertStringToSeconds(textBox7.Text);
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].end_time = (uint)getConvertStringToSeconds(textBox7.Text);

        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props = 0;
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].status = 0;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props = 1;
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].status = 1;
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props = 2;
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].status = 2;
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props = 3;
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].status = 3;
        }

        private void radioButton5_Click(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                gshop_126.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props = 4;
            else if (radioButton7.Checked)
                gshop_14x_client.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].status = 4;
        }

        private void сохранитьВ14хToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog()
            {
                InitialDirectory = "",
                Filter = "gshop.data|*gshop*.data|Все файлы|*.*",
                Title = "Сохранение gshop.data"
            };
            if (save.ShowDialog() != DialogResult.Cancel)
            {
                gshop_14x_client.timestamp = gshop_126.timestamp;
                gshop_14x_client.item_count = gshop_126.item_count;
                gshop_14x_client.items = new List<Items_1>(gshop_14x_client.item_count);
                for (int i = 0; i < gshop_126.item_count; i++)
                {
                    gshop_14x_client.items.Add(new Items_1());
                    gshop_14x_client.items[i].local_id = i + 1;
                    gshop_14x_client.items[i].main_type = gshop_126.items[i].main_type;
                    gshop_14x_client.items[i].sub_type = gshop_126.items[i].sub_type;
                    gshop_14x_client.items[i].icon = gshop_126.items[i].icon;
                    gshop_14x_client.items[i].item_id = gshop_126.items[i].item_id;
                    gshop_14x_client.items[i].item_count = gshop_126.items[i].item_count;
                    gshop_14x_client.items[i].sell_options = new List<Sell_options_1>(4);
                    for (int j = 0; j < 4; j++)
                    {
                        gshop_14x_client.items[i].sell_options.Add(new Sell_options_1());
                        gshop_14x_client.items[i].sell_options[j].price = (uint)gshop_126.items[i].sell_options[j].price;
                        gshop_14x_client.items[i].sell_options[j].end_time = 0;
                        gshop_14x_client.items[i].sell_options[j].time = (uint)gshop_126.items[i].sell_options[j].until_time;
                        gshop_14x_client.items[i].sell_options[j].start_time = 0;
                        gshop_14x_client.items[i].sell_options[j].type = -1;
                        gshop_14x_client.items[i].sell_options[j].day = 0;
                        gshop_14x_client.items[i].sell_options[j].status = 0;
                        gshop_14x_client.items[i].sell_options[j].flag = 0;
                    }
                    gshop_14x_client.items[i].desc = gshop_126.items[i].desc;
                    gshop_14x_client.items[i].name = gshop_126.items[i].name;
                    gshop_14x_client.items[i].idGift = 0;
                    gshop_14x_client.items[i].iGiftNum = 0;
                    gshop_14x_client.items[i].iGiftTime = 0;
                    gshop_14x_client.items[i].iLogPrice = 0;
                }
                gshop_14x_client.cats = gshop_126.cats;
                    
                using (BinaryWriter write = new BinaryWriter(new FileStream(save.FileName, FileMode.Create, FileAccess.Write), Encoding.Unicode))
                {
                    write.Write(gshop_14x_client.timestamp);
                    write.Write(gshop_14x_client.item_count);
                    for (int i = 0; i < gshop_14x_client.item_count; i++)
                    {
                        write.Write(gshop_14x_client.items[i].local_id);
                        write.Write(gshop_14x_client.items[i].main_type);
                        write.Write(gshop_14x_client.items[i].sub_type);
                        write.Write(gshop_14x_client.items[i].icon);
                        getWriteNull(write, gshop_14x_client.items[i].icon.Length, 128);
                        write.Write(gshop_14x_client.items[i].item_id);
                        write.Write(gshop_14x_client.items[i].item_count);
                        for (int j = 0; j < 4; j++)
                        {
                            write.Write(gshop_14x_client.items[i].sell_options[j].price);
                            write.Write(gshop_14x_client.items[i].sell_options[j].end_time);
                            write.Write(gshop_14x_client.items[i].sell_options[j].time);
                            write.Write(gshop_14x_client.items[i].sell_options[j].start_time);
                            write.Write(gshop_14x_client.items[i].sell_options[j].type);
                            write.Write(gshop_14x_client.items[i].sell_options[j].day);
                            write.Write(gshop_14x_client.items[i].sell_options[j].status);
                            write.Write(gshop_14x_client.items[i].sell_options[j].flag);
                        }
                        write.Write(gshop_14x_client.items[i].desc);
                        getWriteNull(write, gshop_14x_client.items[i].desc.Length, 1024);
                        write.Write(gshop_14x_client.items[i].name);
                        getWriteNull(write, gshop_14x_client.items[i].name.Length, 64);
                        write.Write(gshop_14x_client.items[i].idGift);
                        write.Write(gshop_14x_client.items[i].iGiftNum);
                        write.Write(gshop_14x_client.items[i].iGiftTime);
                        write.Write(gshop_14x_client.items[i].iLogPrice);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        write.Write(gshop_14x_client.cats[i].cat_name);
                        getWriteNull(write, gshop_14x_client.cats[i].cat_name.Length, 128);
                        write.Write(gshop_14x_client.cats[i].sub_cat_count);
                        for (int j = 0; j < gshop_14x_client.cats[i].sub_cat_count; j++)
                        {
                            write.Write(gshop_14x_client.cats[i].subcat[j].sub_cat_name);
                            getWriteNull(write, gshop_14x_client.cats[i].subcat[j].sub_cat_name.Length, 128);
                        }
                    }
                    write.Close();
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog()
            {
                InitialDirectory = "",
                Filter = "gshopsev.data|*gshopsev*.data|Все файлы|*.*",
                Title = "Сохранение gshopsev.data"
            };
            if (save.ShowDialog() != DialogResult.Cancel)
            {
                gshop14x_server.timestamp = gshop_126.timestamp;
                gshop14x_server.item_count = gshop_126.item_count;
                gshop14x_server.items = new List<Items_2>(gshop14x_server.item_count);
                for (int i = 0; i < gshop_126.item_count; i++)
                {
                    gshop14x_server.items.Add(new Items_2());
                    gshop14x_server.items[i].local_id = i + 1;
                    gshop14x_server.items[i].main_type = gshop_126.items[i].main_type;
                    gshop14x_server.items[i].sub_type = gshop_126.items[i].sub_type;
                    gshop14x_server.items[i].item_id = gshop_126.items[i].item_id;
                    gshop14x_server.items[i].item_count = gshop_126.items[i].item_count;
                    gshop14x_server.items[i].sell_options = new List<Sell_options_1>(4);
                    for (int j = 0; j < 4; j++)
                    {
                        gshop14x_server.items[i].sell_options.Add(new Sell_options_1());
                        gshop14x_server.items[i].sell_options[j].price = (uint)gshop_126.items[i].sell_options[j].price;
                        gshop14x_server.items[i].sell_options[j].end_time = 0;
                        gshop14x_server.items[i].sell_options[j].time = (uint)gshop_126.items[i].sell_options[j].until_time;
                        gshop14x_server.items[i].sell_options[j].start_time = 0;
                        gshop14x_server.items[i].sell_options[j].type = -1;
                        gshop14x_server.items[i].sell_options[j].day = 0;
                        gshop14x_server.items[i].sell_options[j].status = 0;
                        gshop14x_server.items[i].sell_options[j].flag = 0;
                    }
                    gshop14x_server.items[i].idGift = 0;
                    gshop14x_server.items[i].iGiftNum = 0;
                    gshop14x_server.items[i].iGiftTime = 0;
                    gshop14x_server.items[i].iLogPrice = 0;
                }

                using (BinaryWriter write = new BinaryWriter(new FileStream(save.FileName, FileMode.Create, FileAccess.Write), Encoding.Unicode))
                {
                    write.Write(gshop14x_server.timestamp);
                    write.Write(gshop14x_server.item_count);
                    for (int i = 0; i < gshop14x_server.item_count; i++)
                    {
                        write.Write(gshop14x_server.items[i].local_id);
                        write.Write(gshop14x_server.items[i].main_type);
                        write.Write(gshop14x_server.items[i].sub_type);
                        write.Write(gshop14x_server.items[i].item_id);
                        write.Write(gshop14x_server.items[i].item_count);
                        for (int j = 0; j < 4; j++)
                        {
                            write.Write(gshop14x_server.items[i].sell_options[j].price);
                            write.Write(gshop14x_server.items[i].sell_options[j].end_time);
                            write.Write(gshop14x_server.items[i].sell_options[j].time);
                            write.Write(gshop14x_server.items[i].sell_options[j].start_time);
                            write.Write(gshop14x_server.items[i].sell_options[j].type);
                            write.Write(gshop14x_server.items[i].sell_options[j].day);
                            write.Write(gshop14x_server.items[i].sell_options[j].status);
                            write.Write(gshop14x_server.items[i].sell_options[j].flag);
                        }
                        write.Write(gshop14x_server.items[i].idGift);
                        write.Write(gshop14x_server.items[i].iGiftNum);
                        write.Write(gshop14x_server.items[i].iGiftTime);
                        write.Write(gshop14x_server.items[i].iLogPrice);
                    }
                    write.Close();
                }
            }
        }
    }
}
