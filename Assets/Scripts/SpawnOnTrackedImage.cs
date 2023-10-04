using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class SpawnOnTrackedImage : MonoBehaviour
{

    private ARTrackedImageManager _trackedImageManager;

    public GameObject prefabToSpawn;

    private Dictionary<string, GameObject> _spawnedPrefabs = new Dictionary<string, GameObject>();
    
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

            if (prefabToSpawn.name == imageName && !_spawnedPrefabs.ContainsKey(imageName))
            {
                GameObject spawnedObject = Instantiate(prefabToSpawn, trackedImage.transform);
                _spawnedPrefabs[imageName] = spawnedObject;
            }
        }

        foreach (ARTrackedImage trackedImage in args.updated)
        {
            _spawnedPrefabs[trackedImage.referenceImage.name].SetActive(trackedImage.trackingState == TrackingState.Tracking);
        }
        
        foreach (ARTrackedImage trackedImage in args.removed)
        {
            //May not want to destroy so I can keep game progress still. 
            _spawnedPrefabs[trackedImage.referenceImage.name].SetActive(false);
            //Destroy(_spawnedPrefabs[trackedImage.referenceImage.name]);
            //_spawnedPrefabs.Remove(trackedImage.referenceImage.name);
        }
        
    }
}
