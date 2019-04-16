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

public class RawInteraction : MonoBehaviour
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
    public Material backACtive;
    public UnityEngine.UI.Text outText;



    //public GameObject cube;
    public bool hovering;
    public GameObject stepwiseAgroPod;
    public GameObject stepwiseRocket;
    public GameObject stepwiseSatellite;
    public GameObject stepwiseSolarPanel;
    public GameObject stepwiseRover;
    public GameObject stepwiseGasTanks;

    public GameObject agroPodPanel;
    public GameObject astronautPanel;
    public GameObject rocketPanel;
    public GameObject satellitePanel;
    public GameObject solarPanel_Panel;
    public GameObject gasTanks_Panel;
    public GameObject rover_Panel;

    //public GameObject controlCenterPanel;

    [SerializeField] private Canvas _mainMenuCanvas;
    [SerializeField] private bool _mainMenuActive;
    [SerializeField] private Image _scene1;
    [SerializeField] private Image _scene2;
    [SerializeField] private Image _credits;

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
    

    public Camera auxCamera;
    private float speed;
    private float x;
    private float y;

    private GameObject _prevPanel;
    private GameObject _prevStepwise;
    private string _prevTag;

    Sprite sceneA_Hilite;
    Sprite sceneB_Hilite;
    Sprite credits_Hilite;

    Sprite sceneA_original;
    Sprite sceneB_original;
    Sprite credits_original;

    [SerializeField] private GameObject _agroPodArrow;
    [SerializeField] private GameObject _gasTankArrow;
    [SerializeField] private GameObject _roverArrow;
    [SerializeField] private GameObject _rocketArrow;
    [SerializeField] private GameObject _solarPanelArrow;
    [SerializeField] private GameObject _satelliteArrow;

    void Start()
    {
       // stepwiseControlCenter.SetActive(true);
        stepwiseAgroPod.SetActive(true);
        
        _agroPodConductor = stepwiseAgroPod.GetComponent<Conductor>();
        _agroPodConductor.OnScorePrepared += HandleScorePrepared;
        //_controlCenterConductor = stepwiseControlCenter.GetComponent<Conductor>();
        // _controlCenterConductor.OnScorePrepared += HandleScorePrepared;
        _rocketConductor = stepwiseRocket.GetComponent<Conductor>();
        _rocketConductor.OnScorePrepared += HandleScorePrepared;

        _satelliteConductor = stepwiseSatellite.GetComponent<Conductor>();
        _satelliteConductor.OnScorePrepared += HandleScorePrepared;

        _solarPanelConductor = stepwiseSolarPanel.GetComponent<Conductor>();
        _solarPanelConductor.OnScorePrepared += HandleScorePrepared;

        _roverConductor = stepwiseRover.GetComponent<Conductor>();
        _roverConductor.OnScorePrepared += HandleScorePrepared;

        _gasTanksConductor = stepwiseGasTanks.GetComponent<Conductor>();
        _gasTanksConductor.OnScorePrepared += HandleScorePrepared;

        _prevTag = "";

        oldHoverMatOuter = GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material;
        oldHoverMatInner = GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material;

        //oldHoverMatControlCenter = GameObject.Find("Control_Center_Mat").GetComponent<Renderer>().material;
        oldHoverMatRocket = GameObject.Find("RocketTop").GetComponent<Renderer>().material;
        oldHoverMatSatellite = GameObject.Find("Satelite_plate").GetComponent<Renderer>().material;
        oldHoverMatSolarPanel = GameObject.Find("Solar_panel_panel").GetComponent<Renderer>().material;
        oldHoverMatGasTank = GameObject.Find("Sphere_cell").GetComponent<Renderer>().material;
        oldHoverRover = GameObject.Find("rover").GetComponent<Renderer>().material;

        sceneA_original = Resources.Load<Sprite>("scene-a");
        sceneB_original = Resources.Load<Sprite>("scene-b");
        credits_original = Resources.Load<Sprite>("scene-c");

        sceneA_Hilite = Resources.Load<Sprite>("scene-a-hilite");
        sceneB_Hilite = Resources.Load<Sprite>("scene-b-hilite");
        credits_Hilite = Resources.Load<Sprite>("scene-c-hilite");

        bDownRight = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        panelActive = false;
        hovering = false;
        speed = -2f;
        x = 17;
        y = 0;
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

        /*else if(selectedTag == "controlCenter")
        {
            if (_controlCenterConductor != null)
            {
                _controlCenterConductor.NextStep();
            }
        }*/
    }

    public void Update()
    {
        y += speed * Time.deltaTime;
       //auxCamera.transform.Rotate(0, speed * Time.deltaTime, 0);
       auxCamera.transform.rotation = Quaternion.Euler(x, y, 0);

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
            t.gameObject.GetComponent<Renderer>().material = backACtive;
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
                _scene1.transform.GetChild(0).GetComponent<Image>().sprite = sceneA_Hilite;
                
            }
            
            if(t.gameObject.tag == "Scene2")
            {
                _scene2.GetComponentInChildren<Image>().color = Color.yellow;
                _scene2.transform.GetChild(0).GetComponent<Image>().sprite = sceneB_Hilite;
            }

            if (t.gameObject.tag == "Credits")
            {
                _credits.GetComponentInChildren<Image>().color = Color.yellow;
                _credits.transform.GetChild(0).GetComponent<Image>().sprite = credits_Hilite;
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

            _scene1.transform.GetChild(0).GetComponent<Image>().sprite = sceneA_original;
            _scene2.transform.GetChild(0).GetComponent<Image>().sprite = sceneB_original;
            _credits.transform.GetChild(0).GetComponent<Image>().sprite = credits_original;

            //set hovering bool = false;
            hovering = false;
        }
        if (outText != null)
        {
            outText.text = "<b>Last Interaction:</b>\nHover Exit:" + t.gameObject.name;
        }
    }

    private void HandleScorePrepared(Score score)
    {
        Debug.Log("score prepared:" + score);
        //StartCoroutine(AutoStart());
    }

    private void DeactivatePanel(string selectedTag)
    {
       
        if (panelActive && (!_prevTag.Equals(selectedTag) || _prevTag.Equals("")))
        {
            Debug.Log("deactivate panel called, selected tag: " + selectedTag + "prev tag: " + _prevTag);
            if(_prevPanel != null) 
                _prevPanel.SetActive(false);
            //_prevStepwise.SetActive(false);
            _prevTag = selectedTag;
        }
    }

    private IEnumerator DelayedResetAndNextStep()
    {
        if (selectedTag == "agroPod")
        {
            Debug.Log("DelayedResetAndNextStep: Agropod");
            yield return 0;
            _agroPodConductor.Reset();
            yield return 0;
            _agroPodConductor.NextStep();
        } else if (selectedTag == "rocket")
        {
            Debug.Log("DelayedResetAndNextStep: Rocket");
            yield return 0;
            _rocketConductor.Reset();
            yield return 0;
            _rocketConductor.NextStep();
        } else if (selectedTag == "satellite")
        {
            Debug.Log("DelayedResetAndNextStep: Satellite");
            yield return 0;
            _satelliteConductor.Reset();
            yield return 0;
            _satelliteConductor.NextStep();
        } else if (selectedTag == "solarPanel")
        {
            Debug.Log("DelayedResetAndNextStep: SolarPanel");
            yield return 0;
            _solarPanelConductor.Reset();
            yield return 0;
            _solarPanelConductor.NextStep();
        }
        else if (selectedTag == "GasTank")
        {
            Debug.Log("DelayedResetAndNextStep: Gas Tank");
            yield return 0;
            _gasTanksConductor.Reset();
            yield return 0;
            _gasTanksConductor.NextStep();
        }
        else if (selectedTag == "Rover")
        {
            Debug.Log("DelayedResetAndNextStep: Rover");
            yield return 0;
            _roverConductor.Reset();
            yield return 0;
            _roverConductor.NextStep();
        }
    }

    private IEnumerator DelayedReset()
    {
   
        if (selectedTag == "agroPod")
        {
            yield return 0;
            _agroPodConductor.Reset();
          
        }
        /*else if (selectedTag == "controlCenter")
        {
            yield return 0;
            _controlCenterConductor.Reset();
          
        }*/
    }

    private IEnumerator DelayedNextStep()
    {

        if (selectedTag == "agroPod")
        {
            yield return 0;
            _agroPodConductor.NextStep();

        }
       /* else if (selectedTag == "controlCenter")
        {
            yield return 0;
            _controlCenterConductor.NextStep();

        }*/
    }

    public void OnSelected(Transform t)
    {

        Debug.Log("right trigger pressed");

        Debug.Log("selected tag is: " + selectedTag);
        /*
        if (panelActive)
        {
            _conductor.NextStep();
        }
        */
        //if hovering == true


        if (hovering == true)
        {
            if(selectedTag == "exitButton")
            {

            }
            //activate the stepwise panel for selected object
            if (selectedTag == "agroPod")
            {
                //stepwiseAgroPod.SetActive(true);
                //_conductor = stepwiseAgroPod.GetComponent<Conductor>();

                //Pseudocode
                /*if (panel isn’t active) {
                    make panel active
                    DelayedReset
                }
                DelayedNextStep
                */
                panelActive = true;
                if (!agroPodPanel.activeInHierarchy)
                {
                    Debug.Log("Not active");
                    DeactivatePanel(selectedTag);
                    agroPodPanel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("Agro pod panel already active: next step");
                    _agroPodConductor.NextStep();
                }


                _prevPanel = agroPodPanel;
                _prevStepwise = stepwiseAgroPod;
                _agroPodArrow.SetActive(false);

                //child the panel to the right controller
                //agroPodPanel.transform.SetParent(rightHand.transform);

            }
            else if (selectedTag == "rocket")
            {
                panelActive = true;
                if (!rocketPanel.activeInHierarchy)
                {
                    Debug.Log("Rocket not active");
                    DeactivatePanel(selectedTag);
                    rocketPanel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("rocket panel already active: next step");
                    _rocketConductor.NextStep();
                }

                _prevPanel = rocketPanel;
                _prevStepwise = stepwiseRocket;
                //_controlCenterArrow.SetActive(false);
                _rocketArrow.SetActive(false);
            }
            else if (selectedTag == "satellite")
            {
                panelActive = true;
                if (!satellitePanel.activeInHierarchy)
                {
                    Debug.Log("Satellite not active");
                    DeactivatePanel(selectedTag);
                    satellitePanel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("satellite panel already active: next step");
                    _satelliteConductor.NextStep();
                }

                _prevPanel = satellitePanel;
                _prevStepwise = stepwiseSatellite;
                //_controlCenterArrow.SetActive(false);
                _satelliteArrow.SetActive(false);
            }
            else if (selectedTag == "solarPanel")
            {
                panelActive = true;
                if (!solarPanel_Panel.activeInHierarchy)
                {
                    Debug.Log("SolarPanel not active");
                    DeactivatePanel(selectedTag);
                    solarPanel_Panel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                }
                else
                {
                    Debug.Log("solarPanel panel already active: next step");
                    _solarPanelConductor.NextStep();
                }

                _prevPanel = solarPanel_Panel;
                _prevStepwise = stepwiseSolarPanel;
                _solarPanelArrow.SetActive(false);
            }
            else if (selectedTag == "astronaut")
            {
                panelActive = true;
                //cube.SetActive(true);
                //activate the astronaut stepwise panel
                //stepwise.SetActive(true);
                //astronautPanel.SetActive(true);
                //astronautPanel.transform.SetParent(rightHand.transform);
            }
            else if (selectedTag == "Rover")
            {
                //stepwiseControlCenter.SetActive(true)
                panelActive = true;
                if (!rover_Panel.activeInHierarchy)
                {
                    DeactivatePanel(selectedTag);
                    rover_Panel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                    //StartCoroutine(DelayedNextStep());
                   // _controlCenterConductor.NextStep();
                } else
                {
;                    _roverConductor.NextStep();
                }
                
                
                Debug.Log("control center panel being activated");
                
                _prevPanel = rover_Panel;
                _prevStepwise = stepwiseRover;
                _roverArrow.SetActive(false);
            }
            else if (selectedTag == "GasTank")
            {
                panelActive = true;
                if (!gasTanks_Panel.activeInHierarchy)
                {
                    DeactivatePanel(selectedTag);
                    gasTanks_Panel.SetActive(true);
                    StartCoroutine(DelayedResetAndNextStep());
                    //StartCoroutine(DelayedNextStep());
                    // _controlCenterConductor.NextStep();
                }
                else
                {
                    _gasTanksConductor.NextStep();
                }


                Debug.Log("control center panel being activated");

                _prevPanel = gasTanks_Panel;
                _prevStepwise = stepwiseGasTanks;
                _gasTankArrow.SetActive(false);
            }
            else if (selectedTag == "Scene1")
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
                t.gameObject.GetComponentInChildren<Image>().sprite = Resources.Load("scene-b-hilite") as Sprite;
                Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
                if (SceneManager.GetActiveScene().name != "Scene2")
                {
                    Debug.Log("Load scene 2!");
                    SceneManager.LoadScene("Scene2");
                }
                //TODO: Add warning to show player is pressing on current scene!
            } else if (selectedTag == "Credits")
            {
                Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
                if (SceneManager.GetActiveScene().name != "Credits")
                {
                    Debug.Log("Load credits!");
                    SceneManager.LoadScene("CreditsScene");
                }

            }


            if(t.gameObject.name == "ExitButton")
            {
                if(t.parent.gameObject.tag == "MainMenu")
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
}