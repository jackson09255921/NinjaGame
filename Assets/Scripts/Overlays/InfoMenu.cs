

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
    public GameObject[] pages;
    public Button prevButton;
    public Button nextButton;
    public Button exitButton;
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
                if (!prevButton.interactable && EventSystem.current.currentSelectedGameObject == prevButton.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject((nextButton.interactable ? nextButton : exitButton).gameObject);
                }
                if (!nextButton.interactable && EventSystem.current.currentSelectedGameObject == nextButton.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject((prevButton.interactable ? prevButton : exitButton).gameObject);
                }
            }
        }
    }

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject((pages.Length > 1 ? nextButton : exitButton).gameObject);
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