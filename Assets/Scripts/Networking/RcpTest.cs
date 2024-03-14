using Unity.Netcode;

namespace Networking
{
    public class RcpTest : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            // Only send RCP to the server on the client that owns a network player:
            // if (IsServer && IsOwner) TestServerRpc(0, NetworkObjectId);
        }
    }
}
