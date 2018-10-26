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
        public List<notification> notifications;
        [DataMember]
        public List<string> achievements=new List<string>();
        [DataMember]
        public int duelCount=0;


    }

    enum currencyEnum
    {
        bronze,
        silver,
        gold,
        platinum,
        trinkets,
        crystals,
        starbucks,
        fudgeTokens,
        keys,
        ironOre,
        rupees
    }
}
