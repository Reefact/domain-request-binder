#region Usings declarations

using System;
using System.Collections.Generic;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal class Request_v1 {

        // ReSharper disable once MemberHidesStaticFromOuterClass
        public User_v1?       User      { get; set; }
        public List<Role_v1>? Roles     { get; set; }
        public List<Guid>?    FriendIds { get; set; }

    }

}