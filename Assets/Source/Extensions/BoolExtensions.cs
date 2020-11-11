namespace Assets.Source
{
    public static class BoolExtensions
    {

        /// <summary>
        /// Returns 1.0 if true or 0.0 if false.
        /// </summary>
        /// <param name="boolean"></param>
        /// <returns></returns>
        public static float ToFloat(this bool boolean) 
        {
            return boolean ? 1f : 0f;
        }

    }
}
