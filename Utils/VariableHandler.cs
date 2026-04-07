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
        @"Servicios Informáticos

🌐 Redes Sociales:
- Telegram: @MazMorr
- WhatsApp: +53 5550 5961

⚠️ Todos los precios están en USD pero se acepta cualquier moneda al cambio del momento, incluyendo el pago por tarjeta ⚠️ 
---------------------------------------
INSTALACIÓN O ACTUALIZACIÓN DE  WINDOWS: 3 USD

- Windows 11Pro 22H2
- Windows 10 22H2
- Windows 8  All EditionsES Full
- Windows 7 All EditionsES Full
- Linux Mint 22.2
- Kali Linux 2024.2
- Manjaro 23.0.4
- Ubuntu 2004

- Activación De Windows: 1 USD
----------------------------------
LISTADO DE LOS SOFTWARE  2026
(En cuanto usted como cliente requiera de este servicio yo le envio todo el catálogo para q pueda escoger)

- Cada 8 aplicaciones instaladas con éxito 1 USD 
-----------------------------------
ACTUALIZACIÓN DE DRIVERS⚙️ 
(Es prioritario cuando se acaba de instalar Windows o en caso de incorporar nuevo hardware a su equipo)

- 2 USD Drivers Generales

- 1 USD Drivers Gráficos
-------------------------------
DESINFECCIÓN O ELIMINACIÓN DE VIRUS INFORMÁTICOS 🦠 

- Instalación de ESET NOD 32:  2 USD 

- Actualización de ESET NOD 32: 1 USD 

- Instalación de USB-AV: 2 USD
---------------------------------------------------
MANTENIMIENTO 🪛 (Recomendado)

- Limpieza de polvo básica: 2 USD 

- Cambio de pasta térmica en Microprocesadores + Limpieza de su respectivo disipador 3 USD 

- Cambio de pasta térmica en Tarjetas Gráficas + Limpieza de su respectivo disipador térmico 4 USD 

- Limpieza profunda de teclado y mouse ⌨️ 🐁: 3 USD 
-------------------------------
SERVICIOS DE DESARROLLO  DE SOFTWARE PERSONALIZADO 
⚠️Todos los precios son negociables ⚠️ 

- Diseño e integración de bases de datos para proyectos 📦
- Creación de páginas web. 🕸 
- Creación de bots para Telegram 🤖 
- Creación de aplicaciones para PC 💻",
    ];

    // Intervalo en horas entre envíos de mensajes
    public static int MESSAGE_INTERVAL_HOURS { get; set; } = 1;

    public static Client? Client { get; set; } = null;
}
