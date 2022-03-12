using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class SelectionGenerator : MonoBehaviour
{

    [SerializeField]
    public GameObject[] bedPrefabs;

    [SerializeField]
    GameObject objectItemPrefab;

    private GameObject selectedObject;
    GameObject selectionItem;


    public void DisplaySelections (GameObject selectedObject)
    {
        MainManager.Instance.Debug2("Selection Generator - Display selections ");

        this.selectedObject = selectedObject;

        if (this.selectedObject.CompareTag("Bed"))
        {
            for (int i = 0; i < bedPrefabs.Length; i++)
            {
                int x = i;
                selectionItem = Instantiate(objectItemPrefab, transform);
                selectionItem.GetComponent<Button>().onClick.AddListener( () => { OnItemClicked(x); } );
                selectionItem.transform.GetChild(0).GetComponent<Image>().sprite = bedPrefabs[i].GetComponent<Bed>().Thumbnail;
                selectionItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = bedPrefabs[i].GetComponent<Bed>().ObjectName + " " + i;
            }

        }
        
    }

    public void EmptyScrollList ()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnItemClicked (int index)
    {
        MainManager.Instance.Debug1("Selection Generator - Clicked on " + index);

        if (selectedObject.CompareTag("Bed"))
        {
            Instantiate(bedPrefabs[index], selectedObject.transform.position, selectedObject.transform.rotation, selectedObject.transform.parent);
            Destroy(selectedObject);
        }
    }

}
