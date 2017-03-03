namespace QueryAsJson.Core.Engine
{
    /// <summary>
    /// Prefix types for sql parameter. 
    /// </summary>
    public enum CommandParameterPrefix
    {
        /// <summary>
        /// '@' Prefix for mssql-, sqlite-, postgres- parameters
        /// </summary>
        AtSign,
        /// <summary>
        /// ":" Prefix for oracle parameters
        /// </summary>
        Colon
    }
    /// <summary>
    /// Extension Methods for the enum <see cref="CommandParameterPrefix"/>
    /// </summary>
    public static class CommandParameterPrefixExtension
    {
        /// <summary>
        /// Return the matching prefix char.
        /// '@' or ':'
        /// </summary>
        /// <param name="prefix">prefix type</param>
        /// <returns>for AtSign='@' and Colon=':'</returns>
        public static string ToRealPrefix(this CommandParameterPrefix prefix)
        {
            switch (prefix)
            {
                case CommandParameterPrefix.AtSign:
                    return "@";
                case CommandParameterPrefix.Colon:
                    return ":";
            }
            return null;
        }
    }
}
