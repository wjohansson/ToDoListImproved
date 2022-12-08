using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ToDoListApp2
{
    public class TaskLists
    {
        public void CreateList(FileManager fileManager)
        {
            Console.Write("Enter new list title: ");
            string title = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("List title can not be empty. Try again");

                CreateList(fileManager);

                return;
            }

            foreach (TaskList list in fileManager.Lists)
            {
                if (list.ListTitle == title)
                {
                    Console.WriteLine("List with the same name already exists. Try again");

                    CreateList(fileManager);

                    return;
                }
            }

            Console.Write("Enter new list category (optional): ");
            string category = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(category))
            {
                category = "-";
            }

            var id = 1;

            foreach (TaskList list in fileManager.Lists)
            {
                if (list.ListId >= id)
                {
                    id = list.ListId + 1;
                }
            }

            TaskList newList = new(title, category, id, new List<Task>());

            fileManager.Lists.Add(newList);

            fileManager.Update();

        }

        public void ViewListsCollapsed(FileManager fileManager)
        {
            Console.Clear();

            if (fileManager.GetType() == new ActiveManager().GetType())
            {
                Console.WriteLine("OVERVIEW MENU");
            }
            else if (fileManager.GetType() == new ArchiveManager().GetType())
            {
                Console.WriteLine("ARCHIVE MENU");
            }

            Console.WriteLine();

            if (fileManager.Lists.Count == 0)
            {
                Console.WriteLine("No existing lists");
                Console.WriteLine();

                return;
            }

            Console.WriteLine("Current existing lists:");
            Console.WriteLine();

            foreach (TaskList list in fileManager.Lists)
            {
                var allTasksCompleted = true;

                if (list.Tasks.Count == 0)
                {
                    allTasksCompleted = false;
                }

                foreach (Task task in list.Tasks)
                {
                    if (!task.Completed)
                    {
                        allTasksCompleted = false;
                    }
                }

                if (allTasksCompleted)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }


                Console.WriteLine($"List Position #{fileManager.Lists.IndexOf(list) + 1}");
                Console.WriteLine($"    Title - {list.ListTitle}");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void ViewListsExpanded(FileManager fileManager)
        {
            Console.Clear();

            if (fileManager.GetType() == new ActiveManager().GetType())
            {
                Console.WriteLine("OVERVIEW MENU");
            }
            else if (fileManager.GetType() == new ArchiveManager().GetType())
            {
                Console.WriteLine("ARCHIVE MENU");
            }

            Console.WriteLine();

            if (fileManager.Lists.Count == 0)
            {
                Console.WriteLine("No existing lists.");
                Console.WriteLine();

                return;
            }
            Console.WriteLine("Current existing lists expanded:");
            Console.WriteLine();

            foreach (TaskList list in fileManager.Lists)
            {
                var allTasksCompleted = true;

                if (list.Tasks.Count == 0)
                {
                    allTasksCompleted = false;
                }

                foreach (Task task in list.Tasks)
                {
                    if (!task.Completed)
                    {
                        allTasksCompleted = false;
                    }
                }

                if (allTasksCompleted)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine($"List Position #{fileManager.Lists.IndexOf(list) + 1}");
                Console.WriteLine($"    Title - {list.ListTitle} (Category: {list.ListCategory})");

                Console.ForegroundColor = ConsoleColor.White;

                foreach (Task task in list.Tasks)
                {
                    if (task.Completed)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    Console.WriteLine($"        Task Position #{list.Tasks.IndexOf(task) + 1}");
                    Console.WriteLine($"            Title - {task.TaskTitle} (Prio: {task.Priority})");
                    Console.WriteLine($"                ¤ {task.TaskDescription}");
                    Console.WriteLine();

                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
        }

        public void ViewList(FileManager fileManager)
        {
            if (TaskManager.listPosition == 0)
            {
                try
                {
                    Console.Write("Enter the position of the list you want to view: ");
                    TaskManager.listPosition = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Position must be a number. Try again.");

                    ViewList(fileManager);

                    return;
                }
            }

            try
            {
                if (fileManager.GetType() == new ActiveManager().GetType())
                {
                    var historyManager = new HistoryManager();
                    historyManager.Lists.Insert(0, fileManager.Lists[TaskManager.listPosition - 1]);
                    historyManager.Update();
                }
                else if (fileManager.GetType() == new ArchiveManager().GetType())
                {
                    var archiveHistoryManager = new ArchiveHistoryManager();
                    archiveHistoryManager.Lists.Insert(0, fileManager.Lists[TaskManager.listPosition - 1]);
                    archiveHistoryManager.Update();
                }


                var taskList = new TaskList();
                taskList.ViewTasksCollapsed(fileManager);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Position does not exist. Try again.");

                TaskManager.listPosition = 0;
                ViewList(fileManager);

                return;
            }
        }

        public void DeleteList(FileManager fileManager)
        {

            bool areYouSure = true;
            if (TaskManager.listPosition == 0)
            {
                try
                {
                    Console.Write("Enter the position of the list you want to delete: ");
                    TaskManager.listPosition = Convert.ToInt32(Console.ReadLine());

                }
                catch (FormatException)
                {
                    Console.WriteLine("Position must be a number, try again.");

                    DeleteList(fileManager);

                    return;
                }

                areYouSure = TaskManager.AreYouSure("Are you sure you want to delete this list? y/N: ");
            }



            if (areYouSure)
            {
                try
                {
                    int listId = fileManager.Lists[TaskManager.listPosition - 1].ListId;

                    TaskManager.RemoveListFromHistory(fileManager);

                    fileManager.Lists.RemoveAt(TaskManager.listPosition - 1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Position does not exist, try again.");

                    TaskManager.listPosition = 0;
                    DeleteList(fileManager);

                    return;
                }

                fileManager.Update();
            }


        }

        public bool ExistsContent(FileManager fileManager)
        {
            if (fileManager.Lists.Count == 0)
            {
                Console.WriteLine("No content. Returning");

                Thread.Sleep(2000);

                return false;
            }

            return true;
        }

        public void Sort(FileManager fileManager)
        {
            Console.WriteLine("[N] To sort by name.");
            Console.WriteLine("[C] To sort by category.");
            Console.WriteLine("[T] To sort by number of tasks.");

            Console.WriteLine();
            Console.Write("What do you want to do: ");
            switch (Console.ReadLine().ToUpper())
            {
                case "N":
                    fileManager.Lists = fileManager.Lists.OrderBy(o => o.ListTitle).ToList();

                    break;
                case "C":
                    fileManager.Lists = fileManager.Lists.OrderBy(o => o.ListCategory).ToList();

                    break;
                case "T":
                    fileManager.Lists = fileManager.Lists.OrderByDescending(o => o.Tasks.Count()).ToList();


                    break;
            }

            fileManager.Update();
        }
    }
}

