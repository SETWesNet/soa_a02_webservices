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
    /// <summary>
    /// Used for displaying text in a boxed area.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class TextBoard : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoard"/> class.
        /// </summary>
        public TextBoard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the label componenet of the <see cref="TextBoard"/>.
        /// </summary>
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
