using EngineToolViewer.Services;
using System.Configuration;

var engineViews = ViewService.GetEngineViews();
engineViews.ForEach(Console.WriteLine);
CsvService.ToCsv(engineViews, ConfigurationManager.AppSettings["EngineViewCSVPath"]);

var gameViews = ViewService.GetGameViews(50);
CsvService.ToCsv(gameViews, ConfigurationManager.AppSettings["GameViewCSVPath"]);