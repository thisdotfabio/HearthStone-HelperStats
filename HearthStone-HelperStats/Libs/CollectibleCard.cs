using System.Runtime.Serialization;

namespace HearthStone_HelperStats.Libs
{
    [DataContract]
    class CollectibleCard
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public int cost { get; set; }
    }
}
