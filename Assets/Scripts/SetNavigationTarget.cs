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
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("San José Piso 0"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 001"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 002"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 003"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Centro Infantil"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Hombres Piso 0"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Discapacitados Piso 0"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Mujeres Piso 0"));
        }

        else if (floorNumber == 1)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("San José Piso 1"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 101"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 102"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Auditorio 1"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Auditorio 2"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 103"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Hombres Piso 1"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Discapacitados Piso 1"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Mujeres Piso 1"));
        }

        else if (floorNumber == 3)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("San José Piso 3"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 301"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 302"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 303"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 304"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 305"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 306"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 307"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Hombres Piso 3"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Discapacitados Piso 3"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Mujeres Piso 3"));
        }

        else if (floorNumber == 4)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("San José Piso 4"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 401"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 402"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 403"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 404"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 405"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 406"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 407"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Hombres Piso 4"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Discapacitados Piso 4"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Mujeres Piso 4"));
        }

        else if (floorNumber == 5)
        {
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("San José Piso 5"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 501"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 502"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 503"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 504"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 505"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 506"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Salón 507"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Hombres Piso 5"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Discapacitados Piso 5"));
            navigationTargetDropDown.options.Add(new TMP_Dropdown.OptionData("Baño Mujeres Piso 5"));
        }
    }
}