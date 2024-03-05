namespace EngineToolViewer.Services
{
    public static class CsvService
    {
        private const string Separator = ";";

        public static void ToCsv<T>(List<T> values, string filePath)
        {
            var type = typeof(T);
            var properties = type.GetProperties();

            using var writer = new StreamWriter(filePath, false);

            var header = properties.Select(p => p.Name).Aggregate((acc, curr) => acc + Separator + curr);
            writer.WriteLine(header);

            var contents = values.Select(v => properties.Select(p => p.GetValue(v)).Select(v => v.ToString()).Aggregate((acc, curr) => acc + Separator + curr)).ToList();
            contents.ForEach(writer.WriteLine);
        }
    }
}
