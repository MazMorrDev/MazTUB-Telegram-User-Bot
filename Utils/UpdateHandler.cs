using TL;

namespace MazUserBot;

public static class UpdateHandler
{
    // Manejador de actualizaciones (mensajes nuevos)
    public static async Task OnUpdate(IObject arg)
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
                        await MessageHandler.ProccessMessage(message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en update: {ex.Message}");
        }
    }
}
