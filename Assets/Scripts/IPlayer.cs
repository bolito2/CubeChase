using UnityEngine;

public interface IPlayer
{
    void MovePlayer(string direction);
    bool isAgainstWall(string direction, GameObject Objective);
    void MoveUp();
    void MoveDown();
}

public class CheckIPlayer : MonoBehaviour
{
    static System.Type scriptType;
    public static T isPlayer<T>(T script, out bool isplayer)
    {
        if (script is IPlayer)
            isplayer = true;
        else
            isplayer = false;
        return script;
    }
}