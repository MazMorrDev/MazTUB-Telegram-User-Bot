namespace MazUserBot.Utils;

public static class CommandHandler
{
    public static async Task StartCommandListener()
    {
        Console.WriteLine("\n💡 Comandos disponibles:");
        Console.WriteLine("  /addfilter <texto>     - Añadir filtro");
        Console.WriteLine("  /removefilter <texto>  - Remover filtro");
        Console.WriteLine("  /addlisten <group_id>  - Añadir grupo de escucha");
        Console.WriteLine("  /removelisten <group_id> - Remover grupo de escucha");
        Console.WriteLine("  /addsend <group_id>    - Añadir grupo de envío");
        Console.WriteLine("  /removesend <group_id> - Remover grupo de envío");
        Console.WriteLine("  /addmessage <texto>    - Añadir mensaje para enviar");
        Console.WriteLine("  /removemessage <índice> - Remover mensaje por índice");
        Console.WriteLine("  /listconfig            - Mostrar configuración actual");
        Console.WriteLine("  /help                  - Mostrar esta ayuda");
        Console.WriteLine("  /exit                  - Salir de la aplicación\n");

        while (true)
        {
            try
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                if (input.StartsWith('/'))
                {
                    await ProcessCommand(input.TrimStart('/'));
                }
                else if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                         input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("👋 Saliendo...");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("❌ Comando no reconocido. Use /help para ver los comandos disponibles.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error procesando comando: {ex.Message}");
            }
        }
    }

    private static async Task ProcessCommand(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            return;

        string command = parts[0].ToLower();
        string[] args = parts.Skip(1).ToArray();

        switch (command)
        {
            case "addfilter":
                if (args.Length == 0)
                {
                    Console.WriteLine("❌ Uso: /addfilter <texto>");
                    return;
                }

                string filterToAdd = string.Join(" ", args);
                if (!VariableHandler.FILTROS.Any(f => f.Equals(filterToAdd, StringComparison.OrdinalIgnoreCase)))
                {
                    var newFilters = VariableHandler.FILTROS.ToList();
                    newFilters.Add(filterToAdd);
                    VariableHandler.FILTROS = newFilters.ToArray();
                    Console.WriteLine($"✅ Filtro añadido: '{filterToAdd}'");
                }
                else
                {
                    Console.WriteLine($"⚠️ El filtro '{filterToAdd}' ya existe.");
                }
                break;

            case "removefilter":
                if (args.Length == 0)
                {
                    Console.WriteLine("❌ Uso: /removefilter <texto>");
                    return;
                }

                string filterToRemove = string.Join(" ", args);
                var filtersList = VariableHandler.FILTROS.ToList();
                int removedCount = filtersList.RemoveAll(f =>
                    f.Equals(filterToRemove, StringComparison.OrdinalIgnoreCase));

                if (removedCount > 0)
                {
                    VariableHandler.FILTROS = filtersList.ToArray();
                    Console.WriteLine($"✅ Filtro removido: '{filterToRemove}' ({removedCount} ocurrencia(s))");
                }
                else
                {
                    Console.WriteLine($"⚠️ No se encontró el filtro '{filterToRemove}'.");
                }
                break;

            case "addlisten":
                if (args.Length == 0 || !long.TryParse(args[0], out long listenGroupId))
                {
                    Console.WriteLine("❌ Uso: /addlisten <group_id>");
                    return;
                }

                var listenGroupsList = VariableHandler.GROUPS_TO_LISTEN.ToList();
                if (!listenGroupsList.Contains(listenGroupId))
                {
                    listenGroupsList.Add(listenGroupId);
                    VariableHandler.GROUPS_TO_LISTEN = listenGroupsList.ToArray();
                    Console.WriteLine($"✅ Grupo de escucha añadido: {listenGroupId}");
                }
                else
                {
                    Console.WriteLine($"⚠️ El grupo de escucha {listenGroupId} ya existe.");
                }
                break;

            case "removelisten":
                if (args.Length == 0 || !long.TryParse(args[0], out long removeListenGroupId))
                {
                    Console.WriteLine("❌ Uso: /removelisten <group_id>");
                    return;
                }

                var listenGroupsToModify = VariableHandler.GROUPS_TO_LISTEN.ToList();
                if (listenGroupsToModify.Remove(removeListenGroupId))
                {
                    VariableHandler.GROUPS_TO_LISTEN = listenGroupsToModify.ToArray();
                    Console.WriteLine($"✅ Grupo de escucha removido: {removeListenGroupId}");
                }
                else
                {
                    Console.WriteLine($"⚠️ No se encontró el grupo de escucha {removeListenGroupId}.");
                }
                break;

            case "addsend":
                if (args.Length == 0 || !long.TryParse(args[0], out long sendGroupId))
                {
                    Console.WriteLine("❌ Uso: /addsend <group_id>");
                    return;
                }

                var sendGroupsList = VariableHandler.GROUPS_TO_SEND.ToList();
                if (!sendGroupsList.Contains(sendGroupId))
                {
                    sendGroupsList.Add(sendGroupId);
                    VariableHandler.GROUPS_TO_SEND = sendGroupsList.ToArray();
                    Console.WriteLine($"✅ Grupo de envío añadido: {sendGroupId}");
                }
                else
                {
                    Console.WriteLine($"⚠️ El grupo de envío {sendGroupId} ya existe.");
                }
                break;

            case "removesend":
                if (args.Length == 0 || !long.TryParse(args[0], out long removeSendGroupId))
                {
                    Console.WriteLine("❌ Uso: /removesend <group_id>");
                    return;
                }

                var sendGroupsToModify = VariableHandler.GROUPS_TO_SEND.ToList();
                if (sendGroupsToModify.Remove(removeSendGroupId))
                {
                    VariableHandler.GROUPS_TO_SEND = sendGroupsToModify.ToArray();
                    Console.WriteLine($"✅ Grupo de envío removido: {removeSendGroupId}");
                }
                else
                {
                    Console.WriteLine($"⚠️ No se encontró el grupo de envío {removeSendGroupId}.");
                }
                break;

            case "addmessage":
                if (args.Length == 0)
                {
                    Console.WriteLine("❌ Uso: /addmessage <texto>");
                    return;
                }

                string messageToAdd = string.Join(" ", args);
                var messagesList = VariableHandler.MESSAGES_TO_SEND.ToList();
                messagesList.Add(messageToAdd);
                VariableHandler.MESSAGES_TO_SEND = messagesList.ToArray();
                Console.WriteLine($"✅ Mensaje añadido: '{messageToAdd}'");
                break;

            case "removemessage":
                if (args.Length == 0 || !int.TryParse(args[0], out int messageIndex))
                {
                    Console.WriteLine("❌ Uso: /removemessage <índice>");
                    return;
                }

                var messagesToModify = VariableHandler.MESSAGES_TO_SEND.ToList();
                if (messageIndex >= 0 && messageIndex < messagesToModify.Count)
                {
                    string removedMessage = messagesToModify[messageIndex];
                    messagesToModify.RemoveAt(messageIndex);
                    VariableHandler.MESSAGES_TO_SEND = messagesToModify.ToArray();
                    Console.WriteLine($"✅ Mensaje removido (índice {messageIndex}): '{removedMessage}'");
                }
                else
                {
                    Console.WriteLine($"⚠️ Índice {messageIndex} fuera de rango. Mensajes disponibles: 0-{messagesToModify.Count - 1}");
                }
                break;

            case "listconfig":
                Console.WriteLine("\n📋 Configuración actual:");
                Console.WriteLine($"  Filtros: {string.Join(", ", VariableHandler.FILTROS)}");
                Console.WriteLine($"  Grupos de escucha: {string.Join(", ", VariableHandler.GROUPS_TO_LISTEN)}");
                Console.WriteLine($"  Grupos de envío: {string.Join(", ", VariableHandler.GROUPS_TO_SEND)}");
                Console.WriteLine($"  Mensajes para enviar: {string.Join(" | ", VariableHandler.MESSAGES_TO_SEND.Select((m, i) => $"[{i}] {m}"))}");
                Console.WriteLine();
                break;

            case "help":
                Console.WriteLine("\n💡 Comandos disponibles:");
                Console.WriteLine("  /addfilter <texto>     - Añadir filtro");
                Console.WriteLine("  /removefilter <texto>  - Remover filtro");
                Console.WriteLine("  /addlisten <group_id>  - Añadir grupo de escucha");
                Console.WriteLine("  /removelisten <group_id> - Remover grupo de escucha");
                Console.WriteLine("  /addsend <group_id>    - Añadir grupo de envío");
                Console.WriteLine("  /removesend <group_id> - Remover grupo de envío");
                Console.WriteLine("  /addmessage <texto>    - Añadir mensaje para enviar");
                Console.WriteLine("  /removemessage <índice> - Remover mensaje por índice");
                Console.WriteLine("  /listconfig            - Mostrar configuración actual");
                Console.WriteLine("  /help                  - Mostrar esta ayuda");
                Console.WriteLine("  /exit                  - Salir de la aplicación\n");
                break;

            case "exit":
                Console.WriteLine("👋 Saliendo...");
                Environment.Exit(0);
                break;

            default:
                Console.WriteLine($"❌ Comando desconocido: /{command}. Use /help para ver los comandos disponibles.");
                break;
        }
    }
}