namespace Vtb24.Logging.Interaction
{
    public static class StartInteraction
    {
        public static INameSpecifier With(string subject)
        {
            return new NameSpecifier { Subject = subject };
        }

        public interface INameSpecifier
        {
            IInteractionLogEntry For(string name);
        }

        private class NameSpecifier : INameSpecifier
        {
            public string Subject { private get; set; }

            public IInteractionLogEntry For(string name)
            {
                return new InteractionLogEntry(Subject, name);
            }
        }
    }
}
