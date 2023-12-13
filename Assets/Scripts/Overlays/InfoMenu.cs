

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
                bool firstPage = page == 0;
                bool lastPage = page == pages.Length-1;
                prevButton.interactable = !firstPage;
                nextButton.interactable = !lastPage;
                if (firstPage)
                {
                    EventSystem.current.SetSelectedGameObject((lastPage ? exitButton : nextButton).gameObject);
                }
                if (lastPage)
                {
                    EventSystem.current.SetSelectedGameObject((firstPage ? exitButton : prevButton).gameObject);
                }
            }
        }
    }

    void OnEnable()
    {
        Page = 0;
        EventSystem.current.SetSelectedGameObject((pages.Length > 1 ? nextButton : exitButton).gameObject);
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