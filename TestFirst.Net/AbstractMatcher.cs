using System;

namespace TestFirst.Net
{
    public abstract class AbstractMatcher<T> : IMatcher<T>
    {
        private readonly Nulls m_isNullable;

        protected AbstractMatcher() 
            : this(true)
        {
        }

        protected AbstractMatcher(bool allowNull)
        {
            if (IsNullableType(typeof(T))) 
            {
                m_isNullable = allowNull ? Nulls.Allowed : Nulls.NotAllowed;
            } 
            else 
            {
                m_isNullable = Nulls.NonNullableType;
            }
        }

        private enum Nulls
        {
            NonNullableType, Allowed, NotAllowed
        }

        /// <summary>
        /// Does nothing, subclasses should override if they choose. No need to call this from sub classes
        /// </summary>
        /// <param name="description">The description to populate</param>
        public virtual void DescribeTo(IDescription description)
        {
            // do nothing
        }

        /// <summary>
        /// Calls <see cref="Matches(object, IMatchDiagnostics)"/> with a <see cref="NullMatchDiagnostics"/>
        /// </summary>
        /// <param name="actual">The object to match against</param>
        /// <returns>True if the matcher matches</returns>
        public bool Matches(object actual)
        {
            return Matches(actual, NullMatchDiagnostics.Instance);
        }

        /// <summary>
        /// Performs a type check to ensure the object is of the expected type. 
        /// </summary>
        /// <param name="actual">The object to match against</param>
        /// <param name="diagnostics">The match diagnostics to write the mismatch description to</param>
        /// <returns>true if the matcher matches</returns>
        public bool Matches(object actual, IMatchDiagnostics diagnostics)
        {
            if (actual == null)
            {
                switch (m_isNullable)
                {
                    case Nulls.Allowed:
                        return Matches((T)actual, diagnostics);
                    case Nulls.NotAllowed:
                        diagnostics.MisMatched("Expected non null");
                        return false;
                    case Nulls.NonNullableType:
                        diagnostics.MisMatched("Wrong type, expected type {0} which is non nullable, but got null instead", typeof(T).FullName);
                        return false;
                }
            }
            if (!IsValidType(actual))
            {
                diagnostics.MisMatched(Description.With()
                    .Text("Wrong type, expected {0} but got {1}", typeof(T).FullName, actual.GetType().FullName)
                    .Value("actual", actual.GetType().FullName));
                return false;
            }

            T val;
            try 
            {
                val = Wrap(actual);
            } 
            catch (InvalidCastException e)
            {
                throw new InvalidCastException(string.Format("Could not cast {0} to {1}", actual.GetType().FullName, typeof(T).FullName), e);
            }
            return Matches(val, diagnostics);
        }

        public override string ToString()
        {
            var desc = new Description();
            DescribeTo(desc);
            return desc.ToString();
        }

        /// <summary>
        /// Sub classes must override this
        /// </summary>
        /// <param name="actual">The object to match against</param>
        /// <param name="diag">The match diagnostics to populate with mismatch description</param>
        /// <returns>True if the matcher matches</returns>
        public abstract bool Matches(T actual, IMatchDiagnostics diag);

        protected virtual T Wrap(object actual)
        {
            return (T)actual;
        }

        protected virtual bool IsValidType(object actual)
        {
            return actual is T;
        }

        private static bool IsNullableType(Type t)
        {
            if (!t.IsValueType) 
                return true; // ref-type
            if (Nullable.GetUnderlyingType(t) != null) 
                return true; // Nullable<T>
            return false; // value-type
        }
    }
}
