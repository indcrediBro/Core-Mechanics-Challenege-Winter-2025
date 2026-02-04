using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public void LoadingAnimationCompleted()
    {
        UIManager.Instance.DisableLoadingScreen();
    }

    public void ExpolsionSFX()
    {
        AudioManager.Instance.PlaySound("SFX_Explode");
    }
}
