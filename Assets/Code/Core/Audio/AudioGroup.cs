using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CurlyCore.Audio
{
    [System.Serializable]
    public struct AudioGroup
    {
        public AudioOverride Override;
        public List<AssetReference> AudioReferences;

        public AssetReference GetReference()
        {
            return AudioReferences[0];
        }

        public AssetReference ChooseRandom()
        {
            return AudioReferences[Random.Range(0, AudioReferences.Count - 1)];
        }

        public AssetReference ChooseClip(string clipPath)
        {
            return AudioReferences[0];
        }
    }
}
