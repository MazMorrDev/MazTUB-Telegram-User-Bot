using TL;
using WTelegram;

namespace WorkFilterBot;

public static class MessageBuilder
{
    // Construye los mensajes que luego se enviarán
    public async static Task ProccessMessage(Message message, long[] GRUPOS_A_MONITOREAR, string[] FILTROS, Client client)
    {
        try
        {
            // 1. Verificar que sea un mensaje de texto
            if (string.IsNullOrEmpty(message.message))
                return;

            // 2. Verificar que sea de un grupo o supergrupo
            if (message.Peer is not PeerChannel peerChannel)
                return;

            long grupoId = peerChannel.ID;

            // 3. Si hay lista blanca, verificar que el grupo esté en ella
            if (GRUPOS_A_MONITOREAR.Length > 0 && !GRUPOS_A_MONITOREAR.Contains(grupoId))
                return;

            // 4. APLICAR FILTRO
            bool contieneFiltro = FILTROS.Any(filtro =>
                message.message.Contains(filtro, StringComparison.OrdinalIgnoreCase));

            if (!contieneFiltro) return;

            // 5. Obtener información del grupo
            var chats = await client!.Messages_GetAllChats();
            if (!chats.chats.TryGetValue(grupoId, out var chat))
                return;

            // 6. Obtener información del remitente
            string nombreRemitente = "Desconocido";
            string usernameRemitente = "";

            if (message.From is PeerUser fromUser)
            {
                if (!string.IsNullOrEmpty(message.post_author))
                {
                    nombreRemitente = message.post_author;
                }
                else
                {
                    nombreRemitente = $"Usuario {fromUser.user_id}";

                    try
                    {
                        var users = await client.Users_GetUsers(new InputUser(fromUser.user_id, 0));
                        if (users?.Length > 0 && users[0] is User user)
                        {
                            if (!string.IsNullOrEmpty(user.first_name) || !string.IsNullOrEmpty(user.last_name))
                            {
                                nombreRemitente = $"{user.first_name ?? ""} {user.last_name ?? ""}".Trim();
                                if (string.IsNullOrEmpty(nombreRemitente))
                                    nombreRemitente = user.username ?? $"Usuario {user.id}";
                            }

                            if (!string.IsNullOrEmpty(user.username))
                            {
                                usernameRemitente = $"(@{user.username})";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ No se pudo obtener info del usuario {fromUser.user_id}: {ex.Message}");
                    }
                }
            }

            // 7. Construir mensaje para reenviar (SIN HTML - con caracteres especiales de Telegram)
            string tituloGrupo = chat.Title ?? "Sin título";
            string texto = message.message;
            string fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string filtrosTexto = string.Join(", ", FILTROS);

            if (texto.Length > 1000)
                texto = texto[..1000] + "... (mensaje truncado)";

            string mensajeParaReenviar = $"""
                📨 ¡Mensaje filtrado encontrado! 🔍

                🎯 Filtros detectados: {filtrosTexto}
                👥 Grupo: {tituloGrupo}
                🆔 ID Grupo: {grupoId}
                👤 De: {nombreRemitente} {usernameRemitente}
                📅 Fecha: {fecha}

                📝 Contenido:
                {texto}

                🤖 Enviado por tu userbot
                """;

            // 8. Enviar a Saved Messages con Markdown (SÍ FUNCIONA)
            await client.SendMessageAsync(new InputPeerSelf(), mensajeParaReenviar);

            // 8. Enviar a Saved Messages - VERSIÓN TEXTO PLANO (SIN HTML)
            await client.SendMessageAsync(new InputPeerSelf(), mensajeParaReenviar);

            Console.WriteLine($"✅ [{DateTime.Now:HH:mm:ss}] Mensaje reenviado desde '{chat.Title}'");

            Console.WriteLine($"✅ [{DateTime.Now:HH:mm:ss}] Mensaje reenviado desde '{chat.Title}'");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error procesando mensaje: {ex.Message}");
        }
    }

    // Función para listar todos los grupos donde estás
    public async static Task ListGroups(Client client)
    {
        try
        {
            Console.WriteLine("\n📋 Grupos donde estás:");
            var chats = await client!.Messages_GetAllChats();
            int contador = 0;

            foreach (var (id, chat) in chats.chats)
            {
                // Solo mostrar grupos activos (no chats personales)
                if (chat.IsActive && (chat is Channel { IsGroup: true } || chat is Chat))
                {
                    string tipo = chat is Channel ? "📢 Supergrupo" : "👥 Grupo";
                    Console.WriteLine($"  • {tipo}: {chat.Title}");
                    Console.WriteLine($"    ID: {id}");
                    contador++;
                }
            }

            if (contador == 0)
                Console.WriteLine("  No estás en ningún grupo");
            else
                Console.WriteLine($"\n  Total: {contador} grupos");

            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ No se pudieron listar grupos: {ex.Message}");
        }
    }
}
