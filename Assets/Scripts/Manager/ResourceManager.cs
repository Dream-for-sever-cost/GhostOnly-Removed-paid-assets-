using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
    private readonly Dictionary<string, Object> _resDic = new Dictionary<string, Object>(500);

    public void Init()
    {
        List<GameObject[]> objectArrayList = new List<GameObject[]>();
        objectArrayList.Add(Resources.LoadAll<GameObject>("Prefabs"));
        objectArrayList.Add(Resources.LoadAll<GameObject>("SoundData"));
        objectArrayList.Add(Resources.LoadAll<GameObject>("Sprites"));
        foreach (Object[] resources in objectArrayList)
        {
            foreach (Object resource in resources)
            {
                if (!_resDic.TryAdd(resource.name, resource))
                {
                    Debug.LogWarning($"Add Failed - already contains {resource.name}");
                }
            }
        }
    }

    public Object LoadByName(string name)
    {
        return _resDic[name];
    }

    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(Sprite))
        {
            if (_sprites.TryGetValue(path, out Sprite sprite))
                return sprite as T;

            Sprite sp = Resources.Load<Sprite>(path);
            _sprites.Add(path, sp);
            return sp as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Vector3 pos)
    {
        return Instantiate(path, pos, Quaternion.identity);
    }

    public GameObject Instantiate(string path, Vector3 pos, Vector3 euler)
    {
        Quaternion q = Quaternion.Euler(euler);

        return Instantiate(path, pos, q);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load original : {path}");
            return null;
        }

        /*if (original.TryGetComponent<Poolable>(out Poolable p))
        {
            return Managers.Pool.Pop(original, path, original.transform.position, original.transform.rotation, parent).gameObject;
        }*/

        return Instantiate(original, original.transform.position, original.transform.rotation, parent);
    }

    public GameObject Instantiate(string path, Vector3 position, Quaternion q, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load original : {path}");
            return null;
        }

        /*if (original.TryGetComponent<Poolable>(out Poolable p))
        {
            return Managers.Pool.Pop(original, path, position, q, parent).gameObject;
        }*/

        return Instantiate(original, position, q, parent);
    }

    public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion q, Transform parent = null)
    {
        GameObject go = Object.Instantiate(prefab, position, q, parent);
        go.name = prefab.name;
        return go;
    }


    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        if (!go.activeSelf)
            return;

        /*if (go.TryGetComponent<Poolable>(out Poolable p))
        {
            Managers.Pool.Push(p);
            go = null;
            return;
        }*/

        Object.Destroy(go);
        go = null;
    }

    public void Destroy(MonoBehaviour mob)
    {
        if (mob == null)
            return;
        Destroy(mob.gameObject);
        mob = null;
    }
}