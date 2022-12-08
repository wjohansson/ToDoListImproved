namespace ToDoListApp2
{
    public class SubTask
    {
        public string SubTaskTitle { get; set; }
        public string SubTaskDescription { get; set; }
        public int SubTaskId { get; init; }

        public SubTask()
        {

        }

        public SubTask(string subTaskTitle, string subTaskDescription, int subTaskId)
        {
            SubTaskTitle = subTaskTitle;
            SubTaskDescription = subTaskDescription;
            SubTaskId = subTaskId;
        }
    }
}

