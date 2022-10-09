using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    [SerializeField] private int _ButtonNumber;

    private string textID = "";
    private Image imageComponent;

    public string TextID
    {
        get { return textID; } set { textID = value; }
    }

    public int ButtonNumber
    {
        get { return _ButtonNumber; }
    }

    public void SetFieldImage(Sprite sqr)
    {
        imageComponent = GetComponent<Image>();
        imageComponent.sprite = sqr;
        imageComponent.color = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, 1f);
    }

    public void BlockInput(bool state)
    {
        if (state)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    public void OnClick()
    {
        GameManager.Instance.onInteractEvent?.Invoke(_ButtonNumber);
    }
}
