using System;
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
        private static String savePath = Path.GetTempPath()+"favorites.json";
        
        public static async Task saveFavoriteJSON(ObservableCollection<Favorite> favorites)
        {
            using (StreamWriter sw = new StreamWriter(savePath, false, System.Text.Encoding.Default))
            {
                await sw.WriteAsync(JsonSerializer.Serialize(favorites));
            }
        }

        public static ObservableCollection<Favorite> loadFavoriteJSON()
        {
            if (File.Exists(savePath))
            {
                using (StreamReader fs = new StreamReader(savePath))
                {
                    string json = fs.ReadToEnd();
                    return JsonSerializer.Deserialize<ObservableCollection<Favorite>>(json);
                }
            }
            return new ObservableCollection<Favorite>();
        }
    }
}