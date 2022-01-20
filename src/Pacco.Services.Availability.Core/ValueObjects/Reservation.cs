using System;
using System.Collections.Generic;
using System.Text;

namespace Pacco.Services.Availability.Core.ValueObjects
{
    public struct Reservation: IEquatable<Reservation>
    {
        public DateTime Date { get; }
        public int Priority { get; }

        public Reservation(DateTime date, int priority)
        {
            Date = date;
            Priority = priority;
        }

        public bool Equals(Reservation other)
        {
            return Date.Equals(other.Date) && Priority == other.Priority;
        }

        public override bool Equals(object obj)
        {
            return obj is Reservation other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Priority);
        }
    }
}
