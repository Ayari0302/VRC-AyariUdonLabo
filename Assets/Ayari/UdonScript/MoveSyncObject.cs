using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MoveSyncObject : UdonSharpBehaviour
{
    [SerializeField] private GameObject _gameObject;
    private float xAmountOfMovement = 4.08f;
    private float zAmountOfMovement = 4.39f;
    private CubeUI _cubeUi;

    private void Start()
    {
        _cubeUi = gameObject.transform.root.gameObject.GetComponent<CubeUI>();
    }

    public override void Interact()
    {
        TransferOfAuthority();

        switch (gameObject.name)
        {
            case "forward":
                Forward();
                break;
            case "backward":
                Backward();
                break;
            case "leftward":
                Leftward();
                break;
            case "rightward":
                Rightward();
                break;
        }

        _cubeUi.OnSetActive(false);
    }

    private void TransferOfAuthority()
    {
        if (Networking.GetOwner(this.gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);

        // 他人から見た操作するオブジェクトのオーナー権限を譲渡
        if (Networking.GetOwner(_gameObject) != Networking.LocalPlayer)
            Networking.SetOwner(Networking.LocalPlayer, _gameObject);
    }

    public void Forward()
    {
        TransferOfAuthority();
        var _position = _gameObject.transform.position;
        var z = _position.z + zAmountOfMovement;

        _position = new Vector3(_position.x, _position.y, z);
        _gameObject.transform.position = _position;
    }

    public void Backward()
    {
        TransferOfAuthority();
        var _position = _gameObject.transform.position;
        var z = _position.z - zAmountOfMovement;

        _position = new Vector3(_position.x, _position.y, z);
        _gameObject.transform.position = _position;
    }

    public void Leftward()
    {
        TransferOfAuthority();
        var _position = _gameObject.transform.position;
        var x = _position.x - xAmountOfMovement;

        _position = new Vector3(x, _position.y, _position.z);
        _gameObject.transform.position = _position;
    }

    public void Rightward()
    {
        TransferOfAuthority();
        var _position = _gameObject.transform.position;
        var x = _position.x + xAmountOfMovement;

        _position = new Vector3(x, _position.y, _position.z);
        _gameObject.transform.position = _position;
    }
}