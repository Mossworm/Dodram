using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CountScript : MonoBehaviour
{
    public TextMesh count;
    public GameObject Machine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IngreCount();
    }

    public void IngreCount()
    {
        float cnt = Machine.transform.childCount - 2;
        count.text = cnt.ToString() + "/2";
    }
}
