using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UtilityTools.Modules.Canvas.Views.Components
{
    /// <summary>
    /// Interaction logic for XmlWidgetView
    /// </summary>
    public partial class XmlWidgetView : UserControl
    {
        public XmlWidgetView()
        {
            InitializeComponent();

            xmlContent.AddHandler(TextBox.MouseRightButtonDownEvent, new MouseButtonEventHandler(this.xmlContent_MouseRightButtonDown), true);
        }

        private void xmlContent_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            IDataObject data = Clipboard.GetDataObject();

            // 将数据与指定的格式进行匹配，返回bool
            if (data.GetDataPresent(DataFormats.Text))
            {
                // GetData检索数据并指定一个格式
                string test = (string)data.GetData(DataFormats.Text);

                this.xmlContent.Text = test;
            }
            else
            {
            }
        }
    }
}
