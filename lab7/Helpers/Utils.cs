using System.Text.Json;

namespace lab7.Helpers
{
    public class Utils
    {
        private static Utils _instance;
        public static Utils Instance => _instance ??= new Utils();

        private Utils() { }

        public string ExportToJson<T>(List<T> data, List<string> selectedColumns)
        {
            if (selectedColumns == null || selectedColumns.Count == 0)
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            }

            var filteredData = data.Select(item =>
            {
                var obj = new Dictionary<string, object>();
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (selectedColumns.Contains(prop.Name))
                    {
                        obj[prop.Name] = prop.GetValue(item);
                    }
                }
                return obj;
            });

            return JsonSerializer.Serialize(filteredData, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
