using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoListApp2
{
    public class TaskList
    {
        public string ListTitle { get; set; }
        public string ListCategory { get; set; }
        public int ListId { get; set; }
        public List<Task> Tasks { get; set; }

        public TaskList()
        {

        }

        public TaskList(string listTitle, string listCaterogy, int listId, List<Task> tasks)
        {
            ListTitle = listTitle;
            ListCategory = listCaterogy;
            ListId = listId;
            Tasks = tasks;
        }

        public void CreateTask(FileManager fileManager)
        {
            List<Task> tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks;

            Console.Write("Enter a new task: ");
            string title = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Task title can not be empty. Try again");

                CreateTask(fileManager);

                return;
            }

            foreach (Task task in tasks)
            {
                if (task.TaskTitle == title)
                {
                    Console.WriteLine("Task already exists. Try again.");

                    CreateTask(fileManager);

                    return;
                }
            }

            Console.Write("Enter the task description (optional): ");
            string description = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(description))
            {
                description = "No description";
            }

            Console.Write("Enter priority of the task as a number between 1 and 5 (optional): ");
            string priorityInput = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(priorityInput))
            {
                priorityInput = "None";
            }
            else
            {
                try
                {
                    int priority = Convert.ToInt32(priorityInput);

                    if (priority > 5 || priority < 1)
                    {
                        throw new FormatException();
                    }
                }
                catch (FormatException)
                {

                    Console.WriteLine("Priority must be a number between 1 and 5 or empty. Try again.");

                    CreateTask(fileManager);

                    return;
                }
            }

            var id = 1;

            foreach (Task task in tasks)
            {
                if (task.TaskId >= id)
                {
                    id = task.TaskId + 1;
                }
            }

            Task newTask = new(title, description, id, priorityInput);

            tasks.Add(newTask);

            fileManager.Update();
        }

        public void DeleteTask(FileManager fileManager)
        {
            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            List<Task> tasks = currentList.Tasks;

            bool areYouSure = true;

            if (TaskManager.taskPosition == 0)
            {
                try
                {
                    Console.Write("Enter the position of the task you want to delete: ");
                    TaskManager.taskPosition = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Position must be a number, try again.");

                    DeleteTask(fileManager);

                    return;
                }

                areYouSure = TaskManager.AreYouSure("Are you sure you want to delete this task? y/N: ");
            }

            if (areYouSure)
            {
                try
                {
                    tasks.RemoveAt(TaskManager.taskPosition - 1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Position does not exist, try again.");

                    TaskManager.taskPosition = 0;
                    DeleteTask(fileManager);

                    return;
                }

                fileManager.Update();
            }
        }

        public void Edit(FileManager fileManager)
        {
            FileManager manager = null;

            if (fileManager.GetType() == new ActiveManager().GetType())
            {
                manager = new ArchiveManager();
            }
            else if (fileManager.GetType() == new ArchiveManager().GetType())
            {
                manager = new ActiveManager();
            }

            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            Console.WriteLine();
            Console.WriteLine($"Old title: {currentList.ListTitle}");

            Console.Write("Enter the new title or leave empty to keep the old title: ");
            string newTitle = Console.ReadLine();

            foreach (TaskList list in fileManager.Lists)
            {
                if (list.ListTitle == newTitle)
                {
                    Console.WriteLine("List with the same name already exists. Try again");

                    Edit(fileManager);

                    return;
                }
            }

            Console.WriteLine($"Old category: {currentList.ListCategory}");
            Console.Write("Enter the new category or leave empty to keep the old category: ");
            string newCategory = Console.ReadLine();

            if (TaskManager.AreYouSure("Are you sure you want to edit this list? y/N: "))
            {
                if (!String.IsNullOrWhiteSpace(newTitle))
                {
                    currentList.ListTitle = newTitle;
                }

                if (!String.IsNullOrWhiteSpace(newCategory))
                {
                    currentList.ListCategory = newCategory;
                }

                fileManager.Update();
            }
        }

        public void ToggleCompleteTask(FileManager fileManager)
        {
            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            List<Task> tasks = currentList.Tasks;

            int position;

            try
            {
                Console.Write("Enter the position of the task you want to toggle: ");
                position = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Position must be a number, try again.");

                ToggleCompleteTask(fileManager);

                return;
            }

            Task currentTask;

            try
            {
                currentTask = tasks[position - 1];
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Position does not exist, try again.");

                ToggleCompleteTask(fileManager);

                return;
            }

            currentTask.Completed = !currentTask.Completed;

            fileManager.Update();
        }

        public void ViewTasksCollapsed(FileManager fileManager)
        {
            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            Console.Clear();

            Console.WriteLine("LIST MENU");
            Console.WriteLine();

            Console.WriteLine($"List Title - {currentList.ListTitle} (Category: {currentList.ListCategory})");
            Console.WriteLine();

            List<Task> tasks = currentList.Tasks;

            if (currentList.Tasks.Count == 0)
            {
                Console.WriteLine("No existing tasks.");
                Console.WriteLine();

                return;
            }

            foreach (Task task in tasks)
            {
                if (task.Completed)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine($"    Task Position #{tasks.IndexOf(task) + 1}");
                Console.WriteLine($"        Title - {task.TaskTitle}");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        public void ViewTasksExpanded(FileManager fileManager)
        {
            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            Console.Clear();

            Console.WriteLine("LIST MENU");
            Console.WriteLine();

            Console.WriteLine($"List Title - {currentList.ListTitle} (Category: {currentList.ListCategory})");
            Console.WriteLine();


            if (currentList.Tasks.Count == 0)
            {
                Console.WriteLine("No existing tasks.");
                Console.WriteLine();

                return;
            }

            foreach (Task task in currentList.Tasks)
            {
                if (task.Completed)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine($"    Task Position #{currentList.Tasks.IndexOf(task) + 1}");
                Console.WriteLine($"        Title - {task.TaskTitle} (Prio: {task.Priority})");
                Console.WriteLine($"            ¤ {task.TaskDescription}");
                Console.WriteLine();

                foreach (SubTask subTask in task.SubTasks)
                {
                    Console.WriteLine($"            Sub-Task Position #{task.SubTasks.IndexOf(subTask) + 1}");
                    Console.WriteLine($"                Title - {subTask.SubTaskTitle}");
                    Console.WriteLine($"                    ¤ {subTask.SubTaskDescription}");
                    Console.WriteLine();
                }
            }
        }

        public void ViewTask(FileManager fileManager)
        {
            try
            {
                Console.Write("Enter the position of the task you want to view: ");
                TaskManager.taskPosition = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Position must be a number, try again.");

                ViewTask(fileManager);

                return;
            }

            try
            {
                var task = new Task();
                task.ViewSubTasks(fileManager);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Position does not exist, try again.");

                ViewTask(fileManager);

                return;
            }
        }

        public bool ExistsContent(FileManager fileManager)
        {
            if (fileManager.Lists[TaskManager.listPosition - 1].Tasks.Count == 0)
            {
                Console.WriteLine("No content. Returning");

                Thread.Sleep(2000);

                return false;
            }

            return true;
        }

        public void ToggleArchive(FileManager fileManager)
        {
            Console.Write("Are you sure you want to toggle archivation of this list? y/N: ");

            switch (Console.ReadLine().ToUpper())
            {
                case "Y":
                    break;
                default:
                    ViewTasksCollapsed(fileManager);

                    return;
            }

            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];

            var listExists = false;

            FileManager manager = null;

            if (fileManager.GetType() == new ActiveManager().GetType())
            {
                manager = new ArchiveManager();
            }
            else if (fileManager.GetType() == new ArchiveManager().GetType())
            {
                manager = new ActiveManager();
            }

            foreach (TaskList list in manager.Lists)
            {
                if (list.ListTitle == currentList.ListTitle)
                {
                    listExists = true;
                    break;
                }
            }

            var id = 1;

            foreach (TaskList list in manager.Lists)
            {
                if (list.ListId >= id)
                {
                    id = list.ListId + 1;
                }
            }

            currentList.ListId = id;

            if (listExists)
            {
                Console.WriteLine("List with this name already exists.");

                Edit(fileManager);
            }

            manager.Lists.Add(currentList);
            manager.Update();

            var taskLists = new TaskLists();
            taskLists.DeleteList(fileManager);
        }

        public void Sort(FileManager fileManager)
        {
            Console.WriteLine("[N] To sort by name.");
            Console.WriteLine("[D] To sort by date.");
            Console.WriteLine("[P] To sort by priority.");
            Console.WriteLine("[C] To sort by completed.");
            Console.WriteLine("[T] To sort by number of sub-tasks.");

            Console.WriteLine();
            Console.Write("What do you want to do: ");
            switch (Console.ReadLine().ToUpper())
            {
                case "N":
                    fileManager.Lists[TaskManager.listPosition - 1].Tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks.OrderBy(o => o.TaskTitle).ToList();


                    break;
                case "D":
                    fileManager.Lists[TaskManager.listPosition - 1].Tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks.OrderBy(o => o.DateCreated).ToList();


                    break;
                case "P":
                    fileManager.Lists[TaskManager.listPosition - 1].Tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks.OrderBy(o => o.Priority).ToList();


                    break;
                case "C":
                    fileManager.Lists[TaskManager.listPosition - 1].Tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks.OrderByDescending(o => o.Completed).ToList();


                    break;
                case "T":
                    fileManager.Lists[TaskManager.listPosition - 1].Tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks.OrderBy(o => o.SubTasks.Count()).ToList();


                    break;
            }
        }
    }
}

