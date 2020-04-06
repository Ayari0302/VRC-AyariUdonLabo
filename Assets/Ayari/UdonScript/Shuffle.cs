using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Shuffle : UdonSharpBehaviour
{
    void Start()
    {
    }

    public GameObject[] ShuffleObjects;
    public Vector3[] ShuffleWork;


    public void ShuffleTest()
    {
        ShuffleObjects = ArrayShuffle(ShuffleObjects);


        foreach (var obj in ShuffleObjects)
        {
            Debug.Log(obj);
        }

        //Debug.Log(ShuffleObjects);
    }

    public GameObject[] ArrayShuffle(GameObject[] array)
    {
        var length = array.Length;
        var result = new GameObject[length];

        // 一次元配列の内容をresultにコピー
        array.CopyTo(result, 0);

        for (int i = 0; i < length; i++)
        {
            var tmp = result[i];
            var randomIndex = Random.Range(i, length);
            result[i] = result[randomIndex];
            result[randomIndex] = tmp;
        }

        return result;
    }

    [SerializeField] private GameObject _spawnObject;

    public override void Interact()
    {
        Spawn();
    }

    public void Spawn()
    {
        GameObject obj = VRCInstantiate(_spawnObject);
    }

    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioSource _audioSource;

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        _audioSource.PlayOneShot(_audioClip);
    }

}