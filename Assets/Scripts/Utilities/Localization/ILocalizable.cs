namespace Assets.Scripts.Utilities.Localization
{
    public interface ILocalizableText
    {
        string Key { get; }

        void Localize(string localizedText);
    }
}
