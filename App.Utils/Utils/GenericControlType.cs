using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Utils
{
    public enum GenericControlType
    {
        /// <summary>
        /// Dropdown list
        /// </summary>
        DropdownList = 1,
        /// <summary>
        /// Radio list
        /// </summary>
        RadioList = 2,
        /// <summary>
        /// Checkboxes
        /// </summary>
        Checkboxes = 3,
        /// <summary>
        /// TextBox
        /// </summary>
        TextBox = 4,
        /// <summary>
        /// Multiline textbox
        /// </summary>
        MultilineTextbox = 10,
        /// <summary>
        /// Datepicker
        /// </summary>
        Datepicker = 20,
        /// <summary>
        /// File upload control
        /// </summary>
        FileUpload = 30,
        /// <summary>
        /// Boxes
        /// </summary>
        Boxes = 40,
    }
}
