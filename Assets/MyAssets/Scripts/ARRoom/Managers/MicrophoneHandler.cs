using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice;
using Photon.Voice.Unity;

public class MicrophoneHandler : MonoBehaviour
{
    Button muteMicButton;
    Button unmuteMicButton;

    Button muteSpeakerButton;
    Recorder recorder;

    void Start()
    {
        muteMicButton = GameObject.Find("MuteMicButton").GetComponent<Button>();
        muteMicButton.onClick.AddListener(MuteMicrophone);

        unmuteMicButton = GameObject.Find("UnmuteMicButton").GetComponent<Button>();
        unmuteMicButton.onClick.AddListener(UnmuteMicrophone);

        recorder = GetComponent<Recorder>();
    }

    void MuteMicrophone()
    {
        recorder.TransmitEnabled = false;
        muteMicButton.gameObject.SetActive(false);
        unmuteMicButton.gameObject.SetActive(true);


    }

    void UnmuteMicrophone()
    {
        recorder.TransmitEnabled = true;
        muteMicButton.gameObject.SetActive(true);
        unmuteMicButton.gameObject.SetActive(false);
    }
}
