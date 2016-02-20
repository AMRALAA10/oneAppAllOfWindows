using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneAppAllOfWindows.model;
using System.IO;
using System.Runtime.Serialization.Json;
using Windows.Storage;

namespace OneAppAllOfWindows.helper
{
    public static class PlacesRepo
    {
        private static List<Place> allPlaces;

        public async static Task<List<Place>> GetData(StorageFile file)
        {
            if (allPlaces != null)
                return allPlaces;



            string jsonText = await FileIO.ReadTextAsync(file);


            //StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appdata:///places.json"));
            var fileStream = await file.OpenStreamForReadAsync();
    

            var serializer = new DataContractJsonSerializer(typeof(List<Place>));
            allPlaces = (List<Place>)serializer.ReadObject(fileStream);

            return allPlaces;

        }
    }
}
