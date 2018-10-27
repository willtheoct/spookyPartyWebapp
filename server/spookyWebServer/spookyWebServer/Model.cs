using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace spookyWebServer
{
    [DataContract]
    public class duel
    {
        [DataMember]
        public int src;
        [DataMember]
        public currency[] srcWager;
        [DataMember]
        public int target;
        [DataMember]
        public currency[] targetWager;
        [DataMember]
        public int referee;
        [DataMember]
        public int winner;
        [DataMember]
        public string description;
        [DataMember]
        public string id;
    }
    [DataContract]
    public class currency
    {
        [DataMember]
        public int Type;
        [DataMember]
        public int count;
    }
    [DataContract]
    public class currencies
    {
    }
    [DataContract]
    public class notification
    {
        [DataMember]
        public string text;
        [DataMember]
        public string link;
    }
    [DataContract]
    public class PartyGoer
    {
        [DataMember]
        public string characterName;
        [DataMember]
        public int id;
        [DataMember]
        public int level;
        [DataMember]
        public int[] inventory;
        [DataMember]
        public string passPhrase;
        [DataMember]
        public List<notification> notifications=new List<notification>();
        [DataMember]
        public List<int> achievements=new List<int>();
        [DataMember]
        public int duelCount = 0;
        [DataMember]
        public int slewnMonsters = 0;


    }

    enum currencyEnum
    {
        gold,
        crystals,
        ironOre,/*
        bronze,
        silver,
        platinum,
        trinkets,
        starbucks,
        fudgeTokens,
        keys,
        rupees*/
    }

    enum achievementsEnum
    {
        sneakthief = 1,
        showoff = 2,
        monsterhunter = 3,
        breaksomething = 4,
        devourer = 5,
        galois = 6,
        duelist = 7,
        champion = 8,
        grinding = 9,
        adventurer = 10,
        partyoffive = 11,
        hairywizard = 12,
        aimbot = 13
    }
}
