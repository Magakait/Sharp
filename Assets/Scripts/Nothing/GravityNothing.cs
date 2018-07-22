using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class GravityNothing : MonoBehaviour
{
    public float gravity = 0.1f;
    public float initialBurst = 20;
    public int maxCount = 7;

    [Space(10)]
    public float spawnSphereScale;
    public List<Rigidbody> electrons;

    [Space(10)]
    public Button addButton;
    public Button removeButton;

    private void FixedUpdate()
    {
        foreach (Rigidbody item in electrons)
            item.velocity += gravity * (transform.position - item.position);
    }

    public void AddElectron()
    {
        Rigidbody electron = Instantiate(electrons[0], transform);

        electron.gameObject.SetActive(true);
        electron.transform.position = spawnSphereScale * Random.insideUnitSphere;
        electron.velocity = initialBurst * Random.insideUnitSphere;
        
        electrons.Add(electron);
    }

    public void RemoveElectron()
    {
        int index = electrons.Count - 1;

        electrons[index].velocity = Vector3.zero;
        electrons.RemoveAt(index);
    }

    public void UpdateButtons()
    {
        addButton.interactable = electrons.Count < maxCount;
        removeButton.interactable = electrons.Count > 1;
    }
}