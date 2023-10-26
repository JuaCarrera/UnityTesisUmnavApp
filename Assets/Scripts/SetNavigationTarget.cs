using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetNavigationTarget : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown navigationTargetDropDown;
    [SerializeField] private TextMeshProUGUI sliderValueText;
    [SerializeField] private List<Target> navigationTargetObjects = new List<Target>();
    [SerializeField] private Slider navigationYOffset;
    [SerializeField] private TextMeshProUGUI floorValueText;
    public NavMeshPath path;
    private LineRenderer line;
    public Vector3 targetPosition = Vector3.zero;
    private bool lineToggle = false;

    private void Start()
    {
        path = new NavMeshPath();
        line = transform.GetComponent<LineRenderer>();
        line.enabled = lineToggle;
        //ChangeActiveFloor(Modules.floorNumber);

    }
    private void Update()
    {
        if (lineToggle && targetPosition != Vector3.zero)
        {
            NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);
            line.positionCount = path.corners.Length;
            Vector3[] calculatePathAndOffset = AddLineOffset();
            line.SetPositions(calculatePathAndOffset);
        }      
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }

    public void SetCurrentNavigationTarget(int selectedValue)
    {
        targetPosition = Vector3.zero;
        string selectedText = navigationTargetDropDown.options[selectedValue].text;
        Debug.Log("selectedText: " + selectedText);
        Target currentTarget = navigationTargetObjects.Find(x => x.Name.Equals(selectedText));

        if (currentTarget != null)
        {
            if (!line.enabled)
            {
                ToogleVisibility();
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            targetPosition = currentTarget.PositionObject.transform.position;

            //Debug.Log("Visibilidad linea: " + lineToggle);
            Debug.Log("Dropdown: " + currentTarget.Name);
        }
    }

    public void ToogleVisibility()
    {
        lineToggle = !lineToggle;
        line.enabled = lineToggle;
    }

    public void ChangeActiveFloor(int floorNumber)
    {
        Modules.floorNumber = floorNumber;
        SetNavigationTargetDropdownOptions(Modules.floorNumber);
        floorValueText.text = "Piso: " + Modules.floorNumber.ToString();
        ToogleVisibility();
    }

    private Vector3[] AddLineOffset()
    {
        if(navigationYOffset.value==0)
        {
            return path.corners;
        }

        Vector3[] calculatedLine = new Vector3[path.corners.Length];

        for(int i = 0; i < path.corners.Length; i++)
        {
            calculatedLine[i] = path.corners[i] + new Vector3 (0,navigationYOffset.value,0);
            
        }

        sliderValueText.text = "Line Height : " + navigationYOffset.value.ToString("f2");

        return calculatedLine;
    }

    private void SetNavigationTargetDropdownOptions(int floorNumber)
    {
        navigationTargetDropDown.ClearOptions();
        navigationTargetDropDown.value = 0;

        if (!line.enabled)
        {
            ToogleVisibility();
        }

        if (floorNumber == 0)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("San Jos� Piso 0"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 001"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 002"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 003"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Centro Infantil"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Hombres Piso 0"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Discapacitados Piso 0"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Mujeres Piso 0"));
        }

        else if (floorNumber == 1)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("San Jos� Piso 1"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 101"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 102"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Auditorio 1"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Auditorio 2"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 103"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Hombres Piso 1"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Discapacitados Piso 1"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Mujeres Piso 1"));
        }

        else if (floorNumber == 3)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("San Jos� Piso 3"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 301"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 302"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 303"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 304"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 305"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 306"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 307"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Hombres Piso 3"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Discapacitados Piso 3"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Mujeres Piso 3"));
        }

        else if (floorNumber == 4)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("San Jos� Piso 4"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 401"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 402"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 403"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 404"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 405"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 406"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 407"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Hombres Piso 4"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Discapacitados Piso 4"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Mujeres Piso 4"));
        }

        else if (floorNumber == 5)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("San Jos� Piso 5"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 501"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 502"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 503"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 504"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 505"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 506"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Sal�n 507"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Hombres Piso 5"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Discapacitados Piso 5"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Ba�o Mujeres Piso 5"));
        }
    }
}