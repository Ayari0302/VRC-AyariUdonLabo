using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SyncInstantiate : UdonSharpBehaviour
{
    [SerializeField] private GameObject _instantiateObject;

    // オーナーか
    private bool _isAuthority = false;

    // 生成をするか
    private bool _isInstantiate = false;

    // ワールド内で実行したか
    [UdonSynced(UdonSyncMode.None)] private bool _isSyncGenerated = false;

    // 一度でもローカル環境で生成したか
    private bool _isLocalGenerated = false;

    private void Update()
    {
        // 常にオーナー権限の確認
        CheckOfAuthority();

        // オーナーの場合は処理を実行
        if (_isAuthority)
        {
            if (_isInstantiate)
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ObjectInstantiate");
                _isInstantiate = false;
            }
        }

        if (_isSyncGenerated == true)
        {
            if (_isLocalGenerated == false)
            {
                ObjectInstantiate();
            }
        }
    }

    // GUIから呼び出す
    public void OnInstantiate()
    {
        if (_isAuthority)
        {
            _isInstantiate = true;
            _isSyncGenerated = true;
        }
        else
        {
            TransferOfAuthority();
        }
    }


    public void ObjectInstantiate()
    {
        VRCInstantiate(_instantiateObject);
        _isLocalGenerated = true;
    }

    // 後から入った時に同期させる方法

    // ループ前提で権限付与する
    private void TransferOfAuthority()
    {
        if (Networking.GetOwner(gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
    }

    // オーナー権限の確認
    private void CheckOfAuthority()
    {
        if (Networking.GetOwner(gameObject) != Networking.LocalPlayer)
        {
            _isAuthority = false;
        }
        else
        {
            _isAuthority = true;
        }
    }
}