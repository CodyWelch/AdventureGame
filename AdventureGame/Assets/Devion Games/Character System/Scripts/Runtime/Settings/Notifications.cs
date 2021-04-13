using DevionGames.UIWidgets;
using UnityEngine;
using UnityEngine.Events;

namespace DevionGames.CharacterSystem
{
    public static class NotificationExtension
    {
        public static void Show(this NotificationOptions options, UnityAction<int> result, params string[] buttons)
        {
            if (CharacterManager.UI.dialogBox != null)
            {
                CharacterManager.UI.dialogBox.Show(options, result, buttons);
            }
        }

        public static void Show(this NotificationOptions options, params string[] replacements)
        {
            if (CharacterManager.UI.notification != null)
            {
                CharacterManager.UI.notification.AddItem(options, replacements);
            }
        }
    }
}

namespace DevionGames.CharacterSystem.Configuration
{
    [System.Serializable]
    public class Notifications : Settings
    {

        public override string Name
        {
            get
            {
                return "Notification";
            }
        }
        [Header("Create Character:")]
        public NotificationOptions nameInUse = new NotificationOptions()
        {
            text = "This name is already in use!"
        };

        public NotificationOptions nameIsEmpty = new NotificationOptions()
        {
            text = "The name field can't be empty!"
        };

        [Header("Select Character:")]
        public NotificationOptions noCharacters = new NotificationOptions()
        {
            text = "No characters associated with this account."
        };

        public NotificationOptions deleteConfirmation = new NotificationOptions()
        {
            title= "Delete Character",
            text = "Are you sure you want to delete this Character?"
        };
    }
}