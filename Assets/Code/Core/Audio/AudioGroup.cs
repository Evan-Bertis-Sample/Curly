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
    }
}
