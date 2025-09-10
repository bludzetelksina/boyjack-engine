using System;
using System.Windows.Forms;

namespace BoyJackEngine
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var engine = new BoyJackEngine();
            
            try
            {
                engine.Initialize("BoyJack Engine Windows Demo", 800, 600);
                engine.LoadScript("Assets/scripts/main.bjs");
                engine.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Engine Error: {ex.Message}", "BoyJack Engine Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                engine.Dispose();
            }
        }
    }
}