using System;

namespace Minesweeper
{
    static public class EventHandlerExtensions
    {
        public static void Raise(this EventHandler @event, object sender, EventArgs e)
        {
            var handler = @event;
            if (handler != null)
                handler(sender, e);
        }

        public static void Raise<T>(this EventHandler<T> @event, object sender, T e)
            where T : EventArgs
        {
            var handler = @event;
            if (handler != null)
                handler(sender, e);
        }
    }
}