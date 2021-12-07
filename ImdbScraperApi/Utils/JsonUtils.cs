using ImdbScraperApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImdbScraperApi.Utils
{

    //serialize and deserialize celebrity list into a json file 9and vice versa) using json.net nuget package
    public static class JsonUtils
    {
        public static List<Celebrity> DeserializeCelebritiesFromJson()  //serializer
        {
            try
            {
                var celebritiesList = JsonConvert.DeserializeObject<List<Celebrity>>(File.ReadAllText(@CelebritiesJsonFilePath()));
                return celebritiesList;
            }
            catch (FileNotFoundException ex)
            {
                return null;
            }
        }



        public static void SerializeCelebritiesToJsonFile(List<Celebrity>  celebs) //deserializer
        {
            File.WriteAllText(@CelebritiesJsonFilePath(), JsonConvert.SerializeObject(celebs));
        }

        public static bool DeleteCelebrityfromJsonById(int  celebId)
        {
            var CelebritiesFromJson = DeserializeCelebritiesFromJson();
            var itemToRemove = CelebritiesFromJson.SingleOrDefault(r => r.Id == celebId);
            if (itemToRemove != null)
            {
                var returnVal = CelebritiesFromJson.Remove(itemToRemove);
                SerializeCelebritiesToJsonFile(CelebritiesFromJson);
                return returnVal;
            }

            return false;
        }

        private static string CelebritiesJsonFilePath()   //creates path relative to the project directory inorder to save the json file there.
        {
            string currentDir = Environment.CurrentDirectory;
            string repositoryAdress = currentDir + "/Repository/CelebritiesJsonRepo.json";
            return repositoryAdress;
        }
    }
}
