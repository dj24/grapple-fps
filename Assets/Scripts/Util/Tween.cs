using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tween
{
    public static float Ease(float current, float target){
        return Mathf.Lerp(current, target, Time.deltaTime * GameManager.adsSpeed);
    }
    
    public static float EaseLayerWeight(Animator anim, string layerName, bool enabled){
        var layerIndex = anim.GetLayerIndex(layerName);
        float layerWeight = anim.GetLayerWeight(layerIndex);
        anim.SetLayerWeight(layerIndex,Tween.Ease(layerWeight, enabled ? 1f : 0f));
        return layerWeight;
    }

     public static float EaseLayerWeight(Animator anim, string layerName, bool enabled, float start, float end){
        var layerIndex = anim.GetLayerIndex(layerName);
        float layerWeight = anim.GetLayerWeight(layerIndex);
        anim.SetLayerWeight(layerIndex,Tween.Ease(layerWeight, enabled ? end : start));
        return layerWeight;
    }
}
