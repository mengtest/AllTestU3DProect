using UnityEngine;
using SmoothMoves;
using System.Collections.Generic;


[System.Reflection.ObfuscationAttribute(Exclude = true)]
public class SMAnimationData : ScriptableObject
{
    public TriggerFrame[] triggerFrames;
    public AnimationClipSM_Lite[] mAnimationClips;

    public List<TriggerFrameBone> reusedTriggerFrameBones = new List<TriggerFrameBone>();

#if UNITY_EDITOR
    public void ReduceTriggerFrameBone()
    {
        reusedTriggerFrameBones.Clear();

        int old_count = 0;
        foreach (TriggerFrame trigger_frame in triggerFrames)
        {
            old_count += trigger_frame.triggerFrameBones.Count;
            foreach (TriggerFrameBone trigger_frame_bone in trigger_frame.triggerFrameBones)
            {
                int i = 0;
                for (; i < reusedTriggerFrameBones.Count; i++)
                {
                    TriggerFrameBone backup_tfb = reusedTriggerFrameBones[i];
                    if (TriggerFrameBone.IsEqual(backup_tfb, trigger_frame_bone))
                    {
                        trigger_frame.triggerFrameBoneIndexs.Add(i);
                        break;
                    }
                }

                if (i == reusedTriggerFrameBones.Count)
                {
                    reusedTriggerFrameBones.Add(trigger_frame_bone);
                    trigger_frame.triggerFrameBoneIndexs.Add(i);
                }
            }

            trigger_frame.triggerFrameBones = null;
        }

        if (reusedTriggerFrameBones.Count < 100)
        {
            Debug.Log("trigger frame count, old " + old_count + ",  new  " + reusedTriggerFrameBones.Count);
        }
        else
        {
            Debug.LogError("trigger frame count, old " + old_count + ", new " + reusedTriggerFrameBones.Count + ", 可尝试优化~");
        }
    }
#endif
}

