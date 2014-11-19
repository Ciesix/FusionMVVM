using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using FusionMVVM.Common;

namespace FusionMVVM
{
    public class BaseViewModel : PropertyChangedObject
    {
        #region IsInDesignMode property

        private static bool? _isInDesignMode;

        /// <summary>
        /// Returns true if the application is currently running in a designer, otherwise false.
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                if (_isInDesignMode.HasValue == false)
                {
                    var property = DesignerProperties.IsInDesignModeProperty;
                    _isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(property, typeof(FrameworkElement)).Metadata.DefaultValue;

                    if (_isInDesignMode.Value == false && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
                        _isInDesignMode = true;
                }

                return _isInDesignMode.Value;
            }
        }

        #endregion
    }
}
