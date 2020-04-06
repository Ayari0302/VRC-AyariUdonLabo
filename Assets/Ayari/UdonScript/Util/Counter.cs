using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class Counter : UdonSharpBehaviour
{
    [SerializeField] private Text _notSyncText;
    [SerializeField] private Text _syncText;

    [UdonSynced(UdonSyncMode.NotSynced)] private int _notSyncNum;
    [UdonSynced(UdonSyncMode.None)] private int _syncNum;

    private void Update()
    {
        _notSyncText.text = _notSyncNum.ToString();
        _syncText.text = _syncNum.ToString();
    }

    public void NotSyncCountUp()
    {
        TransferOfAuthority();
        _notSyncNum++;
    }

    public void SyncCountUp()
    {
        TransferOfAuthority();
        _syncNum++;
    }

    private void TransferOfAuthority()
    {
        if (Networking.GetOwner(gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
    }
}