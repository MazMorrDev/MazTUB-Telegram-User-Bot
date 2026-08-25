using System;
using System.IO;
using System.Text.Json;
using MazUserBot.Config;
using MazUserBot.Utils;

namespace MazUserBot
{
    public static class ConfigManager
    {
        private static readonly string ConfigFilePath = "config.json";

        public static void LoadConfig()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    var json = File.ReadAllText(ConfigFilePath);
                    var config = JsonSerializer.Deserialize<BotConfig>(json);

                    if (config != null)
                    {
                        // Aplicar configuración desde JSON (sobrescribir solo si no está vacía)
                        if (config.FILTROS.Length > 0)
                            VariableHandler.FILTROS = config.FILTROS;

                        if (config.GROUPS_TO_LISTEN.Length > 0)
                            VariableHandler.GROUPS_TO_LISTEN = config.GROUPS_TO_LISTEN;

                        if (config.GROUPS_TO_SEND.Length > 0)
                            VariableHandler.GROUPS_TO_SEND = config.GROUPS_TO_SEND;

                        if (config.MESSAGES_TO_SEND.Length > 0)
                            VariableHandler.MESSAGES_TO_SEND = config.MESSAGES_TO_SEND;

                        if (config.MESSAGE_INTERVAL_HOURS > 0)
                            VariableHandler.MESSAGE_INTERVAL_HOURS = config.MESSAGE_INTERVAL_HOURS;

                        Console.WriteLine("✅ Configuración cargada desde config.json");
                    }
                }
                else
                {
                    Console.WriteLine($"⚠️ Archivo {ConfigFilePath} no encontrado, usando valores por defecto");
                    // Guardar configuración por defecto para futuras ejecuciones
                    SaveDefaultConfig();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error cargando configuración: {ex.Message}");
            }
        }

        public static void SaveConfig()
        {
            try
            {
                var config = new BotConfig
                {
                    FILTROS = VariableHandler.FILTROS,
                    GROUPS_TO_LISTEN = VariableHandler.GROUPS_TO_LISTEN,
                    GROUPS_TO_SEND = VariableHandler.GROUPS_TO_SEND,
                    MESSAGES_TO_SEND = VariableHandler.MESSAGES_TO_SEND,
                    MESSAGE_INTERVAL_HOURS = VariableHandler.MESSAGE_INTERVAL_HOURS
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(ConfigFilePath, json);

                Console.WriteLine("✅ Configuración guardada en config.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error guardando configuración: {ex.Message}");
            }
        }

        private static void SaveDefaultConfig()
        {
            try
            {
                var config = new BotConfig
                {
                    FILTROS = VariableHandler.FILTROS,
                    GROUPS_TO_LISTEN = VariableHandler.GROUPS_TO_LISTEN,
                    GROUPS_TO_SEND = VariableHandler.GROUPS_TO_SEND,
                    MESSAGES_TO_SEND = VariableHandler.MESSAGES_TO_SEND,
                    MESSAGE_INTERVAL_HOURS = VariableHandler.MESSAGE_INTERVAL_HOURS
                };

                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(config, options);
                File.WriteAllText(ConfigFilePath, json);

                Console.WriteLine($"✅ Archivo {ConfigFilePath} creado con valores por defecto");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error creando configuración por defecto: {ex.Message}");
            }
        }

        // Métodos para actualizar configuración en tiempo real
        public static void AddFilter(string filter)
        {
            var filters = VariableHandler.FILTROS.ToList();
            if (!filters.Contains(filter, StringComparer.OrdinalIgnoreCase))
            {
                filters.Add(filter);
                VariableHandler.FILTROS = filters.ToArray();
                SaveConfig(); // Guardar cambios
            }
        }

        public static void RemoveFilter(string filter)
        {
            var filters = VariableHandler.FILTROS.ToList();
            filters.RemoveAll(f => string.Equals(f, filter, StringComparison.OrdinalIgnoreCase));
            VariableHandler.FILTROS = filters.ToArray();
            SaveConfig(); // Guardar cambios
        }

        public static void AddListenGroup(long groupId)
        {
            var groups = VariableHandler.GROUPS_TO_LISTEN.ToList();
            if (!groups.Contains(groupId))
            {
                groups.Add(groupId);
                VariableHandler.GROUPS_TO_LISTEN = groups.ToArray();
                SaveConfig(); // Guardar cambios
            }
        }

        public static void RemoveListenGroup(long groupId)
        {
            var groups = VariableHandler.GROUPS_TO_LISTEN.ToList();
            groups.Remove(groupId);
            VariableHandler.GROUPS_TO_LISTEN = groups.ToArray();
            SaveConfig(); // Guardar cambios
        }

        public static void AddSendGroup(long groupId)
        {
            var groups = VariableHandler.GROUPS_TO_SEND.ToList();
            if (!groups.Contains(groupId))
            {
                groups.Add(groupId);
                VariableHandler.GROUPS_TO_SEND = groups.ToArray();
                SaveConfig(); // Guardar cambios
            }
        }

        public static void RemoveSendGroup(long groupId)
        {
            var groups = VariableHandler.GROUPS_TO_SEND.ToList();
            groups.Remove(groupId);
            VariableHandler.GROUPS_TO_SEND = groups.ToArray();
            SaveConfig(); // Guardar cambios
        }

        public static void AddMessage(string message)
        {
            var messages = VariableHandler.MESSAGES_TO_SEND.ToList();
            messages.Add(message);
            VariableHandler.MESSAGES_TO_SEND = messages.ToArray();
            SaveConfig(); // Guardar cambios
        }

        public static void RemoveMessage(int index)
        {
            var messages = VariableHandler.MESSAGES_TO_SEND.ToList();
            if (index >= 0 && index < messages.Count)
            {
                messages.RemoveAt(index);
                VariableHandler.MESSAGES_TO_SEND = messages.ToArray();
                SaveConfig(); // Guardar cambios
            }
        }
    }
}