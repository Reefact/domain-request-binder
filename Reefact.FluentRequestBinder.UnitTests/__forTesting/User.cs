#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal class User {

        #region Constructors declarations

        public User(Guid id, UserName userName) {
            Id       = id;
            UserName = userName;
        }

        #endregion

        public Guid     Id       { get; }
        public UserName UserName { get; }

    }

}