using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomBodySelection : MonoBehaviour
{
    private void OnEnable()
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);
        int randomChild = Random.Range(0, children.Length);
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].parent == transform)
            {
                children[i].gameObject.SetActive(i == randomChild);
            }
        }
    }
}