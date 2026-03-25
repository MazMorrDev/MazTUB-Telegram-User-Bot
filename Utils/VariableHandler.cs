using WTelegram;

namespace MazUserBot;

public static class VariableHandler
{
    // Variables estáticas para acceso desde métodos static
    public static string API_ID { get; set; } = "";
    public static string API_HASH { get; set; } = "";
    public static string PHONE_NUMBER { get; set; } = "";
    public static long MY_USER_ID { get; set; } = 0;

    // El texto a filtrar
    public static string[] FILTROS { get; set; } = [".net", "c#", "azure", "entity framework"];

    // IDs de grupos específicos - SOLO ESTOS GRUPOS SERÁN MONITOREADOS PARA RECIBIR MENSAJES
    public static long[] GROUPS_TO_LISTEN { get; set; } =
    [
        1131530511,  // CubanTech Jobs
        1594268732,  // Cuba CompuJobs
        1382170463,  // Cuban Software Developers
        1449611471,  // Cuban web developers
    ];

    // IDs de grupos específicos - SOLO ESTOS GRUPOS SERÁN MONITOREADOS PARA ENVIAR MENSAJES
    public static long[] GROUPS_TO_SEND { get; set; } =
    [
        1131530511,  // CubanTech Jobs
        1594268732,  // Cuba CompuJobs
        1382170463,  // Cuban Software Developers
        1449611471,  // Cuban web developers
    ];

    public static string[] MESSAGES_TO_SEND { get; set; } =
    [
        "Mensaje1", "Mensaje2"
    ];

    public static Client? Client { get; set; } = null;
}
