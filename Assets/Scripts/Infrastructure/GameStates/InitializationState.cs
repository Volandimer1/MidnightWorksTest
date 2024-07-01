using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class InitializationState : IGameState
{
    private GameStateMachine _gameStateMachine;
    private Scene _currentScene;

    public InitializationState(GameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public Task EnterState()
    {
        _currentScene = SceneManager.GetActiveScene();

        Settings optionSettings = OptionSettings.LoadSettings();
        if (optionSettings != null)
        {
            _gameStateMachine.AudioManager.SetSFXVolume(optionSettings.SFXVolume);
            _gameStateMachine.AudioManager.SetMusicVolume(optionSettings.MusicVolume);
        }
        else
        {
            _gameStateMachine.AudioManager.SetSFXVolume(0.5f);
            _gameStateMachine.AudioManager.SetMusicVolume(0.5f);

            OptionSettings.SaveSettings(0.5f, 0.5f);
        }

        _ = _gameStateMachine.TransitionToState<MenuState>();

        return Task.CompletedTask;
    }

    public Task ExitState()
    {
        SceneManager.UnloadSceneAsync(_currentScene);

        return Task.CompletedTask;
    }
}