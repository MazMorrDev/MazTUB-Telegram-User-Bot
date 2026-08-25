using WTelegram;

namespace MazUserBot.Utils;

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
        1362472995,  // Revolico Oficial Cárdenas
        1586059492,  // VARBON'S Colón Ventas
        1458437447,  // REVOLICO SANTA MARTA
        1225295351,  // Compra y Ventas Varadero
        1424462534,  // La Chopi Cárdenas
        1638394149,  // Supermercado Cárdenas
        2147183997,  // VARADERO VICE Group
        1352130778,  // Todo de PC Cárdenas
        1128327152,  // Gtech Infinity PC & Accesorios Gaming
    ];

    public static string[] MESSAGES_TO_SEND { get; set; } =
    [
        @"Tu informático en Cárdenas 😳

Se hacen todo tipo de servicios de informática
- Instalación o Actualización de Windows
- Instalación y actualización del antivirus
- Limpieza de periféricos (mouse y teclado)
- Mantenimiento especializado para que su equipo dure muchos años más 
- Resolución de cualquier tipo de problema con el BIOS
- Instalación de drivers tanto generales como gráficos 
- Creación de todo tipo de software/web/bot/script personalizado para usted
- Y más, si tiene alguna duda tan solo pregunte sin pena",
    ];

    // Intervalo en horas entre envíos de mensajes
    public static int MESSAGE_INTERVAL_HOURS { get; set; } = 1;

    public static Client? Client { get; set; } = null;
}
