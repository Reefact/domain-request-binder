#region Usings declarations

#endregion

namespace Reefact.FluentRequestBinder.UnitTests.__forTesting {

    internal class User {

        #region Constructors declarations

        public User(Guid id, int? age, UserName userName) {
            Id       = id;
            Age      = age;
            UserName = userName;
        }

        #endregion

        public int?     Age      { get; }
        public Guid     Id       { get; }
        public UserName UserName { get; }

    }

}