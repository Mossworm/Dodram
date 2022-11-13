using Spine.Unity;
using System.Collections;
using UnityEngine;

public class SpineAnimationBehavior : StateMachineBehaviour
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
    public float exitTime = .85f;

    private GameObject _player;
    private SpinePlayerController _spc;
    private SpinePickUpScript pickUpScript;

    [SerializeField]
    private SpinePlayerController.Dir formerDirection;



    private void Awake()
    {
        if (motion != null)
        {
            animationClip = motion.name;
            //Debug.Log(animationClip);
        }
        _player = GameObject.Find("SpinePlayer1");
        _spc = _player.GetComponent<SpinePlayerController>();
        pickUpScript= _player.GetComponent<SpinePickUpScript>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for(int i=0; i< _player.GetComponentsInChildren<SkeletonAnimation>(true).Length; i++)
        {
            _player.GetComponentsInChildren<SkeletonAnimation>(true)[i].gameObject.SetActive(false);
        }

            switch (_spc.direction)
            {
                case SpinePlayerController.Dir.Up:
                    _skeletonAnimation = animator.GetComponentsInChildren<SkeletonAnimation>(true)[1];
                    _player.GetComponentsInChildren<SkeletonAnimation>(true)[1].gameObject.SetActive(true);
                    break;

                case SpinePlayerController.Dir.Down:
                    _skeletonAnimation = animator.GetComponentsInChildren<SkeletonAnimation>(true)[0];
                    _player.GetComponentsInChildren<SkeletonAnimation>(true)[0].gameObject.SetActive(true);
                    break;

                case SpinePlayerController.Dir.Left:
                    _skeletonAnimation = animator.GetComponentsInChildren<SkeletonAnimation>(true)[2];
                    _player.GetComponentsInChildren<SkeletonAnimation>(true)[2].gameObject.SetActive(true);
                    break;

                case SpinePlayerController.Dir.Right:
                    _skeletonAnimation = animator.GetComponentsInChildren<SkeletonAnimation>(true)[3];
                    _player.GetComponentsInChildren<SkeletonAnimation>(true)[3].gameObject.SetActive(true);
                    break;
                default:
                    break;

        }        
        _spineAnimationState = _skeletonAnimation.state;

        if (animationClip != null)
        {
            _skeletonAnimation.AnimationName = animationClip;
            _trackEntry = _spineAnimationState.SetAnimation(layer, animationClip, isLoop);
            _trackEntry.TimeScale = timeScale;
            Debug.Log(animationClip);
        }
    }

}


