using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorScript
{
    public static void PlayAnimationWithName(this Animator animator,string Name,string layerName = "Base Layer")
     {
        animator.Play($"{layerName}.{Name}");
     }

    public static bool StateFinished(this Animator animator,string Name,int layer = 0)
    {
        AnimatorStateInfo clipInfo = animator.GetCurrentAnimatorStateInfo(layer);
        return(clipInfo.IsName(Name) && clipInfo.normalizedTime >= 1);
    }

    
}



