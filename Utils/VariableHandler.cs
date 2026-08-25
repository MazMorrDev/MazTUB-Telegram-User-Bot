using TL;
using WTelegram;

namespace MazUserBot.Utils;

public static class VariableHandler
{
    public static string API_ID { get; set; } = "";
    public static string API_HASH { get; set; } = "";
    public static string PHONE_NUMBER { get; set; } = "";
    public static long MY_USER_ID { get; set; } = 0;

    public static string[] FILTROS { get; set; } = [];
    public static long[] GROUPS_TO_LISTEN { get; set; } = [];
    public static long[] GROUPS_TO_SEND { get; set; } = [];
    public static string[] MESSAGES_TO_SEND { get; set; } = [];
    public static int MESSAGE_INTERVAL_HOURS { get; set; } = 1;

    public static Client? Client { get; set; } = null;

    private static Dictionary<long, InputPeer>? _peerCache;

    public static async Task<InputPeer?> GetInputPeer(long groupId)
    {
        if (Client == null) return null;

        _peerCache ??= new Dictionary<long, InputPeer>();

        if (_peerCache.TryGetValue(groupId, out var cachedPeer))
            return cachedPeer;

        try
        {
            var chats = await Client.Messages_GetAllChats();
            if (chats.chats.TryGetValue(groupId, out var chat))
            {
                InputPeer? resolvedPeer = chat switch
                {
                    Channel channel => new InputPeerChannel(channel.ID, channel.access_hash),
                    Chat chatGroup => new InputPeerChat(chatGroup.ID),
                    _ => null
                };

                if (resolvedPeer != null)
                    _peerCache[groupId] = resolvedPeer;

                return resolvedPeer;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public static void ClearPeerCache()
    {
        _peerCache?.Clear();
    }
}
