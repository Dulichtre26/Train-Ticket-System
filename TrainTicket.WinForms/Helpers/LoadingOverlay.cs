using Guna.UI2.WinForms;

namespace TrainTicket.WinForms.Helpers
{
    // Overlay loading tái s? d?ng cho các form có tác v? async.
    public class LoadingOverlay
    {
        private readonly Guna2Panel _overlay = new();
        private readonly Label _label = new();
        private readonly ProgressBar _progress = new();

        public LoadingOverlay(Control parent)
        {
            _overlay.Dock = DockStyle.Fill;
            _overlay.FillColor = Color.FromArgb(120, 15, 23, 42);
            _overlay.Visible = false;

            _progress.Style = ProgressBarStyle.Marquee;
            _progress.MarqueeAnimationSpeed = 30;
            _progress.Width = 220;
            _progress.Height = 24;
            _progress.Left = Math.Max(20, (parent.Width - _progress.Width) / 2);
            _progress.Top = Math.Max(20, (parent.Height - _progress.Height) / 2 - 18);
            _progress.Anchor = AnchorStyles.None;

            _label.Text = "?ang t?i d? li?u...";
            _label.AutoSize = true;
            _label.ForeColor = Color.White;
            _label.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _label.Left = _progress.Left + 38;
            _label.Top = _progress.Bottom + 8;

            _overlay.Controls.Add(_progress);
            _overlay.Controls.Add(_label);
            parent.Controls.Add(_overlay);
            _overlay.BringToFront();

            parent.SizeChanged += (_, _) =>
            {
                _progress.Left = Math.Max(20, (parent.Width - _progress.Width) / 2);
                _progress.Top = Math.Max(20, (parent.Height - _progress.Height) / 2 - 18);
                _label.Left = _progress.Left + 38;
                _label.Top = _progress.Bottom + 8;
            };
        }

        public void Show(string message = "?ang t?i d? li?u...")
        {
            _label.Text = message;
            _overlay.Visible = true;
            _overlay.BringToFront();
        }

        public void Hide()
        {
            _overlay.Visible = false;
        }
    }
}
