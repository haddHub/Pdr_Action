using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AxModel
{
    /// <summary>
    /// This class implements the IMessageBoxService for WPF purposes.
    /// </summary>
    public class MessageBoxService : IMessageBoxService
    {
        #region IMessageBoxService Members
        /// <summary>
        /// Displays an error dialog with a given message.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        public void ShowError(string message)
        {
            ShowMessage(message, "Erreur", CustomDialogIcons.Stop);
        }

        /// <summary>
        /// Displays an error dialog with a given message.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        public void ShowInformation(string message)
        {
            ShowMessage(message, "Information", CustomDialogIcons.Information);
        }

        /// <summary>
        /// Displays an error dialog with a given message.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        public void ShowWarning(string message)
        {
            ShowMessage(message, "Avertissement", CustomDialogIcons.Warning);
        }

        /// <summary>
        /// Displays a Yes/No dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowYesNo(string message, CustomDialogIcons icon)
        {
            return ShowQuestionWithButton(message, icon, CustomDialogButtons.YesNo);
        }

        /// <summary>
        /// Displays a Yes/No/Cancel dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowYesNoCancel(string message, CustomDialogIcons icon)
        {
            return ShowQuestionWithButton(message, icon, CustomDialogButtons.YesNoCancel);
        }

        /// <summary>
        /// Displays a OK/Cancel dialog and returns the user input.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>User selection.</returns>
        public CustomDialogResults ShowOkCancel(string message, CustomDialogIcons icon)
        {
            return ShowQuestionWithButton(message, icon, CustomDialogButtons.OkCancel);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Shows a standard System.Windows.MessageBox using the parameters requested
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="heading">The heading to be displayed</param>
        /// <param name="icon">The icon to be displayed.</param>
        private static void ShowMessage(string message, string heading, CustomDialogIcons icon)
        {
            MessageBox.Show(message, heading, MessageBoxButton.OK, GetImage(icon));
        }
        
        /// <summary>
        /// Shows a standard System.Windows.MessageBox using the parameters requested
        /// but will return a translated result to enable adhere to the IMessageBoxService
        /// implementation required.
        ///
        /// This abstraction allows for different frameworks to use the same ViewModels but supply
        /// alternative implementations of core service interfaces
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="icon">The icon to be displayed.</param>
        /// <param name="button"></param>
        /// <returns>CustomDialogResults results to use</returns>
        private static CustomDialogResults ShowQuestionWithButton(string message, CustomDialogIcons icon, CustomDialogButtons button)
        {
            MessageBoxResult result = MessageBox.Show(message,AxLanguage.Languages.REAplan_Confirmation, GetButton(button), GetImage(icon));
            return GetResult(result);
        }

        /// <summary>
        /// Translates a CustomDialogIcons into a standard WPF System.Windows.MessageBox MessageBoxImage.
        /// This abstraction allows for different frameworks to use the same ViewModels but supply
        /// alternative implementations of core service interfaces
        /// </summary>
        /// <param name="icon">The icon to be displayed.</param>
        /// <returns>A standard WPF System.Windows.MessageBox MessageBoxImage</returns>
        private static MessageBoxImage GetImage(CustomDialogIcons icon)
        {
            MessageBoxImage image = MessageBoxImage.None;

            switch (icon)
            {
                case CustomDialogIcons.Information:
                    image = MessageBoxImage.Information;
                    break;
                case CustomDialogIcons.Question:
                    image = MessageBoxImage.Question;
                    break;
                case CustomDialogIcons.Exclamation:
                    image = MessageBoxImage.Exclamation;
                    break;
                case CustomDialogIcons.Stop:
                    image = MessageBoxImage.Stop;
                    break;
                case CustomDialogIcons.Warning:
                    image = MessageBoxImage.Warning;
                    break;
            }
            return image;
        }

        /// <summary>
        /// Translates a CustomDialogButtons into a standard WPF System.Windows.MessageBox MessageBoxButton.
        /// This abstraction allows for different frameworks to use the same ViewModels but supply
        /// alternative implementations of core service interfaces
        /// </summary>
        /// <param name="btn">The button type to be displayed.</param>
        /// <returns>A standard WPF System.Windows.MessageBox MessageBoxButton</returns>
        private static MessageBoxButton GetButton(CustomDialogButtons btn)
        {
            MessageBoxButton button = MessageBoxButton.OK;

            switch (btn)
            {
                case CustomDialogButtons.Ok:
                    button = MessageBoxButton.OK;
                    break;
                case CustomDialogButtons.OkCancel:
                    button = MessageBoxButton.OKCancel;
                    break;
                case CustomDialogButtons.YesNo:
                    button = MessageBoxButton.YesNo;
                    break;
                case CustomDialogButtons.YesNoCancel:
                    button = MessageBoxButton.YesNoCancel;
                    break;
            }
            return button;
        }

        /// <summary>
        /// Translates a standard WPF System.Windows.MessageBox MessageBoxResult into a
        /// CustomDialogIcons.
        /// This abstraction allows for different frameworks to use the same ViewModels but supply
        /// alternative implementations of core service interfaces
        /// </summary>
        /// <param name="result">The standard WPF System.Windows.MessageBox MessageBoxResult</param>
        /// <returns>CustomDialogResults results to use</returns>
        private static CustomDialogResults GetResult(MessageBoxResult result)
        {
            CustomDialogResults customDialogResults = CustomDialogResults.None;

            switch (result)
            {
                case MessageBoxResult.Cancel:
                    customDialogResults = CustomDialogResults.Cancel;
                    break;
                case MessageBoxResult.No:
                    customDialogResults = CustomDialogResults.No;
                    break;
                case MessageBoxResult.None:
                    customDialogResults = CustomDialogResults.None;
                    break;
                case MessageBoxResult.OK:
                    customDialogResults = CustomDialogResults.Ok;
                    break;
                case MessageBoxResult.Yes:
                    customDialogResults = CustomDialogResults.Yes;
                    break;
            }
            return customDialogResults;
        }

        #endregion
    }
}
