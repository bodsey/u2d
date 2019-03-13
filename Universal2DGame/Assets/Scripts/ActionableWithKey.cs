using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionableWithKey : MonoBehaviour {

    public List<string> KeyTags;
    public GameObject ObjectToDeactivate;
    public bool ReactivateOnKeyLeave = false;
    public bool DestroyKey = true;

    private GameObject currentKey;

	void testKey(GameObject go)
    {
        if (KeyTags.Contains(go.tag) == false) return;
        if(ObjectToDeactivate == null)
        {
            Debug.LogWarning("TRYING TO OPEN DOOR WITHOUT A DOOR");
            return;
        }
        currentKey = go;
        if (DestroyKey)
            Destroy(go);
        ObjectToDeactivate.SetActive(false);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        testKey(collision.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        testKey(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ReactivateOnKeyLeave && collision.gameObject == currentKey)
            ObjectToDeactivate.SetActive(true);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (ReactivateOnKeyLeave && collision.gameObject == currentKey)
            ObjectToDeactivate.SetActive(true);
    }
}
