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
            AttackClientRpc(id, new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { id } } });
        }

        [ClientRpc]
        private void AttackClientRpc(ulong id, ClientRpcParams clientRpcParams)
        {
            GameManager.Instance.ObtainCorrectController(id)?.CallAttackEvent();
        }

        [ServerRpc(RequireOwnership = false)]
        internal void TakeDamageServerRpc(ulong id)
        {
            TakeDamageClientRpc(id, new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { id } } });
        }

        [ClientRpc]
        private void TakeDamageClientRpc(ulong id, ClientRpcParams clientRpcParams)
        {
            if (GameManager.Instance.ObtainCorrectController(id)) print("test");
            GameManager.Instance.ObtainCorrectController(id)?.CallHitEvent();
        }

        [ServerRpc(RequireOwnership = false)]
        internal void JumpServerRpc(ulong id)
        {
            JumpClientRpc(id, new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { id } } });
        }

        [ClientRpc]
        private void JumpClientRpc(ulong id, ClientRpcParams clientRpcParams)
        {
            GameManager.Instance.ObtainCorrectController(id)?.CallJumpEvent();
        }

        [ServerRpc(RequireOwnership = false)]
        internal void SetDisableAllCharacterChangesServerRpc(ulong id, bool shouldDisableChanges)
        {
            GameManager.Instance.ObtainCorrectController(id).DisableAllCharacterChanges.Value = shouldDisableChanges;
        }
    }
}