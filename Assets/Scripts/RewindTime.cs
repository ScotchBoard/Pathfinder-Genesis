using System.Collections;
using UnityEngine;

// Stores the position and rotation at the giving frames
public class Keyframe
{
    public Vector3 position;
    public Vector3 rotation;
    public float health;

    public Keyframe(Vector3 position, Vector3 rotation, float health)
    {
        this.position = position;
        this.rotation = rotation;
        this.health = health;
    }
}

public class RewindTime: MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private ArrayList keyframes;
    [SerializeField]
    private bool isReversing = false;
    [SerializeField]
    private float rewindEnergyConsumption = 0.1f;

    [SerializeField]
    private int keyframe = 5;
    private int frameCounter = 0;
    private int reverseCounter = 0;

    private Vector3 currentPosition;
    private Vector3 previousPosition;
    private Vector3 currentRotation;
    private Vector3 previousRotation;
    private float currentHealth;
    private float previousHealth;

    private PlayerInfo playerInfo;

    private bool firstRun = true;

    void Start()
    {
        keyframes = new ArrayList();
        playerInfo = player.GetComponent<PlayerInfo>();
    }

    void Update()
    {
        // If Right Click is pressed - rewind time + effects
        if (Input.GetKey(KeyCode.Mouse1) && playerInfo.CanUseEnergy())
        {
            isReversing = true;
        }
        else
        {
            isReversing = false;
            firstRun = true;
        }
    }

    void FixedUpdate()
    {
        if (!isReversing)
        {
            if (frameCounter < keyframe) // Saving the position only once every keyframe frame
            {
                frameCounter += 1;
            }
            else
            {
                frameCounter = 0;

                keyframes.Add(new Keyframe(player.transform.position, player.transform.localEulerAngles, playerInfo.GetPlayerHealth()));
            }
        }
        else
        {
            if (reverseCounter > 0)
            {
                reverseCounter -= 1;
            }
            else
            {
                reverseCounter = keyframe;
                RestorePositions();
            }

            if (firstRun)
            {
                firstRun = false;
                RestorePositions();
            }

            // Interpolates for smoother rewind
            float interpolation = (float)reverseCounter / (float)keyframe;
            player.transform.position = Vector3.Lerp(previousPosition, currentPosition, interpolation);
            player.transform.localEulerAngles = Vector3.Lerp(previousRotation, currentRotation, interpolation);
            playerInfo.SetPlayerHealth(Mathf.Lerp(previousHealth, currentHealth, interpolation));

            playerInfo.SetPlayerEnergy(rewindEnergyConsumption);

            Revive();
        }

        // Doesn't save more than 128 frames
        if (keyframes.Count > 128)
        {
            keyframes.RemoveAt(0);
        }
    }

    private void Revive()
    {
        if (GameManager.INSTANCE.GameOver)
        {
            player.GetComponent<PlayerInfo>().SetPlayerHealth(2);
            GameManager.INSTANCE.GameOver = false;
            GameManager.INSTANCE.IsReviving = true;
        }
    }

    void RestorePositions()
    {
        int lastIndex = keyframes.Count - 1;
        int secondToLastIndex = keyframes.Count - 2;

        if (secondToLastIndex >= 0)
        {
            currentPosition = (keyframes[lastIndex] as Keyframe).position;
            previousPosition = (keyframes[secondToLastIndex] as Keyframe).position;
            
            currentRotation = (keyframes[lastIndex] as Keyframe).rotation;
            previousRotation = (keyframes[secondToLastIndex] as Keyframe).rotation;

            currentHealth = (keyframes[lastIndex] as Keyframe).health;
            previousHealth = (keyframes[secondToLastIndex] as Keyframe).health;

            keyframes.RemoveAt(lastIndex);
        }
    }
}