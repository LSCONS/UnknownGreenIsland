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
        // RenderTexture ũ�⸦ �����մϴ�.
        _texture.width = iconWidth;
        _texture.height = iconHeight;
        _texture.antiAliasing = 4; 

        // ī�޶� RenderTexture�� �Ҵ��ϰ� ������
        _camera.targetTexture = _texture;
        _camera.Render();

        // RenderTexture�� ������ Texture2D�� �о�ɴϴ�.
        RenderTexture.active = _texture;
        Texture2D iconTexture = new Texture2D(iconWidth, iconHeight, TextureFormat.ARGB32, false);
        iconTexture.ReadPixels(new Rect(0, 0, iconWidth, iconHeight), 0, 0);
        iconTexture.Apply();

        // ����
        GL.Clear(true, true, Color.clear);
        _camera.targetTexture = null;
        RenderTexture.active = null;


        SaveIconTexture(iconTexture, iconName);
    }

#if UNITY_EDITOR
    // ĸó�� ������ �ؽ�ó�� ������ ��ο� PNG ���Ϸ� �����ϴ� �޼���
    public void SaveIconTexture(Texture2D iconTexture, string fileName)
    {
        // �ؽ�ó�� PNG ������ ����Ʈ �迭�� ���ڵ�
        byte[] pngData = iconTexture.EncodeToPNG();
        if (pngData != null)
        {
            // ���ϴ� ���� ���: Assets/05. Resources/Icon
            string folderPath = "Assets/05. Resources/Icon";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            // ���� ��� ����
            string filePath = Path.Combine(folderPath, fileName + ".png");
            // ���� ����
            File.WriteAllBytes(filePath, pngData);
            Debug.Log("�������� ����Ǿ����ϴ�: " + filePath);
            // �����Ϳ� ��������� �ݿ�
            AssetDatabase.Refresh();
        }
    }
#endif
}
