using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Browser
{
    public static class ManagementSave
    {
        private static String savePathFavorite = Path.GetTempPath()+"BrowserStounhandJFavorites.json";
        private static String savePathHistory = Path.GetTempPath()+"BrowserStounhandJHistory.json";
        
        public static async Task saveFavoriteJSON(ObservableCollection<Favorite> favorites)
        {
            using (StreamWriter sw = new StreamWriter(savePathFavorite, false, System.Text.Encoding.Default))
            {
                await sw.WriteAsync(JsonSerializer.Serialize(favorites));
            }
        }

        public static ObservableCollection<Favorite> loadFavoriteJSON()
        {
            if (File.Exists(savePathFavorite))
            {
                using (StreamReader fs = new StreamReader(savePathFavorite))
                {
                    string json = fs.ReadToEnd();
                    return JsonSerializer.Deserialize<ObservableCollection<Favorite>>(json);
                }
            }
            return new ObservableCollection<Favorite>();
        }
        
        public static async Task saveHistoryJSON(List<History> historys)
        {
            using (StreamWriter sw = new StreamWriter(savePathHistory, false, System.Text.Encoding.Default))
            {
                await sw.WriteAsync(JsonSerializer.Serialize(historys));
            }
        }

        public static List<History> loadHistoryJSON()
        {
            if (File.Exists(savePathHistory))
            {
                using (StreamReader fs = new StreamReader(savePathHistory))
                {
                    string json = fs.ReadToEnd();
                    return JsonSerializer.Deserialize<List<History>>(json);
                }
            }
            return new List<History>();
        }
    }
}