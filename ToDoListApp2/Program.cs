namespace ToDoListApp2
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Funktioner som jag inte haft tid att implementera:
            // Fixa undo på alla saker, hela vägen tillbaka till ursprung
            // Fixa archive för tasks så att de ser ut likadant som för lists


            // Sista koll:
            // Notera alla funktioner som en kommentar här i program för att komma ihåg allt som går att göra
            // GÅ igenom hela programmet, och försöka nå alla endpoints med debugger för att kolla så att allt fungerar
            // Pusha upp på ny branch i git
            // Kolla igenom alla funktioner så att de fungerar som det ska

            // Frågor:
            // Om man har funktioner som man använder flera gånger,
            //  ska man skapa en ny klass och hämta in den via komposition då?
            //  Eller kan man bara lägga till den i en befintlig klass och göra den 


            //var read = Console.ReadKey().Key.ToString();
            //Console.WriteLine(read.GetType());
            //Console.ReadLine();


            var activeManager = new ActiveManager();

            var taskLists = new TaskLists();
            taskLists.ViewListsCollapsed(activeManager);

            var taskManager = new TaskManager(new Task(), new TaskList(), new TaskLists());
            taskManager.OverviewOptions(activeManager);

        }
    }
}

