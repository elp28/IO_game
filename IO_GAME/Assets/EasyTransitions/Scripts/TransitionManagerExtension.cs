using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTransition
{
    /// <summary>
    /// Extensão do TransitionManager.
    /// Adicione este script no mesmo GameObject do TransitionManager.
    /// Fornece TransitionWithCallback — transição sem troca de cena,
    /// com um Action chamado no cut point.
    /// </summary>
    public class TransitionManagerExtension : MonoBehaviour
    {
        public static TransitionManagerExtension instance;

        [SerializeField] private GameObject transitionTemplate;

        private bool _running = false;

        void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// Roda uma transição sem trocar de cena.
        /// onCutPoint é chamado no meio — use para mostrar painéis, etc.
        /// </summary>
        public void TransitionWithCallback(TransitionSettings settings, float startDelay, Action onCutPoint)
        {
            if (settings == null || _running)
            {
                Debug.LogWarning("[TransitionManagerExtension] Transição já rodando ou settings null.");
                onCutPoint?.Invoke(); // garante que o callback acontece mesmo sem transição
                return;
            }

            StartCoroutine(RunTransition(settings, startDelay, onCutPoint));
        }

        private IEnumerator RunTransition(TransitionSettings settings, float startDelay, Action onCutPoint)
        {
            _running = true;

            yield return new WaitForSecondsRealtime(startDelay);

            // Instancia a transição
            GameObject template = Instantiate(transitionTemplate);
            Transition transition = template.GetComponent<Transition>();
            transition.transitionSettings = settings;

            // Espera o transitionIN terminar
            float transitionTime = settings.transitionTime;
            if (settings.autoAdjustTransitionTime)
                transitionTime = transitionTime / settings.transitionSpeed;

            yield return new WaitForSecondsRealtime(transitionTime);

            // Cut point — chama o callback
            onCutPoint?.Invoke();

            // Dispara o transitionOUT manualmente
            transition.OnSceneLoad(SceneManager.GetActiveScene(), LoadSceneMode.Single);

            // Espera o transitionOUT terminar e destrói
            yield return new WaitForSecondsRealtime(settings.destroyTime);

            Destroy(template);
            _running = false;
        }
    }
}