using NLua;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Windows.Media;
using System.Diagnostics;


namespace WpfLuaTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static void CallLuaFunction(Lua lua, string functionName, params object[] args)
        {
            // if we can find the required function in our list of registered Lua functions...
            var luaFunction = lua[functionName] as LuaFunction;
            if (luaFunction != null)
            {
                // set it up with the arguments and call it
                var result = luaFunction.Call(args);
                if (result != null && result.Length > 0)
                {
                    Console.WriteLine($"{functionName} returned {result[0]}");
                }
            }
            else
            {
                Console.WriteLine($"{functionName} not found");
            }
        }


        private NLua.Lua lua = new NLua.Lua();

        private Dictionary<string, Control> controls = new Dictionary<string, Control>();
        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();
        private Dictionary<string, CheckBox> checkBoxes = new Dictionary<string, CheckBox>();
        private Dictionary<string, Label> labels = new Dictionary<string, Label>();
        private Dictionary<string, ProgressBar> progressBars = new Dictionary<string, ProgressBar>();
        private Dictionary<string, RadioButton> radioButtons = new Dictionary<string, RadioButton>();
        private Dictionary<string, Slider> sliders = new Dictionary<string, Slider>();
        private Dictionary<string, Rectangle> rectangles = new Dictionary<string, Rectangle>();

        private Dictionary<string, Image> images = new Dictionary<string, Image>();


        public MainWindow()
        {
            // make the outer container
            InitializeComponent();

            // load the script and set up the controls
            InitializeLua();
            // Template='{StaticResource NoHoverButtonTemplate}'
        }


        private void InitializeLua()
        {
            // Register C# for each method and potential event handler;
            // look for everything we prefixed with 'Lua', and we can assume it'll be in the Lua script (without the Lua prefix)
            // eg "SetButtonColor" in the Lua script would map to the LuaSetButtonColor method below
            // (the LuaLoadXaml is basically a pre-requisite)
            foreach(var method in GetType().GetMethods())
            {
                if (method.Name.StartsWith("Lua"))
                {
                    lua.RegisterFunction(method.Name.Substring(3), this, method);
                }
            }
            var text = System.IO.File.ReadAllText("luascript.lua");
            lua.DoString(text);
        }


        private void SetupControlsAndHandlers(Panel stackPanel)
        {
            // set up the controls to their event handlers
            foreach (var child in stackPanel.Children)
            {
                if (child is Button button)
                {
                    controls.Add(button.Name, (Control)button);
                    buttons.Add(button.Name, button);
                    button.Click += Button_Click;
                }
                else if (child is Label label)
                {
                    controls.Add(label.Name, (Control)label);
                    labels.Add(label.Name, label);
                }
                else if (child is CheckBox checkbox)
                {
                    controls.Add(checkbox.Name, (Control)checkbox);
                    checkBoxes.Add(checkbox.Name, checkbox);
                    checkbox.Checked += CheckBox_Checked;
                    checkbox.Unchecked += CheckBox_Unchecked;
                }
                else if (child is ProgressBar progressBar)
                {
                    controls.Add(progressBar.Name, (Control)progressBar);
                    progressBars.Add(progressBar.Name, progressBar);
                }
                else if (child is RadioButton radioButton)
                {
                    controls.Add(radioButton.Name, (Control)radioButton);
                    radioButtons.Add(radioButton.Name, radioButton);
                    radioButton.Checked += RadioButton_Checked;
                    radioButton.Unchecked += RadioButton_Unchecked;
                }
                else if (child is Slider slider)
                {
                    controls.Add(slider.Name, (Control)slider);
                    sliders.Add(slider.Name, slider);
                    slider.ValueChanged += Slider_ValueChanged;
                }
                else if (child is Rectangle rectangle)
                {
                    rectangles.Add(rectangle.Name, rectangle);
                }
                else if (child is Image image)
                {
                    images.Add(image.Name, image);
                }
                else if (child is Panel stack)
                {
                    SetupControlsAndHandlers(stack);
                }
            }
        }

        public void LuaLoadXaml(string xaml)
        {
            var panel = (Panel)XamlReader.Parse(xaml);
            this.Content = panel;
            SetupControlsAndHandlers(panel);
        }


        public void LuaActivate()
        {
            // any post-setup activation stuff we might need here

            /*
            var button = GetButton("btn2");
            if (button != null)
            {
                button.Content = "Activated";
            }
            */
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                CallLuaFunction(lua, $"{checkBox.Name}_checked", checkBox.Tag);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                CallLuaFunction(lua, $"{checkBox.Name}_unchecked", checkBox.Tag);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Slider slider)
            {
                CallLuaFunction(lua, $"{slider.Name}_valuechanged", slider.Value);
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                CallLuaFunction(lua, $"{radioButton.Name}_checked", radioButton.Tag);
            }
        }
        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                CallLuaFunction(lua, $"{radioButton.Name}_changed", radioButton.Tag);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                CallLuaFunction(lua, $"{button.Name}_click", button.Tag);
            }
        }

        private Button? GetButton(string name)
        {
            if (buttons.TryGetValue(name, out var button))
            {
                return button;
            }
            return null;
        }
        private Rectangle? GetRectangle(string name)
        {
            if (rectangles.TryGetValue(name, out var rectangle))
            {
                return rectangle;
            }
            return null;
        }

        private Label? GetLabel(string name)
        {
            if (labels.TryGetValue(name, out var label))
            {
                return label;
            }
            return null;
        }

        private CheckBox? GetCheckBox(string name)
        {
            if (checkBoxes.TryGetValue(name, out var checkBox))
            {
                return checkBox;
            }
            return null;
        }


        private ProgressBar? GetProgressBar(string name)
        {
            if (progressBars.TryGetValue(name, out var pb))
            {
                return pb;
            }
            return null;
        }

        private Control? GetControl(string name)
        {
            if (controls.TryGetValue(name, out var control))
            {
                return control;
            }
            return null;
        }


        /*********** external, Lua-facing methods *****************/

        public void LuaToggleButtonEnabled(string buttonName)
        {
            var button = GetButton(buttonName);
            if (button != null)
            {
                button.IsEnabled = !button.IsEnabled;
            }
        }

        public void LuaSetTag(string controlName, object value)
        {
            var control = GetControl(controlName);
            if (control != null)
            {
                control.Tag = value;
            }
        }
        public void LuaSetLabel(string controlName, object value)
        {
            var label = GetLabel(controlName);
            if (label != null)
            {
                label.Content= value;
            }
        }

        public void LuaSetButtonEnabled(string buttonName, int state)
        {
            var button = GetButton(buttonName);
            if (button != null)
            {
                button.IsEnabled = (state == 1 ? true : false);
            }
        }

        public void LuaSetButtonColor(string buttonName, string color)
        {
            var button = GetButton(buttonName);
            if (button != null)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            }
        }

        public void LuaSetButtonCaption(string buttonName, string caption)
        {
            var button = GetButton(buttonName);
            if (button != null)
            {
                button.Content = caption;
            }
        }

        public void LuaSetRectangleColor(string rectangleName, string color)
        {
            var rect = GetRectangle(rectangleName);
            if (rect != null)
            {
                rect.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            }
        }
        public void LuaSetProgressBarValue(string progressBarName, double value)
        {
            var pb = GetProgressBar(progressBarName);
            if (pb != null)
            {
                pb.Value = value;
            }
        }

    }


}