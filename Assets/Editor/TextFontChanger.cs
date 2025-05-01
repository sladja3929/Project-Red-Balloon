using UnityEngine;
using UnityEditor;
using TMPro;

public class TextFontChanger : EditorWindow
{
    // 변경을 시작할 부모 GameObject와 새 폰트 에셋을 지정할 필드
    private GameObject parentObject;
    private TMP_FontAsset newFontAsset;

    [MenuItem("Tools/Text Font Changer")]
    public static void ShowWindow()
    {
        GetWindow<TextFontChanger>("Text Font Changer");
    }

    void OnGUI()
    {
        GUILayout.Label("텍스트 폰트 교체", EditorStyles.boldLabel);
        parentObject = (GameObject)EditorGUILayout.ObjectField("부모 오브젝트", parentObject, typeof(GameObject), true);
        newFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("새 폰트 에셋", newFontAsset, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("폰트 교체 실행"))
        {
            if (parentObject == null || newFontAsset == null)
            {
                EditorUtility.DisplayDialog("오류", "부모 오브젝트와 폰트 에셋을 모두 지정해야 합니다.", "확인");
            }
            else
            {
                ChangeFontRecursive(parentObject, newFontAsset);
                EditorUtility.DisplayDialog("완료", "폰트 변경 작업이 완료되었습니다.", "확인");
            }
        }
    }

    // 재귀적으로 GameObject와 자식들을 탐색하여 이름에 "Text"가 포함된 경우 폰트를 변경
    private void ChangeFontRecursive(GameObject obj, TMP_FontAsset fontAsset)
    {
        if (obj.name.Contains("Text"))
        {
            // TextMeshPro - Text (UI)는 TextMeshProUGUI 컴포넌트임
            TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                Undo.RecordObject(tmp, "Change Font Asset");
                tmp.font = fontAsset;
                EditorUtility.SetDirty(tmp);
            }
        }

        // 모든 자식에 대해 재귀 호출
        foreach (Transform child in obj.transform)
        {
            ChangeFontRecursive(child.gameObject, fontAsset);
        }
    }
}