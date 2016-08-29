using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    //Molecule Control
    public float moveSensitivity = 40f;
    public float moveSpeed = 0.05f;
    public float rotationSensitivity = 0.5f;
    public float rotationSpeed = 1f;
    public float transformSensitivity = 0.3f;
    public float transformSpeed = 0.05f;
    public float rotationSpeedMultiplierByDistance = 0.0f;

    //Menu
    public Camera menuEnablingCamera;
    public float menuDistanceFromHand = 0.13f;
    public int menuShowing = 0;

    private GameObject[] menus;

    //Scripts
    private AddAtoms addAtoms;

    //Gameobjects & Transforms
    private GameObject molRoot;
    private Vector3 molRootOffset;
    private Transform mainCamera;
    private Transform menuTransform;
    
    
	void Start ()
	{
        addAtoms = gameObject.GetComponent<AddAtoms>();
        molRoot = GameObject.Find ("molRoot");
        molRootOffset = Vector3.zero;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        menuTransform = transform.FindChild("Menu");

        // Move children of molRoot to position molRoot origin in middle of ligand
        
        GameObject[] hetatms = GameObject.FindGameObjectsWithTag("hetatmbs");

        Vector3 sumOfPos = Vector3.zero;

        for (int i = 0; i < hetatms.Length; i++)
        {
            sumOfPos += hetatms[i].transform.localPosition;
        }

        Vector3 offset = sumOfPos / hetatms.Length;

        foreach (Transform child in molRoot.transform)
        {
            child.localPosition -= offset;
        }

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.5f);
        menuTransform.gameObject.SetActive(false);
    }
	
	void Update () 
	{

        if (molRoot != null)
        {

            // Input

            if (Input.GetKeyDown(KeyCode.Return))
            {
                molRootOffset = transform.position - molRoot.transform.position;
                
                foreach (Transform child in molRoot.transform)
                {
                    child.position -= molRootOffset;
                }
                
                molRoot.transform.position += molRootOffset;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                SceneManager.LoadScene(0);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            
            
            var leftI = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
            var rightI = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
            if (leftI == rightI)
            {
                // Single Controller
                rightI = -1;
            }

            if (leftI != -1) {
                var left = SteamVR_Controller.Input(leftI);
                if (left.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
                {
                    left.TriggerHapticPulse(1000);
                    if (menuTransform.gameObject.activeSelf)
                    {
                        menuTransform.gameObject.SetActive(false);
                    }
                    else
                    {
                        menuTransform.gameObject.SetActive(true);
                        menuTransform.position = left.transform.pos;
                        menuTransform.rotation = left.transform.rot;
                        menuTransform.position += menuTransform.forward / 5;
                        menuTransform.Rotate(Vector3.right * 45);
                    }
                } else if (left.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                {
                    molRoot.transform.position += left.velocity;
                    molRoot.transform.Rotate(left.angularVelocity);
                } else if (left.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    var s = left.GetAxis().y;
                    float scale = 1.02f;
                    if (s < 0)
                    {
                        scale = .98f;
                    }
                    molRoot.transform.localScale *= scale;
                }
            }

            if (rightI != -1)
            {
                var right = SteamVR_Controller.Input(rightI);
                if (right.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
                {
                    Debug.Log("rsys");
                }
                else if (right.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    Debug.Log("rtrig");
                }
            }
        }
    }

    public void SwitchToMenu(int menuNumber)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (i == menuNumber)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
        
        menuShowing = menuNumber;
    }

}
