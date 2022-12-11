namespace ToDoListApp2
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Funktioner som jag inte haft tid att implementera:
            // Fixa undo på alla saker, hela vägen tillbaka till ursprung
            // Göra om mina kategorier så att man kan skapa en ny kategori. Vid skapande eller editerande av lista ska man då kunna
            //  välja en kategori till den nya listan man skapar eller skapa ytterligare en ny kategori
            // Fixa archive för tasks så att de ser ut likadant som för lists


            // Sista koll:
            // Pusha upp på ny branch i git
            // Kolla igenom alla funktioner så att de fungerar som det ska

            var activeManager = new ActiveManager();

            var taskLists = new TaskLists();
            taskLists.ViewListsCollapsed(activeManager);

            var taskManager = new TaskManager(new Task(), new TaskList(), new TaskLists());
            taskManager.OverviewOptions(activeManager);
        }
    }
}

