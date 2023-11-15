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
        [SerializeField] private TMP_Text healthText;

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

        private void Start()
        {
            debugText.enabled = false;
            healthText.enabled = false;
        }

        public void OnHostButtonClicked()
        {
            debugText.enabled = true;
            
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
            debugText.enabled = true;
            
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

        internal void OnHealth(int amount)
        {
            healthText.text = (int.Parse(healthText.text) - amount).ToString("000");
        }

        internal void SetInitialHealthText(string amount)
        {
            healthText.enabled = true;
            
            healthText.text = amount;
        }
    }
}
