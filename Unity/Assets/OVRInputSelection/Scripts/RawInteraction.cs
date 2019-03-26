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
    protected Material oldHoverMatControlCenter;

    public Material outlineMaterial;
    public Material backIdle;
    public Material backACtive;
    public UnityEngine.UI.Text outText;



    public GameObject cube;
    public bool hovering;
    public GameObject stepwiseAgroPod;
    public GameObject stepwiseControlCenter;

    public GameObject agroPodPanel;
    public GameObject astronautPanel;
    public GameObject controlCenterPanel;
    public string selectedTag;

    private Conductor _controlCenterConductor;
    private Conductor _agroPodConductor;
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

    void Start()
    {
        stepwiseControlCenter.SetActive(true);
        stepwiseAgroPod.SetActive(true);

        _agroPodConductor = stepwiseAgroPod.GetComponent<Conductor>();
        _agroPodConductor.OnScorePrepared += HandleScorePrepared; 
        _controlCenterConductor = stepwiseControlCenter.GetComponent<Conductor>();
        _controlCenterConductor.OnScorePrepared += HandleScorePrepared;

        _prevTag = "";

        oldHoverMatOuter = GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material;
        oldHoverMatInner = GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material;

        oldHoverMatControlCenter = GameObject.Find("Control_Center_Mat").GetComponent<Renderer>().material;

        bDownRight = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        panelActive = false;
        hovering = false;
        speed = -10f;
        x = 17;
        y = 0;

    }

    private IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(.5f);
        if(selectedTag == "agroPod")
        {
            if (_agroPodConductor != null)
            {
                _agroPodConductor.NextStep();
            }
        } else if(selectedTag == "controlCenter")
        {
            if (_controlCenterConductor != null)
            {
                _controlCenterConductor.NextStep();
            }
        }
    }

    public void Update()
    {
        y += speed * Time.deltaTime;
       //auxCamera.transform.Rotate(0, speed * Time.deltaTime, 0);
       auxCamera.transform.rotation = Quaternion.Euler(x, y, 0);


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
            Debug.Log("selected tag is: " + selectedTag);

            //oldHoverMat = t.gameObject.GetComponent<Renderer>().material;
            //t.gameObject.GetComponent<Renderer>().material = yellowMat;
            if(t.gameObject.tag == "agroPod")
            {
                
                GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material = outlineMaterial;
                GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material = outlineMaterial;
            }

            if(t.gameObject.tag == "controlCenter")
            {
                GameObject.Find("Control_Center_Mat").GetComponent<Renderer>().material = outlineMaterial;
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
        if (t.gameObject.name == "BackButton")
        {
            t.gameObject.GetComponent<Renderer>().material = backIdle;
        }
        else
        {

            //t.gameObject.GetComponent<Renderer>().material = oldHoverMat;
            GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material = oldHoverMatInner;
            GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material = oldHoverMatOuter;

            GameObject.Find("Control_Center_Mat").GetComponent<Renderer>().material = oldHoverMatControlCenter;

            //set hovering bool = false;
            hovering = false;
        }
        if (outText != null)
        {
            outText.text = "<b>Last Interaction:</b>\nHover Exit:" + t.gameObject.name;
        }
    }

    public void NextStep()
    {
        if (selectedTag == "agroPod")
        {
            if (_agroPodConductor != null)
            {
                _agroPodConductor.NextStep();
            }
        }
        else if (selectedTag == "controlCenter")
        {
            if (_controlCenterConductor != null)
            {
                _controlCenterConductor.NextStep();
            }
        }
        Debug.Log("next step called");
     
    }

    private void HandleScorePrepared(Score score)
    {
        Debug.Log("score prepared");
        StartCoroutine(AutoStart());
    }

    private void DeactivatePanel(string selectedTag)
    {
        Debug.Log("deactivate panel called, selected tag: " +selectedTag + "prev tag: " + _prevTag);
        if (panelActive && !_prevTag.Equals(selectedTag))
        {
            
            _prevPanel.SetActive(false);
            _prevStepwise.SetActive(false);
            _prevTag = selectedTag;
        }
    }

    public void OnSelected(Transform t)
    {

        Debug.Log("right trigger pressed");
        /*
        if (panelActive)
        {
            _conductor.NextStep();
        }
        */
        //if hovering == true
       

        if (hovering == true)
        {
            DeactivatePanel(selectedTag);
            //activate the stepwise panel for selected object
            if (selectedTag == "agroPod")
            {
                //stepwiseAgroPod.SetActive(true);
                //_conductor = stepwiseAgroPod.GetComponent<Conductor>();
                
                
                if (!agroPodPanel.activeInHierarchy)
                {
                    _agroPodConductor.Reset();
                   // _agroPodConductor.NextStep();
                }

                panelActive = true;
                Debug.Log("agro pod panel being activated");

                agroPodPanel.SetActive(true);
                _prevPanel = agroPodPanel;
                _prevStepwise = stepwiseAgroPod;

                //child the panel to the right controller
                //agroPodPanel.transform.SetParent(rightHand.transform);

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
            else if (selectedTag == "controlCenter")
            {
                //stepwiseControlCenter.SetActive(true)

                if (!controlCenterPanel.activeInHierarchy)
                {
                    _controlCenterConductor.Reset();
                   // _controlCenterConductor.NextStep();
                }

                panelActive = true;
                Debug.Log("control center panel being activated");
                controlCenterPanel.SetActive(true);
                _prevPanel = controlCenterPanel;
                _prevStepwise = stepwiseControlCenter;
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