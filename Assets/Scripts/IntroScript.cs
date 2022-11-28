using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour
{

    public GameObject[] introImages;
    [SerializeField] private int currentImage;
    public GameObject sceneChanger;
    
    // Start is called before the first frame update
    void Start()
    {
        currentImage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentImage < introImages.Length-1)
        {
            SoundController.Instance.PlaySFXSound("버튼");
            currentImage++;
            for (int i = 0; i < introImages.Length; i++)
            {
                if (i == currentImage)
                {
                    introImages[i].SetActive(true);
                }
                else
                {
                    introImages[i].SetActive(false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && currentImage == introImages.Length - 1) 
        {
            SoundController.Instance.PlaySFXSound("버튼");
            sceneChanger.GetComponent<SceneManagement>().SceneChanger();
        }
    }
    
    
}
