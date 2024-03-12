using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class SceneBuildValidation : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            if (!BuildPipeline.isBuildingPlayer) return;

            if (!AreSceneValidationComponentsValid(scene))
            {
                throw new BuildFailedException($"The scene {scene.name} has failed validation.");
            }
        }

        private static bool AreSceneValidationComponentsValid(Scene scene)
        {
            bool isValid = true;

            foreach (GameObject rootGameObject in scene.GetRootGameObjects())
            {
                IValidate[] validates = rootGameObject.GetComponentsInChildren<IValidate>();

                if (validates is not { Length: > 0 }) continue;
                
                foreach (IValidate validate in validates)
                {
                    if (!validate.IsValid)
                        isValid = false;
                }
            }

            return isValid;
        }
    }
}
