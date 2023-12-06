using System.Collections.Generic;
using Unity.Netcode;

namespace Multiplayer.Scripts
{
    public class ServerRpcController : NetworkBehaviour
    {
        public static ServerRpcController Instance;

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

        [ServerRpc(RequireOwnership = false)]
        internal void AttackServerRpc(ulong id)
        {
            List<ulong> ids = GameManager.Instance.RetrieveOtherIds(id);
            AttackClientRpc(id, new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = ids } });
        }

        [ClientRpc]
        private void AttackClientRpc(ulong id, ClientRpcParams clientRpcParams)
        {
            GameManager.Instance.ObtainCorrectCharacterController(id)?.CallAttackEvent();
        }

        [ServerRpc(RequireOwnership = false)]
        internal void JumpServerRpc(ulong id)
        {
            List<ulong> ids = GameManager.Instance.RetrieveOtherIds(id);
            
            JumpClientRpc(id, new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = ids } });
        }

        [ClientRpc]
        private void JumpClientRpc(ulong id, ClientRpcParams clientRpcParams)
        {
            GameManager.Instance.ObtainCorrectCharacterController(id)?.CallJumpEvent();
        }

        [ServerRpc(RequireOwnership = false)]
        internal void SetDisableAllCharacterChangesServerRpc(ulong id, bool shouldDisableChanges)
        {
            GameManager.Instance.ObtainCorrectCharacterController(id).DisableAllCharacterChanges.Value =
                shouldDisableChanges;
        }
        
        [ServerRpc(RequireOwnership = false)]
        internal void SetHealthServerRpc(ulong id, int health)
        {
            GameManager.Instance.ObtainCorrectCharacterController(id).Health =
                health;
            List<ulong> ids = GameManager.Instance.RetrieveOtherIds(id);
            
            HealthClientRpc(id, health, new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = ids } });
        }
        
        [ClientRpc]
        private void HealthClientRpc(ulong id, int health, ClientRpcParams clientRpcParams)
        {
            GameManager.Instance.ObtainCorrectCharacterController(id)?.CallHealthEvent(health);
        }
    }
}