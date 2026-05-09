namespace TrainTicket.WinForms.Helpers
{
    // Toast không ch?n lu?ng UI, t? ?óng sau vài giây.
    public class ToastForm : Form
    {
        private readonly System.Windows.Forms.Timer _lifeTimer = new();
        private readonly System.Windows.Forms.Timer _slideTimer = new();
        private readonly int _targetTop;

        public ToastForm(string message, Color accentColor)
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            Width = 320;
            Height = 92;
            BackColor = Color.FromArgb(15, 23, 42);

            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(12)
            };

            var accent = new Panel
            {
                Dock = DockStyle.Left,
                Width = 6,
                BackColor = accentColor
            };

            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = message,
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnl.Controls.Add(lbl);
            pnl.Controls.Add(accent);
            Controls.Add(pnl);

            var area = Screen.FromPoint(Cursor.Position).WorkingArea;
            Left = area.Right - Width - 16;
            Top = area.Bottom + Height;
            _targetTop = area.Bottom - Height - 20;

            _slideTimer.Interval = 10;
            _slideTimer.Tick += (_, _) =>
            {
                if (Top <= _targetTop)
                {
                    Top = _targetTop;
                    _slideTimer.Stop();
                    return;
                }

                Top -= 12;
            };

            _lifeTimer.Interval = 2600;
            _lifeTimer.Tick += (_, _) =>
            {
                _lifeTimer.Stop();
                Close();
            };
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _slideTimer.Start();
            _lifeTimer.Start();
        }
    }
}
