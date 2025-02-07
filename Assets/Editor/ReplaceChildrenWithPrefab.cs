using UnityEngine;
using UnityEditor;

public class ReplaceChildrenWithPrefab : EditorWindow
{
    // 부모 오브젝트 (예: mushroom empty object)
    private GameObject parentObject;
    // 교체할 prefab (예: mushroom prefab)
    private GameObject prefab;

    [MenuItem("Tools/Replace Children With Prefab")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceChildrenWithPrefab>("자식 prefab 교체");
    }

    private void OnGUI()
    {
        GUILayout.Label("부모 오브젝트 자식들을 prefab 인스턴스로 교체", EditorStyles.boldLabel);

        parentObject = (GameObject)EditorGUILayout.ObjectField("부모 오브젝트", parentObject, typeof(GameObject), true);
        prefab = (GameObject)EditorGUILayout.ObjectField("원본 prefab", prefab, typeof(GameObject), false);

        if (GUILayout.Button("교체 실행"))
        {
            if (parentObject == null)
            {
                EditorUtility.DisplayDialog("오류", "부모 오브젝트를 선택하세요.", "확인");
                return;
            }
            if (prefab == null)
            {
                EditorUtility.DisplayDialog("오류", "원본 prefab을 선택하세요.", "확인");
                return;
            }

            int replacedCount = 0;
            // 부모 오브젝트의 모든 자식을 배열로 복사 (자식 삭제 시 for문 문제가 발생할 수 있으므로)
            Transform[] children = parentObject.GetComponentsInChildren<Transform>(true);
            // 부모 오브젝트 자신은 제외
            foreach (Transform child in children)
            {
                if (child.parent != parentObject.transform)
                    continue; // 직접적인 자식만 처리

                // 기존 자식의 트랜스폼 정보 복사
                Vector3 pos = child.position;
                Quaternion rot = child.rotation;
                Vector3 scale = child.localScale;

                // prefab 인스턴스 생성 (Undo 지원)
                GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                if(newInstance == null)
                {
                    Debug.LogError("prefab 인스턴스 생성 실패");
                    continue;
                }
                Undo.RegisterCreatedObjectUndo(newInstance, "Create Prefab Instance");
                newInstance.transform.SetParent(parentObject.transform);
                newInstance.transform.position = pos;
                newInstance.transform.rotation = rot;
                newInstance.transform.localScale = scale;

                // 기존 자식 제거 (Undo 지원)
                Undo.DestroyObjectImmediate(child.gameObject);

                replacedCount++;
            }

            EditorUtility.DisplayDialog("완료", $"{replacedCount}개의 자식 오브젝트가 prefab 인스턴스로 교체되었습니다.", "확인");
        }
    }
}
