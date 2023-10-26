using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using UnityEngine.AI;
using Unity.AI.Navigation;
using TMPro;
using UnityEngine.XR.ARFoundation;
using static UnityEngine.Rendering.DebugUI;

public class Map : MonoBehaviour
{
    public string apiKey;
    public double lat= 1.2294524d;
    public double lon = -77.2830333d;
    public int zoom = 14;

    public enum resolution { low=1,high=2};
    public resolution mapResolution=resolution.low;

    public enum type { roadmap,satellite,gybrid,terrain};
    public type mapType=type.roadmap;
    private string url = "";
    private int mapWidth = 720;
    private int mapHeight = 720;
    private bool mapIsLoading = false;
    private Rect rect;

    private string apiKeyLast;
    private double latLast = 1.2294524d;
    private double lonLast = -77.2830333d;
    private int zoomLast = 14;
    private resolution mapResolutionLast = resolution.low;
    private type mapTypeLast = type.roadmap;
    private bool updateMap = true;

    public GameObject buildingsPrefab;
    public double latBuildings;
    public double lonBuildings;

    public NavMeshSurface surface;
    public TMP_Dropdown dropdownPlaces;

    public bool isARscale;

    [Serializable]
    public class Building
    {
        public double latitude;
        public double longitude;
        public GameObject pointPrefab;
    }

    public double scaleFactor = 100d;

    public LineRenderer lineRenderer;
    private NavMeshPath path;

    private GameObject currentPoint;
    private GameObject currentBuildingInstance;

    int countRefreshMap;
    int countNewBuildings;
    int countNewPoint;
    int countRefreshPath;

    public TextMeshProUGUI subtitleText;
    public List<Building> points = new List<Building>();
    public Scrollbar heightScrollbar;

    public float minHeight = -10f;
    public float maxHeight = 10f;

    void Start()
    {
        path = new NavMeshPath();

        rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Math.Round(rect.width);
        mapHeight = (int)Math.Round(rect.height);
    }

    void Update()
    {
        if(updateMap && (apiKeyLast!=apiKey ||!Mathf.Approximately((float)latLast,(float)lat) || !Mathf.Approximately((float)lonLast, (float)lon) 
            || zoomLast != zoom || mapResolutionLast!=mapResolution || mapTypeLast!=mapType))
        {
            countRefreshMap++;

            Debug.Log("Actualizando mapa # " + countRefreshMap);

            StartCoroutine(GetGoogleMap());
            rect = gameObject.GetComponent<RawImage>().rectTransform.rect;
            mapWidth = (int)Math.Round(rect.width);
            mapHeight = (int)Math.Round(rect.height);
            updateMap = false;
        }
    }

    private void ChangeSubtitleText(int a)
    {
        if(a == 0)
        {
            subtitleText.text = "Selecciona el lugar al cual desear ir:";
        }
        else
        {
            subtitleText.text = "Vas dirigiendote a:";
        }
    }

