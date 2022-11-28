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

        if (!PlayerPrefs.HasKey("FullScreen"))
        {
            PlayerPrefs.SetInt("FullScreen",System.Convert.ToInt16(false));
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }

        if (toggle != null)
        {
            toggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullScreen"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (volumeSlider != null)
        {
            PlayerPrefs.SetFloat("volume",volumeSlider.value);
        }

        if (toggle != null)
        {
            PlayerPrefs.SetInt("FullScreen",System.Convert.ToInt16(toggle.isOn));
            SetResolution();
        }
    }

    public void SceneChanger()
    {
        SoundController.Instance.PlaySFXSound("페이드인");
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
        Screen.SetResolution(1920,1080,System.Convert.ToBoolean(PlayerPrefs.GetInt("FullScreen")));
    }

    public void PlayButtonSound()
    {
        SoundController.Instance.PlaySFXSound("버튼");
    }


}
