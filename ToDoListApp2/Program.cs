namespace ToDoListApp2
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Lägga till möjlighet att gå till förra sidan från alla funktioner, eller åtminstone de
            //  som skapar och ändrar, alltså en quit/return från överallt
            // Kolla edit funktion ifall den fungerar även fast man använder den från toggelarchive funktionen,
            //  den fungerar inte, måste fixas
            // Kolla are you sure meddelanden, kommer på fel ställen om man kallar dom från togglearchive funktionerna, blir
            //  nog rätt då man fixar edit funktionen
            // Fixa undo på alla saker, hela vägen tillbaka till ursprung


            // Sista koll:
            // Notera alla funktioner som en kommentar här i program för att komma ihåg allt som går att göra
            // GÅ igenom hela programmet, och försöka nå alla endpoints med debugger för att kolla så att allt fungerar


            var activeManager = new ActiveManager();

            var taskLists = new TaskLists();
            taskLists.ViewListsCollapsed(activeManager);

            var taskManager = new TaskManager(new Task(), new TaskList(), new TaskLists());
            taskManager.OverviewOptions(activeManager);

        }
    }
}

