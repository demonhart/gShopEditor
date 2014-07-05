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
        public Elements getElementsReader(Elements element, short version, BinaryReader read)
        {
            if (version == 12 || version == 60 || version == 63 || version == 69 || version == 70 || version == 85 || version == 88 || version == 101)
            {
                using (StreamReader elRead = new StreamReader("configs/PW_v" + version + ".cfg", Encoding.Unicode))
                {
                    string elem = elRead.ReadToEnd();
                    string[] lists = elem.Split('\n');
                    for (int i = 0; i < lists.Length; i++)
                    {
                        switch (i)
                        {
                            case 3:
                                {
                                    if (version == 12)
                                        read.BaseStream.Position = getListReader(read, element.weapons, element.list4_count, 64, 128, 8, 388, 812);
                                    else if (version > 12 && version <= 101)
                                        read.BaseStream.Position = getListReader(read, element.weapons, element.list4_count, 64, 128, 8, 388, 828);
                                    break;
                                }
                            case 6:
                                {
                                    if (version == 12)
                                        read.BaseStream.Position = getListReader(read, element.armor, element.list7_count, 64, 128, 8, 160, 740);
                                    else if (version > 12 && version <= 101)
                                        read.BaseStream.Position = getListReader(read, element.armor, element.list7_count, 64, 128, 8, 160, 764);
                                    break;
                                }
                            case 9:
                                {
                                    if (version == 12)
                                        read.BaseStream.Position = getListReader(read, element.ornaments, element.list10_count, 64, 128, 8, 256, 696);
                                    else if (version > 12 && version <= 101)
                                        read.BaseStream.Position = getListReader(read, element.ornaments, element.list10_count, 64, 128, 8, 256, 708);
                                    break;
                                }
                            case 12:
                                {
                                    read.BaseStream.Position = getListReader(read, element.remedies, element.list13_count, 64, 128, 8, 128, 44);
                                    break;
                                }
                            case 15:
                                {
                                    read.BaseStream.Position = getListReader(read, element.materials, element.list16_count, 64, 128, 8, 128, 36);
                                    break;
                                }
                            case 17:
                                {
                                    read.BaseStream.Position = getListReader(read, element.atk_hierogr, element.list18_count, 64, 128, 4, 128, 36);
                                    break;
                                }
                            case 19:
                                {
                                    read.BaseStream.Position = getListReader(read, element.def_hierogr, element.list20_count, 64, 128, 4, 128, 296);
                                    break;
                                }
                            case 20:
                                {
                                    read.BaseStream.Position += 68 * element.list21_count;
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
                                    break;
                                }
                            case 21:
                                {
                                    read.BaseStream.Position = getListReader(read, element.skills, element.list22_count, 64, 128, 4, 128, 20);
                                    break;
                                }
                            case 22:
                                {
                                    if (version == 12)
                                        read.BaseStream.Position = getListReader(read, element.flyes, element.list23_count, 64, 128, 0, 256, 64);
                                    else if (version > 12)
                                        read.BaseStream.Position = getListReader(read, element.flyes, element.list23_count, 64, 128, 0, 256, 72);
                                    break;
                                }
                            case 26:
                                {
                                    read.BaseStream.Position = getListReader(read, element.key_items, element.list27_count, 64, 128, 0, 128, 28);
                                    break;
                                }
                            case 28:
                                {
                                    read.BaseStream.Position = getListReader(read, element.quest_items, element.list29_count, 64, 128, 0, 0, 12);
                                    break;
                                }
                            case 33:
                                {
                                    read.BaseStream.Position = getListReader(read, element.ammo, element.list34_count, 192, 128, 4, 128, 436);
                                    break;
                                }
                            case 35:
                                {
                                    read.BaseStream.Position = getListReader(read, element.soulgems, element.list36_count, 64, 128, 4, 128, 108);
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
                                    read.BaseStream.Position = getListReader(read, element.quest_rewards, element.list76_count, 64, 128, 0, 128, 20);
                                    break;
                                }
                            case 79:
                                {
                                    if (version < 63)
                                        read.BaseStream.Position = getListReader(read, element.resources, element.list80_count, 64, 0, 4, 0, 380);
                                    else if (version >= 63 && version <= 88)
                                        read.BaseStream.Position = getListReader(read, element.resources, element.list80_count, 64, 0, 4, 0, 396);
                                    else if (version == 101)
                                        read.BaseStream.Position = getListReader(read, element.resources, element.list80_count, 64, 0, 4, 0, 480);
                                    break;
                                }
                            case 83:
                                {
                                    if (version == 12)
                                        read.BaseStream.Position = getListReader(read, element.fashion, element.list84_count, 64, 128, 8, 160, 40);
                                    else if (version >= 60 && version <= 70)
                                        read.BaseStream.Position = getListReader(read, element.fashion, element.list84_count, 64, 128, 8, 160, 80);
                                    else if (version >= 80 && version <= 101)
                                        read.BaseStream.Position = getListReader(read, element.fashion, element.list84_count, 64, 128, 8, 160, 344);
                                    break;
                                }
                            case 95:
                                {
                                    read.BaseStream.Position = getListReader(read, element.pet_eggs, element.list96_count, 64, 128, 0, 128, 304);
                                    break;
                                }
                            case 96:
                                {
                                    read.BaseStream.Position = getListReader(read, element.pet_food, element.list97_count, 64, 128, 0, 128, 36);
                                    break;
                                }
                            case 98:
                                {
                                    read.BaseStream.Position = getListReader(read, element.fireworks, element.list99_count, 64, 128, 0, 128, 156);
                                    break;
                                }
                            case 106:
                                {
                                    read.BaseStream.Position = getListReader(read, element.potions, element.list107_count, 64, 128, 0, 128, 32);
                                    break;
                                }
                            case 107:
                                {
                                    if (version < 70)
                                        read.BaseStream.Position = getListReader(read, element.refining, element.list108_count, 64, 128, 0, 128, 112);
                                    else if (version >= 70)
                                        read.BaseStream.Position = getListReader(read, element.refining, element.list108_count, 64, 128, 0, 128, 120);
                                    break;
                                }
                            case 112:
                                {
                                    if (version < 60)
                                        read.BaseStream.Position = getListReader(read, element.heaven_books, element.list113_count, 64, 128, 0, 128, 60);
                                    else if (version >= 60)
                                        read.BaseStream.Position = getListReader(read, element.heaven_books, element.list113_count, 64, 128, 0, 128, 68);
                                    break;
                                }
                            case 113:
                                {
                                    read.BaseStream.Position = getListReader(read, element.chat_speakers, element.list114_count, 64, 128, 0, 128, 24);
                                    break;
                                }
                            case 114:
                                {
                                    read.BaseStream.Position = getListReader(read, element.mp_hierogr, element.list115_count, 64, 128, 0, 128, 32);
                                    break;
                                }
                            case 115:
                                {
                                    read.BaseStream.Position = getListReader(read, element.hp_hierogr, element.list116_count, 64, 128, 0, 128, 32);
                                    break;
                                }
                            case 116:
                                {
                                    read.BaseStream.Position = getListReader(read, element.multi_exp, element.list117_count, 64, 128, 0, 128, 24);
                                    break;
                                }
                            case 117:
                                {
                                    read.BaseStream.Position = getListReader(read, element.teleport, element.list118_count, 64, 128, 0, 128, 20);
                                    break;
                                }
                            case 118:
                                {
                                    if (version < 80)
                                        read.BaseStream.Position = getListReader(read, element.dyes, element.list119_count, 64, 128, 0, 128, 44);
                                    else if (version >= 80)
                                        read.BaseStream.Position = getListReader(read, element.dyes, element.list119_count, 64, 128, 0, 128, 48);
                                    break;
                                }
                            default:
                                {
                                    string[] list_config = lists[i].Split(';');
                                    for (int j = 0; j < list_config.Length; j++)
                                    {
                                        if (list_config[j] == "int32")
                                            read.BaseStream.Position += 4;
                                        else if (list_config[j] == "float")
                                            read.BaseStream.Position += 4;
                                        else if (list_config[j] == "wstring:16")
                                            read.BaseStream.Position += 16;
                                        else if (list_config[j] == "wstring:32")
                                            read.BaseStream.Position += 32;
                                        else if (list_config[j] == "wstring:64")
                                            read.BaseStream.Position += 64;
                                        else if (list_config[j] == "wstring:128")
                                            read.BaseStream.Position += 128;
                                        else if (list_config[j] == "wstring:256")
                                            read.BaseStream.Position += 256;
                                        else if (list_config[j] == "wstring:512")
                                            read.BaseStream.Position += 512;
                                        else if (list_config[j] == "wstring:1024")
                                            read.BaseStream.Position += 1024;
                                        else if (list_config[j] == "string:32")
                                            read.BaseStream.Position += 32;
                                        else if (list_config[j] == "string:64")
                                            read.BaseStream.Position += 64;
                                        else if (list_config[j] == "string:128")
                                            read.BaseStream.Position += 128;
                                        else if (list_config[j] == "string:256")
                                            read.BaseStream.Position += 256;
                                        else if (list_config[j] == "string:512")
                                            read.BaseStream.Position += 512;
                                        else if (list_config[j] == "string:1024")
                                            read.BaseStream.Position += 1024;
                                        else if (list_config[j].Split(':')[0] == "byte")
                                            read.BaseStream.Position += Int32.Parse(list_config[j].Split(':')[1]);
                                    }
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

        private long getListReader(BinaryReader read, List<ListToRead> list, int count, int name_length, int surface_length, params long[] pos)
        {
            list = new List<ListToRead>(count);
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
            return read.BaseStream.Position;
        }
    }
}
