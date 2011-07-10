using Lokad.Cqrs;

using SimpleCQRS.ReadModel;

namespace CQRSGui
{
    public static class ServiceLocator
    {
        public static IMessageSender Bus { get; set; }
        public static IReadModelFacade ReadModel { get; set; }
    }
}