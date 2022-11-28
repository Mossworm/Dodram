using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneBGMController : MonoBehaviour
{
    public GameObject startSceneBGMController;
    private FMOD.Studio.Bus Master;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("BGM");
        if (objs.Length > 1)
        {
            Destroy(objs[1]);
        }
        DontDestroyOnLoad(startSceneBGMController);
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
    }

    void Update()
    {
        Master.setVolume(PlayerPrefs.GetFloat("volume"));

        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex != 0 && currentScene.buildIndex != 4){

            startSceneBGMController.SetActive(false);
        }
        else
        {
            //startSceneBGMController.SetActive(true);
        }
    }
}
