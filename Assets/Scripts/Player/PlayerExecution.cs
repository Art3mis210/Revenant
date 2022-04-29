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
    public GameObject MobileButton;
    public GameObject KeyboardButton;
    GameObject ExecutionButton;
    PlayerController playerController;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        Weapon = GetComponent<PlayerWeapon>();
        #if MOBILE_INPUT
            ExecutionButton = MobileButton;
        #else
            ExecutionButton = KeyboardButton;
        #endif

    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="ExecutionTrigger")
        {

            ExecutionButton.SetActive(true);
            Debug.Log("Press e");
            if(Input.GetKey(KeyCode.E) || CrossPlatformInputManager.GetButtonDown("Execute"))
            {
                playerController.ChangeMovement(0);
                ExecutionButton.SetActive(false);
                TriggerTransform = other.gameObject.transform;
                TriggerTransform.GetComponent<Collider>().enabled = false;
                enemy=TriggerTransform.GetComponent<EnemyExecution>();
                StartCoroutine(SetPlayerPositionRotation(1f));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ExecutionTrigger")
        {
            ExecutionButton.SetActive(false);
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
        Execution = Random.Range(0, 9);
        Weapon.DisableCurrentWeapon();
        if(KnifeExecution.Contains(Execution))
            Knife.SetActive(true);
        playerAnimator.SetFloat("Executions", Execution);
        playerAnimator.SetTrigger("StartExecution");
        enemy.StartExecution(Execution,GetComponent<Collider>());
        NoiseManager.Noise.CreateNoise(transform.position);

    }
}
