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

    public float maxCamSize;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //FstCam = GameObject.Find("1PCamera").GetComponent<Camera>();
        //SndCam = GameObject.Find("2PCamera").GetComponent<Camera>();

        FstCam.enabled = false;
        SndCam.enabled = false;

        //CameraMove();
    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();
    }

    void CameraMove()
    {
        if (mainCam.orthographicSize >= maxCamSize)
        {
            Invoke("SecondCameraOn", 1f);
        }
        else
        {
            mainCam.orthographicSize += Time.deltaTime;
        }
    }

    void SecondCameraOn()
    {
        FstCam.enabled = true;
        SndCam.enabled = true;
        mainCam.enabled = false;

        Invoke("SecondCameraMove", 0.5f);

    }

    void SecondCameraMove()
    {
        if (FstCam.orthographicSize >= 6)
        {
            FstCam.orthographicSize -= (Time.deltaTime * 3f);
            SndCam.orthographicSize -= (Time.deltaTime * 3f);
        }
    }
}
