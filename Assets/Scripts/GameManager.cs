using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public HueEventSystem.EventManager<string> eventManager;

    [Header("Data")]
    public List<RuleDuality> typeDualities;
    public List<RuleDuality> placeDualities;
    public List<RuleDuality> actionDualities;
    public List<Rule> rules;
    [Header("States")]
    public bool isForceShowRules;
    [Header("Objects")]
    public RulesUIManager rulesUIManager;
    public GameObject MainMenu;
    public GameObject MainGame;
    public PlayerController player;
    

    private void Awake()
    {
        // Create singleton
        if (instance == null) {
            instance = this;        
        }
        else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MainMenu.SetActive(true);
        MainGame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartGame()
    {
        // Wait for button press, then hide menu, show instructions
        MainMenu.SetActive(false);
        MainGame.SetActive(true);
        isForceShowRules = true;
        ShowRules(true);

        // Choose random rules
        GenerateRules();
        // Spin all the spinners
        for (int i = 0; i<rulesUIManager.placeSpinners.Count; i++) {
            rulesUIManager.placeSpinners[i].Spin();
        }
        for (int i = 0; i < rulesUIManager.typeSpinners.Count; i++) {
            rulesUIManager.typeSpinners[i].Spin();
        }
        for (int i = 0; i < rulesUIManager.actionSpinners.Count; i++) {
            rulesUIManager.actionSpinners[i].Spin();
        }
    }

    public IEnumerator DoStartRound()
    {
        yield return new WaitForSeconds(1.0f);

       isForceShowRules = false;

        yield return null;
    }

    public void StartRound()
    {
        // After rules are chosen and all spinners end, this is callback
        StartCoroutine("DoStartRound");
    }


    public void ShowRules(bool show = true)
    {
        if (isForceShowRules) {
            rulesUIManager.gameObject.SetActive(true);
        }
        else {
            rulesUIManager.gameObject.SetActive(show);
        }
    }

    public void GenerateRules ()
    {
        for (int i=0; i<rulesUIManager.typeSpinners.Count; i++) {
            rules.Add(new Rule());
            rulesUIManager.typeSpinners[i].SetDuality(rules[i].typeDuality = typeDualities[Random.Range(0, typeDualities.Count)]);
            rulesUIManager.placeSpinners[i].SetDuality(rules[i].placeDuality = placeDualities[Random.Range(0, typeDualities.Count)]);
            Debug.Log("!!!");
        }
        for (int i = 0; i < rulesUIManager.actionSpinners.Count; i++) {
            rulesUIManager.actionSpinners[i].SetDuality(actionDualities[Random.Range(0, actionDualities.Count)]);
        }

    }

}

// A rule says "+1 point for each (type duality) sheep moved (place duality)"
[System.Serializable]
public class Rule
{
    public RuleDuality typeDuality;
    public RuleDuality placeDuality;
}



