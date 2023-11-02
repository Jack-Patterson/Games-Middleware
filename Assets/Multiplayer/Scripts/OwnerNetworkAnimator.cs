using Unity.Netcode.Components;

namespace Multiplayer.Scripts
{
    public class OwnerNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
