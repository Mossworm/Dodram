using UnityEngine;
using UnityEngine.UI;

public class UI_Round_Image : MonoBehaviour
{
    public GameObject Round;
    public GameObject BGMAudio;
    [SerializeField] private int _timeValue;
    [SerializeField] private int _formerTimeValue;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _timeValue = BGMAudio.GetComponent<GameScene1BGMController>().timeValue;
        if (_timeValue != _formerTimeValue)
        {
            Round.transform.GetChild(_formerTimeValue).gameObject.SetActive(false);
            Round.transform.GetChild(_timeValue).gameObject.SetActive(true);
        }
        _formerTimeValue = _timeValue;
    }
}
