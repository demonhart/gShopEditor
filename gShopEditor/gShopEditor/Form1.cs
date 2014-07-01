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
        public gShop gshop = new gShop();
        gShop_14x_Client gshop14x_client = new gShop_14x_Client();
        gShop_14x_Server gshop14x_server = new gShop_14x_Server();

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
                    gShopRead(read, gshop);
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
                for (int i = 0; i < gshop.cats[listBox1.SelectedIndex].sub_cat_count; i++)
                {
                    count = 0;
                    for (int j = 0; j < gshop.item_count; j++)
                        if (gshop.items[j].main_type == listBox1.SelectedIndex && gshop.items[j].sub_type == i)
                            count++;
                    string[] name = getDecoding(gshop.cats[listBox1.SelectedIndex].sub_cat_name[i], "Unicode").Split('\0');
                    listBox2.Items.Add(name[0] + " (" + count + ")");
                }
                string[] names = listBox1.SelectedItem.ToString().Split(' ');
                textBox13.Text = names[0];
            }                    
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex > -1)
            {
                int count = 0;
                dataGridView1.Rows.Clear();
                for (int i = 0; i < gshop.item_count; i++)
                {
                    if (gshop.items[i].main_type == listBox1.SelectedIndex && gshop.items[i].sub_type == listBox2.SelectedIndex)
                    {
                        count++;
                        string[] name = getDecoding(gshop.items[i].name, "Unicode").Split('\0');
                        dataGridView1.Rows.Add(count, gshop.items[i].local_id, name[0]);
                    }
                }
                string[] tmp_name = listBox2.SelectedItem.ToString().Split(' ');
                textBox14.Text = tmp_name[0];
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

        private void gShopRead(BinaryReader read, gShop gshop)
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
                
                gshop.cats[i].sub_cat_name = new byte[gshop.cats[i].sub_cat_count][];
                for (int j = 0; j < gshop.cats[i].sub_cat_count; j++)
                {
                    string[] tmp_sub_cat_name = getDecoding(read.ReadBytes(128), "Unicode").Split('\0');
                    gshop.cats[i].sub_cat_name[j] = getEncoding(tmp_sub_cat_name[0], "Unicode");
                }
                listBox1.Items.Add(Encoding.Unicode.GetString(gshop.cats[i].cat_name).Replace("\0", string.Empty) + " (" + gshop.cats[i].sub_cat_count + ")");
            }
        }

        public gShop getGshop()
        {
            return this.gshop;
        }

        public List<Items> getItems()
        {
            return this.gshop.items;
        }

        public int getItemsCount()
        {
            return this.gshop.item_count;
        }

        public ListBox getListbox1()
        {
            return listBox1;
        }

        public ListBox getListbox2()
        {
            return listBox2;
        }

        public DataGridView getGrid()
        {
            return this.dataGridView1;
        }

        private void elementsRead(BinaryReader read, Elements element)
        {
            #region elements.data
            element.version = read.ReadInt16();
            element.unk = read.ReadInt16();
            element.unk2 = read.ReadInt32();
            element.list1_count = read.ReadInt32();
            element.list1 = new List<List1>(element.list1_count);
            for (int i = 0; i < element.list1_count; i++)
            {
                element.list1.Add(new List1());
                element.list1[i].id = read.ReadInt32();
                element.list1[i].name = read.ReadBytes(64);
                element.list1[i].bonus_count = read.ReadInt32();
                element.list1[i].bonus1_id = read.ReadInt32();
                element.list1[i].bonus2_id = read.ReadInt32();
                element.list1[i].bonus3_id = read.ReadInt32();
            }
            element.list2_count = read.ReadInt32();
            element.weapons_class = new List<List2>(element.list2_count);
            for (int i = 0; i < element.list2_count; i++)
            {
                element.weapons_class.Add(new List2());
                element.weapons_class[i].id = read.ReadInt32();
                element.weapons_class[i].name = read.ReadBytes(64);
            }
            element.list3_count = read.ReadInt32();
            element.weapons_sub_class = new List<List3>(element.list3_count);
            for (int i = 0; i < element.list3_count; i++)
            {
                element.weapons_sub_class.Add(new List3());
                element.weapons_sub_class[i].id = read.ReadInt32();
                element.weapons_sub_class[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 288;
            }
            element.list4_count = read.ReadInt32();
            element.weapons = new List<List4>(element.list4_count);
            for (int i = 0; i < element.list4_count; i++)
            {
                element.weapons.Add(new List4());
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
            element.armor_class = new List<List5>(element.list5_count);
            for (int i = 0; i < element.list5_count; i++)
            {
                element.armor_class.Add(new List5());
                element.armor_class[i].id = read.ReadInt32();
                element.armor_class[i].name = read.ReadBytes(64);
            }
            element.list6_count = read.ReadInt32();
            element.armor_sub_class = new List<List6>(element.list6_count);
            for (int i = 0; i < element.list6_count; i++)
            {
                element.armor_sub_class.Add(new List6());
                element.armor_sub_class[i].id = read.ReadInt32();
                element.armor_sub_class[i].name = read.ReadBytes(64);
                element.armor_sub_class[i].mask = read.ReadInt32();
            }
            element.list7_count = read.ReadInt32();
            element.armor = new List<List7>(element.list7_count);
            for (int i = 0; i < element.list7_count; i++)
            {
                element.armor.Add(new List7());
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
            element.ornaments = new List<List10>(element.list10_count);
            for (int i = 0; i < element.list10_count; i++)
            {
                element.ornaments.Add(new List10());
                element.ornaments[i].id = read.ReadInt32();
                read.BaseStream.Position += 8;
                element.ornaments[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 256;
                element.ornaments[i].surface = read.ReadBytes(128);
                if (element.version == 12)
                    read.BaseStream.Position += 696;
                else if (element.version == 60)
                    read.BaseStream.Position += 708;
            }
            element.list11_count = read.ReadInt32();
            getChangePos(read, element.list11_count, 68);
            element.list12_count = read.ReadInt32();
            getChangePos(read, element.list12_count, 68);
            element.list13_count = read.ReadInt32();
            element.remedies = new List<List13>(element.list13_count);
            for (int i = 0; i < element.list13_count; i++)
            {
                element.remedies.Add(new List13());
                element.remedies[i].id = read.ReadInt32();
                read.BaseStream.Position += 8;
                element.remedies[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.remedies[i].surface = read.ReadBytes(128);
                if (element.version <= 70)
                    read.BaseStream.Position += 44;
            }
            element.list14_count = read.ReadInt32();
            getChangePos(read, element.list14_count, 68);
            element.list15_count = read.ReadInt32();
            getChangePos(read, element.list15_count, 68);
            element.list16_count = read.ReadInt32();
            element.materials = new List<List16>(element.list16_count);
            for (int i = 0; i < element.list16_count; i++)
            {
                element.materials.Add(new List16());
                element.materials[i].id = read.ReadInt32();
                read.BaseStream.Position += 8;
                element.materials[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.materials[i].surface = read.ReadBytes(128);
                if (element.version <= 60)
                    read.BaseStream.Position += 36;
            }
            element.list17_count = read.ReadInt32();
            getChangePos(read, element.list17_count, 68);
            element.list18_count = read.ReadInt32();
            element.atk_hierogr = new List<List18>(element.list18_count);
            for (int i = 0; i < element.list18_count; i++)
            {
                element.atk_hierogr.Add(new List18());
                element.atk_hierogr[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.atk_hierogr[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.atk_hierogr[i].surface = read.ReadBytes(128);
                if (element.version <= 60)
                    read.BaseStream.Position += 36;
            }
            element.list19_count = read.ReadInt32();
            getChangePos(read, element.list19_count, 68);
            element.list20_count = read.ReadInt32();
            element.def_hierogr = new List<List20>(element.list20_count);
            for (int i = 0; i < element.list20_count; i++)
            {
                element.def_hierogr.Add(new List20());
                element.def_hierogr[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.def_hierogr[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.def_hierogr[i].surface = read.ReadBytes(128);
                if (element.version <= 60)
                    read.BaseStream.Position += 296;
            }
            read.BaseStream.Position += 19;
            element.list21_count = read.ReadInt32();
            getChangePos(read, element.list21_count, 68);
            element.list22_count = read.ReadInt32();
            element.skills = new List<List22>(element.list22_count);
            for (int i = 0; i < element.list22_count; i++)
            {
                element.skills.Add(new List22());
                element.skills[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.skills[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.skills[i].surface = read.ReadBytes(128);
                if (element.version <= 60)
                    read.BaseStream.Position += 20;
            }
            element.list23_count = read.ReadInt32();
            element.flyes = new List<List23>(element.list23_count);
            for (int i = 0; i < element.list23_count; i++)
            {
                element.flyes.Add(new List23());
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
            element.key_items=new List<List27>(element.list27_count);
            for (int i = 0; i < element.list27_count; i++)
            {
                element.key_items.Add(new List27());
                element.key_items[i].id = read.ReadInt32();
                element.key_items[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.key_items[i].surface = read.ReadBytes(128);
                if (element.version <= 60)
                    read.BaseStream.Position += 28;
            }
            element.list28_count = read.ReadInt32();
            getChangePos(read, element.list28_count, 348);
            element.list29_count = read.ReadInt32();
            element.quest_items = new List<List29>(element.list29_count);
            for (int i = 0; i < element.list29_count; i++)
            {
                element.quest_items.Add(new List29());
                element.quest_items[i].id = read.ReadInt32();
                element.quest_items[i].name = read.ReadBytes(64);
                element.quest_items[i].surface = read.ReadBytes(128);
                if (element.version <= 60)
                    read.BaseStream.Position += 12;
            }
            element.list30_count = read.ReadInt32();
            getChangePos(read, element.list30_count, 888);
            element.list31_count = read.ReadInt32();
            getChangePos(read, element.list31_count, 68);
            element.list32_count = read.ReadInt32();
            element.ammo = new List<List32>(element.list32_count);
            for (int i = 0; i < element.list32_count; i++)
            {
                element.ammo.Add(new List32());
                element.ammo[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.ammo[i].name = read.ReadBytes(192);
                read.BaseStream.Position += 128;
                element.ammo[i].surface = read.ReadBytes(128);
                if (element.version <= 60)
                    read.BaseStream.Position += 436;
            }
            element.list33_count = read.ReadInt32();
            getChangePos(read, element.list33_count, 68);
            element.list34_count = read.ReadInt32();
            getChangePos(read, element.list34_count, 340);
            element.list35_count = read.ReadInt32();
            getChangePos(read, element.list35_count, 68);
            element.list36_count = read.ReadInt32();
            element.soulgems = new List<List36>(element.list36_count);
            for (int i = 0; i < element.list36_count; i++)
            {
                element.soulgems.Add(new List36());
                element.soulgems[i].id = read.ReadInt32();
                read.BaseStream.Position += 4;
                element.soulgems[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.soulgems[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 108;
            }
            element.list37_count = read.ReadInt32();
            getChangePos(read, element.list37_count, 84);
            element.list38_count = read.ReadInt32();
            getChangePos(read, element.list38_count, 196);
            element.list39_count = read.ReadInt32();
            getChangePos(read, element.list39_count, 1500);
            element.list40_count = read.ReadInt32();
            getChangePos(read, element.list40_count, 72);
            element.list41_count = read.ReadInt32();
            getChangePos(read, element.list41_count, 1224);
            element.list42_count = read.ReadInt32();
            getChangePos(read, element.list42_count, 72);
            element.list43_count = read.ReadInt32();
            getChangePos(read, element.list43_count, 72);
            element.list44_count = read.ReadInt32();
            getChangePos(read, element.list44_count, 200);
            element.list45_count = read.ReadInt32();
            getChangePos(read, element.list45_count, 200);
            element.list46_count = read.ReadInt32();
            getChangePos(read, element.list46_count, 196);
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
            element.quest_rewards = new List<List76>(element.list76_count);
            for (int i = 0; i < element.list76_count; i++)
            {
                element.quest_rewards.Add(new List76());
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
            element.resources = new List<List80>(element.list80_count);
            for (int i = 0; i < element.list80_count; i++)
            {
                element.resources.Add(new List80());
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
            element.fashion = new List<List84>(element.list84_count);
            for (int i = 0; i < element.list84_count; i++)
            {
                element.fashion.Add(new List84());
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
            element.pet_eggs = new List<List96>(element.list96_count);
            for (int i = 0; i < element.list96_count; i++)
            {
                element.pet_eggs.Add(new List96());
                element.pet_eggs[i].id = read.ReadInt32();
                element.pet_eggs[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.pet_eggs[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 304;
            }
            element.list97_count = read.ReadInt32();
            element.pet_food = new List<List97>(element.list97_count);
            for (int i = 0; i < element.list97_count; i++)
            {
                element.pet_food.Add(new List97());
                element.pet_food[i].id = read.ReadInt32();
                element.pet_food[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.pet_food[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 36;
            }
            element.list98_count = read.ReadInt32();
            getChangePos(read, element.list98_count, 344);
            element.list99_count = read.ReadInt32();
            element.fireworks = new List<List99>(element.list99_count);
            for (int i = 0; i < element.list99_count; i++)
            {
                element.fireworks.Add(new List99());
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
            element.potions = new List<List107>(element.list107_count);
            for (int i = 0; i < element.list107_count; i++)
            {
                element.potions.Add(new List107());
                element.potions[i].id = read.ReadInt32();
                element.potions[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.potions[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 32;
            }
            element.list108_count = read.ReadInt32();
            element.refining = new List<List108>(element.list108_count);
            for (int i = 0; i < element.list108_count; i++)
            {
                element.refining.Add(new List108());
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
            element.heaven_books = new List<List113>(element.list113_count);
            for (int i = 0; i < element.list113_count; i++)
            {
                element.heaven_books.Add(new List113());
                element.heaven_books[i].id = read.ReadInt32();
                element.heaven_books[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.heaven_books[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 60;
            }
            element.list114_count = read.ReadInt32();
            element.chat_speakers = new List<List114>(element.list114_count);
            for (int i = 0; i < element.list114_count; i++)
            {
                element.chat_speakers.Add(new List114());
                element.chat_speakers[i].id = read.ReadInt32();
                element.chat_speakers[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.chat_speakers[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 24;
            }
            element.list115_count = read.ReadInt32();
            element.mp_hierogr = new List<List115>(element.list115_count);
            for (int i = 0; i < element.list115_count; i++)
            {
                element.mp_hierogr.Add(new List115());
                element.mp_hierogr[i].id = read.ReadInt32();
                element.mp_hierogr[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.mp_hierogr[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 32;
            }
            element.list116_count = read.ReadInt32();
            element.hp_hierogr = new List<List116>(element.list116_count);
            for (int i = 0; i < element.list116_count; i++)
            {
                element.hp_hierogr.Add(new List116());
                element.hp_hierogr[i].id = read.ReadInt32();
                element.hp_hierogr[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.hp_hierogr[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 32;
            }
            element.list117_count = read.ReadInt32();
            element.multi_exp = new List<List117>(element.list117_count);
            for (int i = 0; i < element.list117_count; i++)
            {
                element.multi_exp.Add(new List117());
                element.multi_exp[i].id = read.ReadInt32();
                element.multi_exp[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.multi_exp[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 24;
            }
            element.list118_count = read.ReadInt32();
            element.teleport = new List<List118>(element.list118_count);
            for (int i = 0; i < element.list118_count; i++)
            {
                element.teleport.Add(new List118());
                element.teleport[i].id = read.ReadInt32();
                element.teleport[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.teleport[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 20;
            }
            element.list119_count = read.ReadInt32();
            element.dyes = new List<List119>(element.list119_count);
            for (int i = 0; i < element.list119_count; i++)
            {
                element.dyes.Add(new List119());
                element.dyes[i].id = read.ReadInt32();
                element.dyes[i].name = read.ReadBytes(64);
                read.BaseStream.Position += 128;
                element.dyes[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += 44;
            }
            #endregion
        }

        private void getChangePos(BinaryReader read, int count, long position)
        {
            read.BaseStream.Position += position * count;
        }

        #endregion

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_id.ToString();
            textBox2.Text = getDecoding(gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].name, "Unicode");
            textBox3.Text = gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_count.ToString();
            textBox4.Text = getDecoding(gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].icon, "GBK");
            textBox5.Text = gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].price.ToString();
            textBox6.Text = getConvertTimestampToString(gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].until_time);
            textBox7.Text = getConvertSecondsToString(gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].time);
            richTextBox1.Text = getDecoding(gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].desc, "Unicode");
            switch (gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props)
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

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gshop.item_count; i++)
            {
                if (gshop.items[i].main_type == listBox1.SelectedIndex && gshop.items[i].sub_type == listBox2.SelectedIndex)
                    gshop.items[i].sell_options[0].price = Int32.Parse(textBox9.Text) * 100;
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
                    write.Write(gshop.timestamp);
                    write.Write(gshop.item_count);
                    for (int i = 0; i < gshop.item_count; i++)
                    {
                        write.Write(gshop.items[i].local_id);
                        write.Write(gshop.items[i].main_type);
                        write.Write(gshop.items[i].sub_type);
                        write.Write(gshop.items[i].icon);
                        if (gshop.items[i].icon.Length < 128)
                            for (int j = gshop.items[i].icon.Length; j < 128; j++)
                                write.Write((byte)0);
                        write.Write(gshop.items[i].item_id);
                        write.Write(gshop.items[i].item_count);
                        for (int j = 0; j < 4; j++)
                        {
                            write.Write(gshop.items[i].sell_options[j].price);
                            write.Write(gshop.items[i].sell_options[j].until_time);
                            write.Write(gshop.items[i].sell_options[j].time);
                        }
                        write.Write(gshop.items[i].props);
                        write.Write(gshop.items[i].desc);
                        if (gshop.items[i].desc.Length < 1024)
                            for (int j = gshop.items[i].desc.Length; j < 1024; j++)
                                write.Write((byte)0);
                        write.Write(gshop.items[i].name);
                        if (gshop.items[i].name.Length < 64)
                            for (int j = gshop.items[i].name.Length; j < 64; j++)
                                write.Write((byte)0);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        write.Write(gshop.cats[i].cat_name);
                        if (gshop.cats[i].cat_name.Length < 128)
                            for (int j = gshop.cats[i].cat_name.Length; j < 128; j++)
                                write.Write((byte)0);
                        write.Write(gshop.cats[i].sub_cat_count);
                        for (int j = 0; j < gshop.cats[i].sub_cat_count; j++)
                        {
                            write.Write(gshop.cats[i].sub_cat_name[j]);
                            if (gshop.cats[i].sub_cat_name[j].Length < 128)
                                for (int k = gshop.cats[i].sub_cat_name[j].Length; k < 128; k++)
                                    write.Write((byte)0);
                        }
                    }
                    write.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Selected)
                    gshop.items[Convert.ToInt32(dataGridView1[1, i].Value)].sell_options[0].price = Int32.Parse(textBox10.Text) * 100;
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
                Filter = "elements.data|elements.data|Все файлы|*.*",
                Title = "Загрузка elements.data"
            };
            if (open.ShowDialog() != DialogResult.Cancel)
                textBox12.Text = open.FileName;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (BinaryReader read = new BinaryReader(new FileStream(textBox11.Text, FileMode.Open, FileAccess.Read), Encoding.Unicode))
            {
                gShopRead(read, gshop);
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
                gshop.cats.Add(new Category());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            gshop.cats.RemoveAt(listBox1.SelectedIndex);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            listBox1.Items[listBox1.SelectedIndex] = textBox13.Text + " (" + gshop.cats[listBox1.SelectedIndex].sub_cat_count + ")";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int count = 0; ;
            for (int i = 0; i < gshop.item_count; i++)
                if (gshop.items[i].main_type == listBox1.SelectedIndex && gshop.items[i].sub_type == listBox2.SelectedIndex)
                    count++;
            listBox2.Items[listBox2.SelectedIndex] = textBox14.Text + " (" + count + ")";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (gshop.cats[listBox1.SelectedIndex].sub_cat_count >= 8)
                MessageBox.Show("Невозможно добавить подкатегорию.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                listBox2.Items.Add("New");
                gshop.cats[listBox1.SelectedIndex].sub_cat_count++;
                gshop.cats[listBox1.SelectedIndex].sub_cat_name[gshop.cats[listBox1.SelectedIndex].sub_cat_count - 1] = getEncoding("New", "Unicode");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int max_id = gshop.item_count;
            gshop.items.Add(new Items());
            gshop.items[max_id].local_id = max_id;
            gshop.items[max_id].main_type = listBox1.SelectedIndex;
            gshop.items[max_id].sub_type = listBox2.SelectedIndex;
            gshop.items[max_id].icon = new byte[128];
            gshop.items[max_id].item_id = 0;
            gshop.items[max_id].item_count = 0;
            gshop.items[max_id].sell_options = new List<Sell_options>(4);
            for (int j = 0; j < 4; j++)
            {
                gshop.items[max_id].sell_options.Add(new Sell_options());
                gshop.items[max_id].sell_options[j].price = 0;
                gshop.items[max_id].sell_options[j].until_time = 0;
                gshop.items[max_id].sell_options[j].time = 0;
            }
            gshop.items[max_id].props = 1;
            gshop.items[max_id].desc = new byte[1024];
            gshop.items[max_id].name = getEncoding("New", "Unicode");
            gshop.item_count++;
            dataGridView1.Rows.Add(dataGridView1.Rows.Count, gshop.items[max_id].local_id, getDecoding(gshop.items[max_id].name, "Unicode"));
        }

        private void button13_Click(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value);
            int pos = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            gshop.items.RemoveAt(index);
            gshop.item_count--;
            for (int i = index; i < gshop.item_count; i++)
                gshop.items[i].local_id--;
            for (int i = pos; i < dataGridView1.Rows.Count - 1; i++)
            {
                int number = Convert.ToInt32(dataGridView1[0, i].Value);
                int value = Convert.ToInt32(dataGridView1[1, i].Value);
                dataGridView1[0, i].Value = number - 1;
                dataGridView1[1, i].Value = value - 1;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1 && listBox2.SelectedIndex > -1)
            {
                Form2 frm = new Form2();
                frm.cat_index = listBox1.SelectedIndex;
                frm.sub_cat_index = listBox2.SelectedIndex;
                frm.shop = gshop;
                frm.elem = element;
                frm.ShowDialog();
                dataGridView1.Update();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_id = UInt32.Parse(textBox1.Text);
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].item_count = UInt32.Parse(textBox3.Text);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].name = getEncoding(textBox2.Text, "Unicode");
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].desc = getEncoding(richTextBox1.Text, "Unicode");
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].icon = getEncoding(textBox4.Text, "GBK");
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].price = Int32.Parse(textBox5.Text);
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].time = getConvertStringToTimestamp(textBox6.Text);
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].sell_options[0].until_time = getConvertStringToSeconds(textBox7.Text);
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props = 0;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props = 1;
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props = 2;
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props = 3;
        }

        private void radioButton5_Click(object sender, EventArgs e)
        {
            gshop.items[Convert.ToInt32(dataGridView1[1, dataGridView1.CurrentRow.Index].Value)].props = 4;
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
                gshop14x_client.timestamp = gshop.timestamp;
                gshop14x_client.item_count = gshop.item_count;
                gshop14x_client.items = new List<Items_1>(gshop14x_client.item_count);
                for (int i = 0; i < gshop.item_count; i++)
                {
                    gshop14x_client.items.Add(new Items_1());
                    gshop14x_client.items[i].local_id = i + 1;
                    gshop14x_client.items[i].main_type = gshop.items[i].main_type;
                    gshop14x_client.items[i].sub_type = gshop.items[i].sub_type;
                    gshop14x_client.items[i].icon = gshop.items[i].icon;
                    gshop14x_client.items[i].item_id = gshop.items[i].item_id;
                    gshop14x_client.items[i].item_count = gshop.items[i].item_count;
                    gshop14x_client.items[i].sell_options = new List<Sell_options_1>(4);
                    for (int j = 0; j < 4; j++)
                    {
                        gshop14x_client.items[i].sell_options.Add(new Sell_options_1());
                        gshop14x_client.items[i].sell_options[j].price = (uint)gshop.items[i].sell_options[j].price;
                        gshop14x_client.items[i].sell_options[j].end_time = 0;
                        gshop14x_client.items[i].sell_options[j].time = (uint)gshop.items[i].sell_options[j].until_time;
                        gshop14x_client.items[i].sell_options[j].start_time = 0;
                        gshop14x_client.items[i].sell_options[j].type = -1;
                        gshop14x_client.items[i].sell_options[j].day = 0;
                        gshop14x_client.items[i].sell_options[j].status = 0;
                        gshop14x_client.items[i].sell_options[j].flag = 0;
                    }
                    gshop14x_client.items[i].desc = gshop.items[i].desc;
                    gshop14x_client.items[i].name = gshop.items[i].name;
                }
                gshop14x_client.cats = gshop.cats;
                    
                using (BinaryWriter write = new BinaryWriter(new FileStream(save.FileName, FileMode.Create, FileAccess.Write), Encoding.Unicode))
                {
                    write.Write(gshop14x_client.timestamp);
                    write.Write(gshop14x_client.item_count);
                    for (int i = 0; i < gshop14x_client.item_count; i++)
                    {
                        write.Write(gshop14x_client.items[i].local_id);
                        write.Write(gshop14x_client.items[i].main_type);
                        write.Write(gshop14x_client.items[i].sub_type);
                        write.Write(gshop14x_client.items[i].icon);
                        if (gshop14x_client.items[i].icon.Length < 128)
                            for (int j = gshop14x_client.items[i].icon.Length; j < 128; j++)
                                write.Write((byte)0);
                        write.Write(gshop14x_client.items[i].item_id);
                        write.Write(gshop14x_client.items[i].item_count);
                        for (int j = 0; j < 4; j++)
                        {
                            write.Write(gshop14x_client.items[i].sell_options[j].price);
                            write.Write(gshop14x_client.items[i].sell_options[j].end_time);
                            write.Write(gshop14x_client.items[i].sell_options[j].time);
                            write.Write(gshop14x_client.items[i].sell_options[j].start_time);
                            write.Write(gshop14x_client.items[i].sell_options[j].type);
                            write.Write(gshop14x_client.items[i].sell_options[j].day);
                            write.Write(gshop14x_client.items[i].sell_options[j].status);
                            write.Write(gshop14x_client.items[i].sell_options[j].flag);
                        }
                        write.Write(gshop14x_client.items[i].desc);
                        if (gshop14x_client.items[i].desc.Length < 1024)
                            for (int j = gshop14x_client.items[i].desc.Length; j < 1024; j++)
                                write.Write((byte)0);
                        write.Write(gshop14x_client.items[i].name);
                        if (gshop14x_client.items[i].name.Length < 64)
                            for (int j = gshop14x_client.items[i].name.Length; j < 64; j++)
                                write.Write((byte)0);
                        write.Write(gshop14x_client.items[i].idGift);
                        write.Write(gshop14x_client.items[i].iGiftNum);
                        write.Write(gshop14x_client.items[i].iGiftTime);
                        write.Write(gshop14x_client.items[i].iLogPrice);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        write.Write(gshop14x_client.cats[i].cat_name);
                        if (gshop14x_client.cats[i].cat_name.Length < 128)
                            for (int j = gshop14x_client.cats[i].cat_name.Length; j < 128; j++)
                                write.Write((byte)0);
                        write.Write(gshop14x_client.cats[i].sub_cat_count);
                        for (int j = 0; j < gshop14x_client.cats[i].sub_cat_count; j++)
                        {
                            write.Write(gshop14x_client.cats[i].sub_cat_name[j]);
                            if (gshop14x_client.cats[i].sub_cat_name[j].Length < 128)
                                for (int k = gshop14x_client.cats[i].sub_cat_name[j].Length; k < 128; k++)
                                    write.Write((byte)0);
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
                gshop14x_server.timestamp = gshop.timestamp;
                gshop14x_server.item_count = gshop.item_count;
                gshop14x_server.items = new List<Items_2>(gshop14x_server.item_count);
                for (int i = 0; i < gshop.item_count; i++)
                {
                    gshop14x_server.items.Add(new Items_2());
                    gshop14x_server.items[i].local_id = i + 1;
                    gshop14x_server.items[i].main_type = gshop.items[i].main_type;
                    gshop14x_server.items[i].sub_type = gshop.items[i].sub_type;
                    gshop14x_server.items[i].item_id = gshop.items[i].item_id;
                    gshop14x_server.items[i].item_count = gshop.items[i].item_count;
                    gshop14x_server.items[i].sell_options = new List<Sell_options_1>(4);
                    for (int j = 0; j < 4; j++)
                    {
                        gshop14x_server.items[i].sell_options.Add(new Sell_options_1());
                        gshop14x_server.items[i].sell_options[j].price = (uint)gshop.items[i].sell_options[j].price;
                        gshop14x_server.items[i].sell_options[j].end_time = 0;
                        gshop14x_server.items[i].sell_options[j].time = (uint)gshop.items[i].sell_options[j].until_time;
                        gshop14x_server.items[i].sell_options[j].start_time = 0;
                        gshop14x_server.items[i].sell_options[j].type = -1;
                        gshop14x_server.items[i].sell_options[j].day = 0;
                        gshop14x_server.items[i].sell_options[j].status = 0;
                        gshop14x_server.items[i].sell_options[j].flag = 0;
                    }
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
