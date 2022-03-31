using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundAnimator : MonoBehaviour
{
    Vector3 cameraStartPosition = new Vector3(-1.5f, 2.5f, -3.0f);
    Vector3 cameraSecondPosition = new Vector3(1.6f, 2.5f, 5.8f);
    Vector3 cameraThirdPosition = new Vector3(-1.5f, 2.5f, -3.0f);

    Vector3 cameraStartRotation = new Vector3(28, 10, 0);
    Vector3 cameraSecondRotation = new Vector3(28, 210, 0);

    void Start()
    {
        StartCoroutine(CameraMovementOne());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CameraMovementOne()
    {
        for (int i=0; i<800; i++)
        {
            if (i <= 400)
                Camera.main.transform.Translate(new Vector3(0.003f, 0, 0));
            else if (i > 400)
                Camera.main.transform.Translate(new Vector3(0.003f, 0.001f, 0.002f));
            
            if (i == 1)
            {
                Camera.main.transform.position = cameraStartPosition;
                Camera.main.transform.rotation = Quaternion.Euler(cameraStartRotation);
            }
            else if (i == 400)
            {
                Camera.main.transform.position = cameraSecondPosition;
                Camera.main.transform.rotation = Quaternion.Euler(cameraSecondRotation);
            }
            else if (i == 799)
                i = 0;
                

            yield return null;
        }
    }

}
