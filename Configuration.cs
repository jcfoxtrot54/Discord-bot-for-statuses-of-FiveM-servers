using System;
using System.Collections.Generic;
using System.Text;

namespace StatusSharp
{
    class Configuration
    {
        public string embedTitle { get; set; }
        public string embedSubtitle { get; set; }
        public int embedColour { get; set; }
        public int embedUpdate { get; set; }
        public string botToken { get; set; }
        public ulong guildID { get; set; }
        public ulong channelID { get; set; }
        public List<Server> serverList { get; set; }
    }
}
