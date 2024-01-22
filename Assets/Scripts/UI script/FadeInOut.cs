// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;
//
//
// public class FadeInOut : MonoBehaviour
//
// {
//     [SerializeField] private Image fadeImage;
//     [SerializeField] private bool isFadeIn;
//     [SerializeField] private float playTime;
//     [SerializeField] private float delayTime;
//     private Color _color;
//
//     private void Awake()
//     {
//         //fadeImage.gameObject.SetActive(true);
//         //SetAlpha(0f);
//     }
//
//     public void SetTime(float playTime, float delayTime)
//     {
//         this.playTime = playTime;
//         this.delayTime = delayTime;
//     }
//
//     private void SetAlpha(float alpha)
//     {        
//         _color = fadeImage.color;
//         _color.a = alpha;
//         fadeImage.color = _color;    
//     }
//
//     // private IEnumerator FadeIn()
//     // {
//     //     float t = 0f;
//     //
//     //     float start = 0f;
//     //     float end = 1f;
//     //     SetAlpha(0f);
//     //
//     //     yield return new WaitForSeconds(delayTime);
//     //
//     //     while(t < playTime)
//     //     {
//     //         t += Time.deltaTime;
//     //         SetAlpha(Mathf.Lerp(start, end, t));
//     //
//     //         yield return null;
//     //     }
//     // }
//     //
//     // private IEnumerator FadeOut()
//     // {
//     //     float t = 0f;
//     //     float start = 1f;
//     //     float end = 0f;
//     //     SetAlpha(1f);
//     //     
//     //
//     //     yield return new WaitForSeconds(delayTime);
//     //
//     //     while(t < playTime)
//     //     {
//     //         t += Time.deltaTime;
//     //         SetAlpha(Mathf.Lerp(start, end, t));
//     //
//     //         yield return null;
//     //     }
//     // }
// }