using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurlyCore.Saving
{
    public class EncryptorMetadataAttribute : Attribute
    {
        public string ID { get; }
        
        public EncryptorMetadataAttribute(string id)
        {
            this.ID = id;
        }
    }
}
