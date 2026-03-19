using WTelegram;
using TL;
using DotNetEnv;

// Variables estáticas para acceso desde métodos static
string API_ID = "";
string API_HASH = "";
string PHONE_NUMBER = "";
long MI_USER_ID = 0;

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

Client? _client = null;

try
{
    // Cargar variables de entorno
    LoadConfig();

    Console.WriteLine($"🚀 Iniciando userbot para filtrar: {string.Join(", ", FILTROS)}");
    Console.WriteLine($"📱 Número: {PHONE_NUMBER}");
    Console.WriteLine($"🆔 API ID: {API_ID}");

    // Crear cliente con la configuración
    _client = new Client(Config);

    // Login automático (la primera vez pedirá código)
    var user = await _client.LoginUserIfNeeded();
    Console.WriteLine($"✅ Conectado como: {user.first_name} {user.last_name} (ID: {user.id})");

    // Actualizar MI_USER_ID con el ID real si es necesario
    if (MI_USER_ID == 0)
    {
        MI_USER_ID = user.id;
        Console.WriteLine($"ℹ️ Usando tu ID: {MI_USER_ID}");
    }

    // Mostrar todos los grupos disponibles
    await ListGroups();

    // Suscribirse a los mensajes nuevos
    _client.OnUpdates += OnUpdate;

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
    _client?.Dispose();
}

void LoadConfig()
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
    API_ID = Environment.GetEnvironmentVariable("API_ID") ?? "";
    API_HASH = Environment.GetEnvironmentVariable("API_HASH") ?? "";
    PHONE_NUMBER = Environment.GetEnvironmentVariable("PHONE_NUMBER") ?? "";

    string? userIdStr = Environment.GetEnvironmentVariable("USER_ID");
    if (!string.IsNullOrEmpty(userIdStr) && long.TryParse(userIdStr, out long userId))
    {
        MI_USER_ID = userId;
    }

    // Validar configuración mínima
    if (string.IsNullOrEmpty(API_ID)) throw new Exception("❌ API_ID no configurado en .env");
    if (string.IsNullOrEmpty(API_HASH)) throw new Exception("❌ API_HASH no configurado en .env");
    if (string.IsNullOrEmpty(PHONE_NUMBER)) throw new Exception("❌ PHONE_NUMBER no configurado en .env");

    // Verificar que API_ID sea número
    if (!long.TryParse(API_ID, out _)) throw new Exception("❌ API_ID debe ser un número");
}

// Configuración que WTelegramClient llama cuando necesita datos
string? Config(string what)
{
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
                    await ProccessMessage(message);
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error en update: {ex.Message}");
    }
}

async Task ProccessMessage(Message message)
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
        var chats = await _client!.Messages_GetAllChats();
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
                    var users = await _client.Users_GetUsers(new InputUser(fromUser.user_id, 0));
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
        await _client.SendMessageAsync(new InputPeerSelf(), mensajeParaReenviar);

        // 8. Enviar a Saved Messages - VERSIÓN TEXTO PLANO (SIN HTML)
        await _client.SendMessageAsync(new InputPeerSelf(), mensajeParaReenviar);

        Console.WriteLine($"✅ [{DateTime.Now:HH:mm:ss}] Mensaje reenviado desde '{chat.Title}'");

        Console.WriteLine($"✅ [{DateTime.Now:HH:mm:ss}] Mensaje reenviado desde '{chat.Title}'");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error procesando mensaje: {ex.Message}");
    }
}

// Función para listar todos los grupos donde estás
async Task ListGroups()
{
    try
    {
        Console.WriteLine("\n📋 Grupos donde estás:");
        var chats = await _client!.Messages_GetAllChats();
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
