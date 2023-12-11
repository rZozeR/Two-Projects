using UnityEngine;

public class Block : MonoBehaviour
{
    public bool Clicked { get; private set; }

    private GameObject _childX;

    private void Awake()
    {
        _childX = transform.GetChild(0).gameObject;
    }

    public void OpenClick(bool state)
    {
        Clicked = state;
        _childX.SetActive(Clicked);
    }
}
