using UnityEngine;

public class PlayerAnimationControls : MonoBehaviour {
    private Animator animator;
    private Player parentRef;

    private void Start() {
        animator = GetComponent<Animator>();
        parentRef = Player.instance;

        parentRef.inputHandler.onAttack += Attack;
        parentRef.inputHandler.onWalkStart += StartWalk;
        parentRef.inputHandler.onWalkEnd += EndWalk;
        parentRef.inputHandler.onJumpInit += JumpInit;
        parentRef.inputHandler.onJumpPeak += JumpPeak;
        parentRef.collisionHandler.onLand += JumpEnd;
        parentRef.collisionHandler.onFall += SetFalling;
    }

    public void Attack() {
        animator.SetTrigger("Attack");
    }

    public void StartWalk() {
        animator.SetBool("Walking", true);
    }

    public void EndWalk() {
        animator.SetBool("Walking", false);
    }

    public void JumpInit() {
        animator.SetTrigger("JumpInit");
    }

    public void JumpPeak() {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("JumpInit"))
            animator.SetTrigger("JumpPeak");
    }

    public void JumpEnd() {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
            animator.SetTrigger("JumpEnd");
    }

    public void SetFalling() {
        animator.SetTrigger("Fall");
    }
}
