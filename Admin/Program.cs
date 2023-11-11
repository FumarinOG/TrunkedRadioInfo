using Admin.Helpers;
using System;
using System.Windows.Forms;
using Unity;

namespace Admin
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            var container = new UnityContainer();
            var unityConfig = new UnityConfig(container);

            unityConfig.Config();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(MainRadForm.ActiveForm);
            Application.Run(container.Resolve<MainRadForm>());
        }
    }
}
