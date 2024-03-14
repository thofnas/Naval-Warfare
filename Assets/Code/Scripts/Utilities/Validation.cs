using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

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
        
        public static void CheckForDuplicatesByProperty<T, TKey>(IEnumerable<T> items, Func<T, TKey> keySelector, string propertyName)
        {
            var duplicateGroups = items
                .GroupBy(keySelector)
                .Where(group => group.Count() > 1)
                .ToList();

            if (!duplicateGroups.Any()) return;
            
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine($"Duplicate items found in {propertyName}:");
        
            foreach (var group in duplicateGroups)
            {
                errorMessage.AppendLine($"- {typeof(T).Name}: {group.Key}, Count: {group.Count()}");
            }

            throw new InvalidOperationException(errorMessage.ToString());
        }
    }
}