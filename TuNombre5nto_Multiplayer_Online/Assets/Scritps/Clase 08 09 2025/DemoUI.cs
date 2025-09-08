using UnityEngine;

public class DemoUI : MonoBehaviour
{
    MultiplayerBootStarp mp;
    string code = "";
    private void Awake()
        => mp = FindAnyObjectByType<MultiplayerBootStarp>();
    void UGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, 280, 170), GUI.skin.box);
        GUILayout.Label("Multiplayer Demo");
        if (GUILayout.Button("Host (Create Lobby)")) mp.Host();
        if (GUILayout.Button("Quick Join")) mp.QuickJoin();

        GUILayout.Space(6);
        GUILayout.Label("Join with code:");
        code = GUILayout.TextField(code);
        if (GUILayout.Button("Join (code)")) mp.JoinWithCode(code);
        GUILayout.EndArea();
    }
}
