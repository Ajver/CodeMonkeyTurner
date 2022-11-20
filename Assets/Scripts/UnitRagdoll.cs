using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{

    [SerializeField] private Transform ragdollRootBone;

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);
    }

    private void MatchAllChildTransforms(Transform original, Transform clone)
    {
        foreach (Transform originalChild in original)
        {
            Transform cloneChild = clone.Find(originalChild.name);
            if (cloneChild != null)
            {
                cloneChild.position = originalChild.position;
                cloneChild.rotation = originalChild.rotation;
                MatchAllChildTransforms(originalChild, cloneChild);
            }
        }
    }
    
}
