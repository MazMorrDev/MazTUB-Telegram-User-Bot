using DotNetEnv;

namespace MazUserBot;

public static class EnvConfig
{

    public static void LoadEnvConfig()
    {
        // Cargar archivo .env si existe
        try
        {
            Env.Load();
            Console.WriteLine("✅ Archivo .env cargado");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ No se pudo cargar .env: {ex.Message}");
        }

        // Leer variables con validación
        VariablesHandler.API_ID = Environment.GetEnvironmentVariable("API_ID") ?? "";
        VariablesHandler.API_HASH = Environment.GetEnvironmentVariable("API_HASH") ?? "";
        VariablesHandler.PHONE_NUMBER = Environment.GetEnvironmentVariable("PHONE_NUMBER") ?? "";

        string? userIdStr = Environment.GetEnvironmentVariable("USER_ID");
        if (!string.IsNullOrEmpty(userIdStr) && long.TryParse(userIdStr, out long userId))
        {
            VariablesHandler.MY_USER_ID = userId;
        }

        // Validar configuración mínima
        if (string.IsNullOrEmpty(VariablesHandler.API_ID)) throw new Exception("❌ API_ID no configurado en .env");
        if (string.IsNullOrEmpty(VariablesHandler.API_HASH)) throw new Exception("❌ API_HASH no configurado en .env");
        if (string.IsNullOrEmpty(VariablesHandler.PHONE_NUMBER)) throw new Exception("❌ PHONE_NUMBER no configurado en .env");

        // Verificar que API_ID sea número
        if (!long.TryParse(VariablesHandler.API_ID, out _)) throw new Exception("❌ API_ID debe ser un número");
    }
}
