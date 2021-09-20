namespace BetFriend.Bet.Domain.Bets
{
    using System;

    public class Status
    {
        private bool _success;
        private DateTime _dateTime;

        public Status(bool success, DateTime dateTime)
        {
            _success = success;
            _dateTime = dateTime;
        }

        public override bool Equals(object obj)
        {
            return (obj as Status)._success == _success 
                && (obj as Status)._dateTime.CompareTo(_dateTime) == 0;
        }

        internal bool IsSuccess() => _success;
    }
}
