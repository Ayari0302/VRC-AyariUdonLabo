#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using RootMotion.FinalIK;
using Object = UnityEngine.Object;

#endif

public class AyariUtilWindow : EditorWindow
{
    [MenuItem("Editor/AyariUtil")]
    private static void Create()
    {
        // 生成
        var window = GetWindow<AyariUtilWindow>("AyariUtilWindow");
    }

    private void OnSelectionChange()
    {
        var editorEvent = EditorGUIUtility.CommandEvent("ChangeActiveObject");
        editorEvent.type = EventType.Used;
        SendEvent(editorEvent);
    }

    private static Transform CreateTarget(GameObject obj, string targetName)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = targetName;

        cube.transform.parent = obj.transform;
        cube.transform.localPosition = new Vector3(0, 0f, 0);
        cube.transform.parent = null;
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        objectList.Add(cube.transform);
        return cube.transform;
    }

    private static List<Transform> objectList;
    private static string pattern = "";
    
    public static List<GameObject>  GetAll (GameObject obj)
    {
        var allChildren = new List<GameObject> ();
        GetChildren (obj, ref allChildren);
        return allChildren;
    }

    private static void GetChildren (GameObject obj, ref List<GameObject> allChildren)
    {
        var children = obj.GetComponentInChildren<Transform> ();
        //子要素がいなければ終了
        if (children.childCount == 0) {
            return;
        }
        foreach (Transform ob in children) {
            allChildren.Add (ob.gameObject);
            GetChildren (ob.gameObject, ref allChildren);
        }
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle(GUI.skin.label) {wordWrap = true};

        EditorGUILayout.LabelField("アクティブなオブジェクト");
        var activeObject = Selection.activeGameObject;

        using (new GUILayout.VerticalScope(GUI.skin.box))
        {
            EditorGUILayout.LabelField(activeObject ? activeObject.name : "");
        }

        if (!activeObject)
        {
            return;
        }
        
        pattern = EditorGUILayout.TextField ("子階層", pattern);

        
        
        
        //子オブジェクト検索用
        if (GUILayout.Button("検索"))
        {
            var list = GetAll(activeObject);
            pattern = pattern.ToLower();
            
            var hitList = new List<GameObject>();
            foreach (var obj in list)
            {
                //文字列比較
                if (pattern == "") return;
                if (obj.name.ToLower().Contains(pattern))
                {
                    hitList.Add(obj);

                    Selection.activeGameObject = obj;
                }
            }

            Object[] array = hitList.ToArray();
            Selection.objects = array;
        }
        
        //検索窓の文字列初期化
        if (GUILayout.Button("クリア"))
        {
            GUIUtility.keyboardControl = 0;
            pattern = "";
        }

        // fbbikが選択されてない場合以下を実行しない
        if (!activeObject.GetComponent<FullBodyBipedIK>())
        {
            EditorGUILayout.HelpBox("FullBodyBipedIKが設定されてるオブジェクトを選択してください。", MessageType.Warning);
            return;
        }

        using (new GUILayout.VerticalScope(GUI.skin.box))
        {
            GUILayout.Label(
                "Referencesに登録されている位置に自動的にキューブが配置されます。",
                labelStyle
            );
        }


        if (GUILayout.Button("IK調整用キューブを追加する。"))
        {
            objectList = new List<Transform>();
            CreateIKTarget(activeObject);
        }

        if (GUILayout.Button("すべてのPositionWeightを有効にする。"))
        {
            SetIKWeight(activeObject, 1f);
        }

        if (GUILayout.Button("すべてのPositionWeightを無効にする。"))
        {
            SetIKWeight(activeObject, 0f);
        }
    }

    private static void SetIKWeight(GameObject activeObject, float value)
    {
        // FBBIKのreferencesを取得
        var fbbik = activeObject.GetComponent<FullBodyBipedIK>();
        //全体のWeight
        fbbik.solver.IKPositionWeight = value;

        //それぞれのWeight
        fbbik.solver.bodyEffector.positionWeight = value;

        fbbik.solver.leftHandEffector.positionWeight = value;
        fbbik.solver.leftHandEffector.rotationWeight = value;
        fbbik.solver.leftShoulderEffector.positionWeight = value;

        fbbik.solver.rightHandEffector.positionWeight = value;
        fbbik.solver.rightHandEffector.rotationWeight = value;
        fbbik.solver.rightShoulderEffector.positionWeight = value;

        fbbik.solver.leftFootEffector.positionWeight = value;
        fbbik.solver.leftFootEffector.rotationWeight = value;
        fbbik.solver.leftThighEffector.positionWeight = value;

        fbbik.solver.rightFootEffector.positionWeight = value;
        fbbik.solver.rightFootEffector.rotationWeight = value;
        fbbik.solver.rightThighEffector.positionWeight = value;

        fbbik.solver.chain[1].bendConstraint.weight = value;
        fbbik.solver.chain[2].bendConstraint.weight = value;
        fbbik.solver.chain[3].bendConstraint.weight = value;
        fbbik.solver.chain[4].bendConstraint.weight = value;
    }

    private static void CreateIKTarget(GameObject activeObject)
    {
        // FBBIKのreferencesを取得
        var fbbik = activeObject.GetComponent<FullBodyBipedIK>();

        // body target
        var spineObject = fbbik.references.spine[0].gameObject;
        fbbik.solver.bodyEffector.target = CreateTarget(spineObject, "Spine");

        // leftHand target
        fbbik.solver.leftHandEffector.target = CreateTarget(fbbik.references.leftHand.gameObject, "LeftHand");

        // leftShoulder target
        fbbik.solver.leftShoulderEffector.target =
            CreateTarget(fbbik.references.leftUpperArm.gameObject, "LeftShoulder");

        // leftChain BendGoal
        fbbik.solver.chain[1].bendConstraint.bendGoal =
            CreateTarget(fbbik.references.leftForearm.gameObject, "LeftHandChainBendGoal");

        // rightHand target
        fbbik.solver.rightHandEffector.target = CreateTarget(fbbik.references.rightHand.gameObject, "RightHand");

        // rightShoulder target
        fbbik.solver.rightShoulderEffector.target = CreateTarget(fbbik.references.rightUpperArm.gameObject,
            "RightShoulder");

        // rightChain BendGoal
        fbbik.solver.chain[2].bendConstraint.bendGoal =
            CreateTarget(fbbik.references.rightForearm.gameObject, "RightHandChainBendGoal");

        // leftFoot target
        fbbik.solver.leftFootEffector.target = CreateTarget(fbbik.references.leftFoot.gameObject, "LeftFoot");

        // leftThigh target
        fbbik.solver.leftThighEffector.target = CreateTarget(fbbik.references.leftThigh.gameObject, "LeftThigh");

        // leftFootChain BendGoal
        fbbik.solver.chain[3].bendConstraint.bendGoal =
            CreateTarget(fbbik.references.leftCalf.gameObject, "LeftFootChainBendGoal");

        // rightFoot target
        fbbik.solver.rightFootEffector.target = CreateTarget(fbbik.references.rightFoot.gameObject, "RightFoot");

        // rightThigh target
        fbbik.solver.rightThighEffector.target = CreateTarget(fbbik.references.rightThigh.gameObject, "RightThigh");

        // rightFootChain BendGoal
        fbbik.solver.chain[4].bendConstraint.bendGoal =
            CreateTarget(fbbik.references.rightCalf.gameObject, "RightFootChainBendGoal");

        var emptyGameObject = new GameObject("[AyariUtil]FBBIK Targets");
        foreach (var obj in objectList)
        {
            obj.parent = emptyGameObject.transform;
        }

        objectList = null;
    }
}