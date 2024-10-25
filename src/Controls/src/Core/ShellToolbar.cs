﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
	internal class ShellToolbar : Toolbar
	{
		Shell _shell;
		Page? _currentPage;
		BackButtonBehavior? _backButtonBehavior;
		ToolbarTracker _toolbarTracker = new ToolbarTracker();
#if WINDOWS
		MenuBarTracker _menuBarTracker = new MenuBarTracker();
#endif
		bool _drawerToggleVisible;

		public override bool DrawerToggleVisible { get => _drawerToggleVisible; set => SetProperty(ref _drawerToggleVisible, value); }
		public Page? CurrentPage => _currentPage;
		public ShellToolbar(Shell shell) : base(shell)
		{
			_drawerToggleVisible = true;
			BackButtonVisible = false;
			_shell = shell;
			shell.Navigated += (_, __) => ApplyChanges();
			shell.PropertyChanged += (_, p) =>
			{
				if (p.IsOneOf(
					Shell.CurrentItemProperty,
					Shell.FlyoutBehaviorProperty,
					Shell.BackButtonBehaviorProperty,
					Shell.NavBarIsVisibleProperty,
					Shell.TitleViewProperty))
				{
					ApplyChanges();
				}
				else if (p.Is(Shell.TitleProperty))
				{
					UpdateTitle();
				}
			};

			shell.HandlerChanged += (_, __) => ApplyChanges();

			ApplyChanges();
			_toolbarTracker.CollectionChanged += (_, __) => ToolbarItems = _toolbarTracker.ToolbarItems;
#if WINDOWS
			_menuBarTracker.CollectionChanged += (_, __) => ApplyChanges();
#endif
		}

		internal void ApplyChanges()
		{
			var currentPage = _shell.GetCurrentShellPage();

			if (_currentPage != currentPage)
			{
				if (_currentPage != null)
				{
					_currentPage.PropertyChanged -= OnCurrentPagePropertyChanged;
				}

				_currentPage = currentPage;

				if (_currentPage != null)
				{
					_currentPage.PropertyChanged += OnCurrentPagePropertyChanged;
				}
			}

			if (currentPage == null)

/* Unmerged change from project 'Controls.Core(net8.0-windows10.0.19041)'
Before:
				return;

			var stack = _shell.Navigation.NavigationStack;
			if (stack.Count == 0)
				return;

			_toolbarTracker.Target = _shell;
#if WINDOWS
			_menuBarTracker.Target = _shell;
#endif

			Page? previousPage = null;
After:
			{
				return;
			}

			var stack = null;
*/

/* Unmerged change from project 'Controls.Core(net8.0-windows10.0.20348)'
Before:
				return;

			var stack = _shell.Navigation.NavigationStack;
			if (stack.Count == 0)
				return;

			_toolbarTracker.Target = _shell;
#if WINDOWS
			_menuBarTracker.Target = _shell;
#endif

			Page? previousPage = null;
			if (stack.Count > 1)
				previousPage = stack[stack.Count - 1];

			ToolbarItems = _toolbarTracker.ToolbarItems;

			UpdateBackbuttonBehavior();
			bool backButtonVisible = true;

			if (_backButtonBehavior != null)
After:
			{
				return;
			}

			var stack = null;
			if (stack.Count == 0)
			{
				return;
			}

			_toolbarTracker.Target = _shell;
#if WINDOWS
			_menuBarTracker.Target = _shell;
#endif

			Page? previousPage = null;
			if (stack.Count > 1)
			{
				previousPage = stack[stack.Count - 1];
			}

			ToolbarItems = _toolbarTracker.ToolbarItems;

			UpdateBackbuttonBehavior();
			bool backButtonVisible = true;

			if (_backButtonBehavior != null)
*/
			{
				return;
			}

			var stack = _shell.Navigation.NavigationStack;
			if (stack.Count == 0)
			{
				return;
			}

			_toolbarTracker.Target = _shell;
#if WINDOWS
			_menuBarTracker.Target = _shell;
#endif

			Page? previousPage = null;
			if (stack.Count > 1)

/* Unmerged change from project 'Controls.Core(net8.0)'
Before:
				previousPage = stack[stack.Count - 1];

			ToolbarItems = _toolbarTracker.ToolbarItems;

			UpdateBackbuttonBehavior();
			bool backButtonVisible = true;

			if (_backButtonBehavior != null)
			{
				backButtonVisible = _backButtonBehavior.IsVisible;
After:
			{
				previousPage = stack[stack.Count - 1];
*/

/* Unmerged change from project 'Controls.Core(net8.0-ios)'
Before:
				previousPage = stack[stack.Count - 1];

			ToolbarItems = _toolbarTracker.ToolbarItems;

			UpdateBackbuttonBehavior();
			bool backButtonVisible = true;

			if (_backButtonBehavior != null)
			{
				backButtonVisible = _backButtonBehavior.IsVisible;
After:
			{
				previousPage = stack[stack.Count - 1];
*/

/* Unmerged change from project 'Controls.Core(net8.0-maccatalyst)'
Before:
				previousPage = stack[stack.Count - 1];
After:
			{
				previousPage = stack[stack.Count - 1];
			}
*/

/* Unmerged change from project 'Controls.Core(net8.0-android)'
Before:
				previousPage = stack[stack.Count - 1];
After:
			{
				previousPage = stack[stack.Count - 1];
			}
*/
			{
				previousPage = stack[stack.Count - 1];

/* Unmerged change from project 'Controls.Core(net8.0)'
Before:
			_drawerToggleVisible = stack.Count <= 1;
			BackButtonVisible = backButtonVisible && stack.Count > 1;
			BackButtonEnabled = _backButtonBehavior?.IsEnabled ?? true;

			UpdateTitle();

			Func<bool> getDefaultNavBarIsVisible = () =>
			{
				// Shell.GetEffectiveValue doesn't check the Shell itself, so check it here
				if (_shell.IsSet(Shell.NavBarIsVisibleProperty))
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);

				var flyoutBehavior = (_shell as IFlyoutView).FlyoutBehavior;
#if WINDOWS
				return (!String.IsNullOrEmpty(Title) ||
					TitleView != null ||
					_toolbarTracker.ToolbarItems.Count > 0 ||
					_menuBarTracker.ToolbarItems.Count > 0 ||
					flyoutBehavior == FlyoutBehavior.Flyout);
#else
				return (BackButtonVisible ||
					!String.IsNullOrEmpty(Title) ||
					TitleView != null ||
					_toolbarTracker.ToolbarItems.Count > 0 ||
					flyoutBehavior == FlyoutBehavior.Flyout);
#endif
			};

			IsVisible = _shell.GetEffectiveValue(Shell.NavBarIsVisibleProperty, getDefaultNavBarIsVisible, observer: null);

			if (currentPage != null)
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
		}

		void UpdateBackbuttonBehavior()
		{
			var bbb = Shell.GetBackButtonBehavior(_currentPage);

			if (bbb == _backButtonBehavior)
				return;

			if (_backButtonBehavior != null)
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;

			_backButtonBehavior = bbb;

			if (_backButtonBehavior != null)
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
		}

		void OnBackButtonCommandPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			ApplyChanges();
		}

		void OnCurrentPagePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.Is(Page.TitleProperty))
				UpdateTitle();
After:
			ToolbarItems = _toolbarTracker.ToolbarItems;

			UpdateBackbuttonBehavior();
			bool backButtonVisible = true;

			if (_backButtonBehavior != null)
			{
				backButtonVisible = _backButtonBehavior.IsVisible;
			}

			_drawerToggleVisible = stack.Count <= 1;
			BackButtonVisible = backButtonVisible && stack.Count > 1;
			BackButtonEnabled = _backButtonBehavior?.IsEnabled ?? true;

			UpdateTitle();

			Func<bool> getDefaultNavBarIsVisible = () =>
			{
				// Shell.GetEffectiveValue doesn't check the Shell itself, so check it here
				if (_shell.IsSet(Shell.NavBarIsVisibleProperty))
				{
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);
				}

				var flyoutBehavior = (_shell as IFlyoutView).FlyoutBehavior;
#if WINDOWS
				return (!String.IsNullOrEmpty(Title) ||
					TitleView != null ||
					_toolbarTracker.ToolbarItems.Count > 0 ||
					_menuBarTracker.ToolbarItems.Count > 0 ||
					flyoutBehavior == FlyoutBehavior.Flyout);
#else
				return (BackButtonVisible ||
					!String.IsNullOrEmpty(Title) ||
					TitleView != null ||
					_toolbarTracker.ToolbarItems.Count > 0 ||
					flyoutBehavior == FlyoutBehavior.Flyout);
#endif
			};

			IsVisible = _shell.GetEffectiveValue(Shell.NavBarIsVisibleProperty, getDefaultNavBarIsVisible, observer: null);

			if (currentPage != null)
			{
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
			}
		}

		void UpdateBackbuttonBehavior()
		{
			var bbb = Shell.GetBackButtonBehavior(_currentPage);

			if (bbb == _backButtonBehavior)
			{
				return;
			}

			if (_backButtonBehavior != null)
			{
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;
			}

			_backButtonBehavior = bbb;

			if (_backButtonBehavior != null)
			{
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
			}
		}

		void OnBackButtonCommandPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			ApplyChanges();
		}

		void OnCurrentPagePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.Is(Page.TitleProperty))
			{
				UpdateTitle();
			}
*/

/* Unmerged change from project 'Controls.Core(net8.0-ios)'
Before:
			_drawerToggleVisible = stack.Count <= 1;
			BackButtonVisible = backButtonVisible && stack.Count > 1;
			BackButtonEnabled = _backButtonBehavior?.IsEnabled ?? true;

			UpdateTitle();

			Func<bool> getDefaultNavBarIsVisible = () =>
			{
				// Shell.GetEffectiveValue doesn't check the Shell itself, so check it here
				if (_shell.IsSet(Shell.NavBarIsVisibleProperty))
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);

				var flyoutBehavior = (_shell as IFlyoutView).FlyoutBehavior;
#if WINDOWS
				return (!String.IsNullOrEmpty(Title) ||
					TitleView != null ||
					_toolbarTracker.ToolbarItems.Count > 0 ||
					_menuBarTracker.ToolbarItems.Count > 0 ||
					flyoutBehavior == FlyoutBehavior.Flyout);
#else
				return (BackButtonVisible ||
					!String.IsNullOrEmpty(Title) ||
					TitleView != null ||
					_toolbarTracker.ToolbarItems.Count > 0 ||
					flyoutBehavior == FlyoutBehavior.Flyout);
#endif
			};

			IsVisible = _shell.GetEffectiveValue(Shell.NavBarIsVisibleProperty, getDefaultNavBarIsVisible, observer: null);

			if (currentPage != null)
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
		}

		void UpdateBackbuttonBehavior()
		{
			var bbb = Shell.GetBackButtonBehavior(_currentPage);

			if (bbb == _backButtonBehavior)
				return;

			if (_backButtonBehavior != null)
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;

			_backButtonBehavior = bbb;

			if (_backButtonBehavior != null)
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
		}

		void OnBackButtonCommandPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			ApplyChanges();
		}

		void OnCurrentPagePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.Is(Page.TitleProperty))
				UpdateTitle();
After:
			ToolbarItems = _toolbarTracker.ToolbarItems;

			UpdateBackbuttonBehavior();
			bool backButtonVisible = true;

			if (_backButtonBehavior != null)
			{
				backButtonVisible = _backButtonBehavior.IsVisible;
			}

			_drawerToggleVisible = stack.Count <= 1;
			BackButtonVisible = backButtonVisible && stack.Count > 1;
			BackButtonEnabled = _backButtonBehavior?.IsEnabled ?? true;

			UpdateTitle();

			Func<bool> getDefaultNavBarIsVisible = () =>
			{
				// Shell.GetEffectiveValue doesn't check the Shell itself, so check it here
				if (_shell.IsSet(Shell.NavBarIsVisibleProperty))
				{
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);
				}

				var flyoutBehavior = (_shell as IFlyoutView).FlyoutBehavior;
#if WINDOWS
				return (!String.IsNullOrEmpty(Title) ||
					TitleView != null ||
					_toolbarTracker.ToolbarItems.Count > 0 ||
					_menuBarTracker.ToolbarItems.Count > 0 ||
					flyoutBehavior == FlyoutBehavior.Flyout);
#else
				return (BackButtonVisible ||
					!String.IsNullOrEmpty(Title) ||
					TitleView != null ||
					_toolbarTracker.ToolbarItems.Count > 0 ||
					flyoutBehavior == FlyoutBehavior.Flyout);
#endif
			};

			IsVisible = _shell.GetEffectiveValue(Shell.NavBarIsVisibleProperty, getDefaultNavBarIsVisible, observer: null);

			if (currentPage != null)
			{
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
			}
		}

		void UpdateBackbuttonBehavior()
		{
			var bbb = Shell.GetBackButtonBehavior(_currentPage);

			if (bbb == _backButtonBehavior)
			{
				return;
			}

			if (_backButtonBehavior != null)
			{
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;
			}

			_backButtonBehavior = bbb;

			if (_backButtonBehavior != null)
			{
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
			}
		}

		void OnBackButtonCommandPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			ApplyChanges();
		}

		void OnCurrentPagePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.Is(Page.TitleProperty))
			{
				UpdateTitle();
			}
*/

/* Unmerged change from project 'Controls.Core(net8.0-maccatalyst)'
Before:
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);
After:
				{
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);
				}
*/

/* Unmerged change from project 'Controls.Core(net8.0-android)'
Before:
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);
After:
				{
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);
				}
*/

/* Unmerged change from project 'Controls.Core(net8.0-windows10.0.20348)'
Before:
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);
After:
				{
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);
				}
*/

/* Unmerged change from project 'Controls.Core(net8.0-maccatalyst)'
Before:
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
		}
After:
			{
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
			}
		}
*/

/* Unmerged change from project 'Controls.Core(net8.0-android)'
Before:
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
		}
After:
			{
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
			}
		}
*/

/* Unmerged change from project 'Controls.Core(net8.0-windows10.0.20348)'
Before:
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
		}
After:
			{
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
			}
		}
*/

/* Unmerged change from project 'Controls.Core(net8.0-maccatalyst)'
Before:
				return;

			if (_backButtonBehavior != null)
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;

			_backButtonBehavior = bbb;
After:
			{
				return;
			}
*/

/* Unmerged change from project 'Controls.Core(net8.0-android)'
Before:
				return;

			if (_backButtonBehavior != null)
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;

			_backButtonBehavior = bbb;
After:
			{
				return;
			}
*/

/* Unmerged change from project 'Controls.Core(net8.0-windows10.0.20348)'
Before:
				return;

			if (_backButtonBehavior != null)
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;

			_backButtonBehavior = bbb;
After:
			{
				return;
			}
*/

/* Unmerged change from project 'Controls.Core(net8.0-maccatalyst)'
Before:
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
		}
After:
			{
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;
			}

			_backButtonBehavior = bbb;

			if (_backButtonBehavior != null)
			{
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
			}
		}
*/

/* Unmerged change from project 'Controls.Core(net8.0-android)'
Before:
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
		}
After:
			{
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;
			}

			_backButtonBehavior = bbb;

			if (_backButtonBehavior != null)
			{
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
			}
		}
*/

/* Unmerged change from project 'Controls.Core(net8.0-windows10.0.20348)'
Before:
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
		}
After:
			{
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;
			}

			_backButtonBehavior = bbb;

			if (_backButtonBehavior != null)
			{
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
			}
		}
*/

/* Unmerged change from project 'Controls.Core(net8.0-maccatalyst)'
Before:
				UpdateTitle();
After:
			{
				UpdateTitle();
*/

/* Unmerged change from project 'Controls.Core(net8.0-android)'
Before:
				UpdateTitle();
After:
			{
				UpdateTitle();
*/

/* Unmerged change from project 'Controls.Core(net8.0-windows10.0.20348)'
Before:
				UpdateTitle();
After:
			{
				UpdateTitle();
*/
			}

			ToolbarItems = _toolbarTracker.ToolbarItems;

			UpdateBackbuttonBehavior();
			bool backButtonVisible = true;

			if (_backButtonBehavior != null)
			{
				backButtonVisible = _backButtonBehavior.IsVisible;
			}

			_drawerToggleVisible = stack.Count <= 1;
			BackButtonVisible = backButtonVisible && stack.Count > 1;
			BackButtonEnabled = _backButtonBehavior?.IsEnabled ?? true;

			UpdateTitle();

			Func<bool> getDefaultNavBarIsVisible = () =>
			{
				// Shell.GetEffectiveValue doesn't check the Shell itself, so check it here
				if (_shell.IsSet(Shell.NavBarIsVisibleProperty))
				{
					return (bool)_shell.GetValue(Shell.NavBarIsVisibleProperty);
				}

				var flyoutBehavior = (_shell as IFlyoutView).FlyoutBehavior;
#if WINDOWS
				return (!String.IsNullOrEmpty(Title) ||
					TitleView != null ||
					_toolbarTracker.ToolbarItems.Count > 0 ||
					_menuBarTracker.ToolbarItems.Count > 0 ||
					flyoutBehavior == FlyoutBehavior.Flyout);
#else
				return (BackButtonVisible ||
					!String.IsNullOrEmpty(Title) ||
					TitleView != null ||
					_toolbarTracker.ToolbarItems.Count > 0 ||
					flyoutBehavior == FlyoutBehavior.Flyout);
#endif
			};

			IsVisible = _shell.GetEffectiveValue(Shell.NavBarIsVisibleProperty, getDefaultNavBarIsVisible, observer: null);

			if (currentPage != null)
			{
				DynamicOverflowEnabled = PlatformConfiguration.WindowsSpecific.Page.GetToolbarDynamicOverflowEnabled(currentPage);
			}
		}

		void UpdateBackbuttonBehavior()
		{
			var bbb = Shell.GetBackButtonBehavior(_currentPage);

			if (bbb == _backButtonBehavior)
			{
				return;
			}

			if (_backButtonBehavior != null)
			{
				_backButtonBehavior.PropertyChanged -= OnBackButtonCommandPropertyChanged;
			}

			_backButtonBehavior = bbb;

			if (_backButtonBehavior != null)
			{
				_backButtonBehavior.PropertyChanged += OnBackButtonCommandPropertyChanged;
			}
		}

		void OnBackButtonCommandPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			ApplyChanges();
		}

		void OnCurrentPagePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.Is(Page.TitleProperty))
			{
				UpdateTitle();
			}
			}
			else if (e.IsOneOf(
				Shell.BackButtonBehaviorProperty,
				Shell.NavBarIsVisibleProperty,
				Shell.TitleViewProperty))
			{
				ApplyChanges();
			}
		}

		internal void UpdateTitle()
		{
			TitleView = _shell.GetEffectiveValue<VisualElement>(
				Shell.TitleViewProperty,
				Shell.GetTitleView(_shell));

			if (TitleView != null)
			{
				Title = String.Empty;
				return;
			}

			Page? currentPage = _shell.GetCurrentShellPage();
			if (currentPage?.IsSet(Page.TitleProperty) == true)
			{
				Title = currentPage.Title ?? String.Empty;
			}
			// We only want to use the ShellContent as a title if no pages have been
			// Pushed onto the stack
			else if (_shell.Navigation?.NavigationStack?.Count <= 1)
			{
				Title = _shell.CurrentContent?.Title ?? String.Empty;
			}
			else
			{
				Title = String.Empty;
			}
		}
	}
}
