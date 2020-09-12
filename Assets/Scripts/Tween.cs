using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tween
{
    private static float Round(float value){
        return Mathf.Round(value * 1000f) / 1000f;
    }

    public static float Ease(float current, float target){
        return Tween.Round(Mathf.Lerp(current, target, Time.deltaTime * GameManager.adsSpeed));
    }

    public static float Lerp(float start, float end, float progress){
        return Tween.Round(start + (end - start) * progress);
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
