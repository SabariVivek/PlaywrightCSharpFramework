using Framework.Core.Config;
using Newtonsoft.Json;

namespace Framework.Test.Resources
{
    public class TestDataService
    {
        // Static Json key values...
        public readonly static string CORE = "Core";
        public readonly static string ENVIRONMENT = "Envirnoment";

        public TestConfiguration Config { get; set; }

        public static TestConfiguration GetData()
        {
            var dataFile = Path.Combine(StartUp.GetProjectDirectory(), "Resources", "config.json");
            var data = JsonConvert.DeserializeObject<TestConfiguration>(File.ReadAllText(dataFile));
            return data;
        }

        public void LoadData()
        {
            Config = GetData();
        }
    }
}