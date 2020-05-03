using UnityEngine;
using System.Collections;
using System.IO;

public class _R 
{
    public static MonoBehaviour _Attachment;

    public static Object Load(string path)
    {
        return Resources.Load(path);
    }

    public static void Load(string path, System.Action<Object> callback)
    {
        if (_Attachment == null) return;
        var request = Resources.LoadAsync(path);
        _Attachment.StartCoroutine(LoadCoroutine(request, callback));
    }

    public static IEnumerator LoadCoroutine(ResourceRequest request, System.Action<Object> callback)
    {
        while (!request.isDone)
            yield return 0;
        if (request.asset != null)
            callback(request.asset);
        else
            Debug.LogErrorFormat("{0} Load Error!");
    }
}
