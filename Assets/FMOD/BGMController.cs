using FMODUnity;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    StudioEventEmitter emitter;

    //파라미터이름:Zoom
    public GameObject mainCamera;
    public float _zoom;

    //TimeLeft
    public GameObject timer;
    [SerializeField] private float _pureWidth;
    [SerializeField] private float _pureCurrentSizeX;

    public int timeValue; //HalfTime==1, QuarterTime==2;


    //Pause
    public GameObject pauseCanvas;

    private FMOD.Studio.Bus Master;
    

    void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();

        _pureWidth = timer.GetComponent<UI_Timer>()._MAX_TIME - timer.GetComponent<UI_Timer>()._currentTime;
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
    }

    void Update()
    {
        Master.setVolume(PlayerPrefs.GetFloat("volume"));
        
        //Zoom
        _zoom = mainCamera.GetComponent<MultipleTargetCamera>().newZoom;
        emitter.SetParameter("Zoom", _zoom);

        //Timer
        _pureCurrentSizeX = timer.GetComponent<UI_Timer>()._MAX_TIME
            - timer.GetComponent<UI_Timer>()._currentTime;
        //emitter.SetParameter("TimeLeft", 0);

        if (_pureCurrentSizeX == 0)
        {
            Time.timeScale = 0;
        }

        if (_pureCurrentSizeX <= (_pureWidth / 2)&& _pureCurrentSizeX > (_pureWidth / 4))
        {
            emitter.SetParameter("TimeLeft", 1); //HalfTime:Value==1
            timeValue = 1;
        }
        else if (_pureCurrentSizeX <= (_pureWidth / 4)&& _pureCurrentSizeX >0)
        {
            emitter.SetParameter("TimeLeft", 2); //QuarterTime:Value==2
            timeValue = 2;
        }

        //Pause
        if (pauseCanvas.activeSelf)
        {
            emitter.SetParameter("Pause", 1);
        }
        else
        {
            emitter.SetParameter("Pause", 0);
        }

    }
}
