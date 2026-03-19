using RedSilver2.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public sealed class CustomGameManager : GameManager
{
    private UnityEvent<int> onOrbCollected;
    private List<Orb>       orbs;

    private const string FIRST_GAME_OPENING = "FIRST GAME";
    private const string UNLOCKED_GAMEMODE = "UNLOCKED GAMEMODE";

    protected override void Awake()
    {
        base.Awake();
        orbs = new List<Orb>();
        onOrbCollected = new UnityEvent<int>();
    }

    private void Start() {
        SceneLoaderManager?.AddOnSingleSceneLoadStartedListener(sceneIndex => { orbs?.Clear(); });
        CheckSceneLoad();
    }

    private void CheckSceneLoad()
    {
        if (PlayerPrefs.GetInt(FIRST_GAME_OPENING, 0) == 0) {
            SceneLoaderManager?.LoadSingleScene(1);
            PlayerPrefs.SetInt(FIRST_GAME_OPENING, 1);
            PlayerPrefs.SetInt(UNLOCKED_GAMEMODE, 0);
            return;
        }
    }

    public void AddOrb(Orb orb)
    {
        if (orb == null || orbs == null) return;

        if (!orbs.Contains(orb)) {
            orbs?.Add(orb);
        }
    }

    public void AddOnOrbCollectedListener(UnityAction<int> action){
        if (action != null) onOrbCollected?.AddListener(action);
    }

    public void RemoveOnOrbCollectedListener(UnityAction<int> action) {
        if (action != null) onOrbCollected?.RemoveListener(action);
    }

    public void CollectOrb(Orb orb, out bool isCollected)
    {     
        isCollected = false;
        if (orb == null || orbs == null) return;

        isCollected = orbs.Contains(orb);
       
        if (isCollected) {
            orbs.Remove(orb);
            onOrbCollected.Invoke(orbs.Count);
        }

        orbs = orbs.Where(x => x != null).ToList();
    }

    public void UnlockNextGameMode() {
        int nextGamemodeIndex = PlayerPrefs.GetInt(UNLOCKED_GAMEMODE, -1) + 1;
       
        // Add Limit...
        PlayerPrefs.SetInt(UNLOCKED_GAMEMODE, nextGamemodeIndex);

    }

    public bool WasFirstGameLaunch()
    {
        if (PlayerPrefs.GetInt(FIRST_GAME_OPENING, 0) == 0)
        {
            PlayerPrefs.SetInt(FIRST_GAME_OPENING, 1);
            UnlockNextGameMode();
            return true;
        }

        return false;
    }

    public bool IsGameModeUnlocked(int gameModeIndex) {
       return gameModeIndex <= PlayerPrefs.GetInt(UNLOCKED_GAMEMODE, -1);
    }

    public static CustomGameManager GetInstance()
    {
        return instance as CustomGameManager;
    }
}
