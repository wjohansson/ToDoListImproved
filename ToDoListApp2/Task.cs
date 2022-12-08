using System.Collections.Generic;

namespace ToDoListApp2
{
    public class Task
    {

        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public int TaskId { get; init; }
        public bool Completed { get; set; }
        public string Priority { get; set; }
        public string DateCreated { get; init; }
        public List<SubTask> SubTasks { get; set; }

        public Task()
        {

        }
        public Task(string taskTitle, string taskDescription, int taskId, string priority)
        {
            TaskTitle = taskTitle;
            TaskDescription = taskDescription;
            TaskId = taskId;
            Priority = priority;
            Completed = false;
            DateCreated = DateTime.Now.ToString("G");
            SubTasks = new List<SubTask>();
        }

        public void CreateSubTask(FileManager fileManager)
        {
            List<SubTask> subTasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks[TaskManager.taskPosition - 1].SubTasks;

            Console.Write("Enter a new sub-task: ");
            string title = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Sub-task title can not be empty. Try again");

                CreateSubTask(fileManager);

                return;
            }

            foreach (SubTask subTask in subTasks)
            {
                if (subTask.SubTaskTitle == title)
                {
                    Console.WriteLine("Sub-task already exists. Try again");

                    CreateSubTask(fileManager);

                    return;
                }
            }

            Console.Write("Enter the sub-task description (optional): ");
            string description = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(description))
            {
                description = "No description";
            }

            var id = 1;

            foreach (SubTask subTask in subTasks)
            {
                if (subTask.SubTaskId >= id)
                {
                    id = subTask.SubTaskId + 1;
                }
            }

            SubTask newSubTask = new(title, description, id);

            subTasks.Add(newSubTask);

