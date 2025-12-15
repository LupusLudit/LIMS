namespace LIMS.Logic.Events
{
    /// <include file='../../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="BusyStateChangedEvent"]/*'/>
    public static class BusyStateChangedEvent
    {

        public static event Action<bool, string>? OnBusyStateChanged;

        public static void RaiseBusyStateChanged(bool isBusy, string message = "Processing...")
        {
            OnBusyStateChanged?.Invoke(isBusy, message);
        }
    }
}
