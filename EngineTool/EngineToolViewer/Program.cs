using EngineToolViewer.Services;

var engineViews = ViewService.GetEngineViews();
engineViews.ForEach(Console.WriteLine);
CsvService.ToCsv(engineViews, "C:\\Temp\\EngineView.csv");

var gameViews = ViewService.GetGameViews(50);
CsvService.ToCsv(gameViews, "C:\\Temp\\GameView.csv");