
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class NonSyncConter : UdonSharpBehaviour
{
    [SerializeField] private Text _text;

    [UdonSynced(UdonSyncMode.NotSynced)]
    private int _num;
    
    void Start()
    {
        _num = 0;
    }

    private void Update()
    {
        _text.text = _num.ToString();
    }

    public override void Interact()
    {
        if(Networking.GetOwner(this.gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        
        // 処理同期
        //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "CountUp");
        
        // ローカル処理
        LocalCountUp();
    }

    public void LocalCountUp()
    {
        _num++;
    }
}
