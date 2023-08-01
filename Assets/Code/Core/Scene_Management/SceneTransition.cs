using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Core.SceneManagement
{
    public interface ISceneTransition
    {
        public void PrepareAnimation(Canvas screenCanvas);
        public Task Transition(Canvas screenCanvas);
        public void EndAnimation(Canvas screenCanvas);

        public async Task Play(Canvas screenCanvas)
        {
            PrepareAnimation(screenCanvas);
            await Transition(screenCanvas);
            EndAnimation(screenCanvas);
        }
    }
}

