using Unity.Netcode.Components;

namespace Multiplayer.Scripts
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
