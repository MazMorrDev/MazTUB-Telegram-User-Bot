using WTelegram;
using TL;
using WorkFilterBot;

// Variables estáticas para acceso desde métodos static
string API_ID = "";
string API_HASH = "";
string PHONE_NUMBER = "";
long MY_USER_ID = 0;

// El texto a filtrar
string[] FILTROS = [".net", "c#", "azure", "entity framework"];

// IDs de grupos específicos - SOLO ESTOS GRUPOS SERÁN MONITOREADOS
long[] GRUPOS_A_MONITOREAR =
[
    1131530511,  // CubanTech Jobs
    1594268732,  // Cuba CompuJobs
    1382170463,  // Cuban Software Developers
    1449611471,  // Cuban web developers
];

Client? client = null;

try
{
    // Cargar variables de entorno
    EnvConfig.LoadEnvConfig(API_ID, API_HASH, PHONE_NUMBER, MY_USER_ID);

    Console.WriteLine($"🚀 Iniciando userbot para filtrar: {string.Join(", ", FILTROS)}");
    Console.WriteLine($"📱 Número: {PHONE_NUMBER}");
    Console.WriteLine($"🆔 API ID: {API_ID}");

    // Crear cliente con la configuración
    client = new Client(CredentialsConfig.Config);

    // Login automático (la primera vez pedirá código)
    var user = await client.LoginUserIfNeeded();
    Console.WriteLine($"✅ Conectado como: {user.first_name} {user.last_name} (ID: {user.id})");

    // Actualizar MI_USER_ID con el ID real si es necesario
    if (MY_USER_ID == 0)
    {
        MY_USER_ID = user.id;
        Console.WriteLine($"ℹ️ Usando tu ID: {MY_USER_ID}");
    }

    // Mostrar todos los grupos disponibles
    await MessageBuilder.ListGroups(client);

    // Suscribirse a los mensajes nuevos
    client.OnUpdates += OnUpdate;

    Console.WriteLine("\n👂 Escuchando mensajes nuevos...");
    Console.WriteLine("Presiona Ctrl+C para salir\n");

    // Mantener el programa corriendo
    await Task.Delay(-1);
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error fatal: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}
finally
{
    client?.Dispose();
}

// Manejador de actualizaciones (mensajes nuevos)
async Task OnUpdate(IObject arg)
{
    try
    {
        // Procesar diferentes tipos de actualizaciones
        if (arg is UpdatesBase updatesBase)
        {
            foreach (var update in updatesBase.UpdateList)
            {
                // Cuando llega un mensaje nuevo
                if (update is UpdateNewMessage updateNewMessage &&
                    updateNewMessage.message is Message message)
                {
                    await MessageBuilder.ProccessMessage(message, GRUPOS_A_MONITOREAR, FILTROS, client);
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error en update: {ex.Message}");
    }
}


