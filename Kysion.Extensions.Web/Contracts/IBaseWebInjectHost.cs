namespace Kysion.Extensions.Web.Contracts
{
    public interface IBaseWebInjectHost
    {
        void OnInjectStyleComplete();
        void OnInjectScriptComplete();
        void OnSuccessMessage(string message, string? category = null);
        void OnFailureMessage(string message, string? category = null);
    }
}
