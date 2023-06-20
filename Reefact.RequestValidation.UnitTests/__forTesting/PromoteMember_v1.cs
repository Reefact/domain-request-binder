#region Usings declarations

using System;

#endregion

namespace Reefact.RequestValidation.UnitTests.__forTesting {

    internal class PromoteMember_v1 {

        public Guid TeamId { get; set; }

        public string         MemberUtCode { get; set; }
        public Temperature_v1 Temperature  { get; set; }

    }

}