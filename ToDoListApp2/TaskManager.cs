﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoListApp2
{
    public class TaskManager
    {
        public static int listPosition = 0;
        public static int taskPosition = 0;
        public static bool listExists;
        public static bool taskExists;

        private readonly Task _task;
        private readonly TaskList _taskList;
        private readonly TaskLists _taskLists;

        public TaskManager(Task task, TaskList taskList, TaskLists taskLists)
        {
            _task = task;
            _taskList = taskList;
            _taskLists = taskLists;
        }

        public void OverviewOptions(FileManager fileManager)
        {
            Console.WriteLine("[E] To expand all lists.");
            Console.WriteLine("[C] To collapse all lists.");
            Console.WriteLine("[V] To view a list and its tasks.");
            Console.WriteLine("[L] To view recently visited list.");
            Console.WriteLine("[D] To delete a list.");
            Console.WriteLine("[N] To create a new list.");
            Console.WriteLine("[S] To sort all lists.");
            Console.WriteLine("[A] To toggle between archive and overview menu.");
            Console.WriteLine("[H] To delete all history.");
            Console.WriteLine("[DELALL] To delete all lists and tasks.");
            Console.WriteLine("[Q] To quit the program.");

            Console.WriteLine();
            Console.Write("What do you want to do: ");
            switch (Console.ReadLine().ToUpper())
            {
                case "E":
                    _taskLists.ViewListsExpanded(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "C":
                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "V":
                    _taskLists.ViewListsCollapsed(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        _taskLists.ViewList(fileManager);
                        ListOptions(fileManager);
                    }
                    else
                    {
                        _taskLists.ViewListsCollapsed(fileManager);
                        OverviewOptions(fileManager);
                    }

                    break;
                case "L":
                    _taskLists.ViewListsCollapsed(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        int historyListId;

                        try
                        {
                            FileManager historyManager = null;

                            if (fileManager.GetType() == new ActiveManager().GetType())
                            {
                                historyManager = new HistoryManager();
                            }
                            else if (fileManager.GetType() == new ArchiveManager().GetType())
                            {
                                historyManager = new ArchiveHistoryManager();
                            }

                            historyListId = historyManager.Lists[0].ListId;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            _taskLists.ViewListsCollapsed(fileManager);
                            Console.WriteLine("Nothing to view. Returning");

                            Thread.Sleep(2000);

                            _taskLists.ViewListsCollapsed(fileManager);
                            OverviewOptions(fileManager);
                            break;
                        }

                        foreach (TaskList list in fileManager.Lists)
                        {
                            if (historyListId == list.ListId)
                            {
                                listPosition = fileManager.Lists.IndexOf(list) + 1;
                                break;
                            }
                        }
                        _taskLists.ViewList(fileManager);
                        ListOptions(fileManager);
                    }
                    else
                    {
                        _taskLists.ViewListsCollapsed(fileManager);
                        OverviewOptions(fileManager);
                    }


                    break;
                case "D":
                    _taskLists.ViewListsCollapsed(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        _taskLists.DeleteList(fileManager);
                    }

                    listPosition = 0;

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "N":
                    _taskLists.ViewListsCollapsed(fileManager);
                    _taskLists.CreateList(fileManager);
                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "S":
                    _taskLists.ViewListsCollapsed(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        _taskLists.Sort(fileManager);
                    }

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "A":
                    if (fileManager.GetType() == new ActiveManager().GetType())
                    {
                        var archiveManager = new ArchiveManager();
                        _taskLists.ViewListsCollapsed(archiveManager);
                        OverviewOptions(archiveManager);
                    }
                    else if (fileManager.GetType() == new ArchiveManager().GetType())
                    {
                        var activeManager = new ActiveManager();
                        _taskLists.ViewListsCollapsed(activeManager);
                        OverviewOptions(activeManager);
                    }

                    break;
                case "H":
                    _taskLists.ViewListsCollapsed(fileManager);

                    FileManager manager = null;

                    if (fileManager.GetType() == new ActiveManager().GetType())
                    {
                        manager = new HistoryManager();
                    }
                    else if (fileManager.GetType() == new ArchiveManager().GetType())
                    {
                        manager = new ArchiveHistoryManager();
                    }
                    manager.Clear(manager);

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "DELALL":
                    _taskLists.ViewListsCollapsed(fileManager);

                    if (_taskLists.ExistsContent(fileManager))
                    {
                        fileManager.Clear(fileManager);
                    }

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "Q":
                    _taskLists.ViewListsCollapsed(fileManager);
                    QuitProgram();

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                default:
                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
            }
        }

        public void ListOptions(FileManager fileManager)
        {
            Console.WriteLine("[E] To expand all tasks.");
            Console.WriteLine("[C] To collapse all tasks.");
            Console.WriteLine("[M] To modify this list.");
            Console.WriteLine("[A] To toggle archivation of this list.");
            Console.WriteLine("[V] To view a task and its sub-tasks.");
            Console.WriteLine("[D] To delete a task.");
            Console.WriteLine("[N] To create a new task.");
            Console.WriteLine("[T] To toggle completion of a task.");
            Console.WriteLine("[S] To sort tasks.");
            Console.WriteLine("[B] To go back to start page.");
            Console.WriteLine("[Q] To quit the program.");

            Console.WriteLine();
            Console.Write("What do you want to do: ");
            switch (Console.ReadLine().ToUpper())
            {
                case "E":
                    _taskList.ViewTasksExpanded(fileManager);
                    ListOptions(fileManager);

                    break;
                case "C":
                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "M":
                    _taskList.ViewTasksCollapsed(fileManager);
                    _taskList.Edit(fileManager);

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "A":
                    _taskList.ViewTasksCollapsed(fileManager);
                    _taskList.ToggleArchive(fileManager);

                    listPosition = 0;

                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "V":
                    _taskList.ViewTasksCollapsed(fileManager);

                    if (_taskList.ExistsContent(fileManager))
                    {
                        _taskList.ViewTask(fileManager);
                        TaskOptions(fileManager);
                    }
                    else
                    {
                        _taskList.ViewTasksCollapsed(fileManager);
                        ListOptions(fileManager);
                    }

                    break;
                case "D":
                    _taskList.ViewTasksCollapsed(fileManager);

                    if (_taskList.ExistsContent(fileManager))
                    {
                        _taskList.DeleteTask(fileManager);
                    }

                    taskPosition = 0;

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "N":
                    _taskList.ViewTasksCollapsed(fileManager);
                    _taskList.CreateTask(fileManager);

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "T":
                    _taskList.ViewTasksCollapsed(fileManager);

                    if (_taskList.ExistsContent(fileManager))
                    {
                        _taskList.ToggleCompleteTask(fileManager);

                    }

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "S":
                    _taskList.ViewTasksCollapsed(fileManager);

                    if (_taskList.ExistsContent(fileManager))
                    {
                        _taskList.Sort(fileManager);
                    }

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "B":
                    listPosition = 0;
                    _taskLists.ViewListsCollapsed(fileManager);
                    OverviewOptions(fileManager);

                    break;
                case "Q":
                    _taskList.ViewTasksCollapsed(fileManager);
                    QuitProgram();

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                default:
                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
            }
        }

        public void TaskOptions(FileManager fileManager)
        {
            Console.WriteLine("[M] To modify this task.");
            Console.WriteLine("[A] To archive this task.");
            Console.WriteLine("[D] To delete a sub-task.");
            Console.WriteLine("[MS] To edit a sub-task.");
            Console.WriteLine("[N] To create a new sub-task.");
            Console.WriteLine("[B] To go back to list overview.");
            Console.WriteLine("[Q] To quit the program.");

            Console.WriteLine();
            Console.Write("What do you want to do: ");

            switch (Console.ReadLine().ToUpper())
            {
                case "M":
                    _task.ViewSubTasks(fileManager);
                    _task.Edit(fileManager);

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
                case "A":
                    _task.ViewSubTasks(fileManager);
                    _task.ToggleArchive(fileManager);

                    taskPosition = 0;

                    _taskList.ViewTasksCollapsed(fileManager);
                    ListOptions(fileManager);

                    break;
                case "D":
                    _task.ViewSubTasks(fileManager);
                    _task.DeleteSubTask(fileManager);

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
                case "MS":
                    _task.ViewSubTasks(fileManager);
                    _task.EditSubTask(fileManager);

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;

                case "N":
                    _task.ViewSubTasks(fileManager);
                    _task.CreateSubTask(fileManager);

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
                case "B":
                    taskPosition = 0;
                    _taskLists.ViewList(fileManager);
                    ListOptions(fileManager);

                    break;
                case "Q":
                    _task.ViewSubTasks(fileManager);
                    QuitProgram();

                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
                default:
                    _task.ViewSubTasks(fileManager);
                    TaskOptions(fileManager);

                    break;
            }
        }

        public static void RemoveListFromHistory(FileManager fileManager)
        {
            int listId = fileManager.Lists[listPosition - 1].ListId;

            FileManager historyManager = null;

            if (fileManager.GetType() == new ActiveManager().GetType())
            {
                historyManager = new HistoryManager();
            }
            else if (fileManager.GetType() == new ArchiveManager().GetType())
            {
                historyManager = new ArchiveHistoryManager();
            }

            historyManager.Lists.RemoveAll(TaskList => TaskList.ListId == listId);

            historyManager.Update();
        }

        public static bool AreYouSure(string message)
        {
            Console.Write(message);

            switch (Console.ReadLine().ToUpper())
            {
                case "Y":
                    return true;
                default:
                    return false;
            }
        }

        public static void QuitProgram()
        {
            Console.Write("Are you sure you want to quit the program? Y/n: ");

            switch (Console.ReadLine().ToUpper())
            {
                case "N":
                    break;
                default:
                    Environment.Exit(-1);

                    break;
            }
        }
    }
}

