using EngineToolViewer.Config;
using EngineToolViewer.Services;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();
var appSettings = config.Get<AppSettings>();

var engineViews = ViewService.GetEngineViews();
engineViews.ForEach(Console.WriteLine);
CsvService.ToCsv(engineViews, appSettings.EngineViewCSVPath);

var gameViews = ViewService.GetGameViews(50);
CsvService.ToCsv(gameViews, appSettings.GameViewCSVPath);