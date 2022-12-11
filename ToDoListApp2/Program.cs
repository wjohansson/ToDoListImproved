namespace ToDoListApp2
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Funktioner som jag inte haft tid att implementera:
            // Fixa undo på alla saker, hela vägen tillbaka till ursprung

            var activeManager = new ActiveManager();

            var taskLists = new TaskLists();
            taskLists.ViewListsCollapsed(activeManager);

            var taskManager = new TaskManager(new Task(), new TaskList(), new TaskLists());
            taskManager.OverviewOptions(activeManager);
        }
    }
}

