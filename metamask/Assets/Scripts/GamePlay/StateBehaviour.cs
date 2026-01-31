using System;
using UnityEngine;
using UnityEngine.Serialization;

public class StateBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("target")] [SerializeField] private StateBehaviour nextState;
    [SerializeField] private bool isStartingBehaviour = false;

    private void OnEnable()
    {
        if (isStartingBehaviour)
        {
            var findAnyObjectByType = FindObjectsByType<StateBehaviour>(FindObjectsSortMode.None);
            foreach (var stateBehaviour in findAnyObjectByType)
            {
                if (stateBehaviour != this)
                {
                    stateBehaviour.gameObject.SetActive(false);
                }
            }
        }
    }

    public void Switch()
    {
        if (nextState)
        {
            SwitchTo(nextState);
        }
    }

    public void SwitchTo(StateBehaviour targetBehaviour)
    {
        if (targetBehaviour)
        {
            gameObject.SetActive(false);
            targetBehaviour.gameObject.SetActive(true);
        }
    }
}