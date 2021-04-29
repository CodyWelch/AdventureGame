using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace myRPG
{
    public class QuestGiver : MonoBehaviour
    {
        public Quest quest;

        public PlayerStats player;

        public GameObject questWindow;
        public Text titleText;
        public Text descriptionText;
        public Text experienceText;
        public Text goldText;



        public void OpenQuestWindow()
        {
            questWindow.SetActive(true);
            titleText.text = quest.title;
            descriptionText.text = quest.description;
            experienceText.text = quest.expReward.ToString();
            goldText.text = quest.goldReward.ToString();
        }

        public void AcceptQuest()
        {
            questWindow.SetActive(false);
            quest.isActive = true;
            player.quest = quest;

        }

    }
}
