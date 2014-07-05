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
            int version = read.ReadInt16();
            read.BaseStream.Position = 8;
            if (version == 12 || version == 60 || version == 63 || version == 69 || version == 70 || version == 85 || version == 88 || version == 101)
            {
                using (StreamReader elRead = new StreamReader("configs/PW_v" + version + ".cfg"))
                {
                    string elem = elRead.ReadToEnd();
                    string[] lists = elem.Split('\n');
                    for (int i = 0; i < lists.Length; i++)
                    {
                        switch (i)
                        {
                            case 3:
                                {
                                    element.list4_count = read.ReadInt32();
                                    if (version == 12)
                                        element.weapons = getListReader(read, read.BaseStream.Position, element.weapons, element.list4_count, 64, 128, 8, 388, 812);
                                    else if (version > 12 && version <= 101)
                                        element.weapons = getListReader(read, read.BaseStream.Position, element.weapons, element.list4_count, 64, 128, 8, 388, 828);
                                    break;
                                }
                            case 6:
                                {
                                    element.list7_count = read.ReadInt32();
                                    if (version == 12)
                                        element.armor = getListReader(read, read.BaseStream.Position, element.armor, element.list7_count, 64, 128, 8, 160, 740);
                                    else if (version > 12 && version <= 101)
                                        element.armor = getListReader(read, read.BaseStream.Position, element.armor, element.list7_count, 64, 128, 8, 160, 764);
                                    break;
                                }
                            case 9:
                                {
                                    element.list10_count = read.ReadInt32();
                                    if (version == 12)
                                        element.ornaments = getListReader(read, read.BaseStream.Position, element.ornaments, element.list10_count, 64, 128, 8, 256, 696);
                                    else if (version > 12 && version <= 101)
                                        element.ornaments = getListReader(read, read.BaseStream.Position, element.ornaments, element.list10_count, 64, 128, 8, 256, 708);
                                    break;
                                }
                            case 12:
                                {
                                    element.list13_count = read.ReadInt32();
                                    element.remedies = getListReader(read, read.BaseStream.Position, element.remedies, element.list13_count, 64, 128, 8, 128, 44);
                                    break;
                                }
                            case 15:
                                {
                                    element.list16_count = read.ReadInt32();
                                    element.materials = getListReader(read, read.BaseStream.Position, element.materials, element.list16_count, 64, 128, 8, 128, 36);
                                    break;
                                }
                            case 17:
                                {
                                    element.list18_count = read.ReadInt32();
                                    element.atk_hierogr = getListReader(read, read.BaseStream.Position, element.atk_hierogr, element.list18_count, 64, 128, 4, 128, 36);
                                    break;
                                }
                            case 19:
                                {
                                    element.list20_count = read.ReadInt32();
                                    element.def_hierogr = getListReader(read, read.BaseStream.Position, element.def_hierogr, element.list20_count, 64, 128, 4, 128, 296);
                                    break;
                                }
                            case 20:
                                {
                                    if (version == 12)
                                        read.BaseStream.Position += 19;
                                    else if (version == 60)
                                        read.BaseStream.Position += 23;
                                    else if (version == 63)
                                        read.BaseStream.Position += 26;
                                    else if (version == 69)
                                        read.BaseStream.Position += 32;
                                    else if (version == 70)
                                        read.BaseStream.Position += 31;
                                    //else if (version == 80)
                                    //read.BaseStream.Position += ??;
                                    else if (version == 85)
                                        read.BaseStream.Position += 22;
                                    else if (version == 88)
                                        read.BaseStream.Position += 24;
                                    else if (version == 101)
                                        read.BaseStream.Position += 22;

                                    element.list21_count = read.ReadInt32();
                                    read.BaseStream.Position += 68 * element.list21_count;
                                    break;
                                }
                            case 21:
                                {
                                    element.list22_count = read.ReadInt32();
                                    element.skills = getListReader(read, read.BaseStream.Position, element.skills, element.list22_count, 64, 128, 4, 128, 20);
                                    break;
                                }
                            case 22:
                                {
                                    element.list23_count = read.ReadInt32();
                                    if (version == 12)
                                        element.flyes = getListReader(read, read.BaseStream.Position, element.flyes, element.list23_count, 64, 128, 0, 256, 64);
                                    else if (version > 12)
                                        element.flyes = getListReader(read, read.BaseStream.Position, element.flyes, element.list23_count, 64, 128, 0, 256, 72);
                                    break;
                                }
                            case 26:
                                {
                                    element.list27_count = read.ReadInt32();
                                    element.key_items = getListReader(read, read.BaseStream.Position, element.key_items, element.list27_count, 64, 128, 0, 128, 28);
                                    break;
                                }
                            case 28:
                                {
                                    element.list29_count = read.ReadInt32();
                                    element.quest_items = getListReader(read, read.BaseStream.Position, element.quest_items, element.list29_count, 64, 128, 0, 0, 12);
                                    break;
                                }
                            case 33:
                                {
                                    element.list34_count = read.ReadInt32();
                                    element.ammo = getListReader(read, read.BaseStream.Position, element.ammo, element.list34_count, 192, 128, 4, 128, 436);
                                    break;
                                }
                            case 35:
                                {
                                    element.list36_count = read.ReadInt32();
                                    element.soulgems = getListReader(read, read.BaseStream.Position, element.soulgems, element.list36_count, 64, 128, 4, 128, 108);
                                    break;
                                }
                            case 58:
                                {
                                    if (version == 12)
                                        read.BaseStream.Position += 3294300;
                                    else if (version == 60)
                                        read.BaseStream.Position += 3451256;
                                    //else if (version == 63)
                                    //read.BaseStream.Position += ??;
                                    else if (version == 69)
                                        read.BaseStream.Position += 3454290;
                                    else if (version == 70)
                                        read.BaseStream.Position += 3448546;
                                    else if (version == 88)
                                        read.BaseStream.Position += 3390202;

                                    break;
                                }
                            case 75:
                                {
                                    element.list76_count = read.ReadInt32();
                                    element.quest_rewards = getListReader(read, read.BaseStream.Position, element.quest_rewards, element.list76_count, 64, 128, 0, 128, 20);
                                    break;
                                }
                            case 79:
                                {
                                    element.list80_count = read.ReadInt32();
                                    if (version < 63)
                                        element.resources = getListReader(read, read.BaseStream.Position, element.resources, element.list80_count, 64, 0, 4, 0, 380);
                                    else if (version >= 63 && version <= 88)
                                        element.resources = getListReader(read, read.BaseStream.Position, element.resources, element.list80_count, 64, 0, 4, 0, 396);
                                    else if (version == 101)
                                        element.resources = getListReader(read, read.BaseStream.Position, element.resources, element.list80_count, 64, 0, 4, 0, 480);
                                    break;
                                }
                            case 83:
                                {
                                    element.list84_count = read.ReadInt32();
                                    if (version == 12)
                                        element.fashion = getListReader(read, read.BaseStream.Position, element.fashion, element.list84_count, 64, 128, 8, 160, 40);
                                    else if (version >= 60 && version <= 70)
                                        element.fashion = getListReader(read, read.BaseStream.Position, element.fashion, element.list84_count, 64, 128, 8, 160, 80);
                                    else if (version >= 80 && version <= 101)
                                        element.fashion = getListReader(read, read.BaseStream.Position, element.fashion, element.list84_count, 64, 128, 8, 160, 344);
                                    break;
                                }
                            case 95:
                                {
                                    element.list96_count = read.ReadInt32();
                                    element.pet_eggs = getListReader(read, read.BaseStream.Position, element.pet_eggs, element.list96_count, 64, 128, 0, 128, 304);
                                    break;
                                }
                            case 96:
                                {
                                    element.list97_count = read.ReadInt32();
                                    element.pet_food = getListReader(read, read.BaseStream.Position, element.pet_food, element.list97_count, 64, 128, 0, 128, 36);
                                    break;
                                }
                            case 98:
                                {
                                    element.list99_count = read.ReadInt32();
                                    element.fireworks = getListReader(read, read.BaseStream.Position, element.fireworks, element.list99_count, 64, 128, 0, 128, 156);
                                    break;
                                }
                            case 106:
                                {
                                    element.list107_count = read.ReadInt32();
                                    element.potions = getListReader(read, read.BaseStream.Position, element.potions, element.list107_count, 64, 128, 0, 128, 32);
                                    break;
                                }
                            case 107:
                                {
                                    element.list108_count = read.ReadInt32();
                                    if (version < 70)
                                        element.refining = getListReader(read, read.BaseStream.Position, element.refining, element.list108_count, 64, 128, 0, 128, 112);
                                    else if (version >= 70)
                                        element.refining = getListReader(read, read.BaseStream.Position, element.refining, element.list108_count, 64, 128, 0, 128, 120);
                                    break;
                                }
                            case 112:
                                {
                                    element.list113_count = read.ReadInt32();
                                    if (version < 60)
                                        element.heaven_books = getListReader(read, read.BaseStream.Position, element.heaven_books, element.list113_count, 64, 128, 0, 128, 60);
                                    else if (version >= 60)
                                        element.heaven_books = getListReader(read, read.BaseStream.Position, element.heaven_books, element.list113_count, 64, 128, 0, 128, 68);
                                    break;
                                }
                            case 113:
                                {
                                    element.list114_count = read.ReadInt32();
                                    element.chat_speakers = getListReader(read, read.BaseStream.Position, element.chat_speakers, element.list114_count, 64, 128, 0, 128, 24);
                                    break;
                                }
                            case 114:
                                {
                                    element.list115_count = read.ReadInt32();
                                    element.mp_hierogr = getListReader(read, read.BaseStream.Position, element.mp_hierogr, element.list115_count, 64, 128, 0, 128, 32);
                                    break;
                                }
                            case 115:
                                {
                                    element.list116_count = read.ReadInt32();
                                    element.hp_hierogr = getListReader(read, read.BaseStream.Position, element.hp_hierogr, element.list116_count, 64, 128, 0, 128, 32);
                                    break;
                                }
                            case 116:
                                {
                                    element.list117_count = read.ReadInt32();
                                    element.multi_exp = getListReader(read, read.BaseStream.Position, element.multi_exp, element.list117_count, 64, 128, 0, 128, 24);
                                    break;
                                }
                            case 117:
                                {
                                    element.list118_count = read.ReadInt32();
                                    element.teleport = getListReader(read, read.BaseStream.Position, element.teleport, element.list118_count, 64, 128, 0, 128, 20);
                                    break;
                                }
                            case 118:
                                {
                                    element.list119_count = read.ReadInt32();
                                    if (version < 80)
                                        element.dyes = getListReader(read, read.BaseStream.Position, element.dyes, element.list119_count, 64, 128, 0, 128, 44);
                                    else if (version >= 80)
                                        element.dyes = getListReader(read, read.BaseStream.Position, element.dyes, element.list119_count, 64, 128, 0, 128, 48);
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

        private List<ListToRead> getListReader(BinaryReader read, long position, List<ListToRead> list, int count, int name_length, int surface_length, params long[] pos)
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
            return list;
        }
    }
}
