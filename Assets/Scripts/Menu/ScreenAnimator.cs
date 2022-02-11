using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Animator))]
public class ScreenAnimator : MonoBehaviour
{
    private Animator _animator;

    public event UnityAction AnimationDone;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void CloseScreen(bool right)
    {
        if (right) _animator.SetTrigger("OutR");
        else _animator.SetTrigger("OutL");
    }
   
    public void OpenScreen(bool right)
    {
        gameObject.SetActive(true);
        if (right) _animator.SetTrigger("InR");
        else _animator.SetTrigger("InL");
    }
    protected void TurnOff()
    {
        gameObject.SetActive(false);
    }
    protected void AnimationIsDone()
    {
        AnimationDone?.Invoke();
    }
}