using System.IO;
using UnityEngine;
using VInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class IconCapture : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RenderTexture _texture;
    [SerializeField] private int iconWidth = 1024;
    [SerializeField] private int iconHeight = 1024;
    [SerializeField] private string iconName = "test";

    [Button]
    public void CaptureIcon()
    {
        // RenderTexture 크기를 설정합니다.
        _texture.width = iconWidth;
        _texture.height = iconHeight;
        _texture.antiAliasing = 4; 

        // 카메라에 RenderTexture를 할당하고 렌더링
        _camera.targetTexture = _texture;
        _camera.Render();

        // RenderTexture의 내용을 Texture2D로 읽어옵니다.
        RenderTexture.active = _texture;
        Texture2D iconTexture = new Texture2D(iconWidth, iconHeight, TextureFormat.ARGB32, false);
        iconTexture.ReadPixels(new Rect(0, 0, iconWidth, iconHeight), 0, 0);
        iconTexture.Apply();

        // 정리
        GL.Clear(true, true, Color.clear);
        _camera.targetTexture = null;
        RenderTexture.active = null;


        SaveIconTexture(iconTexture, iconName);
    }

#if UNITY_EDITOR
    // 캡처한 아이콘 텍스처를 지정한 경로에 PNG 파일로 저장하는 메서드
    public void SaveIconTexture(Texture2D iconTexture, string fileName)
    {
        // 텍스처를 PNG 포맷의 바이트 배열로 인코딩
        byte[] pngData = iconTexture.EncodeToPNG();
        if (pngData != null)
        {
            // 원하는 폴더 경로: Assets/05. Resources/Icon
            string folderPath = "Assets/05. Resources/Icon";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            // 파일 경로 생성
            string filePath = Path.Combine(folderPath, fileName + ".png");
            // 파일 쓰기
            File.WriteAllBytes(filePath, pngData);
            Debug.Log("아이콘이 저장되었습니다: " + filePath);
            // 에디터에 변경사항을 반영
            AssetDatabase.Refresh();
        }
    }
#endif
}
