using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public HueEventSystem.EventManager<string> eventManager;

    [Header("Data")]
    public List<RuleDuality> typeDualities;
    public List<RuleDuality> placeDualities;
    public List<RuleDuality> actionDualities;
    public List<Rule> rules;
    public int totalRounds = 1;
    public int roundTime = 10;
    public float spinSpeedStart = 90;
    public int score = 0;
    public float spins = 4; // MUST START EVEN!
    public RuleDuality actionDuality;

    [Header("States")]
    public bool isForceShowRules;
    public bool isCharacterControl = false;
    public int currentRound = 0;

    [Header("Objects")]
    public RulesUIManager rulesUIManager;
    public GameObject MainMenu;
    public GameObject MainGame;
    public PlayerController player;
    public List<SheepData> sheep;
    public Text startTimerText;
    public Text gameTimerText;
    public Text ruleCountText;
    public GameObject sheepPointVisual;

    [Header("Sounds")]
    public AudioClip success;
    public AudioClip bong;
    public AudioClip spinFinish;
    public AudioClip goSound;

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
        // Run when start button press, 
        // Hide menu
        MainMenu.SetActive(false);
        MainGame.SetActive(true);

        // Start with character controls off
        isCharacterControl = false;

        // Start the first round of the game!
        StartRound();
    }

    public void SpinAllSpinners()
    {
        for (int i = 0; i < rulesUIManager.placeSpinners.Count; i++) {
            rulesUIManager.placeSpinners[i].maxSpeed = spinSpeedStart + i;
            rulesUIManager.placeSpinners[i].Spin();
        }
        for (int i = 0; i < rulesUIManager.typeSpinners.Count; i++) {
            rulesUIManager.typeSpinners[i].maxSpeed = spinSpeedStart + i;
            rulesUIManager.typeSpinners[i].Spin();
        }
        for (int i = 0; i < rulesUIManager.actionSpinners.Count; i++) {
            rulesUIManager.placeSpinners[i].maxSpeed = spinSpeedStart - (10 * i);
            rulesUIManager.actionSpinners[i].Spin();
        }
    }

    public IEnumerator DoStartGameplayRound()
    {
        // Track round
        currentRound++;
        InitializeSheep();

        // Wait, so they can look at the rules
        yield return new WaitForSeconds(3.0f);

        // Turn off the rules
        isForceShowRules = false;
        ShowRules(false);

        // Clear the game timer and any rules text
        gameTimerText.text = "";
        ruleCountText.text = "";
        yield return null;


        // Do the the Countdown Timer
        int timer = 3;
        while (timer > 0) {
            //Play Sound
            AudioSource.PlayClipAtPoint(bong, Camera.main.transform.position);
            startTimerText.text = "Round " + currentRound + "!\nGet Ready!\n" + timer;
            yield return new WaitForSeconds(1);
            timer--;
        }

        AudioSource.PlayClipAtPoint(goSound, Camera.main.transform.position);

        // Turn off start countdown text
        startTimerText.text = "";

        // Let player be in control
        isCharacterControl = true;

        // Start Game Timer - it will trigger end round when it hits zero!
        yield return StartCoroutine("DoGameTimer");

        // Now, count the sheep
        EndGameplayRound();
    }

    public IEnumerator DoGameTimer()
    {
        int timer = roundTime;
        while (timer > 0) {
            gameTimerText.text = "" + timer;
            yield return new WaitForSeconds(1);
            timer--;
        }
        yield return null;
    }



    public void EndGameplayRound()
    {
        StartCoroutine("DoEndGameplayRound");
    }

    public IEnumerator DoEndGameplayRound()
    {
        // Character can't control during count
        isCharacterControl = false;

        // Make sure rules are off
        ShowRules(false);

        // TODO: Count the sheep
        // Make sure timers are off
        gameTimerText.text = "";
        startTimerText.text = "";

        // Reuse start timer as score
        startTimerText.text = "" + score;

        for (int i = 0; i < rules.Count; i++) {
            // Show the rule
            ruleCountText.text = rules[i].RichString();

            // TODO: ACTUALLY COUNT SHEEP!
            foreach (SheepData sheep in GameManager.instance.sheep) {
                // If it meets the rule --
                if (sheep.MeetsRule(rules[i])) {

                    // Play a sound
                    AudioSource.PlayClipAtPoint(success, Camera.main.transform.position);

                    // Update score
                    score += 1;
                    startTimerText.text = "" + score;

                    // Show the points
                    Instantiate(sheepPointVisual, sheep.transform.position, Quaternion.identity);

                    // Wait 1/2 second
                    yield return new WaitForSeconds(0.5f);
                }
            }

            // wait for next rule
            yield return new WaitForSeconds(2.0f);
        }

        // Hide rule text, but not score
        ruleCountText.text = "";

        // Wait
        yield return new WaitForSeconds(1);

        // Hide score and start next round if needed
        if (currentRound < totalRounds) {
            StartRound();
        }
        else {
            startTimerText.text = "Final Score:\n" + score;
        }

        yield return null;
    }


    public void StartRound()
    {
        // Show instructions
        isForceShowRules = true; // Rules aren't on button - forced on
        ShowRules(true);

        // Choose random rules
        GenerateRules();

        // Spin all the spinners
        SpinAllSpinners();

        // Prepare the sheep
        InitializeSheep();       
    }

    public void StartGameplayRound()
    {
        // Game will continue when ACTION spinner (design in data to be slowest) finishes
        // After rules are chosen and ACTION spinner ends, this is callback
        StartCoroutine("DoStartGameplayRound");
    }

    public void ShuffleSheep()
    {
        for (int i = 0; i < sheep.Count; i++) {
            int choice = Random.Range(0, sheep.Count);
            sheep.Add(sheep[choice]);
            sheep.RemoveAt(choice);
        }
    }

    public void InitializeSheep()
    {
        Debug.Log("Init sheep");

        // Make half black        
        for (int i=0; i<sheep.Count; i++) { 
            if (i<sheep.Count / 2) {
                sheep[i].isBlack = true;
            } else {
                sheep[i].isBlack = false;
            }
        }
        ShuffleSheep();
        // Make half hipsters        
        for (int i = 0; i < sheep.Count; i++) {
            if (i < sheep.Count / 2) {
                sheep[i].isHipster = true;
            } else {
                sheep[i].isHipster = false;
            }
        }

        // Place Sheep At Center
        foreach(SheepData bah in sheep) {
            // Set visuals
            bah.SetVisuals();
            bah.transform.position = Vector3.zero + (Random.insideUnitSphere * 5);
                bah.transform.position = new Vector3(bah.transform.position.x, 0, bah.transform.position.z);
            bah.transform.Rotate(new Vector3(0, Random.Range(0.0f, 365.0f), 0));
        }

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

    public void GenerateRules()
    {
        // Clear out old rules
        rules = new List<Rule>();

        for (int i = 0; i < rulesUIManager.typeSpinners.Count; i++) {
            rules.Add(new Rule());
            // Choose random dualities
            rulesUIManager.typeSpinners[i].SetDuality(rules[i].typeDuality = typeDualities[Random.Range(0, typeDualities.Count)]);
            rulesUIManager.placeSpinners[i].SetDuality(rules[i].placeDuality = placeDualities[Random.Range(0, placeDualities.Count)]);
            // Set to random if true or not
            if (Random.value > 0.5f) {
                rules[i].placeDualityIsOneThing = true;
                rulesUIManager.placeSpinners[i].minTarget = spins;
                rulesUIManager.placeSpinners[i].maxTarget = spins;
            }
            else {
                rules[i].placeDualityIsOneThing = false;
                rulesUIManager.placeSpinners[i].minTarget = spins + 1;
                rulesUIManager.placeSpinners[i].maxTarget = spins + 1;
            }
            if (Random.value > 0.5f) {
                rules[i].typeDualityIsOneThing = true;
                rulesUIManager.placeSpinners[i].minTarget = spins;
                rulesUIManager.placeSpinners[i].maxTarget = spins;
            }
            else {
                rules[i].typeDualityIsOneThing = false;
                rulesUIManager.typeSpinners[i].minTarget = spins + 1;
                rulesUIManager.typeSpinners[i].maxTarget = spins + 1;
            }
        }

        for (int i = 0; i < rulesUIManager.actionSpinners.Count; i++) {
            actionDuality = actionDualities[Random.Range(0, actionDualities.Count)];
            rulesUIManager.actionSpinners[i].SetDuality(actionDuality);
            HumanoidPawn playerPawn = player.pawn as HumanoidPawn;
            if (Random.value > 0.5f) {
                playerPawn.isActionCarry = true;
                rulesUIManager.actionSpinners[i].minTarget = spins + 4;
                rulesUIManager.actionSpinners[i].maxTarget = spins + 4;
            }
            else {
                playerPawn.isActionCarry = false;
                rulesUIManager.actionSpinners[i].minTarget = spins + 1 + 4;
                rulesUIManager.actionSpinners[i].maxTarget = spins + 1 + 4;
            }
        }

    }
}

// A rule says "+1 point for each (type duality) sheep moved (place duality)"
[System.Serializable]
public class Rule
{
    public RuleDuality typeDuality;
    public bool typeDualityIsOneThing;
    public RuleDuality placeDuality;
    public bool placeDualityIsOneThing;

    public string typeString {
        get {
            if (typeDualityIsOneThing) return typeDuality.oneThing;
            else return typeDuality.theOther;
        }
    }

    public string placeString {
        get {
            if (placeDualityIsOneThing) return placeDuality.oneThing;
            else return placeDuality.theOther;
        }
    }

    public string RichString()
    {
        string output;
        output = "Each <color=yellow>";
        output += typeString;
        output += "</color> sheep that is <color=yellow>";
        output += placeString;
        output += "</color>.";
        return output;
    }

    

}



