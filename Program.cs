using WTelegram;
using MazUserBot;

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
    client.OnUpdates += UpdateHandler.OnUpdate;

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




