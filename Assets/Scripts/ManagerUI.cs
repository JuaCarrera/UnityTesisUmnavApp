using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class ManagerUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup[] panels;
    [SerializeField]
    private TMP_InputField _InputFieldUser;
    [SerializeField]
    private TMP_InputField _InputFieldPass;
    [SerializeField]
    private TextMeshProUGUI _TxtDebug;
    [SerializeField]
    private CanvasGroup[] panelsHUD;

    [SerializeField]
    private ARSession session;
    [SerializeField]
    private ARSessionOrigin sessionOrigin;
    [SerializeField]
    private ARCameraManager cameraManager;

    public Transform plane;
    public Transform user;
    public Map map;


    private void Awake()
    {
        session.enabled = false;
        sessionOrigin.enabled = false;
        cameraManager.enabled = false;
    }

    void Start()
    {
        _TxtDebug.text = "";
        StartCoroutine(TimeViewSplash());
    }

    public void ViewPanel(int id)
    {
        foreach (CanvasGroup panel in panels)
        {
            panel.alpha = 0;
            panel.interactable = false;
            panel.blocksRaycasts = false;
        }

        panels[id].alpha = 1;
        panels[id].interactable = true;
        panels[id].blocksRaycasts = true;

        if(id == 2)
        {
            ViewMap();
        }
    }

    public void OpenIndoorScene()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator TimeViewSplash()
    {
        ViewPanel(0);
        yield return new WaitForSeconds(3.5F);
        ViewPanel(1);
    }

    public void Login()
    {
        if (string.IsNullOrEmpty(_InputFieldUser.text) || string.IsNullOrEmpty(_InputFieldPass.text))
        {
            _TxtDebug.text = "Llena los campos vacios";
        }
        else
        {
            if (_InputFieldUser.text == "sebas" && _InputFieldPass.text == "123")
            {
                _TxtDebug.text = "Ingreso exitoso ";
                ViewPanel(2);
            }
            else
            {
                _TxtDebug.text = "Los datos son invalidos";
            }
        }
    }

    public void ViewAR()
    {
        panelsHUD[0].alpha = 0;
        panelsHUD[0].interactable = false;
        panelsHUD[0].blocksRaycasts = false;
        panelsHUD[1].alpha = 1;
        panelsHUD[1].interactable = true;
        panelsHUD[1].blocksRaycasts = true;

        session.enabled = true;
        sessionOrigin.enabled = true;
        cameraManager.enabled = true;
        map.isARscale = true;
    }

    public void ViewMap()
    {
        panelsHUD[1].alpha = 0;
        panelsHUD[1].interactable = false;
        panelsHUD[1].blocksRaycasts = false;
        panelsHUD[0].alpha = 1;
        panelsHUD[0].interactable = true;
        panelsHUD[0].blocksRaycasts = true;

        session.enabled = false;
        sessionOrigin.enabled = false;
        cameraManager.enabled = false;
        map.isARscale = false;
    }
}
