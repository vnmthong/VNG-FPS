using System;
using System.Security.Cryptography;
using System.Text;

namespace PYDFramework.MVC
{
    public struct EventTypeBase : IComparable<EventTypeBase>
    {
        public uint uId;
        public string name;

        public EventTypeBase(string name)
        {
            var md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(name));

            this.name = name;
            this.uId = BitConverter.ToUInt32(hashed, 0);
        }

        public int CompareTo(EventTypeBase other)
        {
            return uId.CompareTo(other.uId);
        }

        public override int GetHashCode()
        {
            return 29111998 + uId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EventTypeBase))
                return false;

            var other = (EventTypeBase)obj;
            return uId == other.uId;
        }

        public static bool operator ==(EventTypeBase a, EventTypeBase b)
        {
            return a.uId == b.uId;
        }

        public static bool operator !=(EventTypeBase a, EventTypeBase b)
        {
            return a.uId != b.uId;
        }
    }

}