            fileManager.Update();
        }

        public void DeleteSubTask(FileManager fileManager)
        {
            List<SubTask> subTasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks[TaskManager.taskPosition - 1].SubTasks;

            int position;

            try
            {
                Console.Write("Position of the sub-task you want to delete: ");
                position = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Position must be a number. Try again");

                DeleteSubTask(fileManager);

                return;
            }

            TaskManager.AreYouSure("Are you sure you want to delete this sub-task? y/N: ");

            try
            {
                subTasks.RemoveAt(position - 1);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Position does not exist. Try again");

                DeleteSubTask(fileManager);

                return;
            }


            fileManager.Update();
        }

        public void Edit(FileManager fileManager)
        {
            List<Task> tasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks;
            Task currentTask = tasks[TaskManager.taskPosition - 1];

            Console.WriteLine($"Old title: {currentTask.TaskTitle}");
            Console.Write("Enter the new title or leave empty to keep old title: ");
            string newTitle = Console.ReadLine();

            foreach (Task task in tasks)
            {
                if (task.TaskTitle == newTitle)
                {
                    Console.WriteLine("Task with the same name already exists. Try again");

                    Edit(fileManager);

                    return;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Old description: {currentTask.TaskDescription}");
            Console.Write("Enter the new description or leave empty to keep old description: ");
            string newDescription = Console.ReadLine();

            string priorityInput;
            int priority;

            try
            {
                Console.WriteLine();
                Console.WriteLine($"Old priority: {currentTask.Priority}");
                Console.Write("Enter the new priority or leave empty to keep old priority: ");
                priorityInput = Console.ReadLine();

                if (!String.IsNullOrWhiteSpace(priorityInput))
                {
                    priority = Convert.ToInt32(priorityInput);

                    if (priority > 5 || priority < 1)
                    {
                        throw new FormatException();
                    }

                    currentTask.Priority = priorityInput;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Priority must be a number between 1 and 5. Try again");

                Edit(fileManager);

                return;
            }

            TaskManager.AreYouSure("Are you sure you want to edit this task? y/N: ");

            if (!String.IsNullOrWhiteSpace(newTitle))
            {
                currentTask.TaskTitle = newTitle;
            }

            if (!String.IsNullOrWhiteSpace(newDescription))
            {
                currentTask.TaskDescription = newDescription;
            }


            fileManager.Update();
        }

        public void ViewSubTasks(FileManager fileManager)
        {
            Task currentTask = fileManager.Lists[TaskManager.listPosition - 1].Tasks[TaskManager.taskPosition - 1];

            Console.Clear();

            Console.WriteLine("TASK MENU");
            Console.WriteLine();

            if (currentTask.Completed)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.WriteLine($"Title - {currentTask.TaskTitle} (Prio: {currentTask.Priority})");
            Console.WriteLine($"    Description - {currentTask.TaskDescription}");

            Console.WriteLine();

            foreach (SubTask subTask in currentTask.SubTasks)
            {
                Console.WriteLine($"        Sub-task Position #{currentTask.SubTasks.IndexOf(subTask) + 1}");
                Console.WriteLine($"            Title - {subTask.SubTaskTitle}");
                Console.WriteLine($"                Description - {subTask.SubTaskDescription}");

                Console.WriteLine();
            }


            Console.ForegroundColor = ConsoleColor.White;
        }

        public void EditSubTask(FileManager fileManager)
        {
            List<SubTask> subTasks = fileManager.Lists[TaskManager.listPosition - 1].Tasks[TaskManager.taskPosition - 1].SubTasks;

            int position;

            try
            {
                Console.Write("Position of the sub-task you want to edit: ");
                position = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Position must be a number. Try again");

                EditSubTask(fileManager);

                return;
            }

            SubTask currentSubTask;

            try
            {
                currentSubTask = subTasks[position - 1];
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Position does not exist. Try again");

                EditSubTask(fileManager);

                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Old title: {currentSubTask.SubTaskTitle}");
            Console.Write("Enter the new title or leave empty to keep old title: ");
            string newTitle = Console.ReadLine();

            foreach (SubTask subTask in subTasks)
            {
                if (subTask.SubTaskTitle == newTitle)
                {
                    Console.WriteLine("There is already another sub-task with the same name. Try again");

                    EditSubTask(fileManager);

                    return;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Old description: {currentSubTask.SubTaskDescription}");
            Console.Write("Enter the new description or leave empty to keep old description: ");
            string newDescription = Console.ReadLine();

            TaskManager.AreYouSure("Are you sure you want to edit this sub-task? y/N: ");

            if (!String.IsNullOrWhiteSpace(newTitle))
            {
                currentSubTask.SubTaskTitle = newTitle;
            }

            if (!String.IsNullOrWhiteSpace(newDescription))
            {
                currentSubTask.SubTaskDescription = newDescription;
            }

            fileManager.Update();
        }

        public void ToggleArchive(FileManager fileManager)
        {
            TaskList currentList = fileManager.Lists[TaskManager.listPosition - 1];
            Task currentTask = currentList.Tasks[TaskManager.taskPosition - 1];

            var listExists = false;
            var taskExists = false;

            List<Task> currentArchiveTasks;
            int archiveListPosition = 0;

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
                    archiveListPosition = manager.Lists.IndexOf(list);
                    break;
                }
            }

            if (listExists)
            {
                currentArchiveTasks = manager.Lists[archiveListPosition].Tasks;

                foreach (Task task in currentArchiveTasks)
                {
                    if (task.TaskTitle == currentTask.TaskTitle)
                    {
                        taskExists = true;
                        break;
                    }
                }

                if (taskExists)
                {
                    Console.WriteLine("List with this task already exists.");

                    if (TaskManager.AreYouSure("Do you want to delete this task? y/N: "))
                    {
                        var taskList = new TaskList();
                        taskList.DeleteTask(fileManager);
                        fileManager.Update();
                    }

                    return;
                }

                manager.Lists[archiveListPosition].Tasks.Add(currentTask);
            }
            else
            {
                var id = 1;

                foreach (TaskList list in manager.Lists)
                {
                    if (list.ListId >= id)
                    {
                        id = list.ListId + 1;
                    }
                }

                TaskList newList = new(currentList.ListTitle, null, id, new List<Task>());

                manager.Lists.Add(newList);

                int newListPosition = manager.Lists.IndexOf(newList);

                manager.Lists[newListPosition].Tasks.Add(currentTask);
            }

            if (TaskManager.AreYouSure("Are you sure you want to toggle archivtion of this task? y/N: "))
            {
                manager.Update();

                var taskList = new TaskList();
                taskList.DeleteTask(fileManager);
                fileManager.Update();
            }
        }
    }
}

