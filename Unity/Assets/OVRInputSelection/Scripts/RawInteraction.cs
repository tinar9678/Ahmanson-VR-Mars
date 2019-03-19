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
    public Material outlineMaterial;
    public Material backIdle;
    public Material backACtive;
    public UnityEngine.UI.Text outText;

    public GameObject cube;
    public bool hovering;
    public GameObject stepwise_agroPod;

    public GameObject agroPodPanel;
    public GameObject astronautPanel;
    public GameObject controlCenterPanel;
    public string selectedTag;

    private Conductor _conductor;
    private bool panelActive;

    public GameObject rightHand;
    //private bool triggerPressed;
    bool bDownRight;

    public Camera auxCamera;
    private float speed;
    private float x;
    private float y;

    void Start()
    {
       

        oldHoverMatOuter = GameObject.Find("Agro_block_outside002").GetComponent<Renderer>().material;
        oldHoverMatInner = GameObject.Find("Agro_propilen002").GetComponent<Renderer>().material;

        _conductor = stepwise_agroPod.GetComponent<Conductor>();
        bDownRight = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        panelActive = false;
        hovering = false;
        speed = -10f;
        x = 17;
        y = 0;

        _conductor.OnScorePrepared += HandleScorePrepared;

    }

    private IEnumerator AutoStart()
    {
        yield return new WaitForSeconds(.5f);
        if (_conductor != null)
        {
            _conductor.NextStep();
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
        Debug.Log("next step called");
        _conductor.NextStep();
     
    }

    private void HandleScorePrepared(Score score)
    {
        Debug.Log("score prepared");
        StartCoroutine(AutoStart());
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
            //activate the stepwise panel for selected object
            if (selectedTag == "agroPod")
            {
                panelActive = true;
                stepwise_agroPod.SetActive(true);
                agroPodPanel.SetActive(true);

                
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
            else if (selectedTag == "controlPod")
            {
                panelActive = true;

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