namespace MazUserBot;

public static class CredentialsConfig
{

    // Configuración que WTelegramClient llama cuando necesita datos
    public static string? Config(string what)
    {
        EnvConfig.LoadEnvConfig();
        switch (what)
        {
            case "api_id": return VariablesHandler.API_ID;
            case "api_hash": return VariablesHandler.API_HASH;
            case "phone_number": return VariablesHandler.PHONE_NUMBER;
            case "verification_code":
                Console.Write("📱 Código de verificación: ");
                return Console.ReadLine();
            case "first_name": return "User";      // Si necesita registro
            case "last_name": return "Bot";        // Si necesita registro
            case "password":
                Console.Write("🔐 Contraseña 2FA: ");
                return Console.ReadLine();         // Si tienes 2FA activado
            default: return null;                   // Valores por defecto
        }
    }
}
