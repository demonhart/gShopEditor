using System.Text;
using System.IO;
using gShopEditor.Structure;
using System.Collections.Generic;

namespace gShopEditor
{
    class ElementsReader
    {
        public Elements getNewPos(int version, BinaryReader read)
        {
            Elements element = new Elements();
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
                                    read.BaseStream.Position = getListReader(read, element.weapons, element.list4_count, 8, 388, 812);
                                else if (version > 12 && version <= 101)
                                    read.BaseStream.Position = getListReader(read, element.weapons, element.list4_count, 8, 388, 828);
                                break;
                            }
                        case 6:
                            {
                                if (version == 12)
                                    read.BaseStream.Position = getListReader(read, element.armor, element.list7_count, 8, 160, 740);
                                else if (version > 12 && version <= 101)
                                    read.BaseStream.Position = getListReader(read, element.armor, element.list7_count, 8, 160, 764);
                                break;
                            }
                        case 9:
                            {
                                if (version == 12)
                                    read.BaseStream.Position = getListReader(read, element.ornaments, element.list10_count, 8, 256, 696);
                                else if (version > 12 && version <= 101)
                                    read.BaseStream.Position = getListReader(read, element.ornaments, element.list10_count, 8, 256, 708);
                                break;
                            }
                        case 12:
                            {
                                break;
                            }
                        case 15:
                            {
                                break;
                            }
                        case 17:
                            {
                                break;
                            }
                        case 21:
                            {
                                break;
                            }
                        case 22:
                            {
                                break;
                            }
                        case 26:
                            {
                                break;
                            }
                        case 28:
                            {
                                break;
                            }
                        case 33:
                            {
                                break;
                            }
                        case 35:
                            {
                                break;
                            }
                        case 58:
                            {
                                break;
                            }
                        case 75:
                            {
                                break;
                            }
                        case 83:
                            {
                                break;
                            }
                        case 94:
                            {
                                break;
                            }
                        case 96:
                            {
                                break;
                            }
                        case 98:
                            {
                                break;
                            }
                        case 106:
                            {
                                break;
                            }
                        case 107:
                            {
                                break;
                            }
                        case 112:
                            {
                                break;
                            }
                        case 113:
                            {
                                break;
                            }
                        case 114:
                            {
                                break;
                            }
                        case 115:
                            {
                                break;
                            }
                        case 117:
                            {
                                break;
                            }
                        case 118:
                            {
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
                                }
                                break;
                            }
                    }
                }
            }
            return element;
        }

        private long getListReader(BinaryReader read, List<ListToRead> list, int count, params long[] pos)
        {
            list = new List<ListToRead>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(new ListToRead());
                list[i].id = read.ReadInt32();
                read.BaseStream.Position += pos[0];
                list[i].name = read.ReadBytes(64);
                read.BaseStream.Position += pos[1];
                list[i].surface = read.ReadBytes(128);
                read.BaseStream.Position += pos[2];
            }
            return read.BaseStream.Position;
        }
    }
}
