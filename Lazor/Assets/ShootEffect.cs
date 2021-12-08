using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class ShootEffect : MonoBehaviour {
    private AudioSource audioSource;
    private Light light;
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        light = GetComponent<Light>();
    }

    private void OnEnable() {
        audioSource.PlayOneShot(audioSource.clip);
        StartCoroutine(TurnOff());
    }

    IEnumerator TurnOff() {
        Noise();
        yield return new WaitForSeconds(0.05f);
        light.enabled = false;
        DeNoise();
        yield return new WaitForSeconds(0.45f);
        light.enabled = true;
        gameObject.SetActive(false);
    }

    void Noise() {
        cinemachineFreeLook.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        cinemachineFreeLook.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        cinemachineFreeLook.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
        cinemachineFreeLook.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;
        cinemachineFreeLook.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;
        cinemachineFreeLook.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;
    }
    
    void DeNoise() {
        cinemachineFreeLook.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        cinemachineFreeLook.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        cinemachineFreeLook.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        cinemachineFreeLook.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        cinemachineFreeLook.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
        cinemachineFreeLook.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
    }
}