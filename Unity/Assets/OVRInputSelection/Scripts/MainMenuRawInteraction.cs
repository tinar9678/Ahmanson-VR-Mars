/************************************************************************************

Copyright   :   Copyright 2017-Present Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.2 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.2

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;
using Opertoon.Stepwise;
using UnityEngine.UI;
using System.Collections;

public class MainMenuRawInteraction : MonoBehaviour
{
    protected Material oldHoverMat;
    public Material yellowMat;
    protected Material oldHoverMatOuter;
    protected Material oldHoverMatInner;
    //protected Material oldHoverMatControlCenter;
    protected Material oldHoverMatRocket;
    protected Material oldHoverMatSatellite;
    protected Material oldHoverMatSolarPanel;
    protected Material oldHoverMatGasTank;
    protected Material oldHoverRover;

    public Material outlineMaterial;
    public Material backIdle;
    public Material backActive;
    public UnityEngine.UI.Text outText;

    public bool hovering;

    //public GameObject controlCenterPanel;

    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private bool _mainMenuActive;
    [SerializeField] private Image _scene1;
    [SerializeField] private Image _scene2;
    
    public string selectedTag;

    private Conductor _roverConductor;
    private Conductor _gasTanksConductor;
    private Conductor _agroPodConductor;
    private Conductor _rocketConductor;
    private Conductor _satelliteConductor;
    private Conductor _solarPanelConductor;
    private bool panelActive;

    public GameObject rightHand;
    //private bool triggerPressed;
    bool bDownRight;

    private string _prevTag;

    void Start()
    {
        _prevTag = "";

        bDownRight = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        panelActive = false;
        hovering = false;
        _mainMenuActive = false;

    }

    private IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(.5f);
        if (selectedTag == "agroPod")
        {
            if (_agroPodConductor != null)
            {
                _agroPodConductor.NextStep();
            }
        }

    }

    public void Update()
    {

        if(OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("A pressed!!");
            _mainMenuCanvas.gameObject.SetActive(!_mainMenuActive);
            _mainMenuActive = !_mainMenuActive;
        }
    }

    public void OnHoverEnter(Transform t)
    {
        if (t.gameObject.name == "BackButton")
        {
            t.gameObject.GetComponent<Renderer>().material = backActive;
        }
        else
        {
            hovering = true;
            //set selectedTag to whatever the tag of selected GameObject is
            selectedTag = t.gameObject.tag;
            Debug.Log("hovered tag is: " + selectedTag);

            //oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
            //t.gameObject.GetComponent<Renderer>().material = yellowMat;
            if(t.gameObject.tag == "agroPod")
            {
                
                GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material = outlineMaterial;
                GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material = outlineMaterial;
            }

            if (t.gameObject.tag == "rocket")
            {
                GameObject.Find("RocketTop").GetComponent<Renderer>().material = outlineMaterial;       
            }

            if (t.gameObject.tag == "Rover")
            {
                GameObject.Find("rover").GetComponent<Renderer>().material = outlineMaterial;
            }

            if (t.gameObject.tag == "satellite")
            {
                GameObject.Find("Satelite_plate").GetComponent<Renderer>().material = outlineMaterial;
            }

            if (t.gameObject.tag == "solarPanel")
            {
                foreach(GameObject solarPanel in GameObject.FindGameObjectsWithTag("solarPanel"))
                {
                    if(solarPanel.name == "Solar_panel_panel")
                    {
                        solarPanel.GetComponent<Renderer>().material = outlineMaterial;
                    }
                } 
            }
           
            if (t.gameObject.tag == "GasTank")
            {
                foreach (GameObject gasTank in GameObject.FindGameObjectsWithTag("GasTank"))
                {
                    if (gasTank.name == "Sphere_cell")
                    {
                        gasTank.GetComponent<Renderer>().material = outlineMaterial;
                    }
                }
            }

            /* if(t.gameObject.tag == "controlCenter")
             {
                 GameObject.Find("Control_Center_Mat").GetComponent<Renderer>().material = outlineMaterial;
             }*/

            if (t.gameObject.tag == "Scene1")
            {
                _scene1.GetComponentInChildren<Image>().color = Color.yellow;
            }
            
            if(t.gameObject.tag == "Scene2")
            {
                _scene2.GetComponentInChildren<Image>().color = Color.yellow;
            }
            
            //set hovering bool = true;

        }
        if (outText != null)
        {
            outText.text = "<b>Last Interaction:</b>\nHover Enter:" + t.gameObject.name;
        }
    }

    public void OnHoverExit(Transform t)
    {
        if(t.gameObject.name == "exitButton")
        {

        }

        if (t.gameObject.name == "BackButton")
        {
            t.gameObject.GetComponent<Renderer>().material = backIdle;
        }
        else
        {

            //t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
            GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material = oldHoverMatInner;
            GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material = oldHoverMatOuter;

            // GameObject.Find("Control_Center_Mat").GetComponent<Renderer>().material = oldHoverMatControlCenter;
            GameObject.Find("RocketTop").GetComponent<Renderer>().material = oldHoverMatRocket;
            GameObject.Find("Satelite_plate").GetComponent<Renderer>().material = oldHoverMatSatellite;
            //GameObject.Find("Solar_panel_panel").GetComponent<Renderer>().material = oldHoverMatSolarPanel;
            foreach (GameObject solarPanel in GameObject.FindGameObjectsWithTag("solarPanel"))
            {
                if (solarPanel.name == "Solar_panel_panel")
                {
                    solarPanel.GetComponent<Renderer>().material = oldHoverMatSolarPanel;
                }
            }

            foreach (GameObject gasTank in GameObject.FindGameObjectsWithTag("GasTank"))
            {
                if (gasTank.name == "Sphere_cell")
                {
                    gasTank.GetComponent<Renderer>().material = oldHoverMatGasTank;
                }
            }
            
            GameObject.Find("rover").GetComponent<Renderer>().material = oldHoverRover;

            _scene2.GetComponentInChildren<Image>().color = Color.clear;
            _scene1.GetComponentInChildren<Image>().color = Color.clear;

            //set hovering bool = false;
            hovering = false;
        }
        if (outText != null)
        {
            outText.text = "<b>Last Interaction:</b>\nHover Exit:" + t.gameObject.name;
        }
    }

    public void OnSelected(Transform t)
    {

        Debug.Log("right trigger pressed");

        Debug.Log("selected tag is: " + selectedTag);

        if (selectedTag == "Scene1")
        {
            Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name != "DemoMarsScene")
            {
                Debug.Log("Load scene 1!");
                SceneManager.LoadScene("DemoMarsScene");
            }

        }
        else if (selectedTag == "Scene2")
        {
            Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name != "Scene2")
            {
                Debug.Log("Load scene 2!");
                SceneManager.LoadScene("Scene2");
            }

            //TODO: Add warning to show player is pressing on current scene!
        }

        if (t.gameObject.name == "ExitButton")
        {
            if (t.parent.gameObject.tag == "MainMenu")
            {
                _mainMenuActive = !_mainMenuActive;
            }

            t.parent.gameObject.SetActive(false);
        }


        if (t.gameObject.name == "BackButton")
        {
            SceneManager.LoadScene("main", LoadSceneMode.Single);
        }

        Debug.Log("Clicked on " + t.gameObject.name);
        if (outText != null)
        {
            outText.text = "<b>Last Interaction:</b>\nClicked On:" + t.gameObject.name;
        }
    }

}