namespace ToDoListApp2
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Kolla ifall ITaskManager kan användas någonstans
            // Kolla edit funktion ifall den fungerar även fast man använder den från toggelarchive funktionen,
            //  den fungerar inte, måste fixas
            // Kolla are you sure meddelanden, kommer på fel ställen om man kallar dom från togglearchive funktionerna, blir
            //  nog rätt då man fixar edit funktionen

            var activeManager = new ActiveManager();

            var taskLists = new TaskLists();
            taskLists.ViewListsCollapsed(activeManager);

            var taskManager = new TaskManager(new Task(), new TaskList(), new TaskLists());
            taskManager.OverviewOptions(activeManager);

        }
    }
}

