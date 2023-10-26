using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Android;

public class LocationService : MonoBehaviour
{
    public TextMeshProUGUI textLocation;
    public bool isEnableLocation;

    private void Awake()
    {
       if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
       {
           textLocation.text = "Permisos de ubicacion activado";
       }
       else
       {
            textLocation.text = "Activar permisos de ubicaicon";
            Permission.RequestUserPermission(Permission.FineLocation);

       }
    }
    public void StartGPS()
    {
        if (isEnableLocation)
        {
            StartCoroutine(StartGPSCoroutine());
        }
    }

    IEnumerator StartGPSCoroutine()
    {
        if (!Input.location.isEnabledByUser)
        {
            textLocation.text = ("Activar permisos de ubicacion");
            Permission.RequestUserPermission(Permission.FineLocation);
            yield break;


        }
        Input.location.Start();

        int maxWait = 20;

        while(Input.location.status==LocationServiceStatus.Initializing && maxWait>0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if(maxWait<1)
        {
            print("TimeOut");
            textLocation.text = ("Timeout");
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unavaliable location");
            textLocation.text = ("Unavaliable location");
            yield break;
        }
        else
        {
            while (true)
            {
                yield return new WaitForSeconds(5f);

                print("Location: " + Input.location.lastData.latitude  
                + " " + Input.location.lastData.longitude
                + " " + Input.location.lastData.altitude
                + " " + Input.location.lastData.horizontalAccuracy
                + " " + Input.location.lastData.verticalAccuracy
                + " " + Input.location.lastData.timestamp
               );

                textLocation.text = ("Latitud: " + Input.location.lastData.latitude
                + "\nLongitud: " + Input.location.lastData.longitude
                + "\nAltitud : " + Input.location.lastData.altitude
                + "\nHorizontal: " + Input.location.lastData.horizontalAccuracy
                + "\nVertical: " + Input.location.lastData.verticalAccuracy
                + "\nTimeStamp: " + Input.location.lastData.timestamp);

                textLocation.text = ("Latitud: " + Input.location.lastData.latitude
                + "\nLongitud: " + Input.location.lastData.longitude);


                gameObject.GetComponent<Map>().lat = Input.location.lastData.latitude;
                gameObject.GetComponent<Map>().lon = Input.location.lastData.longitude;
            }

        }
      
    }

   
}
