using Spine.Unity;
using System.Collections;
using UnityEngine;

public class SpinePlayerAnimationBehavior : StateMachineBehaviour
{

    public AnimationClip motion;
    string animationClip;
    public bool isLoop;

    [Header("Spine Motion Layer")]
    public int layer = 0;
    public float timeScale = 1f;

    private SkeletonAnimation _skeletonAnimation;
    private Spine.AnimationState _spineAnimationState;
    private Spine.TrackEntry _trackEntry;

    private float normalizedTime;
    [SerializeField] private float exitTime=1f;

    [SerializeField] private SpinePlayerController _spc;
    private SpinePickUpScript pickUpScript;

    [SerializeField]
    private SpinePlayerController.Dir formerDirection;

    private int _skeletonNum;
    private Animator animator;



    private void Awake()
    {
        if (motion != null)
        {
            animationClip = motion.name;
            //Debug.Log(animationClip);
        }
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_spc == null)
        {
            _spc = animator.GetComponent<SpinePlayerController>();
            pickUpScript = animator.GetComponent<SpinePickUpScript>();

        }
        for (int i = 0; i < animator.GetComponentsInChildren<SkeletonAnimation>(true).Length; i++)
        {
            animator.GetComponentsInChildren<SkeletonAnimation>(true)[i].gameObject.SetActive(false);
        }

        if (Input.GetKey(pickUpScript.InteractiveKey)&& pickUpScript.isHold
            &&pickUpScript.Hand.transform.GetChild(0).CompareTag("tool"))
        {
            string _toolName = pickUpScript.Hand.transform.GetChild(0).name;

            switch (_spc.direction)
            {
                case SpinePlayerController.Dir.Up:
                    if (_toolName == "Scythe")
                        _skeletonNum = 5;

                    else if (_toolName == "PickAxe")
                        _skeletonNum = 9;

                    else
                        _skeletonNum = 1;
                    break;

                case SpinePlayerController.Dir.Down:
                    if (_toolName == "Scythe")
                        _skeletonNum = 4;

                    else if (_toolName == "PickAxe")
                        _skeletonNum = 8;

                    else
                        _skeletonNum = 0;
                    break;

                case SpinePlayerController.Dir.Left:
                    if (_toolName == "Scythe")
                        _skeletonNum = 6;

                    else if (_toolName == "PickAxe")
                        _skeletonNum = 10;

                    else
                        _skeletonNum = 2;
                    break;

                case SpinePlayerController.Dir.Right:
                    if (_toolName == "Scythe")
                        _skeletonNum = 7;

                    else if (_toolName == "PickAxe")
                        _skeletonNum = 11;

                    else
                        _skeletonNum = 3;
                    break;

                default:
                    break;

            }
        }
        else
        {
            switch (_spc.direction)
            {
                case SpinePlayerController.Dir.Up:
                    _skeletonNum = 1;
                    break;

                case SpinePlayerController.Dir.Down:
                    _skeletonNum = 0;
                    break;

                case SpinePlayerController.Dir.Left:
                    _skeletonNum = 2;
                    break;

                case SpinePlayerController.Dir.Right:
                    _skeletonNum = 3;
                    break;
                default:
                    break;

            }         
        }
        _skeletonAnimation = animator.GetComponentsInChildren<SkeletonAnimation>(true)[_skeletonNum];
        animator.GetComponentsInChildren<SkeletonAnimation>(true)[_skeletonNum].gameObject.SetActive(true);

        _spineAnimationState = _skeletonAnimation.state;

        if (animationClip != null)
        {
            _skeletonAnimation.AnimationName = animationClip;
            _trackEntry = _spineAnimationState.SetAnimation(layer, animationClip, isLoop);
            _trackEntry.TimeScale = timeScale;
            Debug.Log(animationClip);
        }

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        normalizedTime = _trackEntry.AnimationLast / _trackEntry.AnimationEnd;  //3.6����
                                                                              //normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd; //3.8 ����
                                                                              // ������ ��Ÿ�� ���� ���� �ٲ� ���� �Լ� �̸� �ٲٴ°� ���ε� . . .

        //�ִϸ��̼��� ������ �ƴҰ�� , �ִϸ��̼��� ������ Ʈ���� ����
        if (!isLoop && normalizedTime >= exitTime)
        {
            _spc.pickAnimEnd = true;
        }
    }

}


