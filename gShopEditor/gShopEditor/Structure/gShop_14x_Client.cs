using System.Collections.Generic;

namespace gShopEditor.Structure
{
    public class gShop_14x_Client
    {
        public int timestamp;
        public int item_count;
        public List<Items_1> items;
        public List<Category> cats;
    }

    public class Items_1
    {
        public int local_id;
        public int main_type;
        public int sub_type;
        public byte[] icon;
        public uint item_id;
        public uint item_count;
        public List<Sell_options_1> sell_options;
        public byte[] desc;
        public byte[] name;
        public int idGift;
        public int iGiftNum;
        public int iGiftTime;
        public int iLogPrice;
    }

    public class Sell_options_1
    {
        public uint price;
        public uint end_time;
        public uint time;
        public uint start_time;
        public int type;
        public uint day;
        public uint status;
        public uint flag;
    }
}
