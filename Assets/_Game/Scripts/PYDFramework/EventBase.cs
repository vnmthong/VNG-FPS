namespace PYDFramework.MVC
{
    public class EventBase
    {
        public EventTypeBase eventType { get; private set; }
        public object sender { get; private set; }
        public object data { get; private set; }

        public EventBase(EventTypeBase eventType, object sender)
        {
            this.eventType = eventType;
            this.sender = sender;
        }

        public EventBase(EventTypeBase eventType, object sender, object data)
        {
            this.eventType = eventType;
            this.sender = sender;
            this.data = data;
        }

    }
}