using System;
using System.Drawing;
using System.Windows.Forms;

namespace BoyJackEngine.BJGSH
{
    public class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "About BoyJack Engine";
            Size = new Size(400, 300);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var titleLabel = new Label
            {
                Text = "BoyJack Engine",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(360, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DarkBlue
            };

            var versionLabel = new Label
            {
                Text = "Version 0.1.0",
                Font = new Font("Segoe UI", 10),
                Location = new Point(20, 60),
                Size = new Size(360, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var descriptionLabel = new Label
            {
                Text = "A simple yet powerful 2D game engine\ndesigned for beginner game developers",
                Font = new Font("Segoe UI", 9),
                Location = new Point(20, 90),
                Size = new Size(360, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var featuresLabel = new Label
            {
                Text = "Features:\n• BJG Graphics Module\n• BJGSH Game Shell\n• BJS Custom Scripting Language\n• 60 FPS Game Loop\n• Asset Management\n• Scene System",
                Font = new Font("Segoe UI", 9),
                Location = new Point(20, 140),
                Size = new Size(360, 100),
                TextAlign = ContentAlignment.TopLeft
            };

            var okButton = new Button
            {
                Text = "OK",
                Location = new Point(160, 250),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };

            Controls.AddRange(new Control[] { titleLabel, versionLabel, descriptionLabel, featuresLabel, okButton });
        }
    }
}