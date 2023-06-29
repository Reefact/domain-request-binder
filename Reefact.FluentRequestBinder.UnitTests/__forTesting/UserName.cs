#region Usings declarations

using System;

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal class UserName {

        #region Constructors declarations

        public UserName(string firstName, string lastName) {
            if (firstName is null) { throw new ArgumentNullException(nameof(firstName)); }
            if (lastName is null) { throw new ArgumentNullException(nameof(lastName)); }

            FirstName = firstName;
            LastName  = lastName;
        }

        #endregion

        public string FirstName { get; }
        public string LastName  { get; }

    }

}