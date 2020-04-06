using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RingTheBell : UdonSharpBehaviour
{
    [SerializeField] private AudioClip _clip;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public override void OnPlayerJoined(VRCPlayerApi playerApi)
    {
        TransferOfAuthority();
        PlayAudioClip();
    }

    private void PlayAudioClip()
    {
        _audioSource.PlayOneShot(_clip);
    }

    private void TransferOfAuthority()
    {
        if (Networking.GetOwner(gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
    }
}