using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    public static class Validation
    {
        public static bool CheckForNull(Object sender, object obj, string fieldName)
        {
            if (obj != null)
                return true;

            if (sender == null)
                Debug.LogWarning($"Field {fieldName} is null.");
            else
                Debug.LogWarning($"Field {fieldName} is null in object {sender.GetType()}.", sender);

            return false;
        }
        
        public static bool CheckIfEmpty<T>(Object sender, IEnumerable<T> collection, string collectionName)
        {
            if (!CheckForNull(sender, collection, collectionName)) return false;
            
            if (collection.Any())
                return true;

            if (sender == null)
                Debug.LogWarning($"Collection {collectionName} is empty.");
            else
                Debug.LogWarning($"Collection {collectionName} is empty in object {sender.GetType()}.", sender);

            return false;
        }
        
        public static bool CheckColor(Object sender, Color color, string colorFieldName)
        {
            if (!CheckForNull(sender, color, colorFieldName)) return false;
            
            Color pink = new(1f, 0f, 1f);

            if (color != pink)
                return true;

            if (sender == null)
                Debug.LogWarning($"{colorFieldName} is pink (1f, 0f, 1f).");
            else
                Debug.LogWarning($"{colorFieldName} is pink (1f, 0f, 1f) in object {sender.GetType()}.", sender);

            return false;
        }
    }
}