using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    private static UiManager instance;
    public static UiManager Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    [SerializeField] private List<Image> panelList;

    public PlayersJoinedMenu playersJoinedMenu;

    [Header("BroadCast")]
    [SerializeField] private Color32 standardBroadcastColor;
    private bool isBroadcasting;
    private Animator broadcastController;
    public TextMeshProUGUI broadcastText;

    private void Awake()
    {
        Instance = null;
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != null)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        broadcastController = broadcastText.GetComponent<Animator>();
    }

    public void CloseAllPanels()
    {
        for (int i = 0; i < panelList.Count; i++)
        {
            panelList[i].gameObject.SetActive(false);
        }
    }

    public void BroadCastMessage(string message, float broadcastTime, Color32? color = null)
    {
        if (!isBroadcasting)
        {
            isBroadcasting = true;
            broadcastText.faceColor = color ?? standardBroadcastColor;
            broadcastText.SetText(message);
            broadcastController.SetBool("IsShown", true);
            StartCoroutine(StopBroadCasting(broadcastTime));
        }
    }

    IEnumerator StopBroadCasting(float time)
    {
        yield return new WaitForSeconds(time);
        broadcastController.SetBool("IsShown", false);
        isBroadcasting = false;
    }



}
