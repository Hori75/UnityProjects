using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class Resources
{
    public int Fuel;
    public int Ammo;
    public int Scraps;
}

[System.Serializable]
public class Choice
{
    public Resources ResourceEffect;
    public int NextDialog;
    public bool DialogOpen;
}

[System.Serializable]
public class EventDialog
{
    public string Dialog;
    public string[] Choices;

    public Choice[] ChoiceEffect;
}

[System.Serializable]
public class StoryObject
{
    public string EventName;
    public EventDialog[] eventdialog;
}

[System.Serializable]
public class Stories
{
    public StoryObject[] stories;

    public static Stories LoadStories(string Filename)
    {
        string StoryPath = Application.streamingAssetsPath + "/Stories/" + Filename;
        string jsonString = File.ReadAllText(StoryPath);
        return JsonUtility.FromJson<Stories>(jsonString);
    }
}

public class StoryManager : MonoBehaviour
{
    GameSystem gameSystem;

    Stories RandomAttack;
    Stories MainStory;
    StoryObject CurrentEvent;
    EventDialog CurrentDialog;

    public GameObject DialogBox;
    Text EventStory;
    GameObject Choice1;
    GameObject Choice2;
    GameObject Choice3;

    Text TextChoice1;
    Text TextChoice2;
    Text TextChoice3;

    void Start()
    {
        gameSystem = gameObject.GetComponent<GameSystem>();

        RandomAttack = Stories.LoadStories("RandomAttack.json");
        MainStory = Stories.LoadStories("MainStory.json");

        InitializeDialogBox();
    }

    public void NewGameStory()
    {
        CurrentEvent = MainStory.stories[0];
        CurrentDialog = CurrentEvent.eventdialog[0];
        UpdateDialogBox();
    }

    void InitializeDialogBox()
    {
        Choice1 = DialogBox.transform.Find("ChoiceText1").gameObject;
        Choice2 = DialogBox.transform.Find("ChoiceText2").gameObject;
        Choice3 = DialogBox.transform.Find("ChoiceText3").gameObject;
        EventStory = DialogBox.transform.Find("EventText").gameObject.GetComponent<Text>();
        TextChoice1 = Choice1.GetComponent<Text>();
        TextChoice2 = Choice2.GetComponent<Text>();
        TextChoice3 = Choice3.GetComponent<Text>();
    }

    void UpdateDialogBox()
    {
        gameSystem.LockAllButtons();

        EventDialog Dialog;

        Dialog = CurrentDialog;

        EventStory.text = Dialog.Dialog;
        TextChoice1.text = Dialog.Choices[0];
        TextChoice2.text = Dialog.Choices[1];
        TextChoice3.text = Dialog.Choices[2];

        if (TextChoice2.text == " ")
        {
            Choice2.GetComponent<Button>().interactable = false;
        }
        else
        {
            Choice2.GetComponent<Button>().interactable = true;
        }

        if (TextChoice3.text == " ")
        {
            Choice3.GetComponent<Button>().interactable = false;
        }
        else
        {
            Choice3.GetComponent<Button>().interactable = true;
        }

        DialogBox.SetActive(true);
    }

    void CloseDialog()
    {
        Debug.Log("CloseDialog");
        DialogBox.SetActive(false);
        gameSystem.UnlockAllButtons();
    }

    void AdvanceDialog(int choice)
    {
        Choice PickedChoice;
        bool Dialog;

        PickedChoice = CurrentDialog.ChoiceEffect[choice];
        Dialog = PickedChoice.DialogOpen;

        gameSystem.ApplyEffect(PickedChoice.ResourceEffect);
        ChangeCurrentDialog(PickedChoice.NextDialog);

        if (Dialog)
        {
            UpdateDialogBox();
        }
        else
        {
            CloseDialog();
        }
    }

    public void PickChoice1()
    {
        AdvanceDialog(0);
    }

    public void PickChoice2()
    {
        AdvanceDialog(1);
    }

    public void PickChoice3()
    {
        AdvanceDialog(2);
    }

    void ChangeCurrentDialog(int NextDialog)
    {
        if (NextDialog == 0)
        {
            return;
        }
        else CurrentDialog = CurrentEvent.eventdialog[NextDialog];
    }

    public void ChangeCurrentStory()
    {
        float Chance;

        Chance = Mathf.Floor(Random.Range(1, 100));

        if (Chance < 91)
        {
            CurrentEvent = RandomAttack.stories[0];
            CurrentDialog = CurrentEvent.eventdialog[0];
        }
        else
        {
            CurrentEvent = MainStory.stories[0];
            CurrentDialog = CurrentEvent.eventdialog[0];
        }

        UpdateDialogBox();
    }
}
