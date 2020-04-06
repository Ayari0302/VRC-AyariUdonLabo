using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CubeUI : UdonSharpBehaviour
{
    [SerializeField] private GameObject _forwardObj;
    [SerializeField] private GameObject _backwardObj;
    [SerializeField] private GameObject _leftwardObj;
    [SerializeField] private GameObject _rightwardObj;

    private void TransferOfAuthority()
    {
        if (Networking.GetOwner(this.gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);

        if (Networking.GetOwner(_forwardObj) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, _forwardObj);

        if (Networking.GetOwner(_backwardObj) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, _backwardObj);

        if (Networking.GetOwner(_leftwardObj) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, _leftwardObj);

        if (Networking.GetOwner(_rightwardObj) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, _rightwardObj);
    }

    public void OnSetActive(bool activeValue)
    {
        _forwardObj.SetActive(activeValue);
        _backwardObj.SetActive(activeValue);
        _leftwardObj.SetActive(activeValue);
        _rightwardObj.SetActive(activeValue);
    }


    private void OnInteract()
    {
        TransferOfAuthority();
        OnSetActive(true);
    }

    public override void Interact()
    {
        OnInteract();
    }
}