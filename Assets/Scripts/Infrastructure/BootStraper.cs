using UnityEngine;

public class BootStraper : MonoBehaviour
{
    [SerializeField] private Updater _updater;
    [SerializeField] private AudioManager _audioManager;
    private GameStateMachine _gameStateMachine;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _gameStateMachine = new GameStateMachine(_updater, _audioManager);

        _ = _gameStateMachine.TransitionToState<InitializationState>();
    }
}
