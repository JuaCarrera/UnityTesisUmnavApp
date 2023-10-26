using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraCompass : MonoBehaviour
{
    private Gyroscope gyro;
    private Quaternion initialRotation;
    private float initialTrueNorth;
    public TextMeshProUGUI textcamerarotation;
    public bool gyroAvailable;
    void Start()
    {
        initialRotation = transform.rotation;

        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroAvailable = true;
        }
        else
        {
            gyroAvailable = false;

        }

        StartCoroutine(InitializeCompass());

    }

   
    // Update is called once per frame
    void Update()
    {
        if (gyroAvailable)
        {
            Quaternion gyroAttitude = gyro.attitude;
            Vector3 gyroEulerAngles = gyroAttitude.eulerAngles;
            float yawRotation = gyroEulerAngles.z;
            transform.rotation = Quaternion.Euler(90f, -yawRotation-180, 0);
            textcamerarotation.text = "rotation camara: " + transform.rotation.eulerAngles; 
        }
    }
    
    private IEnumerator InitializeCompass()
    {
        Input.compass.enabled = true;
        Input.location.Start();
        int maxWait = 10;


        while(Input.location.status==LocationServiceStatus.Initializing && maxWait>0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait<=0|| Input.location.status == LocationServiceStatus.Failed)
        {
            yield break;
        }

        initialTrueNorth = Input.compass.trueHeading;
        //Input.location.Stop();


    }
}
