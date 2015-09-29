using System.Windows;
using System.Windows.Input;

namespace TestFist.Net.WPF
{
    static public class KeyboardDriver
    {
        public static void Focus(IInputElement element)
        {
            Keyboard.Focus(element);
        }

        public static void SendText(string text)
        {
            var eventArgs = new TextCompositionEventArgs(Keyboard.PrimaryDevice, new TextComposition(InputManager.Current, Keyboard.FocusedElement, text))
            {
                RoutedEvent = UIElement.TextInputEvent
            };
            InputManager.Current.ProcessInput(eventArgs);
        }

        public static void SendTab()
        {
            var tabKey = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab)
            {
                RoutedEvent = Keyboard.KeyDownEvent
            };
            InputManager.Current.ProcessInput(tabKey);  
        }
    }
}