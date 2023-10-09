using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class SpawnOnTrackedImage : MonoBehaviour
{
    private ARTrackedImageManager _trackedImageManager;

    public TextMeshProUGUI statusText;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        _trackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
    }

    private void OnDisable()
    {
        _trackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;

    }

    private void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            string imageName = trackedImage.referenceImage.name;
            
            GameObject prefabToSpawn = SceneManager.Instance.prefabToPaint;
            GameObject spawnedPrefab = SceneManager.Instance.spawnedPrefab;
            
            if (prefabToSpawn.name == imageName && spawnedPrefab == null)
            {
                GameObject spawnedObject = Instantiate(prefabToSpawn, trackedImage.transform.position, trackedImage.transform.rotation, trackedImage.transform);
                SceneManager.Instance.spawnedPrefab = spawnedObject;
                SceneManager.Instance.StartCountdown();
            }
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            statusText.text = trackedImage.trackingState.ToString();
            SceneManager.Instance.spawnedPrefab.SetActive(trackedImage.trackingState == TrackingState.Tracking);
            SceneManager.Instance.spawnedPrefab.transform.position = trackedImage.transform.position;
            SceneManager.Instance.spawnedPrefab.transform.rotation = trackedImage.transform.rotation;
            //statusText.text = trackedImage.trackingState.ToString() + " at " + trackedImage.transform.position + " with object at " + SceneManager.Instance.spawnedPrefab.transform.position;

        }
        
        foreach (ARTrackedImage trackedImage in args.removed)
        {
            SceneManager.Instance.spawnedPrefab.SetActive(false);
        }
    }
}
