using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gShopEditor.Structure
{
    public class gShop_126
    {
        public int timestamp;
        public int item_count;
        public List<Items> items;
        public List<Category> cats;
    }

    public class Items
    {
        public int local_id;
        public int main_type;
        public int sub_type;
        public byte[] icon;
        public uint item_id;
        public uint item_count;
        public List<Sell_options> sell_options;
        public uint props;
        public byte[] desc;
        public byte[] name;
    }

    public class Sell_options
    {
        public int price;
        public int until_time;
        public int time;
    }

    public class Category
    {
        public byte[] cat_name;
        public int sub_cat_count;
        public byte[][] sub_cat_name;
    }
}
