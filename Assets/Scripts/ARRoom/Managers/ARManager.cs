using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    ARPlacementManager aRPlacementManager;
    ARModificationManager aRModificationManager;

    GameObject aRModel;

    [SerializeField] GameObject aRModelPrefab;

    void Start()
    {
        aRPlacementManager = transform.Find("ARPlacementManager").gameObject.GetComponent<ARPlacementManager>();
        aRModificationManager = transform.Find("ARModificationManager").gameObject.GetComponent<ARModificationManager>();

        InitializeARModel();
    }

    public void SetActiveARPlacementManager (bool isActive)
    {
        aRPlacementManager.gameObject.SetActive(isActive);
    }

    public void SetActiveARModificationManager (bool isActive)
    {
        aRModificationManager.gameObject.SetActive(isActive);
    }

    private void InitializeARModel()
    {
        // TODO Check which condotype selected
        aRModel = Instantiate(aRModelPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        aRModel.SetActive(false);

        aRPlacementManager.aRModel = aRModel;
    }
}
