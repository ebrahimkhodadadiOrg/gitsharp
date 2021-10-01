if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Clipboard.SetText(lblText.Content.ToString());
            }