using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {

    public AnimatorOverrideController playerHurt;

    private Animator animator;
    private RuntimeAnimatorController playerHealthy;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        playerHealthy = animator.runtimeAnimatorController;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1"))
        {
            animator.runtimeAnimatorController = playerHurt;
        }
        if (Input.GetKeyDown("2"))
        {
            animator.runtimeAnimatorController = playerHealthy;
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "With robe"))
            animator.runtimeAnimatorController = playerHealthy;
        if (GUI.Button(new Rect(10, 60, 150, 50), "Without robe"))
            animator.runtimeAnimatorController = playerHurt;

    }
}
