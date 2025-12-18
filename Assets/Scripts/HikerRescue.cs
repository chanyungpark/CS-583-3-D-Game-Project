using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HikerRescue : MonoBehaviour
{
    [Header("Rescue Settings")]
    public float holdDuration = 2f;
    private float holdTimer = 0f;

    [Header("UI (auto-found in scene)")]
    [SerializeField] private RectTransform rescueUIRoot;   // parent with text + bar (RescueUI)
    [SerializeField] private Image progressBarImage;       // child Image used as fill bar (RescueProgress)

    [Header("Bar Position")]
    public Vector3 worldOffset = new Vector3(0f, 2f, 0f);  // height above hiker

    private bool playerInRange = false;
    private bool isSaved = false;
    private Camera mainCam;

    private static RectTransform sharedRescueUIRoot;
    private static Image sharedProgressBarImage;
    private static HikerRescue activeHiker;
    private static TMP_Text sharedRescueLabel;

    private void Awake()
    {
        mainCam = Camera.main;

        if (sharedRescueUIRoot == null)
        {
            RectTransform[] allRects = Resources.FindObjectsOfTypeAll<RectTransform>();
            foreach (RectTransform rect in allRects)
            {
                if (rect.name == "RescueUI")
                {
                    sharedRescueUIRoot = rect;
                    break;
                }
            }
        }

        if (sharedProgressBarImage == null && sharedRescueUIRoot != null)
        {
            Image[] images = sharedRescueUIRoot.GetComponentsInChildren<Image>(true);
            foreach (Image img in images)
            {
                if (img.gameObject.name == "RescueProgress")
                {
                    sharedProgressBarImage = img;
                    break;
                }
            }

            if (sharedProgressBarImage == null && images.Length > 0)
            {
                sharedProgressBarImage = images[0];
            }
        }

        if (sharedRescueLabel == null && sharedRescueUIRoot != null)
        {
            var labels = sharedRescueUIRoot.GetComponentsInChildren<TMP_Text>(true);
            foreach (TMP_Text t in labels)
            {
                if (t.gameObject.name == "RescueText")
                {
                    sharedRescueLabel = t;
                    break;
                }
            }
            if (sharedRescueLabel == null && labels.Length > 0)
            {
                sharedRescueLabel = labels[0];
            }
        }

        rescueUIRoot = sharedRescueUIRoot;
        progressBarImage = sharedProgressBarImage;
    }

    private void Start()
    {
        if (rescueUIRoot != null)
        {
            rescueUIRoot.gameObject.SetActive(false);
        }

        if (progressBarImage != null)
        {
            progressBarImage.fillAmount = 0f;
        }

        if (sharedRescueLabel != null)
        {
            sharedRescueLabel.enabled = false;
            sharedRescueLabel.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        activeHiker = this;
        playerInRange = true;

        if (activeHiker == this && rescueUIRoot != null)
        {
            rescueUIRoot.gameObject.SetActive(true);
            if (progressBarImage != null)
            {
                progressBarImage.fillAmount = 0f;
            }
            if (sharedRescueLabel != null)
            {
                sharedRescueLabel.gameObject.SetActive(true);
                sharedRescueLabel.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (activeHiker == this)
        {
            activeHiker = null;

            if (rescueUIRoot != null)
            {
                rescueUIRoot.gameObject.SetActive(false);
            }

            if (progressBarImage != null)
            {
                progressBarImage.fillAmount = 0f;
            }

            if (sharedRescueLabel != null)
            {
                sharedRescueLabel.enabled = false;
                sharedRescueLabel.gameObject.SetActive(false);
            }
        }

        playerInRange = false;
        holdTimer = 0f;
    }

    private void Update()
    {
        if (isSaved) return;
        if (activeHiker != this) return;

        UpdateUIPosition();

        if (!playerInRange) return;

        if (Input.GetKey(KeyCode.E))
        {
            holdTimer += Time.deltaTime;

            if (progressBarImage != null)
            {
                progressBarImage.fillAmount = Mathf.Clamp01(holdTimer / holdDuration);
            }

            if (holdTimer >= holdDuration)
            {
                SaveHiker();
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            holdTimer = 0f;

            if (progressBarImage != null)
            {
                progressBarImage.fillAmount = 0f;
            }
        }
    }

    private void UpdateUIPosition()
    {
        if (activeHiker != this) return;
        if (rescueUIRoot == null || mainCam == null) return;

        Vector3 worldPos = transform.position + worldOffset;
        Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);

        // If hiker is behind the camera, hide the UI
        if (screenPos.z < 0f)
        {
            rescueUIRoot.gameObject.SetActive(false);
            return;
        }

        if (playerInRange && !isSaved && !rescueUIRoot.gameObject.activeSelf)
        {
            rescueUIRoot.gameObject.SetActive(true);
        }

        rescueUIRoot.position = screenPos;
    }

    private void SaveHiker()
    {
        isSaved = true;

        if (GameManager.Instance != null)
        {
            ScoreManager.Instance.AddHikerSaved();
            Debug.Log("Hiker saved! Total: " + ScoreManager.Instance.hikersSaved);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.HikerSaved();
        }

        if (activeHiker == this)
        {
            activeHiker = null;

            if (rescueUIRoot != null)
            {
                rescueUIRoot.gameObject.SetActive(false);
            }

            if (sharedRescueLabel != null)
            {
                sharedRescueLabel.enabled = false;
                sharedRescueLabel.gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(false);
    }
}