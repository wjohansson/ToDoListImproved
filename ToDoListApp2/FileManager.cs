using System.Text.Json;

namespace ToDoListApp2
{
    public abstract class FileManager : IStateManager
    {
        public readonly string _currentDir = Environment.CurrentDirectory;
        public abstract string Path { get; set; }
        public abstract string FileName { get; set; }
        public abstract List<TaskList> Lists { get; set; }

        public FileManager()
        {
            Create();
            Lists = Get();
        }


        public virtual void Create()
        {
            Path = Directory.GetParent(_currentDir).Parent.Parent.FullName + FileName;

            if (!File.Exists(Path) || String.IsNullOrEmpty(File.ReadAllText(Path)))
            {
                using (FileStream fs = File.Create(Path)) { }

                File.WriteAllText(Path, "[]");
            }
        }

        public List<TaskList> Get()
        {
            string jsonData = File.ReadAllText(Path);

            List<TaskList> lists = JsonSerializer.Deserialize<List<TaskList>>(jsonData);

            return lists;
        }

        public void Update()
        {
            string jsonData = JsonSerializer.Serialize(Lists);

            File.WriteAllText(Path, jsonData);
        }

        public void Clear(FileManager fileManager)
        {
            string[] fileType = fileManager.GetType().ToString().Split(".");

            if (TaskManager.AreYouSure($"Are you sure you want to delete ALL {fileType[1]} lists, this can not be undone? y/N: "))
            {
                Lists.Clear();

                Update();
            }

        }
    }
}

