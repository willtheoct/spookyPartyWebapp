using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace spookyWebServer
{
    [DataContract]
    public struct duel
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
    public struct currency
    {
        [DataMember]
        public int Type;
        [DataMember]
        public int count;
    }
    [DataContract]
    public struct currencies
    {
    }
    [DataContract]
    public struct notification
    {
        [DataMember]
        public string text;
    }
    [DataContract]
    public struct PartyGoer
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
