namespace HelsiTaskManager.WebAPI.Attributes
{
    public class HelsiRigthCheckAttribute: Attribute
    {
        private readonly HelsiRightType _helsiRightTypes;
        public HelsiRightType Type => _helsiRightTypes;

        public HelsiRigthCheckAttribute(HelsiRightType helsiRightTypes)
        {
            _helsiRightTypes = helsiRightTypes;
        }

        public enum HelsiRightType
        {
            Owner,
            LinkedUser,
            Full
        }
    }
}
