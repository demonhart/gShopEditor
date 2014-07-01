using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gShopEditor.Structure
{
    class gShop_14x_Server
    {
        public int timestamp;
        public int item_count;
        public List<Items_2> items;
    }

    public class Items_2
    {
        public int local_id;
        public int main_type;
        public int sub_type;
        public uint item_id;
        public uint item_count;
        public List<Sell_options_1> sell_options;
        public int idGift;
        public int iGiftNum;
        public int iGiftTime;
        public int iLogPrice;
    }
}
