using WTelegram;
using MazUserBot;

try
{
    // Cargar variables de entorno
    EnvConfig.LoadEnvConfig();

    Console.WriteLine($"🚀 Iniciando userbot para filtrar: {string.Join(", ", VariableHandler.FILTROS)}");
    Console.WriteLine($"📱 Número: {VariableHandler.PHONE_NUMBER}");
    Console.WriteLine($"🆔 API ID: {VariableHandler.API_ID}");

    // Crear cliente con la configuración

    VariableHandler.Client = new Client(CredentialsConfig.Config);

    // Login automático (la primera vez pedirá código)
    var user = await VariableHandler.Client.LoginUserIfNeeded();
    Console.WriteLine($"✅ Conectado como: {user.first_name} {user.last_name} (ID: {user.id})");

    // Actualizar MI_USER_ID con el ID real si es necesario
    if (VariableHandler.MY_USER_ID == 0)
    {
        VariableHandler.MY_USER_ID = user.id;
        Console.WriteLine($"ℹ️ Usando tu ID: {VariableHandler.MY_USER_ID}");
    }

    // Mostrar todos los grupos disponibles
    await MessageBuilder.ListGroups(VariableHandler.Client);

    // Suscribirse a los mensajes nuevos
    VariableHandler.Client.OnUpdates += UpdateHandler.OnUpdate;

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
    VariableHandler.Client?.Dispose();
}




