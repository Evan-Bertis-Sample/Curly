using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurlyCore.Saving
{
    public interface IEncryptor
    {
        public void Encrypt(string path);
    }
}
