using UnityEngine;

public class BlackScreenController : MonoBehaviour
{
    [SerializeField] private Animator AnimatorReference;

    public void FadeOut()
    {
        AnimatorReference.Play("FadeOut");
    }

    public void FadeIn()
    {
        AnimatorReference.Play("FadeIn");
    }

    public void FadeInAndOut()
    {
        FadeIn();
        Invoke("FadeOut", 1f);
    }
}
