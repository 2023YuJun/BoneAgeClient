using Common.Helpers;

namespace FloatingWindowApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
                LogHelper.Log(LogHelper.LogLevel.Error, "UI线程异常", e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                LogHelper.Log(LogHelper.LogLevel.Error, "非UI线程异常", (Exception)e.ExceptionObject);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}