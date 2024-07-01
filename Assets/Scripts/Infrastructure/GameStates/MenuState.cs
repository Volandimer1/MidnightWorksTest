using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuState : IGameState
{
    private GameStateMachine _gameStateMachine;
    private PopUpService _popUpService;
    private const string MenuSceneName = "MenuScene";
    private const string PlayButtonName = "MenuPlayButton";
    private const string SettingsButtonName = "MenuSettingsButton";
    private const string MenuSFXBundlePath = "AudioBundles/MainMenu/DefaultMenuSounds";
    private const string MenuMusicBundlePath = "AudioBundles/MainMenu/DefaultMenuMusic";

    private Scene menuScene;
    private AudioClipsBundle _mainMenuSFXBundle;
    private AudioClipsBundle _mainMenuMusicBundle;

    public MenuState(GameStateMachine gameStateMachine, PopUpService popUpService)
    {
        _gameStateMachine = gameStateMachine;
        _popUpService = popUpService;
    }

    public async Task EnterState()
    {
        if (await LoadMenuSceneAsync())
        {
            await PrepareNewScene();
        }
    }

    public async Task ExitState()
    {
        AsyncOperation asyncUnLoad = null;
        if (menuScene.isLoaded)
        {
            asyncUnLoad = SceneManager.UnloadSceneAsync(menuScene);
        }

        while (!asyncUnLoad.isDone)
        {
            await Task.Yield();
        }

        Resources.UnloadUnusedAssets();
    }

    private async Task<bool> LoadMenuSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MenuSceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            await Task.Yield();
        }

        if (SceneManager.GetSceneByName(MenuSceneName).isLoaded)
        {
            menuScene = SceneManager.GetSceneByName(MenuSceneName);
            SceneManager.SetActiveScene(menuScene);
            return true;
        }
        else
        {
            Debug.LogError("Failed to load the MenuScene.");
            return false;
        }
    }

    private async Task PrepareNewScene()
    {
        await Task.Yield();

        _mainMenuSFXBundle = Resources.Load<AudioClipsBundle>(MenuSFXBundlePath);
        if (_mainMenuSFXBundle == null)
        {
            Debug.LogError($"Failed to load MenuSFXBundle from path: {MenuSFXBundlePath}");
        }
        else
        {
            _gameStateMachine.AudioManager.LoadEnumClipsPairDictionaryFromBundle(_mainMenuSFXBundle);
            
        }

        _mainMenuMusicBundle = Resources.Load<AudioClipsBundle>(MenuMusicBundlePath);
        if (_mainMenuMusicBundle == null)
        {
            Debug.LogError($"Failed to load MenuMusicBundle from path: {MenuMusicBundlePath}");
        }
        else
        {
            _gameStateMachine.AudioManager.LoadEnumMusicPairDictionaryFromBundle(_mainMenuMusicBundle);

        }

        _gameStateMachine.AudioManager.PlayMusic(AudioClipsEnum.MenuMusic, true);

        GameObject playButtonObject = GameObject.Find(PlayButtonName);
        if (playButtonObject == null)
        {
            Debug.LogError($"Failed to find {PlayButtonName} in the scene.");
            return;
        }

        Button playButton = playButtonObject.GetComponent<Button>();
        playButton.onClick.AddListener(DoPlayButtonClickedAction);

        GameObject settingsButtonObject = GameObject.Find(SettingsButtonName);
        if (settingsButtonObject == null)
        {
            Debug.LogError($"Failed to find {SettingsButtonName} in the scene.");
            return;
        }

        Button settingsButton = settingsButtonObject.GetComponent<Button>();
        settingsButton.onClick.AddListener(DoSettingsButtonClickedAction);

        await Task.Yield();
    }

    private void DoPlayButtonClickedAction()
    {
        _gameStateMachine.AudioManager.PlaySFX(AudioClipsEnum.MenuClick, Vector3.zero);

        _gameStateMachine.AudioManager.StopMusic();
        _gameStateMachine.AudioManager.ReleaseMusicClips();
        _gameStateMachine.AudioManager.ReleaseSoundClips();

        _ = _gameStateMachine.TransitionToState<LevelState>();
    }

    private void DoSettingsButtonClickedAction()
    {
        _gameStateMachine.AudioManager.PlaySFX(AudioClipsEnum.MenuClick, Vector3.zero);

        _popUpService.ShowOptionsPopUp(_gameStateMachine);
    }
}