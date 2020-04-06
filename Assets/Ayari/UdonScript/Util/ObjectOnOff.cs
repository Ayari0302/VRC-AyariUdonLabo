using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ObjectOnOff : UdonSharpBehaviour
{
    [SerializeField] private GameObject _gameObject;

    public void ObjectActiveToggle()
    {
        _gameObject.SetActive(!_gameObject.activeSelf);
    }
}