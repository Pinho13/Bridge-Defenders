using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour
{
    void Awake()=>FindReferences();
    void Reset()=>FindReferences();

    Animator animator;

    void FindReferences()
    {
        if(animator==null) animator = GetComponent<Animator>();
    }

    public void PlayAnimationAndWait(Action onOver,string stateName,string layerName = "Base Layer")
    {
        StartCoroutine(PlayAnimation(onOver,stateName,layerName));
    }

    IEnumerator PlayAnimation(Action onOver,string stateName,string layerName = "Base Layer")
    {
        animator.PlayAnimationWithName(stateName,layerName);
        yield return new WaitUntil(()=>animator.StateFinished(stateName));
        onOver?.Invoke();
    }
}
