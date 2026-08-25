using MazUserBot;
using MazUserBot.Utils;
using WTelegram;

try
{
    Helpers.Log = (lvl, str) => { if (lvl == 0) Console.WriteLine(str); };
    // Cargar variables de entorno
    EnvConfig.LoadEnvConfig();

    // Cargar configuración desde archivo JSON
    ConfigManager.LoadConfig();

    Console.WriteLine($"🚀 Iniciando userbot para filtrar: {string.Join(", ", VariableHandler.FILTROS)}");
    Console.WriteLine($"📱 Número: {VariableHandler.PHONE_NUMBER}");
    Console.WriteLine($"🆔 API ID: {VariableHandler.API_ID}");

    // Crear cliente con la configuración
    VariableHandler.Client = new Client(CredentialsConfig.Config);

    // Login automático (la primera vez pedirá código)
    var user = await VariableHandler.Client.LoginUserIfNeeded();
    Console.WriteLine($"✅ Conectado como: {user.first_name} {user.last_name} (ID: {user.id})");

    // Actualizar MY_USER_ID con el ID real si es necesario
    if (VariableHandler.MY_USER_ID == 0)
    {
        VariableHandler.MY_USER_ID = user.id;
        Console.WriteLine($"ℹ️ Usando tu ID: {VariableHandler.MY_USER_ID}");
    }

    // Mostrar todos los grupos disponibles
    await MessageHandler.ListGroups(VariableHandler.Client);

    // Suscribirse a los mensajes nuevos
    VariableHandler.Client.OnUpdates += UpdateHandler.OnUpdate;

    // Iniciar el listener de comandos en segundo plano
    _ = Task.Run(() => CommandHandler.StartCommandListener());

    Console.WriteLine("\n👂 Escuchando mensajes nuevos...");
    Console.WriteLine("Escribe /help para ver los comandos disponibles o Ctrl+C para salir\n");

    // Iniciar el envío de mensajes en segundo plano
    _ = Task.Run(() => MessageHandler.SendMessage());

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
    VariableHandler.Client?.Dispose();
}
