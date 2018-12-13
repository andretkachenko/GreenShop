using Common.Models.Entity.Interfaces;
using System;
using System.Linq;

namespace Common.Validatiors
{
    public class EqualityValidator
    {
        /// <summary>
        /// Compars two object 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool ReflectiveEquals(object first, object second)
        {
            if (first == null && second == null) return true;
            if (first == null || second == null) return false;

            var firstType = first.GetType();

            if (second.GetType() != firstType)
                throw new Exception("Trying to compare two different object types!");

            return !(from propertyInfo in firstType.GetProperties()
                     where propertyInfo.CanRead
                     let serverValue = propertyInfo.GetValue(first, null)
                     let localValue = propertyInfo.GetValue(second, null)
                     where !Equals(serverValue, localValue)
                     select serverValue).Any();
        }
    }
}
