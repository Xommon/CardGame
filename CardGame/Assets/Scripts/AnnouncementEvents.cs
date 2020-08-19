using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class AnnouncementEvents : MonoBehaviour
{
    public event EventHandler<OnAnnouncementStartEventArgs> OnAnnouncementStart;
    public BattleManager battleManager;
    public TextMeshProUGUI bigAnnouncement;
    public TextMeshProUGUI smallAnnouncement;
    public int announcementCounter;
    public class OnAnnouncementStartEventArgs : EventArgs
    {
        public int announcementCount;
    }

    private void Start()
    {
        battleManager = GetComponent<BattleManager>();
    }

    public delegate void AnnouncementEventDelegate(float f);

    private int announcementCount;

    void Update()
    {
        if (bigAnnouncement.gameObject.activeInHierarchy || smallAnnouncement.gameObject.activeInHierarchy)
        {
            announcementCount++;
            OnAnnouncementStart?.Invoke(this, new OnAnnouncementStartEventArgs { announcementCount = announcementCount });
        }
        else
        {
            announcementCounter = 0;
        }
    }
}
