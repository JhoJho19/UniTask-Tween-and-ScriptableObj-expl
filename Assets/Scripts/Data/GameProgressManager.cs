using UnityEngine;
using UnityEngine.Events;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Data
{
    public class GameProgressManager : MonoBehaviour
    {
        public static GameProgressManager Instance { get; private set; }
        public UnityEvent OnIncrementScore;
        public UnityEvent OnDecrementScore;

        public int LvlCount { get; set; }
        public int GoldCoinCounter { get; set; }
        public int SilverCoinCounter { get; set; }
        public int IndexOfPlayerSkin { get; set; }
        public List<bool> ListOfSkins { get; set; }

        private string savePath;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            savePath = Application.persistentDataPath + "/gameProgress.json";
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SaveProgress();
        }

        public void SaveProgress()
        {
            GameProgressData data = new GameProgressData
            {
                LvlCount = LvlCount,
                GoldCoinCounter = GoldCoinCounter,
                SilverCoinCounter = SilverCoinCounter, // Added this line
                IndexOfPlayerSkin = IndexOfPlayerSkin,
                ListOfSkins = ListOfSkins
            };

            string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(savePath, json);
        }

        public void LoadProgress()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                GameProgressData data = JsonConvert.DeserializeObject<GameProgressData>(json);

                LvlCount = data.LvlCount;
                GoldCoinCounter = data.GoldCoinCounter;
                SilverCoinCounter = data.SilverCoinCounter;
                IndexOfPlayerSkin = data.IndexOfPlayerSkin;
                ListOfSkins = data.ListOfSkins;
            }
            else
            {
                LvlCount = 1;
                GoldCoinCounter = 0;
                SilverCoinCounter = 0;
                IndexOfPlayerSkin = 0;
            }
        }

        public void IncrementGold()
        {
            GoldCoinCounter += 1;
            OnIncrementScore.Invoke();
        }

        public void DecrementGold()
        {
            if (GoldCoinCounter > 0)
                GoldCoinCounter -= 1;
            OnDecrementScore.Invoke();
        }

        public void IncrementSilver()
        {
            SilverCoinCounter += 1;
            OnIncrementScore.Invoke();
        }

        public void DecrementSilver()
        {
            if (SilverCoinCounter > 0)
                SilverCoinCounter -= 1;
            OnDecrementScore.Invoke();
        }
    }

    [System.Serializable]
    public class GameProgressData
    {
        public int LvlCount;
        public int GoldCoinCounter;
        public int SilverCoinCounter; // Added this line
        public int IndexOfPlayerSkin;
        public List<bool> ListOfSkins;
    }
}
