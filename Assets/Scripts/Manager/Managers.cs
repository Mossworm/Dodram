using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Managers : MonoBehaviour
{
    static Managers _instance;  
    static Managers Instance { get { Init(); return _instance; } }

    private SoundManager _sound = new SoundManager();
    public static SoundManager Sound { get { return Instance._sound; } }
    
    private InputManager _input = new InputManager();
    public static InputManager Input { get { return Instance._input; } }
    
    private ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return Instance._resource; } }

    public static bool isReady;

    void Start()
    {
        Init();
        isReady = false;
    }
    
    void Update()
    {
        _input.OnUpdate();

    }
    
    static void Init()
    {
        if(_instance == null)
        {
            //@Managers 가 존재하는지 확인
            GameObject go = GameObject.Find("@Managers");
            //없으면 생성
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
            }
            if(go.GetComponent<Managers>() == null)
            {
                go.AddComponent<Managers>();
            }
            
            //없어지지 않도록 해줌
            DontDestroyOnLoad(go);
            //instance 할당
            _instance = go.GetComponent<Managers>();
            
            _instance._sound.Init();
        }
    }
    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
    }
}
