using System;
using System.Collections.Generic;
using System.Windows.Forms;
using gShopEditor.Structure;

namespace gShopEditor
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        Form1 frm = new Form1();
        public int cat_index, sub_cat_index;
        public int type = -1;
        public gShop_126 shop_126 = new gShop_126();
        public gShop_14x_Client shop_14x_client = new gShop_14x_Client();
        public Elements elem = new Elements();
        int count = 0;

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list4_count; i++)
            {
                string[] name = frm.getDecoding(elem.weapons[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.weapons[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list7_count; i++)
            {
                string[] name = frm.getDecoding(elem.armor[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.armor[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list10_count; i++)
            {
                string[] name = frm.getDecoding(elem.ornaments[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.ornaments[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list13_count; i++)
            {
                string[] name = frm.getDecoding(elem.remedies[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.remedies[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list16_count; i++)
            {
                string[] name = frm.getDecoding(elem.materials[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.materials[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list18_count; i++)
            {
                string[] name = frm.getDecoding(elem.atk_hierogr[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.atk_hierogr[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list20_count; i++)
            {
                string[] name = frm.getDecoding(elem.def_hierogr[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.def_hierogr[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list22_count; i++)
            {
                string[] name = frm.getDecoding(elem.skills[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.skills[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list23_count; i++)
            {
                string[] name = frm.getDecoding(elem.flyes[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.flyes[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list27_count; i++)
            {
                string[] name = frm.getDecoding(elem.key_items[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.key_items[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list29_count; i++)
            {
                string[] name = frm.getDecoding(elem.quest_items[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.quest_items[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list32_count; i++)
            {
                string[] name = frm.getDecoding(elem.ammo[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.ammo[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list36_count; i++)
            {
                string[] name = frm.getDecoding(elem.soulgems[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.soulgems[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list76_count; i++)
            {
                string[] name = frm.getDecoding(elem.quest_rewards[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.quest_rewards[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list80_count; i++)
            {
                string[] name = frm.getDecoding(elem.resources[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.resources[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list84_count; i++)
            {
                string[] name = frm.getDecoding(elem.fashion[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.fashion[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list96_count; i++)
            {
                string[] name = frm.getDecoding(elem.pet_eggs[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.pet_eggs[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list97_count; i++)
            {
                string[] name = frm.getDecoding(elem.pet_food[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.pet_food[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list99_count; i++)
            {
                string[] name = frm.getDecoding(elem.fireworks[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.fireworks[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list107_count; i++)
            {
                string[] name = frm.getDecoding(elem.potions[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.potions[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list108_count; i++)
            {
                string[] name = frm.getDecoding(elem.refining[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.refining[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list113_count; i++)
            {
                string[] name = frm.getDecoding(elem.heaven_books[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.heaven_books[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton23_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list114_count; i++)
            {
                string[] name = frm.getDecoding(elem.chat_speakers[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.chat_speakers[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton24_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list115_count; i++)
            {
                string[] name = frm.getDecoding(elem.mp_hierogr[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.mp_hierogr[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton25_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list116_count; i++)
            {
                string[] name = frm.getDecoding(elem.hp_hierogr[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.hp_hierogr[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton26_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list117_count; i++)
            {
                string[] name = frm.getDecoding(elem.multi_exp[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.multi_exp[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton27_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list118_count; i++)
            {
                string[] name = frm.getDecoding(elem.teleport[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.teleport[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void radioButton28_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            count = 0;
            for (int i = 0; i < elem.list119_count; i++)
            {
                string[] name = frm.getDecoding(elem.dyes[i].name, "Unicode").Split('\0');
                dataGridView1.Rows.Add(elem.dyes[i].id, name[0]);
                count++;
            }
            label1.Text = "Всего элементов: " + count;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (type == 1)
            {
                int max_id = shop_126.item_count;
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                    {
                        shop_126.items.Add(new Items());
                        shop_126.items[max_id].local_id = max_id;
                        shop_126.items[max_id].main_type = cat_index;
                        shop_126.items[max_id].sub_type = sub_cat_index;
                        shop_126.items[max_id].icon = new byte[128];
                        shop_126.items[max_id].item_id = Convert.ToUInt32(dataGridView1[0, i].Value);
                        shop_126.items[max_id].item_count = 1;
                        shop_126.items[max_id].sell_options = new List<Sell_options>(4);
                        for (int j = 0; j < 4; j++)
                        {
                            shop_126.items[max_id].sell_options.Add(new Sell_options());
                            if (j == 0)
                                shop_126.items[max_id].sell_options[j].price = 100;
                            else
                                shop_126.items[max_id].sell_options[j].price = 0;
                            shop_126.items[max_id].sell_options[j].until_time = 0;
                            shop_126.items[max_id].sell_options[j].time = 0;
                        }
                        shop_126.items[max_id].props = 1;
                        shop_126.items[max_id].desc = new byte[1024];
                        shop_126.items[max_id].name = frm.getEncoding(dataGridView1[1, i].Value.ToString(), "Unicode");
                        frm.getGrid().Rows.Add(frm.getGrid().Rows.Count, max_id, dataGridView1[1, i].Value);
                        max_id++;
                    }
                }
                shop_126.item_count = max_id;
                frm.gshop_126 = shop_126;
            }
            else if (type==2)
            {
                int max_id = shop_14x_client.item_count;
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                    {
                        shop_14x_client.items.Add(new Items_1());
                        shop_14x_client.items[max_id].local_id = max_id;
                        shop_14x_client.items[max_id].main_type = cat_index;
                        shop_14x_client.items[max_id].sub_type = sub_cat_index;
                        shop_14x_client.items[max_id].icon = new byte[128];
                        shop_14x_client.items[max_id].item_id = Convert.ToUInt32(dataGridView1[0, i].Value);
                        shop_14x_client.items[max_id].item_count = 1;
                        shop_14x_client.items[max_id].sell_options = new List<Sell_options_1>(4);
                        for (int j = 0; j < 4; j++)
                        {
                            shop_14x_client.items[max_id].sell_options.Add(new Sell_options_1());
                            if (j == 0)
                                shop_14x_client.items[max_id].sell_options[j].price = 100;
                            else
                                shop_14x_client.items[max_id].sell_options[j].price = 0;
                            shop_14x_client.items[max_id].sell_options[j].end_time = 0;
                            shop_14x_client.items[max_id].sell_options[j].time = 0;
                            shop_14x_client.items[max_id].sell_options[j].start_time = 0;
                            shop_14x_client.items[max_id].sell_options[j].type = 0;
                            shop_14x_client.items[max_id].sell_options[j].day = 0;
                            shop_14x_client.items[max_id].sell_options[j].status = 1;
                            shop_14x_client.items[max_id].sell_options[j].flag = 0;
                        }
                        shop_14x_client.items[max_id].desc = new byte[1024];
                        shop_14x_client.items[max_id].name = frm.getEncoding(dataGridView1[1, i].Value.ToString(), "Unicode");
                        shop_14x_client.items[max_id].idGift = 0;
                        shop_14x_client.items[max_id].iGiftNum = 0;
                        shop_14x_client.items[max_id].iGiftTime = 0;
                        shop_14x_client.items[max_id].iLogPrice = 0;
                        frm.getGrid().Rows.Add(frm.getGrid().Rows.Count, max_id, dataGridView1[1, i].Value);
                        max_id++;
                    }
                }
                shop_14x_client.item_count = max_id;
                frm.gshop_14x_client = shop_14x_client;
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm.getGrid().Update();
        }
    }
}
