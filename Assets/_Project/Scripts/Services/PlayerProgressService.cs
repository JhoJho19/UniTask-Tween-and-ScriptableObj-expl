using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Services
{
    public class PlayerProgressService : MonoBehaviour
    {
        private readonly PlayerProgressData progressData = new PlayerProgressData();
        private CoinService coins;
        private string savePath;

        public int CurrentLevelBuildIndex
        {
            get => progressData.CurrentLevelBuildIndex;
            set => progressData.CurrentLevelBuildIndex = value;
        }

        public int SelectedPlayerSkinIndex
        {
            get => progressData.SelectedPlayerSkinIndex;
            set => progressData.SelectedPlayerSkinIndex = value;
        }

        public List<bool> UnlockedSkins
        {
            get => progressData.UnlockedSkins;
            set => progressData.UnlockedSkins = value ?? new List<bool>();
        }

        public CoinService Coins
        {
            get
            {
                if (coins == null)
                {
                    coins = new CoinService(progressData);
                }

                return coins;
            }
        }

        private void Awake()
        {
            savePath = Path.Combine(Application.persistentDataPath, "gameProgress.json");
            LoadProgress();
            _ = Coins;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SaveProgress();
        }

        public void SaveProgress()
        {
            string json = JsonConvert.SerializeObject(progressData, Formatting.Indented);
            File.WriteAllText(savePath, json);
        }

        public void LoadProgress()
        {
            if (!File.Exists(savePath))
            {
                ApplyProgress(CreateDefaultProgress());
                return;
            }

            string json = File.ReadAllText(savePath);
            ApplyProgress(JsonConvert.DeserializeObject<PlayerProgressData>(json) ?? CreateDefaultProgress());
        }

        private static PlayerProgressData CreateDefaultProgress()
        {
            return new PlayerProgressData
            {
                CurrentLevelBuildIndex = 1,
                GoldCoinCounter = 0,
                SilverCoinCounter = 0,
                SelectedPlayerSkinIndex = 0,
                UnlockedSkins = new List<bool>()
            };
        }

        private void ApplyProgress(PlayerProgressData source)
        {
            progressData.CurrentLevelBuildIndex = source.CurrentLevelBuildIndex;
            progressData.GoldCoinCounter = source.GoldCoinCounter;
            progressData.SilverCoinCounter = source.SilverCoinCounter;
            progressData.SelectedPlayerSkinIndex = source.SelectedPlayerSkinIndex;
            progressData.UnlockedSkins = source.UnlockedSkins ?? new List<bool>();
        }
    }

    [System.Serializable]
    public class PlayerProgressData
    {
        [JsonProperty("LvlCount")]
        public int CurrentLevelBuildIndex;

        [JsonProperty("GoldCoinCounter")]
        public int GoldCoinCounter;

        [JsonProperty("SilverCoinCounter")]
        public int SilverCoinCounter;

        [JsonProperty("IndexOfPlayerSkin")]
        public int SelectedPlayerSkinIndex;

        [JsonProperty("ListOfSkins")]
        public List<bool> UnlockedSkins;
    }
}
