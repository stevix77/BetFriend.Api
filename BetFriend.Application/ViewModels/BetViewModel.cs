namespace BetFriend.Application.ViewModels
{
    using System;
    using System.Collections.Generic;

    public class BetViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid Creator { get; set; }
        public IReadOnlyCollection<MemberViewModel> Participants { get; set; } = new List<MemberViewModel>();
    }
}
