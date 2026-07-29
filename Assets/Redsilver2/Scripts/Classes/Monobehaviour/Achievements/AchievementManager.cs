using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Achievements
{
    public sealed class AchievementManager : MonoBehaviour {
        // Add Icon Loader Later
        private UnityEvent<string, bool> onAchievementAchieved;

        private void Awake() {
            onAchievementAchieved = new UnityEvent<string, bool>();
            SetSteamAchievements();
        }

        private void Start()
        {
            if(Application.isEditor) ClearSteamAchievements();
        }

        public void AddOnAchievementAchievedListener(UnityAction<string, bool> action) {
            if (action != null) onAchievementAchieved?.AddListener(action);
        }

        public void RemoveOnAchievementAchievedListener(UnityAction<string, bool> action)  {
            if (action != null) onAchievementAchieved?.RemoveListener(action);
        }

        private void SetSteamAchievements() {
            if (!SteamManager.Initialized) return;
        }

        public void UnlockSteamAchievement(string name){
            UnlockSteamAchievement(GetSteamAchievementIndex(name));
        }

        public void UnlockSteamAchievement(int index) {
           string name = GetSteamAchievementName(index);
           if (string.IsNullOrEmpty(name)) return;

           SteamUserStats.GetAchievement(name, out bool isAchieved);
           if (isAchieved) return;

           SteamUserStats.SetAchievement(name);
           SteamUserStats.StoreStats();

           onAchievementAchieved?.Invoke(name, true);
        }

        public void ClearSteamAchievement(string name) {
            ClearSteamAchievement(GetSteamAchievementIndex(name));
        }

        public void ClearSteamAchievement(int index)
        {
            string name = GetSteamAchievementName(index);
            if (string.IsNullOrEmpty(name)) return;

            SteamUserStats.ClearAchievement(name);
            SteamUserStats.StoreStats();

            onAchievementAchieved?.Invoke(name, false);
        }


        public void ClearSteamAchievements()
        {
            for (int i = 0; i < GetSteamAchievementCount(); i++)
                ClearSteamAchievement(i);
        }

        public uint GetSteamAchievementCount()
        {
            if (!SteamManager.Initialized) return 0;
            return SteamUserStats.GetNumAchievements();
        }

        public int GetSteamAchievementIndex(string achievementName) {
            string[] names = GetSteamAchievementNames();
            if(names == null || string.IsNullOrEmpty(achievementName)) return -1;

            achievementName = achievementName.ToLower();

            for(int i = 0; i < names.Length; i++) {
                if (string.IsNullOrEmpty(names[i])) continue;
                else if (names[i].ToLower() == achievementName) return i;
            }

            return -1;
        }

        public string GetSteamAchievementName(int index) {
            string[] names = GetSteamAchievementNames();

            if (!SteamManager.Initialized || names == null || index < 0 || index >= names.Length) 
                return string.Empty;

            return names[index];
        }

        public string[] GetSteamAchievementNames()
        {
            List<string> results = new List<string>();
            if (!SteamManager.Initialized) return results.ToArray();

            for (uint i = 0; i < GetSteamAchievementCount(); i++)
                results?.Add(SteamUserStats.GetAchievementName(i));

            return results.ToArray();
        }

        public static AchievementManager GetInstance()
        {
            return GameManager.AchievementManager;
        }
    }
}
