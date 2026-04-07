using System;

namespace MazUserBot.Config
{
    public class BotConfig
    {
        public string[] FILTROS { get; set; } = Array.Empty<string>();
        public long[] GROUPS_TO_LISTEN { get; set; } = Array.Empty<long>();
        public long[] GROUPS_TO_SEND { get; set; } = Array.Empty<long>();
        public string[] MESSAGES_TO_SEND { get; set; } = Array.Empty<string>();
        public int MESSAGE_INTERVAL_HOURS { get; set; } = 1;
    }
}