    IEnumerator GetGoogleMap() 
    {
        url = "https://maps.googleapis.com/maps/api/staticmap?center="+lat+","+lon+"&zoom="+zoom+"&size="+mapWidth
            +"x"+mapHeight+"&scale="+mapResolution+"&maptype="+mapType+ "&markers=color:red%7Clabel:E%7C6.154257,%20-75.610292" + "&style=feature:all|element:labels|visibility:off" + "&key="+apiKey;
        //Debug.Log(url);
        mapIsLoading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
        }
        else 
        {
            mapIsLoading = false;
            gameObject.GetComponent<RawImage>().texture=((DownloadHandlerTexture)www.downloadHandler).texture;

            apiKeyLast = apiKey;
            latLast = lat;
            lonLast = lon;
            zoomLast = zoom;
            mapResolutionLast = mapResolution;
            mapTypeLast = mapType;
            updateMap = true;

            PlaceBuildings(latLast, lonLast);
        }        
    }

    public void SimplePlaceBuildings()
    {
        PlaceBuildings(latLast, lonLast);
    }

    public void PlaceBuildings(double lat, double lon)
    {
        countNewBuildings++;

        Debug.Log("Instanciando edificio # " + countNewBuildings);

        if (currentBuildingInstance != null)
        {
            Destroy(currentBuildingInstance);
        }

        double differenceLat = latBuildings - lat;
        double differenceLon = lonBuildings - lon;

        double positionX = differenceLon * scaleFactor;
        double positionZ = differenceLat * scaleFactor;

        currentBuildingInstance = Instantiate(buildingsPrefab, new Vector3((float)positionX, 0, (float)positionZ), buildingsPrefab.transform.localRotation);
        currentBuildingInstance.name = "Edificio " + countNewBuildings;

        //if(isARscale)
        //{
        //    currentBuildingInstance.transform.localScale = new Vector3(44 * 20, 22 * 20, 44 * 20);
        //}
        //else
        //{
        //    currentBuildingInstance.transform.localScale = new Vector3(44, 22, 44);
        //}

        StartCoroutine(UpdateNavMesh());
    }

    IEnumerator UpdateNavMesh()
    {
        yield return new WaitForSeconds(0.25f);
        surface.BuildNavMesh();
        Debug.Log("Se actualizó el navmesh");

    }

    public void SelectPlaceWithDropdown(int selectedValue)
    {
        lineRenderer.enabled = true;
        PlacePoint(selectedValue, latLast, lonLast);
        ChangeSubtitleText(1);


        if (currentPoint != null)
        {
            InvokeRepeating("CalculateAndShowPathToBuilding", 0.25f, 0.25f);
        }
    }

    public void StopNavigation()
    {
        ChangeSubtitleText(0);

        if (currentPoint != null)
        {
            Destroy(currentPoint);
        }

        StopAllCoroutines();
        lineRenderer.enabled = false;
    }

    public void PlacePoint(int id, double lat, double lon)
    {
        if (id == 0) return;

        countNewPoint++;

        Debug.Log("Instanciando punto # " + countNewPoint);

        if (currentPoint != null)
        {
            Destroy(currentPoint);
        }

        double differenceLat = points[id-1].latitude - lat;
        double differenceLon = points[id-1].longitude - lon;

        double positionX = differenceLon * scaleFactor;
        double positionZ = differenceLat * scaleFactor;

        currentPoint = Instantiate(points[id - 1].pointPrefab, new Vector3((float)positionX, 0, 
            (float)positionZ), points[id-1].pointPrefab.transform.localRotation);

        if (isARscale)
        {
            currentPoint.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else
        {
            currentPoint.transform.localScale = new Vector3(2, 2, 2);
        }
    }     


    public void CalculateAndShowPathToBuilding()
    {
        if(currentPoint != null)
        {
            countRefreshPath++;

            Debug.Log("Actualizando ruta");
            //Debug.Log("Actualizando ruta # " + countRefreshPath);
            NavMesh.CalculatePath(lineRenderer.transform.position, currentPoint.transform.position, NavMesh.AllAreas, path);
            lineRenderer.positionCount = path.corners.Length;
            lineRenderer.SetPositions(path.corners);

            if (isARscale)
            {
                lineRenderer.SetWidth(0.1f, 0.1f);

                // Convert the scrollbar value (0 to 1) to a height value (minHeight to maxHeight)
                float currentHeight = Mathf.Lerp(minHeight, maxHeight, heightScrollbar.value);

                // Assuming the LineRenderer has two points: start and end.
                Vector3 startPosition = lineRenderer.GetPosition(0);
                Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + currentHeight, startPosition.z);

                // Obtener el número de puntos del LineRenderer
                int pointCount = lineRenderer.positionCount;

                // Crear un array para almacenar las posiciones
                Vector3[] positions = new Vector3[pointCount];

                // Llenar el array con las posiciones del LineRenderer
                lineRenderer.GetPositions(positions);

                for(int i = 0; i < pointCount; i++)
                {
                    positions[i].y = endPosition.y;

                    lineRenderer.SetPosition(i, positions[i]);
                }         
            }
            else
            {
                lineRenderer.SetWidth(1f, 1f);

            }
        }
    }

}
