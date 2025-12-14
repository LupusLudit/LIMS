namespace LIMS.Logic.Events
{
    public static class BusyStateChangedEvent
    {

        public static event Action<bool, string>? OnBusyStateChanged;

        public static void RaiseBusyStateChanged(bool isBusy, string content = "Processing...")
        {
            OnBusyStateChanged?.Invoke(isBusy, content);
        }
    }
}
