﻿using ScreenshotBox.Logic;
using System;
using System.Windows;
using System.Windows.Forms;

namespace ScreenshotBox.Forms
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		NotifyIcon notifyIcon = new NotifyIcon();

		public MainWindow()
		{
			InitializeComponent();
			Hide();

			ClipboardNotification.ClipboardUpdatedEvent += ClipboardNotification_ClipboardUpdatedEvent;

			PrepareNotificationIcon();
			RegisterWindowEvents();
		}


		private static object thisLock = new object();

		[STAThread]
		private void ClipboardNotification_ClipboardUpdatedEvent(object sender, EventArgs e)
		{
			if (System.Windows.Clipboard.ContainsImage())
			{
				lock (thisLock)
				{
					var image = System.Windows.Clipboard.GetImage();
					var blinkWindow = ShowBlinkWindow();
					FileManager.SaveBitmap(image);
					blinkWindow.Hide();
				}
			}
		}

		private static Blink ShowBlinkWindow()
		{
			var blinkWindow = new Blink();
			blinkWindow.Show();
			blinkWindow.Topmost = true;

			return blinkWindow;
		}

		private void PrepareNotificationIcon()
		{
			CreateNotificationIcon();
			SetContextMenuForNotificationIcon();
		}

		private void CreateNotificationIcon()
		{
			notifyIcon.Icon = Properties.Resources.status_bar_icon;
			notifyIcon.Visible = true;
			notifyIcon.Text = "ScreenshotBox";
		}

		private void SetContextMenuForNotificationIcon()
		{
			notifyIcon.ContextMenu = new ContextMenu();
			notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Settings", OpenSettigns));
			notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("About", OpenAboutWindow));
			notifyIcon.ContextMenu.MenuItems.Add(new MenuItem("Exit", CloseApplication));
		}

		private void OpenAboutWindow(object sender, EventArgs e)
		{
			var aboutWindow = new About();
			aboutWindow.Show();
		}

		private void OpenSettigns(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void CloseApplication(object sender, EventArgs e)
		{
			Close();
		}

		private void RegisterWindowEvents()
		{
			Closing += MainWindow_Closing;
		}

		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			notifyIcon.Visible = false;
		}
	}
}