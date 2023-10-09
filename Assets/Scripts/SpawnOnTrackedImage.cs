using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class SpawnOnTrackedImage : MonoBehaviour
{
    private ARTrackedImageManager _trackedImageManager;
    
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
            
            GameObject spawnedPrefab = SceneManager.Instance.spawnedPrefab;
            
            if (_trackedImageManager.trackedImagePrefab.name == imageName && spawnedPrefab == null)
            {
                GameObject spawnedObject = GameObject.FindWithTag("Trackable");
                SceneManager.Instance.spawnedPrefab = spawnedObject;
                SceneManager.Instance.StartCountdown();
            }
        }
    }
}
