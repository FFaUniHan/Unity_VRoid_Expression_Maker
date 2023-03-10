using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

//This class will find VRM model that has been loaded every second.
//It will then inform other scripts through event broadcast
public class VRMFinder : MonoBehaviour
{
    public UnityEvent onVRMFound;
    public SnapCamera snapCamera;
    public SaveExpression saveExpression;
    public SliderCreator sliderCreator;
    public SliderMinMaxUpdate sliderMinMaxUpdate;
    public RuntimeAnimatorController anim;
    private Animator animator;

    public GameObject vrm1;

    public void Awake()
    {
        InvokeRepeating("SearchVRM1", 0f, 1f);
    }

    public void TriggerEvent()
    {
        // Call all functions or variables on the Unity Event list
        onVRMFound.Invoke();
    }

    public void SearchVRM1()
    {
        if (vrm1 == null)
        {
            //Debug.Log("Searching...");

            vrm1 = GameObject.Find("VRM1");
            if (vrm1 != null)
            {
                //Debug.Log("Target Found!");
                TriggerEvent();
                snapCamera.SetCamera(vrm1);
                saveExpression.UpdateVRMMesh(vrm1.gameObject);
                sliderCreator.CreateSlider(vrm1);
                sliderMinMaxUpdate.GetSliderList();
		        animator = vrm1.GetComponent<Animator>();
		        animator.runtimeAnimatorController = anim as RuntimeAnimatorController;

                //Set the arm position to be not T-posed
                Transform t = vrm1.transform.Find("Root").Find("J_Bip_C_Hips").Find("J_Bip_C_Spine").Find("J_Bip_C_Chest").Find("J_Bip_C_UpperChest");
                t.Find("J_Bip_L_Shoulder").GetChild(0).localRotation = Quaternion.Euler(Vector3.forward *  70);
                t.Find("J_Bip_R_Shoulder").GetChild(0).localRotation = Quaternion.Euler(Vector3.forward * -70);
            }
        }
    }
}
