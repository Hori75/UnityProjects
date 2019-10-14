using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSystem : MonoBehaviour
{
    GameObject ResouceUI;
    Text FuelUI;
    Text AmmoUI;
    Text ScrapsUI;

    public Text JumpUI;
    public Text TimeUI;

    public Text TimeAdvanceUI;
    public Button JumpButton;
    public Image JumpButtonImage;
    public Button WaitButton;
    public Image WaitButtonImage;

    public int jumps = 5;
    public int fuel = 5;
    public int Ammo = 30;
    public int Scraps = 100;
    GameObject JumpLight;
    GameObject Playership;
    GameObject Cam;
    Animator JumpLightAnimator;
    AudioManager audiomanager;
    StoryManager storyManager;

    int Day = 1;
    int Cycle = 1;

    bool ActionInProgress = false;
    bool newgame = true;

    public void Start()
    {
        storyManager = gameObject.GetComponent<StoryManager>();
        audiomanager = gameObject.GetComponent<AudioManager>();

        Cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (Cam == null)
        {
            Debug.Log("Cam is null!");
        }

        JumpLight = GameObject.FindGameObjectWithTag("JumpLight");
        if (JumpLight == null)
        {
            Debug.Log("Jumplight is null!");
        }
        JumpLight.SetActive(false);

        Playership = GameObject.FindGameObjectWithTag("Player");
        if (Playership == null)
        {
            Debug.Log("Playership is null!");
        }

        ResouceUI = GameObject.FindGameObjectWithTag("Resources");
        if (ResouceUI == null)
        {
            Debug.Log("Resource UI is null!");

        }

        FuelUI = ResouceUI.transform.Find("FuelText").GetComponent<Text>();
        AmmoUI = ResouceUI.transform.Find("AmmoText").GetComponent<Text>();
        ScrapsUI = ResouceUI.transform.Find("ScrapsText").GetComponent<Text>();

        JumpUI.text = "Jumps left: " + jumps;
        TimeAdvanceUI.text = "";
        TimeUI.text = "Day " + Day + " Cycle " + Cycle;

        UpdateResources();
    }

    private void Update()
    {
        if (newgame)
        {
            newgame = false;
            NewGame();
        }
    }

    void NewGame()
    {
        storyManager.NewGameStory();
    }

    public void ApplyEffect (Resources ResourceEffect)
    {
        fuel += ResourceEffect.Fuel;
    }

    public void LockAllButtons ()
    {
        if (JumpButtonImage.color != Color.red) JumpButtonImage.color = Color.black;
        WaitButtonImage.color = Color.black;
        ActionInProgress = true;
    }

    public void UnlockAllButtons()
    {
        if (JumpButtonImage.color != Color.red) JumpButtonImage.color = Color.white;
        WaitButtonImage.color = Color.white;
        ActionInProgress = false;
    }

    public void PlayerJump()
    {
        if (ActionInProgress) return;

        fuel--;

        StartCoroutine(Jump());
    }

    public void PlayerWait()
    {
        if (ActionInProgress) return;
        StartCoroutine(Wait());
    }

    // This is to update the UI
    void UpdateResources()
    {
        FuelUI.text = "Fuel: " + fuel;

        if (fuel == 0)
        {
            JumpButton.interactable = false;
            JumpButtonImage.color = Color.red;
        }
        else
        {
            JumpButton.interactable = true;
            JumpButtonImage.color = Color.white;
        }

        AmmoUI.text = "Ammo: " + Ammo;

        ScrapsUI.text = "Scraps: " + Scraps;
    }

    void AdvanceTime()
    {
        Cycle++;
        if (Cycle > 8)
        {
            Day++;
            Cycle = 1;
        }

        TimeUI.text = "Day " + Day + " Cycle " + Cycle;
    }

    void UpdateJumps()
    {
        jumps--;
        JumpUI.text = "Jumps left: " + jumps;
    }

    IEnumerator Jump()
    {
        UpdateJumps();
        TimeAdvanceUI.text = "Time flies fast...";
        LockAllButtons();

        for (int x = 1; x <= 20; x++)
        {
            Cam.GetComponent<Camera>().orthographicSize++;
            yield return new WaitForEndOfFrame();
        }

        audiomanager.PlayAudio("Jump In");

        JumpLight.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        Playership.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        JumpLight.SetActive(false);
        
        yield return new WaitForSeconds(3);

        UpdateResources();
        AdvanceTime();
        audiomanager.PlayAudio("Jump Out");

        JumpLight.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        Playership.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        JumpLight.SetActive(false);

        for (int x = 1; x <= 20; x++)
        {
            Cam.GetComponent<Camera>().orthographicSize--;
            yield return new WaitForEndOfFrame();
        }

        TimeAdvanceUI.text = "";
        UnlockAllButtons();

        storyManager.ChangeCurrentStory();
    }

    IEnumerator Wait()
    {
        TimeAdvanceUI.text = "Time flies fast...";
        LockAllButtons();
        yield return new WaitForSeconds(3);
        UnlockAllButtons();
        TimeAdvanceUI.text = "";
        AdvanceTime();
    }
}
