using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MaterialChange : UdonSharpBehaviour
{
    //[UdonSynced(UdonSyncMode.None)]
    public MeshRenderer MeshRenderer;

    public Material beforeMaterial;
    public Material afterMaterial;
    
    public void ChangeMaterial()
    {
        if (MeshRenderer.sharedMaterial == beforeMaterial)
        {
            MeshRenderer.sharedMaterial = afterMaterial;
        }
        else
        {
            MeshRenderer.sharedMaterial = beforeMaterial;
        }
    }

    void Start()
    {
        MeshRenderer.sharedMaterial = beforeMaterial;
        //RandomTest();
    }

    public void ChangeTest()
    {
        if (Networking.GetOwner(this.gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);

        // 処理同期
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeMaterial");
    }



    [SerializeField] private GameObject _gameObject;
    private float _randomY;
    
    [UdonSynced(UdonSyncMode.None)] private Vector3 syncCube;

    public void RandomTest()
    {
        if(Networking.GetOwner(this.gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        
        // 他人から見た操作するオブジェクトのオーナー権限を譲渡
        if(Networking.GetOwner(_gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, _gameObject);
        
        _randomY = Random.Range(0f, 1.5f);
        Debug.Log(_randomY);

        var _position = _gameObject.transform.position;
        var x = _position.x;
        var y = _position.y;
        var z = _position.z;
        y = _randomY;

        syncCube = new Vector3(x, _randomY, z);
        
        _gameObject.transform.position = syncCube;
    }
}