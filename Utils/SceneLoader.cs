using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main
{
    public class SceneLoader : MonoBehaviour
    {

        [Scene] public string targetScene;
        public bool loadAdditive;

        [Button("Load Target Scene", EButtonEnableMode.Playmode)]
        public void LoadTargetScene()
        {
            if (loadAdditive)
            {
                SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
            }
        }

        [Button("Activate Target Scene", EButtonEnableMode.Playmode)]
        public void ActivateTargetScene()
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene));
        }

    }
}
