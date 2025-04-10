namespace NIF.NET
{
    public interface INIFValidator
    {
        bool IsValid(string nif);
        bool IsValid(string nif, NIFType type);
    }
}
