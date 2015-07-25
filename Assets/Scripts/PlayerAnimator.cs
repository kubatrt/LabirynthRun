using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {

	Animator	animator;

	void Awake()
	{
		animator = GetComponent<Animator> ();
	}

	public void SetJumpAnim()
	{
		animator.SetTrigger ("Jump");
	}
	
	public void SetRollAnim()
	{
		animator.SetTrigger("Roll");
	}
	
	public void SetMovingAnim() 
	{ 
		ResetAnimations();
		animator.SetBool("Run", true);
	}

	public void SetSlowDownAnim(bool choice)
	{
		animator.SetBool ("SlowDown", choice);
	}
	
	public float SetDedAnim()
	{
		ResetAnimations();
		animator.SetBool ("Ded", true);
		return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;	// just cheking
	}
	
	public void SetCelebrateAnim()
	{
		ResetAnimations();
		animator.SetBool ("Celebrate", true);
	}
	
	public void ResetAnimations()
	{
		animator.SetBool("Run", false);
		animator.SetBool("Ded", false);
		animator.SetBool("SlowDown", false);
		animator.SetBool("Celebrate", false);
		animator.SetBool("SpeedUp", false);
	}
	
	public void AnimEvent_DeadEnd()
	{
		if(GameManager.Instance.state == GameState.Run)
			Invoke("RestartPlayer", 2f);
	}
	
	public void PauseAnimations()
	{
		animator.enabled = false;
	}
	
	public void UnpauseAnimations()
	{
		animator.enabled = true;
	}
	
	public void SetSpeedUpAnimation(bool choice)
	{
		animator.SetBool("SpeedUp", choice);
	}
}
