using MazUserBot.Utils;

namespace MazUserBot;

public static class CredentialsConfig
{
    private static bool _envLoaded = false;

    // Configuración que WTelegramClient llama cuando necesita datos
    public static string? Config(string what)
    {
        if (!_envLoaded)
        {
            EnvConfig.LoadEnvConfig();
            _envLoaded = true;
        }

        return what switch
        {
            "api_id" => VariableHandler.API_ID,
            "api_hash" => VariableHandler.API_HASH,
            "phone_number" => VariableHandler.PHONE_NUMBER,
            "verification_code" => PromptInput("📱 Código de verificación: "),
            "first_name" => "User",
            "last_name" => "Bot",
            "password" => PromptInput("🔐 Contraseña 2FA: "),
            _ => null
        };
    }

    private static string? PromptInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}
