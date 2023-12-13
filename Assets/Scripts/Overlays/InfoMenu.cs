

using UnityEngine;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
    public GameObject[] pages;
    public Button prevButton;
    public Button nextButton;
    int page;

    internal int Page
    {
        get => page;
        set
        {
            if (value >= 0 && value < pages.Length)
            {
                pages[page].SetActive(false);
                page = value;
                pages[page].SetActive(true);
                prevButton.interactable = page > 0;
                nextButton.interactable = page < pages.Length-1;
            }
        }
    }

    void OnEnable()
    {
        Page = 0;
    }

    public void PrevPage()
    {
        Page--;
    }

    public void NextPage()
    {
        Page++;
    }
}