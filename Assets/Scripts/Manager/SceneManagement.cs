using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class SceneManagement : MonoBehaviour
{

    public Animator transitionAnim;
    public int sceneNum;
    public Toggle toggle;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        DontDestroyOnLoad(gameObject);
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume",0.5f);
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (volumeSlider != null)
        {
            PlayerPrefs.SetFloat("volume",volumeSlider.value);
        }
    }

    public void SceneChanger()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(sceneNum);
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(1.5f);
        Destroy(gameObject);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SetResolution()
    {
        Screen.SetResolution(1920,1080,toggle.isOn);
    }



}
