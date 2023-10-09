using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    #region SingleTon
     
     public static SceneManager Instance { get; private set; }
 
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
     }
 
    #endregion
     
    [SerializeField] private TextMeshProUGUI initialCountdownText;
    [SerializeField] private TextMeshProUGUI timeLimitText;
    [SerializeField] private TextMeshProUGUI objectRevealText;
    [Space]
    [HideInInspector] public Color currentColor;
    [Space]
    [SerializeField] private List<ParticleSystem> particleSystems;
    [Space] 
    public GameObject prefabToPaint;
    [HideInInspector] public int currentRound;
    [HideInInspector] public GameObject spawnedPrefab;
    
    private List<GameObject> _prefabParts;
    private float _currentTimeLeft = 120;
    private bool _gameStarted;
    
    public void StartCountdown()
    {
        // This is called when the prefab is spawned on the marker
        _prefabParts = new List<GameObject>();
        // We save the children
        for (int i = 0; i < spawnedPrefab.transform.childCount; i++)
        {
            _prefabParts.Add(spawnedPrefab.transform.GetChild(i).gameObject);
        }        
        // And enable only the right part
        EnableRightPart();
        initialCountdownText.gameObject.SetActive(true);
        StartCoroutine(nameof(Countdown));
    }

    private IEnumerator Countdown () {
        int counter = 3;
        initialCountdownText.text = counter.ToString();
        while (counter > 0) {
            yield return new WaitForSeconds (1);
            counter--;
            initialCountdownText.text = counter.ToString();
        }
        initialCountdownText.text = "GO";
        yield return new WaitForSeconds (1);
        StartGame();
    }
    
    private void StartGame()
    {
        initialCountdownText.gameObject.SetActive(false);
        _gameStarted = true;
        HUDManager.Instance.EnableColorDetectionHUD();
    }
    
    // do coroutine

    private void Update()
    {
        if (!_gameStarted) return;

        if (_currentTimeLeft < 0)
        {
            timeLimitText.text = "0";
            LoseGame();
            return;
        }
        
        _currentTimeLeft -= Time.deltaTime;
        timeLimitText.text = Mathf.RoundToInt(_currentTimeLeft).ToString();
    }

    public void IncreaseRound()
    {
        DisableParticles();
        currentRound++;
        if (currentRound > _prefabParts.Count)
        {
            WinGame();
            return;
        }
        EnableRightPart();
        HUDManager.Instance.EnableColorDetectionHUD();
    }

    private void EnableRightPart()
    {
        for (int i = 0; i < _prefabParts.Count; i++)
        {
            _prefabParts[i].SetActive(i == currentRound - 1);
        }
    }
    
    private void EnableAllParts()
    {
        foreach (GameObject g in _prefabParts)
        {
            g.SetActive(true);
        }
    }

    public void EnableParticles()
    {
        foreach (ParticleSystem p in particleSystems)
        {
            p.Play();
        }
    }
    
    public void DisableParticles()
    {
        foreach (ParticleSystem p in particleSystems)
        {
            p.Stop();
        }
    }

    private void WinGame()
    {
        HUDManager.Instance.DisableHUD();
        EnableAllParts();
        objectRevealText.gameObject.SetActive(true);
    }
    
    private void LoseGame()
    {
        HUDManager.Instance.DisableHUD();
    }
}