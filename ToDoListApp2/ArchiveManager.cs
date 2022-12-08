using System.Text.Json;

namespace ToDoListApp2
{
    public class ArchiveManager : FileManager
    {
        public override string FileName { get; set; }
        public override List<TaskList> Lists { get; set; }
        public override string Path { get; set; }

        public override void Create()
        {
            FileName = @"\ToDoListArchive.json";

            base.Create();
        }
    }
}

