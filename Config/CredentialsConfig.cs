namespace WorkFilterBot;

public static class CredentialsConfig
{
    // Variables estáticas para acceso desde métodos static
    static readonly string API_ID = "";
    static readonly string API_HASH = "";
    static readonly string PHONE_NUMBER = "";
    static readonly long MY_USER_ID = 0;

    // Configuración que WTelegramClient llama cuando necesita datos
    public static string? Config(string what)
    {
        EnvConfig.LoadEnvConfig(API_ID, API_HASH, PHONE_NUMBER, MY_USER_ID);
        switch (what)
        {
            case "api_id": return API_ID;
            case "api_hash": return API_HASH;
            case "phone_number": return PHONE_NUMBER;
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
