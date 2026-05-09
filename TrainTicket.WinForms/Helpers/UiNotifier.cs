namespace TrainTicket.WinForms.Helpers
{
    // Helper hi?n th? thông báo th?ng nh?t toàn ?ng d?ng.
    public static class UiNotifier
    {
        private static readonly Queue<(string Message, Color Accent)> _toastQueue = new();
        private static bool _isShowingToast;

        public static void Info(string message, string title = "Thông báo")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Error(string message, string title = "L?i")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void SuccessToast(string message)
        {
            ShowToast(message, Color.FromArgb(16, 185, 129));
        }

        public static void ErrorToast(string message)
        {
            ShowToast(message, Color.FromArgb(239, 68, 68));
        }

        public static void InfoToast(string message)
        {
            ShowToast(message, Color.FromArgb(37, 99, 235));
        }

        private static void ShowToast(string message, Color accentColor)
        {
            _toastQueue.Enqueue((message, accentColor));
            TryShowNextToast();
        }

        private static void TryShowNextToast()
        {
            if (_isShowingToast || _toastQueue.Count == 0)
            {
                return;
            }

            if (Application.OpenForms.Count == 0)
            {
                _toastQueue.Clear();
                return;
            }

            _isShowingToast = true;
            var (message, accent) = _toastQueue.Dequeue();
            var toast = new ToastForm(message, accent);
            toast.FormClosed += (_, _) =>
            {
                _isShowingToast = false;
                TryShowNextToast();
            };

            toast.Show();
        }
    }
}
