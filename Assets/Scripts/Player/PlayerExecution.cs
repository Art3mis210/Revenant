using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerExecution : MonoBehaviour
{
    public int Execution;
    public GameObject Knife;
    Animator playerAnimator;
    Transform TriggerTransform;
    EnemyExecution enemy;
    PlayerWeapon Weapon;
    public List<int> KnifeExecution;
    public GameObject MobileExecutionButton;
    public GameObject KeyboardExecutionButton;

    public GameObject MobileInterrogateButton;
    public GameObject KeyboardInterrogateButton;

    public GameObject InterrogationMenu;

    GameObject ExecutionButton;
    GameObject InterrogateButton;
    PlayerController playerController;
    bool InterrogationMode;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        Weapon = GetComponent<PlayerWeapon>();
        #if MOBILE_INPUT
            ExecutionButton = MobileExecutionButton;
            InterrogateButton = MobileInterrogateButton;
        #else
            ExecutionButton = KeyboardExecutionButton;
            InterrogateButton = KeyboardInterrogateButton;
        #endif

    }
    private void Update()
    {
        if(InterrogationMode)
        {
            if (!InterrogationMenu.activeInHierarchy)
                InterrogationMenu.SetActive(true);
            if(Input.GetKey(KeyCode.I) || CrossPlatformInputManager.GetButtonDown("InterrogateEnemy"))
            {
                //Mark Enemies
            }
            else if(Input.GetKey(KeyCode.O) || CrossPlatformInputManager.GetButtonDown("Kill"))
            {
                //kill
                playerAnimator.SetInteger("InterrogatePos", 1);
                enemy.InterrogationKill();
                InterrogationMode = false;
                InterrogationMenu.SetActive(false);
            }
            else if(Input.GetKey(KeyCode.P) || CrossPlatformInputManager.GetButtonDown("Release"))
            {
                //release
                playerAnimator.SetInteger("InterrogatePos", -1);
                enemy.InterrogationRelease();
                InterrogationMode = false;
                InterrogationMenu.SetActive(false);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!InterrogationMode)
        {
            if (other.gameObject.tag == "ExecutionTrigger")
            {
                if (TriggerTransform != other.gameObject.transform)
                {
                    TriggerTransform = other.gameObject.transform;
                    enemy = TriggerTransform.GetComponent<EnemyExecution>();
                    ExecutionButton.SetActive(true);
                    if (enemy.CurrentEnemyType==EnemyExecution.EnemyType.Human) 
                        InterrogateButton.SetActive(true);
                }
                if (CrossPlatformInputManager.GetButtonDown("Execute"))
                {
                    Weapon.DisableCurrentWeapon();
                    playerController.ChangeMovement(0);
                    ExecutionButton.SetActive(false);
                    InterrogateButton.SetActive(false);
                    TriggerTransform = other.gameObject.transform;
                    TriggerTransform.GetComponent<Collider>().enabled = false;
                    enemy = TriggerTransform.GetComponent<EnemyExecution>();
                    enemy.ReduceEnemyCollider(true);
                    StartCoroutine(SetPlayerPositionRotation(1f));
                    InterrogationMode = false;
                }
                else if (CrossPlatformInputManager.GetButtonDown("Interrogate") && enemy.CurrentEnemyType == EnemyExecution.EnemyType.Human)
                {
                    Weapon.DisableCurrentWeapon();
                    playerController.ChangeMovement(0);
                    ExecutionButton.SetActive(false);
                    InterrogateButton.SetActive(false);
                    TriggerTransform.GetComponent<Collider>().enabled = false;
                    enemy.ReduceEnemyCollider(true);
                    StartCoroutine(SetPlayerPositionRotation(1f));
                    InterrogationMode = true;
                }

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ExecutionTrigger")
        {
            ExecutionButton.SetActive(false);
            InterrogateButton.SetActive(false);
        }
    }
    IEnumerator SetPlayerPositionRotation(float Duration)
    {
        float t = 0;
        while(t<Duration)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(TriggerTransform.position.x,transform.position.y, TriggerTransform.position.z), t / Duration);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x,TriggerTransform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
        if (!InterrogationMode)
        {
            Execution = Random.Range(0, 9);
            if (KnifeExecution.Contains(Execution))
                Knife.SetActive(true);
            playerAnimator.SetFloat("Executions", Execution);
            playerAnimator.SetTrigger("StartExecution");
            enemy.StartExecution(Execution);
            NoiseManager.Noise.CreateNoise(transform.position);
        }
        else
        {
            Knife.SetActive(true);
            playerAnimator.SetTrigger("Interrogate");
            playerAnimator.SetInteger("InterrogatePos", 0);
            enemy.StartInterrogation(Execution);
        }

    }
}
