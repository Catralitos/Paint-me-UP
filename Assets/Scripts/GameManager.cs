using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioManager))]
public class GameManager : MonoBehaviour
{
    #region SingleTon
     
    public static GameManager Instance { get; private set; }
 
    private void Awake()
    {
        if (Instance == null)
        {
            audioManager = GetComponent<AudioManager>();
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

    [HideInInspector] public AudioManager audioManager;
    
    private void Start()
    {
        beatenLevels = new List<int>();
        audioManager.Play("MenuMusic");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex < 2)
        {
            if (audioManager != null) audioManager.Play("MenuMusic");
        }
        else
        {
            if (audioManager != null) audioManager.Stop("MenuMusic");
        }
    }

    public void BeatLevel(int sceneIndex)
    {
        // There are two scenes that are not levels, but we don't want to add 0 for level 1, so we add 1
        beatenLevels.Add(sceneIndex - 2 + 1);
    }
}
