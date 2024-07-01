using System.Collections.Generic;
using UnityEngine;

public class Updater : MonoBehaviour
{
    private List<IUpdatable> _updatables = new List<IUpdatable>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void AddUpdatable(IUpdatable updatable)
    {
        _updatables.Add(updatable);
    }

    public void RemoveUpdatable(IUpdatable updatable)
    {
        _updatables.Remove(updatable);
    }

    private void Update()
    {
        for (int i = 0; i < _updatables.Count; i++)
        {
            _updatables[i].Tick();
        }
    }
}
