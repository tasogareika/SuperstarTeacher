using UnityEngine;

public class StarEndingPage : MonoBehaviour
{
    private int SFXNo;

    public void startAnim()
    {
        SFXNo = 6;
    }

    public void playSFX()
    {
        SFXNo++;
        BackendHandler.singleton.playSFX(SFXNo);
    }
}
