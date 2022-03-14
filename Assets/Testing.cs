using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testing : MonoBehaviour
{
    public GameObject selectedObject;
    public GameObject[] prefabs;

    public Button btn;

    private void Start()
    {
        Button newbtn = Instantiate(btn, btn.transform.position - new Vector3(50, 0, 0), btn.transform.rotation, btn.transform.parent);
        newbtn.onClick.AddListener(() => { ChangeObject(prefabs[0]); });
    }

    public void ChangeObject (GameObject prefab)
    {
        Debug.Log(prefab.name);
        GameObject newObject = Instantiate(prefabs[0], selectedObject.transform.position, selectedObject.transform.rotation, selectedObject.transform.parent);
        Destroy(selectedObject);
        DisplayMessage(prefabs[0]);
    }

    void DisplayMessage (GameObject prefab)
    {
        //Debug.Log(prefab.name);
    }
}
