using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Hit : UdonSharpBehaviour
{
    void Start()
    {
    }
    
    private void TransferOfAuthority()
    {
        if (Networking.GetOwner(this.gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
    }

//    void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "red")
//        {
//            TransferOfAuthority();
//            GetRed(other.gameObject);
//        }
//
//        if (other.tag == "blue")
//        {
//            TransferOfAuthority();
//            GetBlue(other.gameObject);
//        }
//    }

    private void OnTriggerEnter(Collider test)
    {
        Debug.Log(test.gameObject.name);
        if (test.gameObject.name == "blockA")
        {
            test.gameObject.transform.position = new Vector3(0,0,0);
        }

        if (test.gameObject.name == "blockB")
        {
            
            test.gameObject.transform.position = new Vector3(0,0,0);
        }
    }

    void GetRed(GameObject obj)
    {
        switch (obj.name)
        {
            case "1b":
                obj.transform.position = new Vector3();
                break;
            case "1c":
                break;
            case "1d":
                break;
            case "1e":
                break;

            case "2b":
                break;
            case "2c":
                break;
            case "2d":
                break;
            case "2e":
                break;
        }
        obj.transform.position = new Vector3();
    }
    
    void GetBlue(GameObject obj)
    {
        switch (obj.name)
        {
            case "5b":
                break;
            case "5c":
                break;
            case "5d":
                break;
            case "5e":
                break;

            case "6b":
                break;
            case "6c":
                break;
            case "6d":
                break;
            case "6e":
                break;
        }
        obj.transform.position = new Vector3();
    }
}