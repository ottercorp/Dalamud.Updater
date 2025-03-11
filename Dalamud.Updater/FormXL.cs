using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dalamud.Updater
{
    public class MidiPlayer
    {
        // Import necessary functions from winmm.dll
        [DllImport("winmm.dll", SetLastError = true)]
        private static extern long mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

        public static void PlayMidi(string fileName)
        {
            string command = $"open \"{fileName}\" type sequencer alias midi";
            mciSendString(command, null, 0, IntPtr.Zero);
            mciSendString("play midi", null, 0, IntPtr.Zero);
        }

        public static void StopMidi()
        {
            mciSendString("stop midi", null, 0, IntPtr.Zero);
            mciSendString("close midi", null, 0, IntPtr.Zero);
        }

        public static string ExtractEmbeddedResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string tempFile = Path.Combine(Path.GetTempPath(), resourceName);

            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null) throw new ArgumentException($"Resource {resourceName} not found.");
                using (FileStream fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }

            return tempFile;
        }
    }

    public partial class FormXL : Form
    {
        public FormXL()
        {
            InitializeComponent();
            try
            {
                string midiFile = MidiPlayer.ExtractEmbeddedResource(@"Dalamud.Updater.Resources.48186_One-Last-Kiss2.mid");
                MidiPlayer.PlayMidi(midiFile);
            }
            catch { }
        }

        private void FormXL_FormClosed(object sender, FormClosedEventArgs e)
        {
            try {
                MidiPlayer.StopMidi(); 
            }
            catch {
            }
            //Application.Exit();
        }

        private void FormXL_Paint(object sender, PaintEventArgs e)
        {
            // Define the gradient colors
            var color1 = Color.FromArgb(19, 47, 70);
            var color2 = Color.FromArgb(55, 141, 255);
            // new color "#132f46"


            // Create a LinearGradientBrush
            using var brush = new LinearGradientBrush(this.ClientRectangle, color1, color2, 30F);
            // Fill the form's background with the gradient
            //e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("https://aonyx.ffxiv.wang/?form=du");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
