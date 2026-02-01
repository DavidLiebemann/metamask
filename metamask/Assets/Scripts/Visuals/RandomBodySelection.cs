using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomBodySelection : MonoBehaviour
{
    private void OnEnable()
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);
        int randomChild = Random.Range(0, transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            
                transform.GetChild(i).gameObject.SetActive(i == randomChild);
            
        }
    }
}