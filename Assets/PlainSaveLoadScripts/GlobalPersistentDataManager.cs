using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlainSaveLoad
{
    public class GlobalPersistentDataManager : MonoBehaviour
    {
        /// <summary>
        /// Custom class for interfacing with saved data on disk. Also keeps a "cache" of game data
        /// so that it isn't constantly reading disk, which is much slower.
        /// This also means that you need to manually save when appropriate, as newest information is
        /// always in this class, not on the disk.
        /// </summary>
        private static class GameDataJsonStorage
        {
            private static JObject gameData = new JObject();

            private static readonly string GAME_DATA = "GameData";
            private static readonly string META_GAME_DATA = "MetaGameData";

            public static void LoadSave(int saveId)
            {
                DirectoryInfo directory = Directory.CreateDirectory(Application.persistentDataPath + "/saves");
                string fileName = $"{directory.FullName}/{saveId}.txt";
                if (!File.Exists(fileName))
                {
                    throw new Exception("Missing save file");
                }
                string saveFile = File.ReadAllText($"{directory.FullName}/{saveId}.txt");
                gameData = JsonConvert.DeserializeObject<JObject>(saveFile);
            }
            public static void Save(int saveId)
            {
                Debug.Log("Saving now...");
                Debug.Log($"This is application path (persistentDataPath) {Application.persistentDataPath}");
                DirectoryInfo directory = Directory.CreateDirectory(Application.persistentDataPath + "/saves");
                try
                {
                    File.WriteAllText($"{directory.FullName}/{saveId}.txt", gameData.ToString());
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
            public static void SetGameData<T>(string name, T data)
            {
                SetData(name, GAME_DATA, data);
            }
            public static void SetMetaGameData<T>(string name, T data)
            {
                SetData(name, META_GAME_DATA, data);
            }
            public static T GetGameData<T>(string name)
            {
                return GetData<T>(name, GAME_DATA);
            }
            public static T GetMetaGameData<T>(string name)
            {
                return GetData<T>(name, META_GAME_DATA);
            }
            public static void SetData<T>(string name, string category, T data)
            {
                JToken jsonData = JToken.Parse(JsonConvert.SerializeObject(data));
                JProperty prop = new JProperty(name, jsonData);
                if (gameData[category] == null)
                {
                    gameData.Add(category, new JObject());
                    gameData[category][name] = jsonData;
                }
                else
                {
                    gameData[category][name] = jsonData;
                }
            }
            public static T GetData<T>(string name, string category)
            {
                if (gameData[category] != null && gameData[category][name] != null)
                {
                    return gameData[category][name].ToObject<T>();
                }
                else
                {
                    Debug.LogWarning($"Missing value in storage, category {category} data {name} could not be found. Returning default({typeof(T)})");
                    return default;
                }
            }
            public static void ResetData()
            {
                gameData[GAME_DATA] = new JObject();
            }
            public static void ResetCache()
            {
                gameData = new JObject();
            }
            public static void LogCache()
            {
                Debug.Log(gameData);
            }
        }
        void Awake()
        {
            GameDataJsonStorage.LoadSave(0);

            JsonConvert.DefaultSettings = GetDefaultJsonSettings;
        }
        /// <summary>
        /// The default for this simple load save implementation is to preserve references.
        /// This requires less space for JSON on the disk, but also references can be preserved
        /// when you load data again, so that if you compare two classes Fruit == Fruit, if they
        /// happen to be the same class instance, the comparison will yield true.
        /// 
        /// Without this preservation, you would end up with two separate instances of Fruits, even 
        /// if the fruits have identical fields and properties.
        /// 
        /// OBS: References are only preserved within game data, not across game data.
        /// If you save Farm1 under the name "EnemyFarm" and also save Farm1 under the name "NeutralFarm"
        /// those two farms will still have separate instances, even if they are the same, because they are
        /// saved at separate occasions.
        /// </summary>
        /// <returns></returns>
        public JsonSerializerSettings GetDefaultJsonSettings()
        {
            return new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All
            };
        }
        /// <summary>
        /// Use to retrieve saved game data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetGameData<T>(string name)
        {
            return GameDataJsonStorage.GetGameData<T>(name);
        }
        /// <summary>
        /// Save any game data, such as "EnemyUnits" or "PlayerHealth".
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetGameData<T>(string name, T value)
        {
            GameDataJsonStorage.SetGameData(name, value);
        }
        /// <summary>
        /// Use to retrieve saved meta data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetMetaGameData<T>(string name)
        {
            return GameDataJsonStorage.GetMetaGameData<T>(name);
        }
        /// <summary>
        /// Save any meta game data, such as "FirstBossKilled".
        /// 
        /// Has similar usage to Unity's own "PlayerPrefs"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetMetaGameData<T>(string name, T value)
        {
            GameDataJsonStorage.SetMetaGameData(name, value);
        }
        /// <summary>
        /// Load data from disk into game state.
        /// </summary>
        /// <param name="saveId"></param>
        public void LoadSave(int saveId)
        {
            GameDataJsonStorage.LoadSave(saveId);
        }
        /// <summary>
        /// Save game state onto disk.
        /// </summary>
        /// <param name="saveId"></param>
        public void Save(int saveId)
        {
            GameDataJsonStorage.Save(saveId);
        }
        public void Save()
        {
            GameDataJsonStorage.Save(0);
        }
        /// <summary>
        /// Reset entire game state. Note, does not reset any data on disk. 
        /// If you want to do that, you need to save after resetting.
        /// </summary>
        public void SetupNewGame()
        {
            GameDataJsonStorage.ResetData();
        }
    }
}