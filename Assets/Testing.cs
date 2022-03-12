using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public GameObject selectedObject;
    public GameObject[] prefabs;

    public void ChangeObject ()
    {
        Instantiate(prefabs[0], selectedObject.transform.position, selectedObject.transform.rotation, selectedObject.transform.parent);
        Destroy(selectedObject);
    }
}
