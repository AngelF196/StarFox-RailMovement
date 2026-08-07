//using Cinemachine;
//using DG.Tweening;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

//public class Particles : MonoBehaviour
//{
//    [Header("Particle Refs")]
//    public ParticleSystem trail;
//    public ParticleSystem circle;
//    public ParticleSystem barrel;
//    public ParticleSystem stars;

//    public void PlayBoostRing()
//    {

//    }

//    public void PlayRollRing()
//    {

//    }

//    public void PlayBoosting()
//    {

//    }

//    public void CancelBoosting()
//    {

//    }

//    void SetCameraZoom(float zoom, float duration)
//    {
//        cameraParent.DOLocalMove(new Vector3(0, 0, zoom), duration);
//    }

//    void DistortionAmount(float x)
//    {
//        Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<LensDistortion>().intensity.value = x;
//    }

//    void FieldOfView(float fov)
//    {
//        cameraParent.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = fov;
//    }

//    void Chromatic(float x)
//    {
//        Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<ChromaticAberration>().intensity.value = x;
//    }


//    public void Boost(bool state)
//    {

//        if (state)
//        {
//            cameraParent.GetComponentInChildren<CinemachineImpulseSource>().GenerateImpulse();
//            trail.Play();
//            circle.Play();
//        }
//        else
//        {
//            trail.Stop();
//            circle.Stop();
//        }
//        trail.GetComponent<TrailRenderer>().emitting = state;

//        float origFov = state ? 40 : 55;
//        float endFov = state ? 55 : 40;
//        float origChrom = state ? 0 : 1;
//        float endChrom = state ? 1 : 0;
//        float origDistortion = state ? 0 : -30;
//        float endDistorton = state ? -30 : 0;
//        float starsVel = state ? -20 : -1;
//        float speed = state ? forwardSpeed * 2 : forwardSpeed;
//        float zoom = state ? -7 : 0;

//        DOVirtual.Float(origChrom, endChrom, .5f, Chromatic);
//        DOVirtual.Float(origFov, endFov, .5f, FieldOfView);
//        DOVirtual.Float(origDistortion, endDistorton, .5f, DistortionAmount);
//        var pvel = stars.velocityOverLifetime;
//        pvel.z = starsVel;

//        DOVirtual.Float(dolly.m_Speed, speed, .15f, SetSpeed);
//        SetCameraZoom(zoom, .4f);
//    }

//    public void Break(bool state)
//    {
//        float speed = state ? forwardSpeed / 3 : forwardSpeed;
//        float zoom = state ? 3 : 0;

//        DOVirtual.Float(dolly.m_Speed, speed, .15f, SetSpeed);
//        SetCameraZoom(zoom, .4f);
//    }
//}
