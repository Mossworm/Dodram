using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

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
    public float exitTime = 0f;

    //[SerializeField] private SpinePlayerController _spc;
    //private SpinePickUpScript pickUpScript;

    //[SerializeField]
    //private SpinePlayerController.Dir formerDirection;

    //private int _skeletonNum;
    //private Animator animator;



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
       
        _skeletonAnimation = animator.GetComponentInChildren<SkeletonAnimation>(true);

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
        normalizedTime = _trackEntry.AnimationLast / _trackEntry.AnimationEnd;  //3.6기준
                                                                                //normalizedTime = trackEntry.AnimationLast / trackEntry.AnimationEnd; //3.8 기준
                                                                                // 스파인 런타임 쪽은 버젼 바뀔때 마다 함수 이름 바꾸는게 일인듯 . . .

        //애니메이션이 루프가 아닐경우 , 애니메이션이 끝나면 트리거 실행
        if (!isLoop && normalizedTime >= exitTime)
        {
            //_spc.pickAnimEnd = true;
        }
    }
}
