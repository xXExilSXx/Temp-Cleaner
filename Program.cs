using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

class Program
{
    static NotifyIcon trayIcon;
    static Thread cleaningThread;
    static int cleaningInterval = 7200000; // 2 Stunden
    static bool isEnglish = true; // Default

    static void Main(string[] args)
    {
        trayIcon = new NotifyIcon();
        trayIcon.Icon = new System.Drawing.Icon("logo.ico");
        trayIcon.Text = "Temp File Cleaner";
        trayIcon.Visible = true;


        trayIcon.ContextMenu = new ContextMenu(new MenuItem[] {
            new MenuItem("Delete temp files",DeleteTempFilesNow),
            new MenuItem("-"),
            new MenuItem("Set cleaning time", new MenuItem[] {
                new MenuItem("2 hours",Check2hours),
                new MenuItem("1 hour", Check1hour),
                new MenuItem("30 minutes", Check30min)
            }),
            new MenuItem("-"),
            new MenuItem("Language", new MenuItem[] {
                new MenuItem("English", new EventHandler(ToggleLanguage), Shortcut.CtrlE),
                new MenuItem("German", new EventHandler(ToggleLanguage), Shortcut.CtrlG)
            }),
            new MenuItem("-"),
            new MenuItem("Exit", Exit),
            new MenuItem("-"),
            new MenuItem("-"),
        });
        SetLanguage();

        cleaningThread = new Thread(new ThreadStart(DeleteTempFiles));
        cleaningThread.Start();

        Application.Run();
    }

    static void ToggleLanguage(object sender, EventArgs e)
    {
        MenuItem item = (MenuItem)sender;
        isEnglish = !isEnglish;
        item.Checked = !item.Checked;
        SetLanguage();
    }
    static void SetLanguage()
    {
        if (isEnglish)
        {
            trayIcon.ContextMenu.MenuItems[0].Text = "Delete temp files";
            trayIcon.ContextMenu.MenuItems[2].Text = "Set cleaning time";
            trayIcon.ContextMenu.MenuItems[2].MenuItems[0].Text = "2 hours";
            trayIcon.ContextMenu.MenuItems[2].MenuItems[1].Text = "1 hour";
            trayIcon.ContextMenu.MenuItems[2].MenuItems[2].Text = "30 minutes";
            trayIcon.ContextMenu.MenuItems[4].Text = "Language";
            trayIcon.ContextMenu.MenuItems[4].MenuItems[0].Text = "English";
            trayIcon.ContextMenu.MenuItems[4].MenuItems[1].Text = "German";
            trayIcon.ContextMenu.MenuItems[6].Text = "Exit";
            trayIcon.ContextMenu.MenuItems[8].Text = "Made with <3 by Exil";
        }
        else
        {
            trayIcon.ContextMenu.MenuItems[0].Text = "Lösche temporäre Dateien";
            trayIcon.ContextMenu.MenuItems[2].Text = "Setze Reinigungszeit";
            trayIcon.ContextMenu.MenuItems[2].MenuItems[0].Text = "2 Stunden";
            trayIcon.ContextMenu.MenuItems[2].MenuItems[1].Text = "1 Stunde";
            trayIcon.ContextMenu.MenuItems[2].MenuItems[2].Text = "30 Minuten";
            trayIcon.ContextMenu.MenuItems[4].Text = "Sprache";
            trayIcon.ContextMenu.MenuItems[4].MenuItems[0].Text = "Englisch";
            trayIcon.ContextMenu.MenuItems[4].MenuItems[1].Text = "Deutsch";
            trayIcon.ContextMenu.MenuItems[6].Text = "Beenden";
            trayIcon.ContextMenu.MenuItems[8].Text = "Mit <3 gemacht von Exil";
        }
    }
    static void DeleteTempFiles()
    {
        // Get the temp folder path
        string tempPath = Path.GetTempPath();
        try
        {
            // Get all files and directories in the temp folder
            string[] tempFiles = Directory.GetFiles(tempPath, "*", SearchOption.AllDirectories);

            // Loop through all files and directories and try to delete them
            foreach (string file in tempFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch (UnauthorizedAccessException)
                {
                    // Skip file or folder if it can't be deleted
                }
                catch (IOException)
                {
                    // Skip file or folder if it can't be deleted
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Skip the temp folder if it can't be accessed
        }

        Thread.Sleep(cleaningInterval);
    }


    static void DeleteTempFilesNow(object sender, EventArgs e)
    {
        string tempPath = Path.GetTempPath();

        string[] tempFiles = Directory.GetFiles(tempPath, "*", SearchOption.AllDirectories);

        foreach (string file in tempFiles)
        {
            try
            {
                File.Delete(file);
            }
            catch (UnauthorizedAccessException)
            {
                
            }
            catch (IOException)
            {
                
            }
        }

        if (isEnglish)
            MessageBox.Show("Cleaning Done!");
        else
            MessageBox.Show("Bereinigung abgeschlossen!");


    }
    

    static void Check2hours(object sender, EventArgs e)
    {
        cleaningInterval = 7200000;
    }

    static void Check1hour(object sender, EventArgs e)
    {
        cleaningInterval = 3600000;
    }

    static void Check30min(object sender, EventArgs e)
    {
        cleaningInterval = 1800000;
    }

    static void Exit(object sender, EventArgs e)
    {
        cleaningThread.Abort();

        trayIcon.Visible = false;
        Application.Exit();
    }
}

