using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Core.Money
{
    public static class MoneySaveSystem
    {
        public static void SaveMoneyData(MoneyData data)
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "/money.lol";
            var fileStream = new FileStream(path, FileMode.Create);
            
            formatter.Serialize(fileStream, data);
            fileStream.Close();
        }

        public static MoneyData LoadMoneyData()
        {
            var path = Application.persistentDataPath + "/money.lol";
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var fileStream = new FileStream(path, FileMode.Open);

                var data = (MoneyData)formatter.Deserialize(fileStream);
                
                fileStream.Close();
                return data;
            }
            else
            {
                Debug.LogError($"Money save file not found in {path}");
                return default;
            }
        }
    }
}
