using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameHud : NetworkBehaviour
{
    [SerializeField] private TMP_Text lobbyCodeText;

    private NetworkVariable<FixedString32Bytes> lobbyCode = new NetworkVariable<FixedString32Bytes>("");

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            lobbyCode.OnValueChanged += HandleLobbyCodeChanged;
            HandleLobbyCodeChanged(string.Empty, lobbyCode.Value);
        }

        if (!IsHost) { return; }

        lobbyCode.Value = HostSingleTon.Instance.GameManager.joinCode;
    }
    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            lobbyCode.OnValueChanged -= HandleLobbyCodeChanged;
        }
    }
    private void HandleLobbyCodeChanged(FixedString32Bytes oldCode, FixedString32Bytes newCode)
    {
        lobbyCodeText.text = newCode.ToString();
    }
}

