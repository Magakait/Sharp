using UnityEngine;

public class NothingManager : MonoBehaviour
{
    public StringVariable code;

    [Space(10)]
    public GameObject[] objects;
    public GameObject[] panels;

    private void Awake()
    {
        CameraManager.Position = Vector3.zero;

        for (int i = 0; i < objects.Length; i++)
            if (objects[i].name == code.Value)
            {
                objects[i].SetActive(true);
                panels[i].SetActive(true);

                break;
            }
    }
}