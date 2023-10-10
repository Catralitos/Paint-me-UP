using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SingleTon
     
    public static GameManager Instance { get; private set; }
 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
 
    #endregion

    [HideInInspector] public List<int> beatenLevels;

    private void Start()
    {
        beatenLevels = new List<int>();
    }

    public void BeatLevel(int sceneIndex)
    {
        // There are two scenes that are not levels, but we don't want to add 0 for level 1, so we add 1
        beatenLevels.Add(sceneIndex - 2 + 1);
    }
}
