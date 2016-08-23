using UnityEngine;
using System.Collections;

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
    private Vector3 menuInEnablingCameraView;
    public float menuDistanceFromHand = 0.13f;
    public int menuShowing = 0;

    private GameObject[] menus;

    //Scripts
    private AddAtoms addAtoms;

    //Gameobjects & Transforms
    private GameObject molRoot;
    private Vector3 molRootOffset;
    private Transform mainCamera;
    private Transform LeapHandControllerTransform;
    private Transform LeftHandObjectTransform;
    private Transform menuTransform;
    
    
	void Start ()
	{
        addAtoms = gameObject.GetComponent<AddAtoms>();
        molRoot = GameObject.Find ("molRoot");
        molRootOffset = Vector3.zero;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        menuTransform = transform.FindChild("Menu");

        //Hide menu toggle if only 1 ligand is present

        if (addAtoms.CollectionOfLigands == null)
        {
            menuTransform.FindChild("ToggleMenuButtons").gameObject.SetActive(false);
        }

        menus = new GameObject[menuTransform.childCount - 1];

        for (int i = 0; i < menus.Length; i++)
        {
            menus[i] = menuTransform.GetChild(i).gameObject;
            menus[i].SetActive(false);
        }

        menus[menuShowing].SetActive(true);

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
        
    }
	
	void Update () 
	{

        if (molRoot != null)
        {

            // Input

            if (Input.GetKey("4") || Input.GetButton("Fire2")) // Reset molroot
            {
                molRoot.transform.position = (mainCamera.position + mainCamera.forward * 50) - molRootOffset;
            }

            if (Input.GetAxis("Vertical") > 0)
            {
                transform.Translate(Vector3.Scale(Vector3.forward, new Vector3(moveSpeed, moveSpeed, moveSpeed)), mainCamera);
            }

            if (Input.GetAxis("Vertical") < 0)
            {
                transform.Translate(Vector3.Scale(Vector3.back, new Vector3(moveSpeed, moveSpeed, moveSpeed)), mainCamera);
            }

            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.Translate(Vector3.Scale(Vector3.right, new Vector3(moveSpeed, moveSpeed, moveSpeed)), mainCamera);
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.Translate(Vector3.Scale(Vector3.left, new Vector3(moveSpeed, moveSpeed, moveSpeed)), mainCamera);
            }

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
                Application.LoadLevel(0);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            //Menu
            
            if (LeftHandObjectTransform != null)
            {
                menuTransform.position = (LeftHandObjectTransform.position);
                menuTransform.Translate(Vector3.right * menuDistanceFromHand);
                menuTransform.LookAt(2 * menuTransform.position - LeapHandControllerTransform.position);

                menuInEnablingCameraView = menuEnablingCamera.WorldToViewportPoint(menuTransform.position);

                if (menuInEnablingCameraView.x > 0 &&
                    menuInEnablingCameraView.x < 1 &&
                    menuInEnablingCameraView.y > 0 &&
                    menuInEnablingCameraView.y < 1 &&
                    menuInEnablingCameraView.z > 0.15)
                {
                    menuTransform.gameObject.SetActive(true);
                }
                else
                {
                    menuTransform.gameObject.SetActive(false);
                }
            }
            else
            {
                menuTransform.gameObject.SetActive(false);
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
