using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    public Animator transitionAnim;
    public int sceneNum;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
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



}
