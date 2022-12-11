namespace ToDoListApp2
{
    public class CategoryManager : FileManager
    {
        public override string FileName { get; set; }
        public override List<TaskList> Lists { get; set; }
        public override string Path { get; set; }

        public override void Create()
        {
            FileName = @"\ToDoListCategory.json";

            Path = Directory.GetParent(_currentDir).Parent.Parent.FullName + FileName;

            if (!File.Exists(Path) || String.IsNullOrEmpty(File.ReadAllText(Path)) || File.ReadAllText(Path) == "[]")
            {
                using (FileStream fs = File.Create(Path)) { }

                File.WriteAllText(Path, @"[{""Title"":""No category"",""Category"":null,""Id"":0,""Tasks"":null}]");
            }
        }

        public override void ClearLists(FileManager fileManager, bool fromCategory)
        {
            string[] fileType = fileManager.GetType().ToString().Split(".");

            if (TaskManager.AreYouSure($"Are you sure you want to delete ALL {fileType[1]} lists, this can not be undone? y/N: "))
            {
                fileManager.Lists.Clear();
                fileManager.Update();

                File.WriteAllText(Path, @"[{""Title"":""No category"",""Category"":null,""Id"":0,""Tasks"":null}]");
                Lists = Get();
            }
        }
    }
}

