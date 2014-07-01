using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gShopEditor.Structure
{
    public class Elements
    {
        public short version;
        public short unk;
        public int unk2;
        public int list1_count;
        public List<List1> list1;
        public int list2_count;
        public List<List2> weapons_class;
        public int list3_count;
        public List<List3> weapons_sub_class;
        public int list4_count;
        public List<List4> weapons;
        public int list5_count;
        public List<List5> armor_class;
        public int list6_count;
        public List<List6> armor_sub_class;
        public int list7_count;
        public List<List7> armor;
        public int list8_count;
        public int list9_count;
        public int list10_count;
        public List<List10> ornaments;
        public int list11_count;
        public int list12_count;
        public int list13_count;
        public List<List13> remedies;
        public int list14_count;
        public int list15_count;
        public int list16_count;
        public List<List16> materials;
        public int list17_count;
        public int list18_count;
        public List<List18> atk_hierogr;
        public int list19_count;
        public int list20_count;
        public List<List20> def_hierogr;
        public int list21_count;
        public int list22_count;
        public List<List22> skills;
        public int list23_count;
        public List<List23> flyes;
        public int list24_count;
        public int list25_count;   
        public int list26_count;   
        public int list27_count;
        public List<List27> key_items;
        public int list28_count;   
        public int list29_count;
        public List<List29> quest_items;
        public int list30_count;
        public int list31_count;  
        public int list32_count;
        public List<List32> ammo;
        public int list33_count;  
        public int list34_count;  
        public int list35_count; 
        public int list36_count;
        public List<List36> soulgems;
        public int list37_count;  
        public int list38_count;  
        public int list39_count;
        public int list40_count;
        public int list41_count;
        public int list42_count;
        public int list43_count;
        public int list44_count;
        public int list45_count;
        public int list46_count;
        public int list47_count;
        public int list48_count;
        public int list49_count;
        public int list50_count;
        public int list51_count;
        public int list52_count;
        public int list53_count;
        public int list54_count;
        public int list55_count;
        public int list56_count;
        public int list57_count;
        public int list58_count;
        public int list59_count;
        public int list60_count;
        public int list61_count;
        public int list62_count;
        public int list63_count;
        public int list64_count;
        public int list65_count;
        public int list66_count;
        public int list67_count;
        public int list68_count;
        public int list69_count;
        public int list70_count;
        public int list71_count;
        public int list72_count;
        public int list73_count;
        public int list74_count;
        public int list75_count;
        public int list76_count;
        public List<List76> quest_rewards;
        public int list77_count;
        public int list78_count;
        public int list79_count;
        public int list80_count;
        public List<List80> resources;
        public int list81_count;
        public int list82_count;
        public int list83_count;
        public int list84_count;
        public List<List84> fashion;
        public int list85_count;
        public int list86_count;
        public int list87_count;
        public int list88_count;
        public int list89_count;
        public int list90_count;
        public int list91_count;
        public int list92_count;
        public int list93_count;
        public int list94_count;
        public int list95_count;
        public int list96_count;
        public List<List96> pet_eggs;
        public int list97_count;
        public List<List97> pet_food;
        public int list98_count;
        public int list99_count;
        public List<List99> fireworks;
        public int list100_count;
        public int list101_count;
        public int list102_count;
        public int list103_count;
        public int list104_count;
        public int list105_count;
        public int list106_count;
        public int list107_count;
        public List<List107> potions;
        public int list108_count;
        public List<List108> refining;
        public int list109_count;
        public int list110_count;
        public int list111_count;
        public int list112_count;
        public int list113_count;
        public List<List113> heaven_books;
        public int list114_count;
        public List<List114> chat_speakers;
        public int list115_count;
        public List<List115> mp_hierogr;
        public int list116_count;
        public List<List116> hp_hierogr;
        public int list117_count;
        public List<List117> multi_exp;
        public int list118_count;
        public List<List118> teleport;
        public int list119_count;
        public List<List119> dyes;
    }

    public class List1
    {
        public int id;
        public byte[] name;
        public int bonus_count;
        public int bonus1_id;
        public int bonus2_id;
        public int bonus3_id;
    }

    public class List2
    {
        public int id;
        public byte[] name;
    }

    public class List3
    {
        public int id;
        public byte[] name;
    }

    public class List4
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List5
    {
        public int id;
        public byte[] name;
    }

    public class List6
    {
        public int id;
        public byte[] name;
        public int mask;
    }

    public class List7
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List10
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List13
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }
    
    public class List16
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List18
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List20
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List22
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List23
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List27
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List29
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List32
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }
    
    public class List36
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }
    
    public class List76
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List80
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }
    
    public class List84
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List96
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List97
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List99
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List107
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List108
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List113
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List114
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List115
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List116
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List117
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List118
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }

    public class List119
    {
        public int id;
        public byte[] name;
        public byte[] surface;
    }
}
