using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public delegate void SizeEventHandler(int size);
    public static event SizeEventHandler OnSizeChange;

    [SerializeField] private TextMeshProUGUI _textMatchCount;

    private void Awake()
    {
        DisplayMatchCount(0);
    }

    private void OnEnable()
    {
        MatchManager.OnValidMatch += DisplayMatchCount;
    }

    private void OnDisable()
    {
        MatchManager.OnValidMatch -= DisplayMatchCount;
    }

    private void DisplayMatchCount(int matches)
    {
        _textMatchCount.text = $"Match Count : {matches}";
    }

    public void OnInputFieldEndEdit(GameObject inputFieldObject)
    {
        TMP_InputField inputField = inputFieldObject.GetComponent<TMP_InputField>();

        string text = inputField.text;
        
        if (string.IsNullOrEmpty(text))
        {
            Debug.LogError("Field is empty!");
            return;
        }

        if (int.TryParse(inputField.text, out int value))
        {
            if (value <= 1)
            {
                Debug.LogError("Invalid Size!");
                return;
            }

            OnSizeChange?.Invoke(value);
        }
        else
        {
            Debug.LogError("Input is not a valid integer!");
            return;
        }
    }
}
