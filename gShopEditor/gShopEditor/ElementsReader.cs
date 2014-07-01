using System.Text;
using System.IO;

namespace gShopEditor
{
    class ElementsReader
    {
        public long getNewPos(int version, BinaryReader read)
        {
            using (StreamReader elRead = new StreamReader("configs/PW_v" + version + ".cfg", Encoding.Unicode))
            {
                string elem = elRead.ReadToEnd();
                string[] lists = elem.Split('\n');
                for (int i = 0; i < lists.Length; i++)
                {
                    if (i == 3 || i == 6 || i == 9 || i == 12 || i == 15 || i == 17 || i == 21 || i == 22 || i == 26 || i == 28 || i == 33 || i == 35 || i == 58 || i == 75 || i == 83 || i == 94 || i == 96 || i == 98 || i == 106 || i == 107
                        || i == 112 || i == 113 || i == 114 || i == 115 || i == 117 || i == 118)
                        continue;
                    else
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
                            else if (list_config[j] == "wstring:192")
                                read.BaseStream.Position += 192;
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
                    }
                }
            }
            return read.BaseStream.Position;
        }
    }
}
