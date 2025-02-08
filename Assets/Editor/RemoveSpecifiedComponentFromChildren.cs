using UnityEngine;
using UnityEditor;
using System;

public class RemoveSpecifiedComponentFromChildren : EditorWindow
{
    // 대상 오브젝트 (예: Tree 오브젝트)
    private GameObject targetObject;
    // 제거할 컴포넌트의 타입 이름을 문자열로 입력받음
    private string componentTypeName = "Rigidbody";

    [MenuItem("Tools/Remove Component By Name From Children")]
    public static void ShowWindow()
    {
        GetWindow<RemoveSpecifiedComponentFromChildren>("컴포넌트 제거");
    }

    private void OnGUI()
    {
        GUILayout.Label("대상 오브젝트 자식의 지정 컴포넌트 제거", EditorStyles.boldLabel);

        // 대상 오브젝트 지정
        targetObject = (GameObject)EditorGUILayout.ObjectField("대상 오브젝트", targetObject, typeof(GameObject), true);

        // 제거할 컴포넌트 타입 이름 입력
        componentTypeName = EditorGUILayout.TextField("제거할 컴포넌트 타입", componentTypeName);

        if (GUILayout.Button("제거 실행"))
        {
            if (targetObject == null)
            {
                EditorUtility.DisplayDialog("오류", "대상 오브젝트를 선택하세요.", "확인");
                return;
            }

            if (string.IsNullOrEmpty(componentTypeName))
            {
                EditorUtility.DisplayDialog("오류", "제거할 컴포넌트 타입 이름을 입력하세요.", "확인");
                return;
            }

            // 입력받은 문자열을 기반으로 타입 검색
            Type compType = GetComponentType(componentTypeName);
            if (compType == null || !typeof(Component).IsAssignableFrom(compType))
            {
                EditorUtility.DisplayDialog("오류", "유효한 컴포넌트 타입이 아닙니다.", "확인");
                return;
            }

            int count = 0;
            // 대상 오브젝트의 모든 자식(비활성 포함)에서 지정한 타입의 컴포넌트 검색
            Component[] components = targetObject.GetComponentsInChildren(compType, true);
            foreach (Component comp in components)
            {
                // Undo 기능 지원: 실수로 제거한 경우 Ctrl+Z 혹은 Command+Z로 복구 가능
                Undo.DestroyObjectImmediate(comp);
                count++;
            }

            EditorUtility.DisplayDialog("완료", $"{compType.Name} 컴포넌트가 {count}개 제거되었습니다.", "확인");
        }
    }

    /// <summary>
    /// 입력받은 타입 이름으로 컴포넌트의 타입을 반환합니다.
    /// 만약 직접 GetType으로 찾지 못하면, UnityEngine 네임스페이스를 붙여서 다시 시도합니다.
    /// </summary>
    /// <param name="typeName">컴포넌트 타입 이름 (예: "Rigidbody")</param>
    /// <returns>찾은 타입 또는 null</returns>
    private Type GetComponentType(string typeName)
    {
        // 우선, 전체 이름을 입력받았을 경우 직접 검색
        Type type = Type.GetType(typeName);
        if (type != null)
            return type;

        // UnityEngine 네임스페이스가 누락된 경우, 붙여서 검색 (예: "UnityEngine.Rigidbody")
        type = Type.GetType("UnityEngine." + typeName + ", UnityEngine");
        if (type != null)
            return type;

        // 혹은 다른 어셈블리 내에 있을 수도 있으므로, 추가 검색을 진행할 수 있음
        // 간단한 예제로는 여기까지 처리

        return null;
    }
}
