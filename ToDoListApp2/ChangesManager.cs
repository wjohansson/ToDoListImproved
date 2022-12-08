namespace ToDoListApp2
{
    public class ChangesManager : FileManager
    {
        public override string FileName { get; set; }
        public override List<TaskList> Lists { get; set; }
        public override string Path { get; set; }

        public override void Create()
        {
            FileName = @"\ToDoListChanges.json";

            base.Create();
        }
    }
}

