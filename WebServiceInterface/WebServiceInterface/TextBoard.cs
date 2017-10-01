/* 
 *  
 *  Filename: TextBoard.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the TextBoard class
 * 
 */

using System.ComponentModel;
using System.Windows.Forms;

namespace WebServiceInterface
{
    public partial class TextBoard : UserControl
    {
        public TextBoard()
        {
            InitializeComponent();
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text
        {
            get { return lblTextBoard.Text; }
            set { lblTextBoard.Text = value; }
        }
    }
}
