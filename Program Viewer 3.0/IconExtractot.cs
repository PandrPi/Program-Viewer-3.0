﻿using System;
using Shell32;
using System.IO;
using System.Drawing;
using System.Drawing.IconLib;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using QuickZip.Tools;

namespace Program_Viewer_3
{
    public static class IconExtractor
    {
        private static FileToIconConverter fic = new FileToIconConverter();
        private static readonly int DefaultIconSize = 192;

        public static ImageSource GetIcon(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            if (extension == ".lnk")
            {
                fileName = GetShortcutTargetFile(Path.GetFullPath(fileName));
                extension = Path.GetExtension(fileName);
            }

            if (extension == ".exe")
            {
                MultiIcon mIcon = new MultiIcon();
                mIcon.Load(fileName);

                if (mIcon.Count > 0)
                {
                    SingleIcon temp = mIcon[0];
                    if (temp.Count > 0)
                    {
                        Icon icon = temp[temp.Count - 1].Icon;
                        return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle,
                            new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
                    }
                    else
                        return fic.GetImage(fileName, DefaultIconSize);
                }
                else
                    return fic.GetImage(fileName, DefaultIconSize);
            }
            else
            {
                return fic.GetImage(fileName, DefaultIconSize);
            }
        }

        private static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

    }
}
