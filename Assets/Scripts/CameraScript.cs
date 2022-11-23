using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CameraScript : MonoBehaviour
{
    public Camera mainCam;
    public Camera FstCam;
    public Camera SndCam;

    public GameObject Arrow1;
    public GameObject Arrow2;

    public float CamMoveTime;
    private float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();

        FstCam.enabled = false;
        SndCam.enabled = false;
        currentTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        currentTime += Time.deltaTime;

        if (CamMoveTime >= currentTime)
        {
            mainCam.transform.position += new Vector3(0.15f,0,0) * Time.deltaTime;
        }
        else
        {
            Managers.isReady = true;
            Arrow1.SetActive(false);
            Arrow2.SetActive(false);
            SecondCameraOn();
            this.GetComponent<CameraScript>().enabled = false;
        }
        
        
        
    }

    void SecondCameraOn()
    {
        FstCam.enabled = true;
        SndCam.enabled = true;
        mainCam.enabled = false;
    }
    
}
