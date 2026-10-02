using UnityEditor;
using UnityEngine;

public static class CheckWebCompression
{
    [MenuItem("Tools/Set Web Compression Disabled")]
    public static void SetDisabled()
    {
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;

        Debug.Log("Web Compression Format changed to = " +
                  PlayerSettings.WebGL.compressionFormat);
    }
}