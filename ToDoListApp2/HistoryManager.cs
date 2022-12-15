using System.IO.Enumeration;
using System.Text.Json;

namespace ToDoListApp2
{
    public class HistoryManager : FileManager
    {
        public override string FileName { get; set; }
        public override List<TaskList> Lists { get; set; }
        public override string Path { get; set; }

        public override void Create()
        {

            FileName = @"\ToDoListHistory.json";

            base.Create();
        }

    }
}

