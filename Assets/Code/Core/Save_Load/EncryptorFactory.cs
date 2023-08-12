using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace CurlyCore.Saving
{
    public class EncryptorFactory
    {
        private static readonly Dictionary<string, Type> _encryptors = new Dictionary<string, Type>();
        private static readonly Dictionary<string, string> _tagToID = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> _idToTag = new Dictionary<string, string>();

        public EncryptorFactory()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IEncryptor).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<EncryptorMetadataAttribute>();
                if (attribute != null)
                {
                    _encryptors[attribute.ID] = type;
                    IEncryptor encryptor = CreateEncryptorFromID(attribute.ID);
                    byte[] idBytes = System.Text.Encoding.UTF8.GetBytes(attribute.ID);
                    byte[] bytesEncrypted = encryptor.Encrypt(idBytes);
                    string tag = System.Text.Encoding.UTF8.GetString(bytesEncrypted);
                    _tagToID[tag] = attribute.ID;
                    _idToTag[attribute.ID] = tag;
                }
            }
        }

        public IEncryptor CreateEncryptorFromID(string serializerID)
        {
            if (_encryptors.TryGetValue(serializerID, out Type type))
            {
                return (IEncryptor)Activator.CreateInstance(type);
            }

            throw new InvalidOperationException("Unknown encryptor type.");
        }

        public IEncryptor CreateEncryptorFromTag(byte[] tagBytes)
        {
            string tag = System.Text.Encoding.UTF8.GetString(tagBytes);
            string id = _tagToID[tag];
            return CreateEncryptorFromID(id);
        }

        public byte[] GetTag(IEncryptor encryptor)
        {
            var attribute = encryptor.GetType().GetCustomAttribute<EncryptorMetadataAttribute>();
            if (attribute == null)
            {
                throw new Exception("Encrytpor is unregistered. Add an EncryptorMetadataAttribute to register the encryptor");
            }

            string tag = _idToTag[attribute.ID];
            return System.Text.Encoding.UTF8.GetBytes(tag);
        }
    }
}
