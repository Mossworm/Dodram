using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public GameObject bgmController;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(bgmController);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
