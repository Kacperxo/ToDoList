using Microsoft.Toolkit.Uwp.Notifications;

public class Notifier
{
    public void ShowToast(string title, string message)
    {
        new ToastContentBuilder()
            .AddText(title)
            .AddText(message)
            .Show();
    }
}