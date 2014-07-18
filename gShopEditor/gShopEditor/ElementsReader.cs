using System.Text;
using System.IO;
using gShopEditor.Structure;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace gShopEditor
{
    public class ElementsReader
    {
        public Elements getElementsReader(Elements element, BinaryReader read)
        {
            List<Dialogs> dialogs = new List<Dialogs>();
            read.BaseStream.Position = 0;
            int version = read.ReadInt16();
            read.BaseStream.Position += 6;
            if (version > 7 && version <= 101)
            {
                using (StreamReader elRead = new StreamReader("configs/PW_v" + version + ".cfg"))
                {
                    string elem = elRead.ReadToEnd();
                    string[] lists = elem.Split('\n');
                    for (int i = 0; i < lists.Length - 1; i++)
                    {
                        switch (i)
                        {
                            case 3:
                                {
                                    element.list4_count = read.ReadInt32();
                                    if (version == 12)
                                        element.weapons = getListReader(read, read.BaseStream.Position, element.weapons, i, element.list4_count, 64, 128, 8, 388, 812);
                                    else if (version > 12 && version <= 101)
                                        element.weapons = getListReader(read, read.BaseStream.Position, element.weapons, i, element.list4_count, 64, 128, 8, 388, 828);
                                    break;
                                }
                            case 6:
                                {
                                    element.list7_count = read.ReadInt32();
                                    if (version == 12)
                                        element.armor = getListReader(read, read.BaseStream.Position, element.armor, i, element.list7_count, 64, 128, 8, 160, 740);
                                    else if (version > 12 && version <= 101)
                                        element.armor = getListReader(read, read.BaseStream.Position, element.armor, i, element.list7_count, 64, 128, 8, 160, 764);
                                    break;
                                }
                            case 9:
                                {
                                    element.list10_count = read.ReadInt32();
                                    if (version == 12)
                                        element.ornaments = getListReader(read, read.BaseStream.Position, element.ornaments, i, element.list10_count, 64, 128, 8, 256, 696);
                                    else if (version > 12 && version <= 101)
                                        element.ornaments = getListReader(read, read.BaseStream.Position, element.ornaments, i, element.list10_count, 64, 128, 8, 256, 708);
                                    break;
                                }
                            case 12:
                                {
                                    element.list13_count = read.ReadInt32();
                                    element.remedies = getListReader(read, read.BaseStream.Position, element.remedies, i, element.list13_count, 64, 128, 8, 128, 44);
                                    break;
                                }
                            case 15:
                                {
                                    element.list16_count = read.ReadInt32();
                                    element.materials = getListReader(read, read.BaseStream.Position, element.materials, i, element.list16_count, 64, 128, 8, 128, 36);
                                    break;
                                }
                            case 17:
                                {
                                    element.list18_count = read.ReadInt32();
                                    element.atk_hierogr = getListReader(read, read.BaseStream.Position, element.atk_hierogr, i, element.list18_count, 64, 128, 4, 128, 36);
                                    break;
                                }
                            case 19:
                                {
                                    element.list20_count = read.ReadInt32();
                                    element.def_hierogr = getListReader(read, read.BaseStream.Position, element.def_hierogr, i, element.list20_count, 64, 128, 4, 128, 296);
                                    break;
                                }
                            case 20:
                                {
                                    getCheckPos(read, 145, 1);
                                    element.list21_count = read.ReadInt32();
                                    read.BaseStream.Position += 68 * element.list21_count;
                                    break;
                                }
                            case 21:
                                {
                                    element.list22_count = read.ReadInt32();
                                    element.skills = getListReader(read, read.BaseStream.Position, element.skills, i, element.list22_count, 64, 128, 4, 128, 20);
                                    break;
                                }
                            case 22:
                                {
                                    element.list23_count = read.ReadInt32();
                                    if (version == 12)
                                        element.flyes = getListReader(read, read.BaseStream.Position, element.flyes, i, element.list23_count, 64, 128, 0, 256, 64);
                                    else if (version > 12)
                                        element.flyes = getListReader(read, read.BaseStream.Position, element.flyes, i, element.list23_count, 64, 128, 0, 256, 72);
                                    break;
                                }
                            case 26:
                                {
                                    element.list27_count = read.ReadInt32();
                                    element.key_items = getListReader(read, read.BaseStream.Position, element.key_items, i, element.list27_count, 64, 128, 0, 128, 28);
                                    break;
                                }
                            case 28:
                                {
                                    element.list29_count = read.ReadInt32();
                                    element.quest_items = getListReader(read, read.BaseStream.Position, element.quest_items, i, element.list29_count, 64, 128, 0, 0, 12);
                                    break;
                                }
                            case 33:
                                {
                                    element.list34_count = read.ReadInt32();
                                    element.ammo = getListReader(read, read.BaseStream.Position, element.ammo, i, element.list34_count, 64, 128, 4, 128, 12);
                                    break;
                                }
                            case 35:
                                {
                                    element.list36_count = read.ReadInt32();
                                    element.soulgems = getListReader(read, read.BaseStream.Position, element.soulgems, i, element.list36_count, 64, 128, 4, 128, 108);
                                    break;
                                }
                            case 58:
                                {
                                    int dialogs_count = read.ReadInt32();
                                    for (int j = 0; j < dialogs_count; j++)
                                    {
                                        dialogs.Add(new Dialogs());
                                        dialogs[j].id_talk = read.ReadUInt32();
                                        dialogs[j].text = read.ReadBytes(128);
                                        dialogs[j].num_window = read.ReadInt32();
                                        dialogs[j].windows = new List<Dialogs.Windows>(dialogs[j].num_window);
                                        for (int k = 0; k < dialogs[j].num_window; k++)
                                        {
                                            dialogs[j].windows.Add(new Dialogs.Windows());
                                            dialogs[j].windows[k].id = read.ReadUInt32();
                                            dialogs[j].windows[k].id_parent = read.ReadUInt32();
                                            dialogs[j].windows[k].length = read.ReadInt32();
                                            dialogs[j].windows[k].text = read.ReadBytes(dialogs[j].windows[k].length * 2);
                                            dialogs[j].windows[k].num_option = read.ReadInt32();
                                            dialogs[j].windows[k].options = new List<Dialogs.Options>(dialogs[j].windows[k].num_option);
                                            for (int l = 0; l < dialogs[j].windows[k].num_option; l++)
                                            {
                                                dialogs[j].windows[k].options.Add(new Dialogs.Options());
                                                dialogs[j].windows[k].options[l].id = read.ReadUInt32();
                                                dialogs[j].windows[k].options[l].text = read.ReadBytes(128);
                                                dialogs[j].windows[k].options[l].param = read.ReadUInt32();
                                            }
                                        }
                                    }
                                    break;
                                }
                            case 75:
                                {
                                    element.list76_count = read.ReadInt32();
                                    element.quest_rewards = getListReader(read, read.BaseStream.Position, element.quest_rewards, i, element.list76_count, 64, 128, 0, 128, 20);
                                    break;
                                }
                            case 79:
                                {
                                    element.list80_count = read.ReadInt32();
                                    if (version < 63)
                                        element.resources = getListReader(read, read.BaseStream.Position, element.resources, i, element.list80_count, 64, 0, 4, 0, 380);
                                    else if (version >= 63 && version <= 88)
                                        element.resources = getListReader(read, read.BaseStream.Position, element.resources, i, element.list80_count, 64, 0, 4, 0, 396);
                                    else if (version == 101)
                                        element.resources = getListReader(read, read.BaseStream.Position, element.resources, i, element.list80_count, 64, 0, 4, 0, 480);
                                    break;
                                }
                            case 83:
                                {
                                    element.list84_count = read.ReadInt32();
                                    if (version == 12)
                                        element.fashion = getListReader(read, read.BaseStream.Position, element.fashion, i, element.list84_count, 64, 128, 8, 160, 40);
                                    else if (version >= 60 && version <= 70)
                                        element.fashion = getListReader(read, read.BaseStream.Position, element.fashion, i, element.list84_count, 64, 128, 8, 160, 80);
                                    else if (version >= 80 && version <= 101)
                                        element.fashion = getListReader(read, read.BaseStream.Position, element.fashion, i, element.list84_count, 64, 128, 8, 160, 344);
                                    break;
                                }
                            case 95:
                                {
                                    element.list96_count = read.ReadInt32();
                                    element.pet_eggs = getListReader(read, read.BaseStream.Position, element.pet_eggs, i, element.list96_count, 64, 128, 0, 128, 304);
                                    break;
                                }
                            case 96:
                                {
                                    element.list97_count = read.ReadInt32();
                                    element.pet_food = getListReader(read, read.BaseStream.Position, element.pet_food, i, element.list97_count, 64, 128, 0, 128, 36);
                                    break;
                                }
                            case 98:
                                {
                                    element.list99_count = read.ReadInt32();
                                    element.fireworks = getListReader(read, read.BaseStream.Position, element.fireworks, i, element.list99_count, 64, 128, 0, 128, 156);
                                    break;
                                }
                            case 100:
                                {
                                    getCheckPos(read, 45, 39);
                                    element.list100_count = read.ReadInt32();
                                    read.BaseStream.Position += 148 * element.list100_count;
                                    break;
                                }
                            case 106:
                                {
                                    element.list107_count = read.ReadInt32();
                                    element.potions = getListReader(read, read.BaseStream.Position, element.potions, i, element.list107_count, 64, 128, 0, 128, 32);
                                    break;
                                }
                            case 107:
                                {
                                    element.list108_count = read.ReadInt32();
                                    if (version < 70)
                                        element.refining = getListReader(read, read.BaseStream.Position, element.refining, i, element.list108_count, 64, 128, 0, 128, 112);
                                    else if (version >= 70)
                                        element.refining = getListReader(read, read.BaseStream.Position, element.refining, i, element.list108_count, 64, 128, 0, 128, 120);
                                    break;
                                }
                            case 112:
                                {
                                    element.list113_count = read.ReadInt32();
                                    if (version < 60)
                                        element.heaven_books = getListReader(read, read.BaseStream.Position, element.heaven_books, i, element.list113_count, 64, 128, 0, 128, 60);
                                    else if (version >= 60)
                                        element.heaven_books = getListReader(read, read.BaseStream.Position, element.heaven_books, i, element.list113_count, 64, 128, 0, 128, 68);
                                    break;
                                }
                            case 113:
                                {
                                    element.list114_count = read.ReadInt32();
                                    element.chat_speakers = getListReader(read, read.BaseStream.Position, element.chat_speakers, i, element.list114_count, 64, 128, 0, 128, 24);
                                    break;
                                }
                            case 114:
                                {
                                    element.list115_count = read.ReadInt32();
                                    element.mp_hierogr = getListReader(read, read.BaseStream.Position, element.mp_hierogr, i, element.list115_count, 64, 128, 0, 128, 32);
                                    break;
                                }
                            case 115:
                                {
                                    element.list116_count = read.ReadInt32();
                                    element.hp_hierogr = getListReader(read, read.BaseStream.Position, element.hp_hierogr, i, element.list116_count, 64, 128, 0, 128, 32);
                                    break;
                                }
                            case 116:
                                {
                                    element.list117_count = read.ReadInt32();
                                    element.multi_exp = getListReader(read, read.BaseStream.Position, element.multi_exp, i, element.list117_count, 64, 128, 0, 128, 24);
                                    break;
                                }
                            case 117:
                                {
                                    element.list118_count = read.ReadInt32();
                                    element.teleport = getListReader(read, read.BaseStream.Position, element.teleport, i, element.list118_count, 64, 128, 0, 128, 20);
                                    break;
                                }
                            case 118:
                                {
                                    element.list119_count = read.ReadInt32();
                                    if (version < 80)
                                        element.dyes = getListReader(read, read.BaseStream.Position, element.dyes, i, element.list119_count, 64, 128, 0, 128, 44);
                                    else if (version >= 80)
                                        element.dyes = getListReader(read, read.BaseStream.Position, element.dyes, i, element.list119_count, 64, 128, 0, 128, 48);
                                    break;
                                }
                            default:
                                {
                                    int list_count = read.ReadInt32();
                                    long new_pos = 0;
                                    string[] list_config = lists[i].Split(new char[] { ';', '\r' });
                                    for (int j = 0; j < list_config.Length; j++)
                                    {
                                        list_config[j] = list_config[j].Replace("\r", string.Empty);

                                        if (list_config[j] == "int32")
                                            new_pos += 4;
                                        else if (list_config[j] == "float")
                                            new_pos += 4;
                                        else if (list_config[j] == "wstring:16")
                                            new_pos += 16;
                                        else if (list_config[j] == "wstring:32")
                                            new_pos += 32;
                                        else if (list_config[j] == "wstring:64")
                                            new_pos += 64;
                                        else if (list_config[j] == "wstring:128")
                                            new_pos += 128;
                                        else if (list_config[j] == "wstring:192")
                                            new_pos += 192;
                                        else if (list_config[j] == "wstring:256")
                                            new_pos += 256;
                                        else if (list_config[j] == "wstring:512")
                                            new_pos += 512;
                                        else if (list_config[j] == "wstring:1024")
                                            new_pos += 1024;
                                        else if (list_config[j] == "string:32")
                                            new_pos += 32;
                                        else if (list_config[j] == "string:64")
                                            new_pos += 64;
                                        else if (list_config[j] == "string:128")
                                            new_pos += 128;
                                        else if (list_config[j] == "string:192")
                                            new_pos += 192;
                                        else if (list_config[j] == "string:256")
                                            new_pos += 256;
                                        else if (list_config[j] == "string:512")
                                            new_pos += 512;
                                        else if (list_config[j] == "string:1024")
                                            new_pos += 1024;
                                        else if (list_config[j].Split(':')[0] == "byte")
                                            new_pos += Int64.Parse(list_config[j].Split(':')[1]);
                                    }
                                    new_pos *= list_count;
                                    read.BaseStream.Position += new_pos;
                                    break;
                                }
                        }
                    }
                }
                
            }
            else
                MessageBox.Show("Версия [" + version + "] не поддерживается", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return element;
        }

        private List<ListToRead> getListReader(BinaryReader read, long position, List<ListToRead> list, int index, int count, int name_length, int surface_length, params long[] pos)
        {
            try
            {
                list = new List<ListToRead>(count);
                read.BaseStream.Position = position;
                for (int i = 0; i < count; i++)
                {
                    list.Add(new ListToRead());
                    list[i].id = read.ReadInt32();
                    read.BaseStream.Position += pos[0];
                    list[i].name = read.ReadBytes(name_length);
                    read.BaseStream.Position += pos[1];
                    list[i].surface = read.ReadBytes(surface_length);
                    read.BaseStream.Position += pos[2];
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Ошибка при чтении листа [" + index + "]", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return list;
        }

        private void getCheckPos(BinaryReader read, byte num1, byte num2)
        {
            byte number1 = 0;
            while (true)
            {
                number1 = read.ReadByte();
                if (number1 == num1)
                {
                    byte number2 = read.ReadByte();
                    if (number2 == num2)
                        break;
                }
            }
            read.BaseStream.Position -= 6;
        }
    }
}
