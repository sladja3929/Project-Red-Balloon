using UnityEngine;
using UnityEditor;
using TMPro;         // TextMeshPro 관련 네임스페이스
using System.IO;   // 파일 입출력 관련 네임스페이스
using System.Collections.Generic;

public class TextExtractorWindow : EditorWindow
{
    private GameObject targetObject;  // 대상 GameObject를 할당받을 변수

    // 메뉴에 "Extract TextMeshPro Texts" 항목 추가
    [MenuItem("Tools/Extract TextMeshPro Texts")]
    public static void ShowWindow()
    {
        GetWindow<TextExtractorWindow>("Text Extractor");
    }

    private void OnGUI()
    {
        GUILayout.Label("대상 GameObject 선택", EditorStyles.boldLabel);
        targetObject = (GameObject)EditorGUILayout.ObjectField("Target GameObject", targetObject, typeof(GameObject), true);

        if (GUILayout.Button("텍스트 추출 및 파일 저장"))
        {
            if (targetObject == null)
            {
                EditorUtility.DisplayDialog("오류", "대상 GameObject를 지정하세요.", "확인");
            }
            else
            {
                ExtractTexts();
            }
        }
    }

    private void ExtractTexts()
    {
        // 대상 GameObject와 모든 자식에서 Transform 컴포넌트를 가져옴(비활성 오브젝트 포함)
        Transform[] allTransforms = targetObject.GetComponentsInChildren<Transform>(true);
        List<string> textLines = new List<string>();

        // 각 Transform에 대해 이름에 "Text"가 포함되어 있는지 체크
        foreach (Transform t in allTransforms)
        {
            if (t.gameObject.name.Contains("Text"))
            {
                // TextMeshPro 컴포넌트(TMP_Text)를 가져옴.
                TMP_Text tmpText = t.gameObject.GetComponent<TMP_Text>();
                if (tmpText != null)
                {
                    textLines.Add(tmpText.text);
                }
                else
                {
                    Debug.LogWarning($"오브젝트 '{t.gameObject.name}'에 TMP_Text 컴포넌트가 없습니다.");
                }
            }
        }

        // 파일 저장 경로를 선택하는 창을 띄움 (기본 경로는 프로젝트의 Assets 폴더)
        string filePath = EditorUtility.SaveFilePanel("텍스트 파일 저장", Application.dataPath, "ExtractedTexts", "txt");
        if (string.IsNullOrEmpty(filePath))
        {
            return; // 저장 취소 시 종료
        }

        // 텍스트 내용을 줄 단위로 파일에 작성
        File.WriteAllLines(filePath, textLines);

        // 파일이 프로젝트 내에 저장되었다면 에셋 데이터베이스 새로고침
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("성공", $"텍스트 파일이 저장되었습니다:\n{filePath}", "확인");
    }
}
