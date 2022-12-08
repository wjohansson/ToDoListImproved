using System.Text.Json;

namespace ToDoListApp2
{
    public class ActiveManager : FileManager
    {
        public override string FileName { get; set; }
        public override List<TaskList> Lists { get; set; }
        public override string Path { get; set; }

        public override void Create()
        {
            FileName = @"\ToDoList.json";

            base.Create();
        }
    }
}

