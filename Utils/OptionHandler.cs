namespace MazUserBot;

public static class OptionHandler
{
    public static void ManageOptions()
    {
        Console.WriteLine("Commands List:\n"+
        "1. List Groups with his respective IDs\n"+
        "2. Add Group to send Messages\n"+
        "3. Remove Group to send Messages\n"+
        "4. Add message to send to Groups\n"+
        "5. Remove message to send to groups"+
        "6. Add filter to receive messages\n"+
        "7. Remove filter to receive messages\n");
    }
}
