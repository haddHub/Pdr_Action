// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowCloseBehavior.cs" company="Reed Copsey, Jr.">
//   Copyright 2009, Reed Copsey, Jr.
// </copyright>
// <summary>
//   Simple behavior to prevent a window from being closable, based on a data bindable boolean property
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace AxViewModel
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using GalaSoft.MvvmLight.Command;
    using AxData;
    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Simple behavior to prevent a window from being closable, based on a data bindable boolean property
    /// </summary>
    public class WindowCloseBehavior : Behavior<Window>
    {
        /// <summary>
        /// Identifies the CloseCommand dependency property.
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Dependency Property.  Allow public.")]
        public static DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(RelayCommand), typeof(WindowCloseBehavior), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the CloseFailCommand dependency property.
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Dependency Property.  Allow public.")]
        public static DependencyProperty CloseFailCommandProperty = DependencyProperty.Register("CloseFailCommand", typeof(RelayCommand), typeof(WindowCloseBehavior), new PropertyMetadata(null));

        /// <summary>
        /// TO store the actual window...
        /// </summary>
        private Window window;

        /// <summary>
        /// Gets or sets a value indicating whether the Window can be closed
        /// </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return (RelayCommand)GetValue(CloseCommandProperty);
            }

            set
            {
                SetValue(CloseCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Window can be closed
        /// </summary>
        public RelayCommand CloseFailCommand
        {
            get
            {
                return (RelayCommand)GetValue(CloseFailCommandProperty);
            }

            set
            {
                SetValue(CloseFailCommandProperty, value);
            }
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            base.OnAttached();

            // Trap our loaded event, since we can't get a window until we've loaded
            this.AssociatedObject.Loaded += this.AssociatedObject_Loaded;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= this.AssociatedObject_Loaded;

            if (this.window != null)
            {
                this.window.Closing -= this.Window_Closing;
                this.window.Closed -= this.Window_Closed;
                this.window = null;
            }

            base.OnDetaching();
        }

        /// <summary>
        /// Handles the Loaded event of the user control to which we attached.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            this.window = Window.GetWindow(this.AssociatedObject);
            if (this.window != null)
            {
                this.window.Closing += this.Window_Closing;
                this.window.Closed += this.Window_Closed;
            }
        }

        /// <summary>
        /// Handles the Closed event of the Window control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Window_Closed(object sender, System.EventArgs e)
        {
            if (this.CloseCommand != null)
            {
                ConfigData.ChangerBonneFermeture(true);
                Messenger.Default.Send(true, "StopSendPositions");
                //Messenger.Default.Send("n", "StopRobot"); 
            }
        }

        /// <summary>
        /// Handles the Closing event of the Window.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cancel if we can't close...
            bool cancel = false;
            if (this.CloseCommand != null)
            {
                cancel = !this.CloseCommand.CanExecute(null);
            }

            // If we're not allowed to close, execute the CloseFail command
            if (cancel && this.CloseFailCommand != null)
            {
                this.CloseFailCommand.Execute(null);
            }

            e.Cancel = cancel;
        }
    }
}