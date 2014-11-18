using System;

namespace FusionMVVM.Common
{
    public class TypeAndName
    {
        public Type Type { get; private set; }
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the TypeAndName class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public TypeAndName(Type type, string name)
        {
            if (type == null) throw new ArgumentNullException("type");
            Type = type;
            Name = name;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected bool Equals(TypeAndName other)
        {
            return Type == other.Type && string.Equals(Name, other.Name);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TypeAndName)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}
