using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private KharisiriController _controller;
    [SerializeField] private KharasiriSounds _kharasiriSounds;
    [SerializeField] private Animator _kharasiriAnimator;

    public void OnStartButtonClicked()
    {
        _controller.StartBehaviorTree();
        _kharasiriSounds.StartGrowling();
        _kharasiriAnimator.SetBool("bStart", true);
        gameObject.SetActive(false);
    }

    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
