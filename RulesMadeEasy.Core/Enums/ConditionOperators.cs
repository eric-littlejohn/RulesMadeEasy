namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The operators that are supported by the engine
    /// </summary>
    public enum ConditionOperator : int
    {
        Unspecified = -1,
        And,
        Or,
        Equal,
        NotEqual,
        LessThan,
        LessEqualTo,
        GreaterThan,
        GreaterThanEqualTo
    }
}
