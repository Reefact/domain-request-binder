#region Usings declarations

using System;

#endregion

namespace Reefact.RequestValidation.UnitTests.__forTesting {

    public sealed class PromoteMember {

        #region Constructors declarations

        public PromoteMember(AnyId teamId, string memberUtCode, Temperature temperature) {
            if (teamId is null) { throw new ArgumentNullException(nameof(teamId)); }
            if (memberUtCode is null) { throw new ArgumentNullException(nameof(memberUtCode)); }
            if (temperature is null) { throw new ArgumentNullException(nameof(temperature)); }

            TeamId       = teamId;
            MemberUtCode = memberUtCode;
            Temperature  = temperature;
        }

        #endregion

        public AnyId       TeamId       { get; }
        public string      MemberUtCode { get; }
        public Temperature Temperature  { get; }

    }

}