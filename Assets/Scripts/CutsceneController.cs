using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private CanvasGroup skipText;
    [SerializeField] private PlayableDirector[] directors;
    private PlayableDirector activeDirector;
    private static CutsceneController _instance;

    private void Start()
    {
        _instance = this;
        PlayCutscene(directors[0].gameObject.name);
        LeanTween.alphaCanvas(skipText, 0, 1f).setLoopPingPong();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Skip();
    }

    public void Skip()
    {
        if (activeDirector == null) return;
        activeDirector.time = activeDirector.playableAsset.duration;
        activeDirector = null;
    }

    private void PlayCutsceneInternal(string cutsceneName)
    {
        Skip();
        var cutscene = directors.First(dir => dir.gameObject.name == cutsceneName);
        if (cutscene == null)
        {
            throw new System.NullReferenceException("No cutscene with name " + cutsceneName);
        }
        cutscene.Play();
        activeDirector = cutscene;
    }

    public static void PlayCutscene(string name)
    {
        _instance.PlayCutsceneInternal(name);
    }
}
