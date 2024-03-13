using System;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    public class UnityMainThread : MonoBehaviour
    {
        private readonly Queue<Action> _jobs = new();

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
    }
}