using System;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.UI;

namespace Multiplayer.Scripts
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance;

        [SerializeField] private GameObject hostClientPanel;
        [SerializeField] private TMP_Text debugText;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void OnHostButtonClicked()
        {
            if (NetworkManager.Singleton.StartHost())
            {
                debugText.text = "Host started.";
                DisableHostClientPanel();
            }
            else
            {
                debugText.text = "Host failed to start.";
            }
        }
        
        public void OnClientButtonClicked()
        {
            if (NetworkManager.Singleton.StartClient())
            {
                debugText.text = "Client started.";
                DisableHostClientPanel();
            }
            else
            {
                debugText.text = "Client failed to start.";
            }
        }

        private void DisableHostClientPanel()
        {
            hostClientPanel.SetActive(false);
        }
    }
}
