using System;
using MazUserBot;

namespace MazUserBot.Test
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Probando inicialización de componentes...");
            
            // Probar carga de configuración
            ConfigManager.LoadConfig();
            
            // Mostrar algunos valores para verificar
            Console.WriteLine($"Filtros: {string.Join(", ", VariableHandler.FILTROS)}");
            Console.WriteLine($"Grupos de escucha: {string.Join(", ", VariableHandler.GROUPS_TO_LISTEN)}");
            Console.WriteLine($"Intervalo de mensajes: {VariableHandler.MESSAGE_INTERVAL_HOURS} horas");
            
            Console.WriteLine("Inicialización completada correctamente.");
        }
    }
}