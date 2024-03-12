using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class UnityMainThread : MonoBehaviour
    {
        public static UnityMainThread Instance;
        private readonly Queue<Action> _jobs = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            const int maxJobsPerFrame = 10; // Limit the number of jobs processed per frame
            int jobsProcessed = 0;

            while (_jobs.Count > 0 && jobsProcessed < maxJobsPerFrame)
            {
                _jobs.Dequeue()?.Invoke();
                jobsProcessed++;
            }
        }

        public void AddJob(Action newJob)
        {
            _jobs.Enqueue(newJob);
        }

        public void DestroyInstance()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }
}